<template>
  <div class="container vh-100 py-5">
    <form class="h-100" @submit.prevent="handleSubmit">
      <div class="row h-75 justify-content-around">

        <div class="col-sm-4 pb-4 ">
          <div class="row">
            <div class="col">
            <h4 class="text-center pb-2">Game Settings</h4>
              <div class="border border-2 rounded p-2">
                <label for="proposerSelection">Proposer type</label>
                <select v-model="form.proposerType" id="proposerSelection" class="form-select mb-2" aria-label="Proposer">
                  <option>AI</option>
                  <option>Player</option>
                </select>
                <Input
                  v-model="form.guessersCount"
                  class="mb-2"
                  error=""
                  type="number"
                  min="1"
                  max="6"
                  id="playersCountInput"
                  label="Number of guessers"
                />
                <Input
                  v-model="form.imagesCount"
                  class="mb-2"
                  error=""
                  type="number"
                  min="1"
                  max="10"
                  id="picturesCountInput"
                  label="Number of pictures"
                />
                <Input
                  v-model="form.roundDuration"
                  class="mb-2"
                  error=""
                  type="number"
                  min="10"
                  max="120"
                  id="roundLengthInput"
                  label="Round length (seconds)"
                />
                <div v-if="error.length" class="alert alert-danger" role="alert">{{ error }}</div>
                <div class="form-group d-flex justify-content-around mt-2">
                  <Submit :disabled="disabled">Create game</Submit>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="col-md-6 col-sm-6 h-100 ">
          <div class="h-100 d-flex flex-column">
            <h4 class="text-center pb-2">Select Categories</h4>
            <div class="overflow-auto border border-2 rounded p-2">
              <div class="mb-1" v-for="category in categories" :key="category">
                <input
                  class="form-check-input me-1"
                  type="checkbox"
                  :id="'category_id_' + category"
                  name="vehicle1"
                  v-model="form.categoryIds"
                  :value="category.id"
                />
                <label class="form-check-label d-inline" for="vehicle1">{{
                  category.name
                }}</label>
              </div>
            </div>
          </div>
        </div>


      </div>
    </form>
  </div>
  <teleport to="body">
    <LoadingGameModal v-if="createdGame" v-model:game="createdGame" isCreator />
  </teleport>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";
import { createGame, fetchCategories } from "@/api/Lobby";
import { Category, Game, GameSlotUpdateNotification } from "@/typings";
import LoadingGameModal from "@/components/Modal/LoadingGameModal.vue";
import * as ws from "@/api/Lobby/subscriptions";

const errors = {
  noCategorySelected: "At least one category must be picked",
  default: "Something went wrong! Try again later"
}

export default defineComponent({
  name: "NewGame",
  components: {
    Input,
    Submit,
    LoadingGameModal,
  },
  created() {
    this.loadCategories();
    this.subscribeToGames();

  },
  data() {
    return {
      form: {
        proposerType: "AI",
        categoryIds: [] as number[],
        guessersCount: 1,
        imagesCount: 1,
        roundDuration: 30,
      },
      createdGame: null as Game | null,
      error: "",
      disabled: false,
      categories: [] as Category[],
    };
  },
  methods: {
    validate() {
      if (!this.form.categoryIds.length) {
        this.error = errors.noCategorySelected;
        return false;
      }

      this.error = "";
      return true;
    },
    handleSubmit() {
      if (!this.validate()) return;

      this.disabled = true;
      createGame(this.form)
        .then((game) => (this.createdGame = game))
        .finally(() => (this.disabled = false));
    },
    loadCategories() {
      fetchCategories().then((categories) => {
        this.form.categoryIds = categories.map(c => c.id);
        this.categories = categories;
      });
    },
    updateGameRoom(data: GameSlotUpdateNotification) {
      if (!this.createdGame) {
        return;
      }
      if (data.gameId !== this.createdGame.id) {
        return;
      }
      this.createdGame.occupiedSlotsCount = data.occupiedSlotsCount;
    },
    startGame() {
      this.$router.push({ name: "Game" });
    },
    subscribeToGames() {
      ws.subscribeToGameRoomsUpdates(this.updateGameRoom);
      ws.subscribeToGameStart(this.startGame);
    },
  },
});
</script>
