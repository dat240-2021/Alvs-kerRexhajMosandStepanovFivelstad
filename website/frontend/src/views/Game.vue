<template>
  <div class="d-flex">
    <div class="ms-auto m-2">
      <button class="btn btn-outline-primary" type="button" @click="leaveGame">
        Leave Game
      </button>
    </div>
  </div>
  <div class="container-fluid min-vh-100">
    <div class="row mt-3">
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
              <button class="btn btn-outline-primary" type="submit">
                Guess
              </button>
            </div>
          </div>
        </form>
      </div>
      <div v-else class="d-flex ms-auto" />

      <h2 v-if="isProposer" class="text-center">{{ this.label }}</h2>

      <div class="col-1">
        <table>
          <thead>
            <tr>
              <th>Guesses:</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="g in guesses" :key="g">
              <td>{{ g.userId }}: {{ g.guess }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="col position-relative" id="canvas-div">
        <div class="position-relative">
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
            style="width: 100%"
            :id="im.id"
            class="position-absolute top-0 start-0"
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
} from "@/typings";

declare interface BaseComponentData {
  players: Player[];
  guesses: Guess[];
  imageSlices: ImageSlice[];
  guess: string;
  label: string;
  started: boolean;
  isProposer: boolean;
  myTurn: boolean;
}

export default defineComponent({
  name: "Game",
  data(): BaseComponentData {
    return {
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
      if (!this.myTurn && !this.isProposer) return;

      let x = event.offsetX;
      let y = event.offsetY;
      var element = event.target;
      x -= element.offsetLeft;
      y -= element.offsetTop;

      for (let slice of this.imageSlices) {
        var img = document.getElementById(
          slice.id.toString()
        ) as HTMLImageElement;

        let canvas = document.createElement("canvas");
        canvas.width = img.width;
        canvas.height = img.height;
        var ctx = canvas.getContext("2d");
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
      var player = this.players.find((x) => x.PlayerId == score.userId);
      if (player) {
        player.Score += score.score;
      }
    },

    subscribeToGame() {
      /// Received by guesser
      //
      ws.subscribeToGuessersTurn(this.guessersTurn);
      ws.subscribeToNewProposal(this.addSlice);
      ws.subscribeToNewImageGuesser(this.newImageGuesser);

      /// Received by proposer
      //
      ws.subscribeToProposersTurn(this.proposersTurn);
      ws.subscribeToNewImageProposer(this.newImageProposer);

      /// Received by all
      ws.subscribeToPlayerScores(this.updateScores);
      ws.subscribeToNewGuess(this.addIncomingGuess);
      ws.subscribeToInvalidGame(this.leaveGame);
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
