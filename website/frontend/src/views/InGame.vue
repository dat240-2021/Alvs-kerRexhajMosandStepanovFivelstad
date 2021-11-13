<template>
  <div class="d-flex">
    <div class="ms-auto m-2">
      <button class="btn btn-outline-primary" type="button" @click="LeaveGame">
        Leave Game
      </button>
    </div>
  </div>
  <div class="container-fluid">
    <div class="row mt-5">
      <form @submit.prevent="SendGuess" class="form-control border-0">
        <div class="input-group mb-3 mt-5">
          <input
            type="text"
            class="form-control"
            placeholder="Your Guess"
            v-model="newGuess"
            aria-label=""
            aria-describedby="basic-addon1"
          />
          <div class="input-group-prepend">
            <button class="btn btn-outline-primary" type="submit">Guess</button>
          </div>
        </div>
      </form>

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
      <div class="col">
        <img
          class="img-fluid"
          src="https://upload.wikimedia.org/wikipedia/commons/3/3f/Fronalpstock_big.jpg"
        />
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
export class Guess {
  Guess: string;

  constructor(guess: string) {
    this.Guess = guess;
  }
}

declare interface BaseComponentData {
  players: Player[];
  guesses: Guess[];
  imageSlices: string[];
  newGuess: string;
  correct: string;
}

export default defineComponent({
  name: "InGame",
  data(): BaseComponentData {
    return {
      players: [
        new Player("Jamie Lannister", 10, "1"),
        new Player("The Hound", 11, "2"),
        new Player("Rob Stark", 9, "3"),
        new Player("Elvis", 12, "4"),
        new Player("Santa Claus", 4, "5"),
        new Player("Madonna", 2, "6"),
        new Player("Lady Gaga", 0, "7"),
      ] as Player[],

      guesses: [
        new Guess("Monkey"),
        new Guess("Donkey"),
        new Guess("Cat"),
      ] as Guess[],

      imageSlices: [],
      //incorrect: true,
      correct: "Fish",
      newGuess: "",
      //player: '',
    };
  },
  methods: {
    SendGuess() {
      console.log(this.newGuess);
    },

    // LeaveGame(){
    // },
  },
});
</script>
