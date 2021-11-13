<template>
  <div class="d-flex">
    <div class="ms-auto m-2">
      <button
        class="btn btn-outline-primary mb-5"
        type="button"
        @click="LeaveGame"
      >
        Leave Game
      </button>
    </div>
  </div>
  <div class="container-fluid min-vh-100">
    <div class="row mt-5">
      <div v-if="!isProposer">
        <form @submit.prevent="SendGuess" class="form-control border-0">
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
              <td>{{ g.Guess }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="col position-relative" id="canvas-div">
        <div v-if="!isProposer" class="position-relative">
          <img
            v-for="im in GenerateImagePaths()"
            :key="im.id"
            :src="im.src"
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
  imageSlices: string[];
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
    SendGuess() {
      console.log(this.newGuess);
    },
    LeaveGame() {
      console.log("leaving game");
    },

    AddImageProposer() {
      //identifies role
      this.isProposer = true;

      var paths = this.GenerateImagePaths();
      var div = document.getElementById("canvas-div");
      if (div == null) {
        return;
      }

      //scales the image to a max size
      //optimises draw time for canvas
      const width = 500;
      for (var i = 0; i < paths.length; i++) {
        this.AddSliceProposer(paths[i].src, width);
      }

      div.style.transformOrigin = "0 0";
      console.log(div.clientWidth);
      //use css to scale, much faster.
      div.style.transform = "scale(" + div.clientWidth / width + ")";
    },

    AddSliceProposer(path: string, inWidth: number) {
      var div = document.getElementById("canvas-div");
      var canvas = document.createElement("canvas");

      canvas?.classList.add("position-absolute", "top-0", "start-0");
      canvas.style.filter = "brightness(60%)";

      canvas?.addEventListener("click", this.SelectedTile);

      div?.appendChild(canvas);
      var context = canvas.getContext("2d");
      if (context == null) {
        console.log("NOT FOUND: context");
        return;
      }

      var img = new Image();
      img.src = path;
      img.crossOrigin = "anonymous";

      img.onload = function () {
        //scale the image and canvas
        canvas.height = (img.height * inWidth) / img.height;
        canvas.width = inWidth;
        img.width = canvas.width;
        img.height = canvas.height;

        context?.drawImage(
          img,
          0,
          0,
          Math.floor(img.width),
          Math.floor(img.height)
        );
      };
    },
    SelectedTile(event: any) {
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
          //here we call the api to send the image slice id.
          return;
        }
      }
    },

    // HandleSliceGuesser(){},
    // HandleImageProposer(){},
    // Handle

    GenerateImagePaths() {
      var _paths = [
        "https://i.ibb.co/rkvRk2v/1.png",
        "https://i.ibb.co/Qn2fzTD/1.png",
        "https://i.ibb.co/1vv6ZHx/1.png",
        "https://i.ibb.co/5cRWN0W/1.png",
        "https://i.ibb.co/Lrj1JqZ/1.png",
        "https://i.ibb.co/1rnGKwS/1.png",
        "https://i.ibb.co/T2KbjFv/1.png",
        "https://i.ibb.co/CM0yLDb/1.png",
        "https://i.ibb.co/X8XF0Zm/1.png",
        "https://i.ibb.co/tcLrjyX/1.png",
        "https://i.ibb.co/n79x3d2/1.png",
        "https://i.ibb.co/T2sHsZw/1.png",
        "https://i.ibb.co/9VcfNNW/1.png",
        "https://i.ibb.co/RN7xQCP/1.png",
        "https://i.ibb.co/6mFbwT3/1.png",
        "https://i.ibb.co/Db0hrxN/1.png",
        "https://i.ibb.co/0C3nvXF/1.png",
        "https://i.ibb.co/HFHsB8L/1.png",
        "https://i.ibb.co/RQjKCtK/1.png",
        "https://i.ibb.co/7XzdmMb/1.png",
        "https://i.ibb.co/LRr44qp/1.png",
        "https://i.ibb.co/brqc1pT/1.png",
        "https://i.ibb.co/d6V6VBL/1.png",
        "https://i.ibb.co/W2ZWKmD/1.png",
        "https://i.ibb.co/PFpBRp7/1.png",
        "https://i.ibb.co/KsV0xfX/1.png",
        "https://i.ibb.co/VSBHvFV/1.png",
        "https://i.ibb.co/TKC1Vm2/1.png",
        "https://i.ibb.co/Mg0KYv5/1.png",
        "https://i.ibb.co/9t2zyZr/1.png",
        "https://i.ibb.co/2tntL8b/1.png",
        "https://i.ibb.co/V9CLzkf/1.png",
        "https://i.ibb.co/kSm7nXY/1.png",
        "https://i.ibb.co/VmZGRkW/1.png",
        "https://i.ibb.co/CWg1TjL/1.png",
        "https://i.ibb.co/KNfp9db/1.png",
        "https://i.ibb.co/qDHvLtZ/1.png",
        "https://i.ibb.co/0y8ndtv/1.png",
        "https://i.ibb.co/P4Ktddb/1.png",
        "https://i.ibb.co/Z8MsfvQ/1.png",
        "https://i.ibb.co/myRTRX7/1.png",
        "https://i.ibb.co/sgxszkf/1.png",
        "https://i.ibb.co/jkqdKhQ/1.png",
        "https://i.ibb.co/kMpmyGN/1.png",
        "https://i.ibb.co/4W66fw0/1.png",
        "https://i.ibb.co/RN4FvdH/1.png",
        "https://i.ibb.co/N3TKrNx/1.png",
        "https://i.ibb.co/xGmGVrV/1.png",
        "https://i.ibb.co/B6VWy5f/1.png",
      ];

      let paths: Slice[] = [];

      for (let i = 0; i < _paths.length; i++) {
        paths.push(new Slice(_paths[i], i as any as string));
      }

      return paths;
    },
  },
});

//TODO: SignalR coms
//TODO:
</script>
