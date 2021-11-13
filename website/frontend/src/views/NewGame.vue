<template>
  <div class="container vh-100 py-5">
    <div class="row h-75 justify-content-around">
      <div class="col-4">
        <div class="row">
          <div class="col">
            <form @submit.prevent="handleSubmit">
              <Input
                class="mb-2"
                error=""
                type="number"
                model-value="1"
                min="1"
                max="10"
                id="picturesCountInput"
                label="Number of pictures"
              />
              <Input
                class="mb-2"
                error=""
                type="number"
                model-value="1"
                min="1"
                max="6"
                id="playersCountInput"
                label="Number of players"
              />
              <Input
                class="mb-2"
                error=""
                type="number"
                model-value="1"
                min="10"
                max="120"
                id="rounndLengthInput"
                label="Round length in seconds"
              />
              <div class="form-group d-flex justify-content-around mt-2">
                <Submit>Start game</Submit>
              </div>
            </form>
          </div>
        </div>
      </div>
      <div class="col-md-4 col-6">
        <div class="categories h-100 d-flex flex-column p-4">
          <div class="text-center pb-2">select categories</div>
          <div class="mx-5">
            <div>
              <label for="vehicle1"> I have a bike</label>
              <input
                type="checkbox"
                id="vehicle1"
                name="vehicle1"
                value="Bike"
              />
            </div>
            <div>
              <label for="vehicle1"> I have a bike</label>
              <input
                type="checkbox"
                id="vehicle1"
                name="vehicle1"
                value="Bike"
              />
            </div>
            <div>
              <label for="vehicle1"> I have a bike</label>
              <input
                type="checkbox"
                id="vehicle1"
                name="vehicle1"
                value="Bike"
              />
            </div>
            <div>
              <label for="vehicle1"> I have a bike</label>
              <input
                type="checkbox"
                id="vehicle1"
                name="vehicle1"
                value="Bike"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";
import { createGame, fetchCategories } from "@/api/BackendGame";

export default {
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
      categories: [],
    };
  },
  methods: {
    handleSubmit() {
      const settings = {
        "PlayersCount": 2,
        "ImagesCount": 1,
        "Duration": 30,
        // "Categories": ["Animals", "Cars"]
      };

      createGame(settings).then((id) => {
        this.$router.push({ name: "Game", params: { id } });
      });
    },
    loadCategories() {
      fetchCategories();
    }
  },
};
</script>

<style scoped>
.categories {
  background: #e7efbd;
}
</style>
