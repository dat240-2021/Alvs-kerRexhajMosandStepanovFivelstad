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
        <div v-if="!isProposer" class="position-relative">
          <img
            v-for="im in imageSlicesConverted"
            :key="im.id"
            :src="im.imageData"
            style="width: 100%"
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
  subscribeToNewImageProposer,
  subscribeToNewProposal,
  subscribeToNewGuess,
  sendNewProposal,
  sendNewGuess,
  subscribeToProposersTurn,
} from "@/api/InGame";
import { Image, ImageSlice, Guess, Proposal } from "@/typings";

export class Player {
  Name: string;
  Score: number;
  PlayerId: string;

  constructor(Name: string, Score: number, PlayerId: string) {
    this.Name = Name;
    this.Score = Score;
    this.PlayerId = PlayerId;
  }
}

export class Slice {
  src: string;
  id: string;

  constructor(src: string, id: string) {
    this.src = src;
    this.id = "slice-".concat(id);
  }
}

declare interface BaseComponentData {
  players: Player[];
  guesses: string[];
  imageSlices: ImageSlice[];
  imageSlicesConverted: any[];
  newGuess: string;
  correct: string;
  isProposer: boolean;
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
      players: [
        new Player("Jamie Lannister", 10, "1"),
        new Player("The Hound", 11, "2"),
        new Player("Rob Stark", 9, "3"),
        new Player("Elvis", 12, "4"),
        new Player("Santa Claus", 4, "5"),
        new Player("Madonna", 2, "6"),
        new Player("Lady Gaga", 0, "7"),
      ] as Player[],

      imageSlicesConverted: [],

      guesses: ["test1", "ship", "helloworld", "i am testing"],

      isProposer: true,

      imageSlices: [],
      //incorrect: true,
      correct: "Fish",
      newGuess: "",
      //player: '',
    };
  },
  methods: {
    mounted() {
      console.log("FUCK OFF");
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

    newImageProposer(image: Image) {
      this.isProposer = true;
      for (var i = 0; i < image.slices.length; i++) {
        this.AddSlice(image.slices[i]);
      }
      // //identifies role
      // this.isProposer = true;

      // var div = document.getElementById("canvas-div");
      // if (div == null) {
      //   return;
      // }

      // //scales the image to a max size
      // //optimises draw time for canvas
      // const width = 500;
      // for (var i = 0; i < image.slices.length; i++) {
      //   this.AddSliceProposer(image.slices[i], width);
      // }

      // div.style.transformOrigin = "0 0";
      // console.log(div.clientWidth);
      // //use css to scale, much faster.
      // div.style.transform = "scale(" + div.clientWidth / width + ")";
    },

    AddSlice(slice: ImageSlice) {
      this.imageSlices.push(slice);
      this.imageSlicesConverted.push(
        URL.createObjectURL(
          new Blob([slice.imageData.buffer], { type: "image/png" } /* (1) */)
        )
      );
    },

    proposersTurn() {
      this.isProposer = true;
    },

    checkTransparency(slice: ImageSlice, x: number, y: number): boolean {
      var width = 1;
      var data = slice.imageData;
      let index = (y * width + x) * 4;
      let alpha = data[index + 3];

      if (alpha < 5) {
        return true;
      }

      return false;
    },

    proposerSelectedSlice(event: any) {
      var x = event.offsetX;
      var y = event.offsetY;

      var children = document.getElementById("canvas-div")?.children;
      if (children == null) {
        return;
      }

      for (let i = 0; i < children.length; i++) {
        var cnvs = children.item(i);
        if (!(cnvs instanceof HTMLCanvasElement)) {
          return;
        }
        var canvas: HTMLCanvasElement = cnvs;
        var context = canvas.getContext("2d");
        if (context == null) {
          return;
        }

        var data = context.getImageData(0, 0, canvas.width, canvas.height);
        let index = (y * data.width + x) * 4;
        let alpha = data?.data[index + 3];
        if (alpha > 5) {
          console.log(i);
          canvas.style.filter = "brightness(100%)";
          sendNewProposal(i);
          return;
        }
      }
    },

    addIncomingGuess(guess: Guess) {
      this.guesses.push(guess.guess);
    },

    subscribeToActiveGame() {
      /// Received by guesser
      // 
      // subscribeToGuessersTurn();
      // subscribeToNewProposal(this.AddSlice);
      // subscribeToNewImageProposer();

      /// Received by proposer
      //
      // subscribeToProposersTurn(this.proposersTurn);
      // subscribeToNewImageProposer(this.newImageProposer);

      /// Received by all
      subscribeToNewGuess(this.addIncomingGuess);

    },
  },
});

//TODO: SignalR coms
//TODO:
</script>
