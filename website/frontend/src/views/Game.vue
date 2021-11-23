<template>
  <div class="d-flex">
    <div class="ms-auto m-2">
      <button class="btn btn-outline-primary" type="button" @click="leaveGame">
        Leave Game
      </button>
    </div>
  </div>
  <div class="container min-vh-100">
    <div class="row mt-3">
      <div
        v-if="!isOver"
        class="text-center turn-label rounded mb-4"
        :class="{ 'bg-success': myTurn, 'bg-danger': !myTurn }"
      >
        {{ turnLabel }}
      </div>
      <div class="d-flex" v-if="!isProposer && started">
        <div class="ms-auto">
          <p>
            <b>Playing as: </b>
            <i>Guesser</i>
          </p>
        </div>

        <form @submit.prevent="sendGuess" class="form-control border-0">
          <div class="input-group mb-3">
            <input
              type="text"
              class="form-control"
              placeholder="Your Guess"
              v-model="guess"
              aria-label=""
              aria-describedby="basic-addon1"
              :disabled="!myTurn"
            />
            <div class="input-group-prepend">
              <button
                class="btn btn-outline-primary"
                type="submit"
                :disabled="!myTurn"
              >
                Guess
              </button>
            </div>
          </div>
        </form>
      </div>
      <div v-else class="d-flex ms-auto" />
      <div class="row">
        <div class="col d-flex flex-column pb-3">
          <h2 v-if="isProposer" class="text-center">{{ this.label }}</h2>
          <div v-if="inlineAlert" class="alert" :class="inlineAlert.type">
            {{ this.inlineAlert.message }}
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-1">
          <table>
            <thead>
              <tr>
                <th>Guesses:</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(guess, i) in latestValidGuesses" :key="'guess_' + i">
                <td>{{ guess.user }} : {{ guess.guess }}</td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="col position-relative d-flex" id="canvas-div">
          <div class="position-relative w-50 mx-auto">
            <div v-if="!started" class="d-flex justify-content-center m-5">
              <div>
                <div
                  class="spinner-border"
                  role="status"
                  style="width: 3rem; height: 3rem"
                />
              </div>
              <span class="ms-5">Waiting for game to start...</span>
            </div>
            <div
              v-if="isResetGuesser"
              class="d-flex justify-content-center m-5"
            >
              <div>
                <div
                  class="spinner-border"
                  role="status"
                  style="width: 3rem; height: 3rem"
                />
              </div>
              <span class="ms-5">Waiting for proposer...</span>
            </div>
            <img
              v-on:click="proposerSelectedSlice"
              v-for="im in imageSlices"
              :key="im.id"
              :src="'data:image/png;base64,' + im.imageData"
              style="object-fit: cover"
              :id="im.id"
              class="position-absolute top-0 start-0 images w-100"
            />
          </div>
          <!-- <canvas id="image-canvas" width="1000" height="1000"></canvas> -->
        </div>
        <div class="col-2">
          <table>
            <thead>
              <tr>
                <th>Players:</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="p in sortedPlayers" :key="p.Name">
                <td>{{ p.Name }}</td>
                <td>{{ p.Score }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
  <teleport to="body">
    <AlertInGameModal
      v-if="modalAlert"
      v-model:alert="modalAlert"
      :imageSlices="this.modalAlert.imageSlices"
      :alertType="this.modalAlert.type"
      :alertMessage="this.modalAlert.message"
    />
  </teleport>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import {
  sendNewGuess,
  sendNewProposal,
  sendConnect,
  sendDisconnect,
} from "@/api/Game";
import * as ws from "@/api/Game/subscriptions";
import {
  Image as GameImage,
  ImageSlice,
  Guess,
  Player,
  Score,
  CorrectGuess,
  User,
} from "@/typings";
import { getCurrentUser } from "@/utils/auth";
import AlertInGameModal from "@/components/Modal/AlertInGameModal.vue";

interface Alert {
  message: string;
  type: string;
  imageSlices: ImageSlice[] | null;
}

declare interface BaseComponentData {
  players: Player[];
  guesses: Guess[];
  imageSlices: ImageSlice[];
  guess: string;
  label: string;
  started: boolean;
  isProposer: boolean;
  myTurn: boolean;
  currentPlayer: User;
  inlineAlert: Alert | null;
  modalAlert: Alert | null;
  isOver: boolean;
  proposerScore: number;
}

const getRoundAlertMessage = {
  wonRound: (willAutoContinue: boolean) =>
    `Congrats! You won this round. Keep going! ${
      willAutoContinue ? "Next round will start soon." : ""
    }`,
  lostRound: (
    username: string,
    correctGuess: string,
    willAutoContinue: boolean
  ) =>
    `Ohh no! ${username} won this round by guessing word ${correctGuess}. ${
      willAutoContinue ? "Next round will start soon." : ""
    }`,
  winInfo: (username: string) =>
    `${username} won this round. You are doing a great job as a proposer.`,
  noGuesses: (correctGuess: string) =>
    `No one won this round! The correct guessing word was ${correctGuess}`,
};

const getGameAlertMessage = {
  won: (score: number) => `You won the game! Your score is ${score}.`,
  lost: (score: number, highestScore: number, winnerName: string) =>
    `You lost the game! You gathered ${score} points. Highest score is ${highestScore} by ${winnerName}.`,
  proposer: (score: number, username: string) =>
    `Game is finished. Your score is ${score}. Winner is ${username}`,
};

const getAlertType = {
  won: "alert-success",
  lost: "alert-danger",
  info: "alert-warning",
};

export default defineComponent({
  name: "Game",
  components: {
    AlertInGameModal,
  },
  data(): BaseComponentData {
    return {
      currentPlayer: getCurrentUser(),
      players: [] as Player[],

      guesses: [] as Guess[],

      isProposer: false,
      started: false,

      imageSlices: [],
      //incorrect: true,
      label: "",
      guess: "",
      myTurn: false,
      //player: '',
      modalAlert: null,
      inlineAlert: null,
      isOver: false,

      proposerScore: 0,
    };
  },
  computed: {
    sortedPlayers: function (): Player[] {
      return [...this.players].sort(
        (a: Player, b: Player) => b.Score - a.Score
      );
    },
    isResetGuesser: function (): boolean {
      return this.started && !this.isProposer && this.imageSlices.length == 0;
    },
    turnLabel: function (): string {
      if (this.myTurn) {
        return "Your turn";
      }
      if (this.isProposer) {
        return "Guesser's turn";
      }

      return "Proposer's turn";
    },
    latestValidGuesses(): Guess[] {
      const takeElements = 10;

      return [...this.guesses]
        .reverse()
        .filter((guess) => guess.guess.trim().length)
        .filter((_, i) => i < takeElements);
    },
  },
  created() {
    this.subscribeToGame();
    sendConnect();
  },
  unmounted() {
    sendDisconnect();
    this.unsubscribeToGame();
  },
  methods: {
    sendGuess() {
      sendNewGuess(this.guess);
      this.guess = "";
      this.myTurn = false;
    },
    newProposal(i: number) {
      sendNewProposal(i);
      this.myTurn = false;
    },
    leaveGame() {
      this.$router.push({ name: "Home" });
    },

    newImageGuesser() {
      this.imageSlices = [];
      this.guesses = [];
      this.started = true;
    },

    newImageProposer(image: GameImage) {
      this.isProposer = true;
      this.myTurn = true;
      this.started = true;

      this.imageSlices = image.slices;
      this.label = image.label.label;
      this.guesses = [];
    },

    addSlice(slice: ImageSlice) {
      this.imageSlices = [...this.imageSlices, slice];
    },

    proposersTurn() {
      this.myTurn = this.isProposer;
    },

    guessersTurn() {
      this.myTurn = !this.isProposer;
    },

    proposerSelectedSlice(event: any) {
      if (!this.myTurn || !this.isProposer || this.isOver) return;

      let x = event.offsetX;
      let y = event.offsetY;
      const element = event.target;
      x -= element.offsetLeft;
      y -= element.offsetTop;

      for (let slice of this.imageSlices) {
        const img = document.getElementById(
          slice.id.toString()
        ) as HTMLImageElement;

        let canvas = document.createElement("canvas");
        canvas.width = img.width;
        canvas.height = img.height;
        const ctx = canvas.getContext("2d");
        ctx?.drawImage(img, 0, 0, img.width, img.height);

        let alpha = ctx?.getImageData(x, y, 1, 1)?.data[3];
        if (alpha && alpha > 5) {
          sendNewProposal(slice.sequenceNumber);
          img?.classList.add("opacity-25");
          break;
        }
      }
    },

    addIncomingGuess(guess: Guess) {
      this.guesses = [...this.guesses, guess];
    },
    updateScores(score: Score) {
      const player = this.players.find((x) => x.Name == score.PlayerName);
      if (player) {
        player.Score = score.score;
      }
    },
    handleSubmittedCorrectGuessAsGuesser(guess: CorrectGuess) {
      if (guess.guesser != this.currentPlayer.username) {
        return;
      }
      if (this.isProposer) return;
      this.myTurn = false;
      const userWin = this.currentPlayer.username === guess.guesser;

      // userId should be swapped with user name
      this.modalAlert = {
        message: userWin
          ? getRoundAlertMessage.wonRound(guess.willAutoContinue)
          : getRoundAlertMessage.lostRound(
              guess.guesser,
              guess.guess,
              guess.willAutoContinue
            ),
        type: userWin ? getAlertType.won : getAlertType.lost,
        imageSlices: guess.image.slices,
      };
    },
    handleSubmittedCorrectGuessAsProposer(guess: CorrectGuess) {
      if (guess.proposer != this.currentPlayer.username) {
        return;
      }
      if (!this.isProposer) return;
      this.myTurn = true;

      // userId should be swapped with user name
      this.modalAlert = {
        type: getAlertType.info,
        message: getRoundAlertMessage.winInfo(guess.guesser),
        imageSlices: guess.image.slices,
      };
    },
    handleCorrectGuess(guess: CorrectGuess) {
      let player = this.players.find((x) => x.Name == guess.guesser);
      if (player != null) {
        player.Score = guess.newGuesserScore;
      } else {
        this.players = [
          ...this.players,
          { Name: guess.guesser, Score: guess.newGuesserScore } as Player,
        ];
      }

      if (this.isProposer){
        this.proposerScore = guess.newProposerScore;
      }
    },
    handleNoGuesses(guess: string) {
      this.modalAlert = {
        type: getAlertType.info,
        message: getRoundAlertMessage.noGuesses(guess),
        imageSlices: null,
      };
    },
    handleGameOver() {
      console.log("THE FUCKING GAME IS OVER!!!")
      this.isOver = true;
      const highestScore = this.sortedPlayers[0];

      if (this.isProposer && this.proposerScore) {
        this.inlineAlert = {
          type: getAlertType.info,
          message: getGameAlertMessage.proposer(this.proposerScore, highestScore.Name),
          imageSlices: null,
        };
        return;
      }

      let player = this.players.find( p => p.Name == this.currentPlayer.username );
      let playerScore = 0
      if (player){
          playerScore = player.Score;
      }

      const isWinner = highestScore.Name === this.currentPlayer.username;
      if (this.modalAlert?.imageSlices) {
        this.imageSlices = this.modalAlert.imageSlices;
      }

      this.inlineAlert = {
        type: isWinner ? getAlertType.won : getAlertType.lost,
        message: isWinner
          ? getGameAlertMessage.won(playerScore)
          : getGameAlertMessage.lost(
              playerScore,
              highestScore.Score,
              highestScore.Name
            ),
        imageSlices: null,
      };
    },

    subscribeToGame() {
      /// Received by guesser
      //
      ws.subscribeToGuessersTurn(this.guessersTurn);
      ws.subscribeToNewProposal(this.addSlice);
      ws.subscribeToNewImageGuesser(this.newImageGuesser);
      ws.subscribeToCorrectGuess(this.handleSubmittedCorrectGuessAsGuesser);

      /// Received by proposer
      //
      ws.subscribeToProposersTurn(this.proposersTurn);
      ws.subscribeToNewImageProposer(this.newImageProposer);
      ws.subscribeToCorrectGuess(this.handleSubmittedCorrectGuessAsProposer);
      ws.subscribeToCorrectGuess(this.handleCorrectGuess);

      /// Received by all
      ws.subscribeToPlayerScores(this.updateScores);
      ws.subscribeToNewGuess(this.addIncomingGuess);
      ws.subscribeToInvalidGame(this.leaveGame);
      ws.subscribeToGameOver(this.handleGameOver);
      ws.subscribeToNoneGuessedCorrectly(this.handleNoGuesses);
    },
    unsubscribeToGame() {
      ws.unsubscribeToCorrectGuess(this.handleCorrectGuess);
      ws.unsubscribeToGuessersTurn(this.guessersTurn);
      ws.unsubscribeToNewProposal(this.addSlice);
      ws.unsubscribeToNewImageGuesser(this.newImageGuesser);
      ws.unsubscribeToProposersTurn(this.proposersTurn);
      ws.unsubscribeToNewImageProposer(this.newImageProposer);
      ws.unsubscribeToPlayerScores(this.updateScores);
      ws.unsubscribeToNewGuess(this.addIncomingGuess);
      ws.unsubscribeToInvalidGame(this.leaveGame);
      ws.unsubscribeToGameOver(this.handleGameOver);
      ws.unsubscribeToNoneGuessedCorrectly(this.handleNoGuesses);
      ws.unsubscribeToCorrectGuess(this.handleSubmittedCorrectGuessAsProposer);
      ws.unsubscribeToCorrectGuess(this.handleSubmittedCorrectGuessAsGuesser);
    },
  },
});
</script>
