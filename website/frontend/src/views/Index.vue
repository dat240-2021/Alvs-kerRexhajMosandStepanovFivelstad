<template>
  <div class="vh-100">
    <div class="h-25 pt-5 text-center">
      <h1>WEBSITE NAME</h1>
    </div>
    <div class="d-flex justify-content-start flex-column align-items-center h-75">
      <router-link class="btn btn-primary mb-5" to="/login">Login</router-link>
      <router-link class="btn btn-primary" to="/registration">Register</router-link>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Index",
});
</script>
