<template>
  <div class="vh-100 mx-2">
    <div class="h-20 pt-3 text-center">
      <h1>Image Quizzer</h1>
    </div>
    <div class="d-flex justify-content-center">
      <img
        :src="require('@/assets/horse.png')"
        class="img-fluid img-thumbnail rounded"
      />
    </div>
    <div class="d-grid gap-3 d-md-flex justify-content-center h-10 mt-3">
      <router-link class="btn btn-primary" to="/login">Login</router-link>
      <router-link class="btn btn-primary" to="/registration"
        >Register</router-link
      >
    </div>
    <div class="h-20 pt-5 text-center">
      <h6>
        Play a fun game alone, with your friends or other likeminded souls.
      </h6>
      <h6>Guess what the image is, based on the tiles presented to you.</h6>
      <h6>
        Each wrong guess will reveal a new tile, until you or others guess
        correctly.
      </h6>
      <h6>
        The game supports single player mode and multi-player
        mode.
      </h6>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Index",
});
</script>
