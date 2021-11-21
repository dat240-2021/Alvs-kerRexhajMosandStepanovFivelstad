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
      <div v-if="!isOver" class="text-center turn-label rounded" :class="{ 'bg-success': myTurn, 'bg-danger': !myTurn }">{{ turnLabel }}</div>
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
              <button class="btn btn-outline-primary" type="submit" :disabled="!myTurn">
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
          <div v-if="gameAlert" class="alert" :class="gameAlert.type">{{ this.gameAlert.message }}</div>
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
            <tr v-for="(guess, i) in reversedGuesses" :key="'guess_' + i">
              <td>{{ guess }}</td>
            </tr>
            </tbody>
          </table>
        </div>
        <div class="col position-relative d-flex" id="canvas-div">
          <div class="position-relative w-50 mx-auto">
            <div v-if="!started" class="d-flex justify-content-center m-5">
              <div
                class="spinner-border"
                role="status"
                style="width: 3rem; height: 3rem"
              />
              <span class="ms-5">Waiting for game to start...</span>
            </div>
            <div v-if="isResetGuesser" class="d-flex justify-content-center m-5">
              <div
                class="spinner-border"
                role="status"
                style="width: 3rem; height: 3rem"
              />
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
            <tr v-for="p in sortedPlayers" :key="p.PlayerId">
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
    <CorrectGuessModal v-if="roundAlert" v-model:alert="roundAlert" :imageSlices="this.roundAlert.imageSlices" :alertType="this.roundAlert.type" :alertMessage="this.roundAlert.message" />
  </teleport>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import {
  sendNewGuess,
  sendNewProposal,
  sendConnect,
  sendDisconnect
} from "@/api/Game";
import * as ws from "@/api/Game/subscriptions";
import {
  Image as GameImage,
  ImageSlice,
  Guess,
  Player,
  Score, CorrectGuess, User
} from "@/typings";
import { getCurrentUser } from "@/utils/auth";
import CorrectGuessModal from "@/components/Modal/CorrectGuessModal.vue";

interface Alert {
  message: string,
  type: string,
  imageSlices: ImageSlice[] | null
}

declare interface BaseComponentData {
  players: Player[];
  guesses: string[];
  imageSlices: ImageSlice[];
  guess: string;
  label: string;
  started: boolean;
  isProposer: boolean;
  myTurn: boolean;
  currentPlayer: User;
  gameAlert: Alert | null;
  roundAlert: Alert | null;
  isOver: boolean;
}

const getRoundAlertMessage = {
  wonRound: (willAutoContinue: boolean) => `Congrats! You won this round. Keep going! ${ willAutoContinue ? "Next round will start soon." : "" }`,
  lostRound: (userName: string, correctGuess: string, willAutoContinue: boolean) => `Ohh no! ${ userName } won this round by guessing word ${ correctGuess }. ${ willAutoContinue ? "Next round will start soon." : "" }`,
  winInfo: (userName: string) => `${ userName } won this round. You are doing a great job as a proposer.`,
};

const getGameAlertMessage = {
  won: (score: number) => `You won the game! Your score is ${ score }.`,
  lost: (score: number, highestScore: number, winnerName: string) => `You lost the game! You gathered ${ score } points. Highest score is ${ highestScore } by ${ winnerName }.`,
  proposer: (score: number, userName: string) => `Game is finished. Your score is ${ score }. Winner is ${ userName }`,
};


const getAlertType = {
  won: "alert-success",
  lost: "alert-danger",
  info: "alert-warning"
};


export default defineComponent({
  name: "Game",
  components: {
    CorrectGuessModal
  },
  data(): BaseComponentData {
    return {
      currentPlayer: getCurrentUser(),
      players: [] as Player[],

      guesses: [] as string[],

      isProposer: false,
      started: false,

      imageSlices: [],
      //incorrect: true,
      label: "",
      guess: "",
      myTurn: false,
      //player: '',
      roundAlert: null,
      gameAlert: null,
      isOver: false
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
    turnLabel: function(): string {
      if (this.myTurn) {
        return "Your turn";
      }
      if (this.isProposer) {
        return "Guesser's turn";
      }

      return "Proposer's turn";
    },
    reversedGuesses(): string[] {
      return [...this.guesses].reverse();
    }
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
      this.guesses = [...this.guesses, this.guess];
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
      //this.guesses = [...this.guesses, guess];
    },
    updateScores(score: Score) {
      const player = this.players.find((x) => x.PlayerId == score.userId);
      if (player) {
        player.Score += score.score;
      }
    },
    handleSubmittedCorrectGuessAsGuesser(guess: CorrectGuess) {
      if (this.isProposer) return;
      this.myTurn = false;
      const userWin = this.currentPlayer.id === guess.userId;

      // userId should be swapped with user name
      this.roundAlert = {
        message: userWin ? getRoundAlertMessage.wonRound(guess.willAutoContinue) : getRoundAlertMessage.lostRound(guess.userId, guess.guess, guess.willAutoContinue),
        type: userWin ? getAlertType.won : getAlertType.lost,
        imageSlices: guess.image.slices
      };
    },

    handleSubmittedCorrectGuessAsProposer(guess: CorrectGuess) {
      if (!this.isProposer) return;
      this.myTurn = true;

      // userId should be swapped with user name
      this.roundAlert = {
        type: getAlertType.info,
        message: getRoundAlertMessage.winInfo(guess.userId),
        imageSlices: guess.image.slices
      };
    },
    handleGameOver(guessersScore: Map<string, number>, proposerScore: number | null) {
      this.isOver = true;
      const highestScore = [...guessersScore.entries()]
        .reduce((acc, el) => el[1] < acc.score ? acc : { id: el[0], score: el[1] }, { id: "", score: 0 });


      if (this.isProposer && proposerScore) {
        this.gameAlert = {
          type: getAlertType.info,
          message: getGameAlertMessage.proposer(proposerScore, highestScore.id),
          imageSlices: null
        };
        return;
      }

      const playerScore = guessersScore.get(this.currentPlayer.id) ?? 0;
      const isWinner = highestScore.id === this.currentPlayer.id;

      console.log(guessersScore);
      console.log(this.currentPlayer.id);

      if (this.roundAlert?.imageSlices) {
        this.imageSlices = this.roundAlert.imageSlices;
      }

      this.gameAlert = {
        type: isWinner ? getAlertType.won : getAlertType.lost,
        message: isWinner ? getGameAlertMessage.won(playerScore) : getGameAlertMessage.lost(playerScore, highestScore.score, highestScore.id),
        imageSlices: null
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

      /// Received by all
      ws.subscribeToPlayerScores(this.updateScores);
      ws.subscribeToNewGuess(this.addIncomingGuess);
      ws.subscribeToInvalidGame(this.leaveGame);
      ws.subscribeToGameOver(this.handleGameOver);
    },
    unsubscribeToGame() {
      ws.unsubscribeToGuessersTurn(this.guessersTurn);
      ws.unsubscribeToNewProposal(this.addSlice);
      ws.unsubscribeToNewImageGuesser(this.newImageGuesser);
      ws.unsubscribeToProposersTurn(this.proposersTurn);
      ws.unsubscribeToNewImageProposer(this.newImageProposer);
      ws.unsubscribeToPlayerScores(this.updateScores);
      ws.unsubscribeToNewGuess(this.addIncomingGuess);
      ws.unsubscribeToInvalidGame(this.leaveGame);
    },
  },
});
</script>

<style scoped>
.turn-label {
  width: 100px;
}
</style>
