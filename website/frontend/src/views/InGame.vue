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
            :src="'data:image/png;base64,' + im.imageData"
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
  imageSlicesConverted: {
    sequenceNumber: number;
    DataBytes: Uint8Array;
  }[];
  newGuess: string;
  correct: string;
  isProposer: boolean;
  myTurn: boolean;
  imageSize: {
    width: number;
    height: number;
  };
}

export default defineComponent({
  name: "InGame",
  data(): BaseComponentData {
    return {
      imageSize: {
        width: 0,
        height: 0,
      },
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

      if (this.imageSize.width == 0) {
        var image = new Image();
        image.src = slice.imageData;
        image.onload = () => {
          return;
        };
      }

      this.imageSlicesConverted.push({
        sequenceNumber: slice.sequenceNumber,
        DataBytes: this._base64ToArrayBuffer(slice.imageData),
      });
    },

    _base64ToArrayBuffer(base64: string) {
      var binary_string = window.atob(base64);
      var len = binary_string.length;
      var bytes = new Uint8Array(len);
      for (var i = 0; i < len; i++) {
        bytes[i] = binary_string.charCodeAt(i);
      }
      return bytes;
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

      //these need to be scaled
      var x: number = event.offsetX;
      var y: number = event.offsetY;
      var slices_len = this.imageSlicesConverted.length;

      //need visible image size to scale down mouse position
      var img = document.getElementById(this.imageSlices[0].id.toString());
      var h1 = img?.clientHeight;
      var w1 = img?.clientWidth;

      if (h1 == null || w1 == null) {
        return;
      }

      //original size of the image
      var h0 = this.imageSize.width;
      var w0 = this.imageSize.height;

      var yScaled = Math.floor((h0 * y) / h1);
      var xScaled = Math.floor((w0 * x) / w1);
      let index = (yScaled * w0 + xScaled) * 4;

      //check all the image alpha values and return on the first one thats not transparent.
      for (let i = 0; i < slices_len; i++) {
        var data = this.imageSlicesConverted[i].DataBytes;
        let alpha = data[index + 3];

        if (alpha > 5) {
          return i;
        }
      }

      return;
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
