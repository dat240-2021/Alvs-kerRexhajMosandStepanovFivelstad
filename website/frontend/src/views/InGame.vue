<template>
  <div class="d-flex">
    <div class="ms-auto m-2">
      <button
        class="btn btn-outline-primary mb-5"
        type="button"
        @click="leaveGame"
      >
        Leave Game
      </button>
    </div>
  </div>
  <div class="container-fluid min-vh-100">
    <div class="row mt-5">
      <div v-if="!isProposer">
        <form @submit.prevent="sendGuess" class="form-control border-0">
          <div class="input-group mb-3">
            <input
              type="text"
              class="form-control"
              placeholder="Your Guess"
              v-model="newGuess"
              aria-label=""
              aria-describedby="basic-addon1"
            />
            <div class="input-group-prepend">
              <button class="btn btn-outline-primary" type="submit">
                Guess
              </button>
            </div>
          </div>
        </form>
      </div>

      <h2 v-if="isProposer" class="text-center">Solution Text</h2>

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
          <img
            v-for="im in imageSlices"
            :key="im.id"
            :src="'data:image/png;base64,'+im.imageData"
            style="width: 70%"
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
</template>

<script lang="ts">
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
  Image,
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
  imageSlicesConverted: any[];
  newGuess: string;
  correct: string;
  isProposer: boolean;
  myTurn: boolean;
  coords: {
    x: number;
    y: number;
  };
}

export default defineComponent({
  name: "InGame",
  data(): BaseComponentData {
    return {
      coords: { x: 0, y: 0 },
      players: [] as Player[],

      imageSlicesConverted: [],

      guesses: ["test1", "ship", "helloworld", "i am testing"],

      isProposer: false,

      imageSlices: [],
      //incorrect: true,
      correct: "Fish",
      newGuess: "",
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
      sendNewGuess(this.newGuess);
    },
    newProposal(i: number) {
      sendNewProposal(i);
    },
    leaveGame() {
      console.log("leaving game");
    },

    newImageGuesser() {
      //clear variables
      //get scores
    },

    newImageProposer(image: Image) {
      this.isProposer = true;
      console.log("Adding new image as proposer");

      for (var i = 0; i < image.slices.length; i++) {
        this.AddSlice(image.slices[i]);
      }
    },

    AddSlice(slice: ImageSlice) {
      this.imageSlices.push(slice);

      /*
      this.imageSlicesConverted.push(
        URL.createObjectURL(
          fetch(slice.imageData)
          .then(res => res.blob())
        )
      );
      */
    },

    proposersTurn() {
      this.myTurn = true;
    },

    guessersTurn() {
      this.myTurn = true;
    },

    proposerSelectedSlice(event: any) {
      if (!this.isProposer) {
        return;
      }
      /*

      //these need to be scaled
      var x: number = event.offsetX;
      var y: number = event.offsetY;

      if (this.imageSlices.length < 1) {
        return;
      }

      for (let i = 0; i < this.imageSlices.length; i++) {
        //width needs to be calculated
        var width = 1;
        var data = this.imageSlices[i].imageData;
        let index = (y * width + x) * 4;
        let alpha = data[index + 3];

        if (alpha > 5) {
          return this.imageSlices[i];
        }
      }

      return;
      */
    },

    //old ----- to be deleted, kept for refrence
    // proposerSelectedSlice(event: any) {
    //   var x = event.offsetX;
    //   var y = event.offsetY;

    //   var children = document.getElementById("canvas-div")?.children;
    //   if (children == null) {
    //     return;
    //   }

    //   for (let i = 0; i < children.length; i++) {
    //     var cnvs = children.item(i);
    //     if (!(cnvs instanceof HTMLCanvasElement)) {
    //       return;
    //     }
    //     var canvas: HTMLCanvasElement = cnvs;
    //     var context = canvas.getContext("2d");
    //     if (context == null) {
    //       return;
    //     }

    //     var data = context.getImageData(0, 0, canvas.width, canvas.height);
    //     let index = (y * data.width + x) * 4;
    //     let alpha = data?.data[index + 3];
    //     if (alpha > 5) {
    //       console.log(i);
    //       canvas.style.filter = "brightness(100%)";
    //       sendNewProposal(i);
    //       return;
    //     }
    //   }
    // },

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
      subscribeToNewProposal(this.AddSlice);
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
