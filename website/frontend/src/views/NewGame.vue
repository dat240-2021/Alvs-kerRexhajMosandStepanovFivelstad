<template>
  <div class="container vh-100 py-5">
    <div class="row h-75">
      <div class="col">
        <div class="row">
          <div class="col">
            <form>
              <Input
                class="mb-2"
                error=""
                type="number"
                model-value="1"
                id="picturesCountInput"
                label="Number of pictures"
              />
              <Input
                class="mb-2"
                error=""
                type="number"
                model-value="1"
                id="playersCountInput"
                label="Number of players"
              />
              <Input
                class="mb-2"
                error=""
                type="number"
                model-value="1"
                id="rounndLengthInput"
                label="Round length in minutes"
              />
              <div class="form-group d-flex justify-content-around mt-2">
                <Submit>Start game</Submit>
              </div>
            </form>
          </div>
        </div>
      </div>
      <div class="col">
        <div class="categories h-100 d-flex flex-column">
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

export default {
  name: "NewGame",
  components: {
    Input,
    Submit,
  },
};
</script>

<style scoped>
.categories {
  background: #e7efbd;
}
</style>
