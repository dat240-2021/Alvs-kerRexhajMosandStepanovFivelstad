<template>
  <div class="container vh-100 py-5">
    <form class="h-100" @submit.prevent="handleSubmit">
      <div class="row h-75 justify-content-around">
        <div class="col-md-4 col-sm-6 pb-4 pb-sm-0 h-100">
          <div class="categories h-100 d-flex flex-column p-4">
            <div class="text-center pb-2">Select Categories</div>
            <div class="overflow-scroll">
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
        <div class="col-sm-4">
          <div class="row">
            <div class="col">
              <label for="proposerSelection">Proposer type</label>
              <select v-model="form.proposerType" id="proposerSelection" class="form-select mb-2" aria-label="Proposer">
                <option>AI</option>
                <option>Player</option>
              </select>
              <Input
                :model-value="form.playersCount"
                class="mb-2"
                error=""
                type="number"
                min="1"
                max="6"
                id="playersCountInput"
                label="Number of guessers"
              />
              <Input
                :model-value="form.imagesCount"
                class="mb-2"
                error=""
                type="number"
                min="1"
                max="10"
                id="picturesCountInput"
                label="Number of pictures"
              />
              <Input
                :model-value="form.roundDuration"
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
                <Submit :disabled="disabled">Start game</Submit>
              </div>
            </div>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";
import { createGame, fetchCategories } from "@/api/BackendGame";
import { Category } from "@/typings";

const errors = {
  noCategorySelected: "At least one category must be picked",
  default: "Something went wrong! Try again later"
}

export default defineComponent({
  name: "NewGame",
  components: {
    Input,
    Submit,
  },
  created() {
    this.loadCategories();
  },
  data() {
    return {
      form: {
        proposerType: "AI",
        categoryIds: [] as number[],
        playersCount: 1,
        imagesCount: 1,
        roundDuration: 30,
      },
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
        .then((id) => {
          this.$router.push({ name: "Game", params: { id } });
        })
        .catch(() => this.error = errors.default)
        .finally(() => this.disabled = false);
    },
    loadCategories() {
      fetchCategories().then((categories) => {
        this.form.categoryIds = categories.map(c => c.id);
        this.categories = categories;
      });
    },
  },
});
</script>

<style scoped>
.categories {
  background: #e7efbd;
}
</style>
