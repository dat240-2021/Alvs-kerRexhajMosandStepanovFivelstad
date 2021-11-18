<template>
  <div class="d-flex">
    <div class="ms-auto m-2">
      <button
        class="btn btn-outline-primary"
        type="button"
        @click="leaveGame"
      >
        Leave Game
      </button>
    </div>
  </div>
  <div class="container-fluid min-vh-100">
    <div class="row mt-3">
      
      <div class='d-flex' v-if="!isProposer && started">
        <div class='ms-auto'>
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
              style='opacity: 0.3;'
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
      <div v-else class='d-flex ms-auto' />

      <h2 v-if="isProposer" class="text-center">{{this.label}}</h2>

      <div class="col-1">
        <table>
          <thead>
            <tr>
              <th>Guesses:</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="g in guesses" :key="g">
              <td>{{ g }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="col position-relative" id="canvas-div">
        <div class="position-relative">
          <div v-if="!started" class="d-flex justify-content-center m-5">
            <div class="spinner-border" role="status" style="width: 3rem; height: 3rem;"/>
            <span class="ms-5">Waiting for game to start...</span>
          </div>
          <div v-if="started && !isProposer && imageSlices.length == 0" class="d-flex justify-content-center m-5">
            <div class="spinner-border" role="status" style="width: 3rem; height: 3rem;"/>
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
            <tr
              v-for="p in players.sort((a, b) => b.Score - a.Score)"
              :key="p.PlayerId"
            >
              <td>{{ p.Name }}</td>
              <td>{{ p.Score }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
  <teleport to="body">
    <GameAlertModal v-if="joinedGame" v-model:text="Test" :title="Alert"/>
  </teleport>
</template>

<script lang="ts">
import GameAlertModal from "@/components/Modal/GameAlertModal.vue";
import { defineComponent } from "vue";
import {
  subscribeToNewImageGuesser,
  subscribeToNewImageProposer,
  subscribeToNewProposal,
  subscribeToNewGuess,
  sendNewProposal,
  sendNewGuess,
  subscribeToGuessersTurn,
  subscribeToProposersTurn,
  subscribeToPlayerScores,
} from "@/api/InGame";
import {
  Image as GameImage,
  ImageSlice,
  Guess,
  Player,
  Proposal,
  newScore,
} from "@/typings";

declare interface BaseComponentData {
  players: Player[];
  guesses: string[];
  imageSlices: ImageSlice[];
  guess: string;
  label: string;
  started: boolean;
  isProposer: boolean;
  myTurn: boolean;
}

export default defineComponent({
  name: "InGame",
  data(): BaseComponentData {
    return {
      players: [] as Player[],

      guesses: [],

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
  created() {
    this.subscribeToActiveGame();
  },
  methods: {
    mounted() {
      console.log("hello world");
    },
    sendGuess() {
      sendNewGuess(this.guess);
      this.guess = "";
    },
    newProposal(i: number) {
      sendNewProposal(i);
    },
    leaveGame() {
      console.log("leaving game");
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
      console.log("Adding slice");
      this.imageSlices.push(slice);
    },

    proposersTurn() {
      this.myTurn = this.isProposer;
    },

    guessersTurn() {
      this.myTurn = !this.isProposer;
    },

    proposerSelectedSlice(event: any) {
      if (!this.myTurn) return;

      var x = event.offsetX;
      var y = event.offsetY;
      let element = event.target;
      x -= element.offsetLeft;
      y -= element.offsetTop;

      for (let slice of this.imageSlices) {
        var img = document.getElementById(slice.id.toString()) as HTMLImageElement;
        let h1 = img?.clientHeight ?? 1;
        let w1 = img?.clientWidth ?? 1;

        let canvas = document.createElement('canvas');
        canvas.width = img.height;
        canvas.height = img.width;
        var ctx = canvas.getContext('2d');
        ctx?.drawImage(img, 0, 0, img.width, img.height);

        let data = ctx?.getImageData(x, y, 1, 1);
        let alpha = data?.data[3];
        if (alpha && alpha > 5) {
          sendNewProposal(slice.sequenceNumber);
          document.getElementById(slice.id.toString())?.classList.add("opacity-25");
          break;
        }
      }
    },

    addIncomingGuess(guess: Guess) {
      this.guesses.push(guess.guess);
    },
    updateScores(score: newScore) {
      var player = this.players.find((x) => x.PlayerId == score.userId);
      if (player) {
        player.Score += score.score;
      }
    },

    subscribeToActiveGame() {
      /// Received by guesser
      //
      subscribeToGuessersTurn(this.guessersTurn);
      subscribeToNewProposal(this.addSlice);
      subscribeToNewImageGuesser(this.newImageGuesser);

      /// Received by proposer
      //
      subscribeToProposersTurn(this.proposersTurn);
      subscribeToNewImageProposer(this.newImageProposer);

      /// Received by all
      subscribeToPlayerScores(this.updateScores);
      subscribeToNewGuess(this.addIncomingGuess);
    },
  },
});
</script>
