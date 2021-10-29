<template>
  <div class="d-flex vh-100 justify-content-center align-items-center">
    <form @submit="handleSubmit">
      <div class="form-group mb-2">
        <label for="loginUsername">Username</label>
        <input v-model="form.userName" type="text" class="form-control" id="loginUsername" placeholder="Enter username">
      </div>
      <div class="form-group mb-2">
        <label for="loginPassword">Password</label>
        <input v-model="form.password" type="password" class="form-control" id="loginPassword" placeholder="Password">
      </div>
      <div class="form-group d-flex justify-content-around">
        <router-link type="submit" class="btn btn-secondary" to="/">Go back</router-link>
        <button :disabled="submitting" type="submit" class="btn btn-primary">Login</button>
      </div>
    </form>
  </div>
</template>

<script>
import auth from "@/api/Auth";
import { setAuthUser } from "@/utils";

export default {
  name: "Login",
  data() {
    return {
      form: {
        userName: "",
        password: "",
      },
      errors: {
        userName: "",
        password: ""
      },
      submitting: false
    }
  },
  methods: {
    handleSubmit(e) {
      e.preventDefault();
      this.submitting = true;

      const { userName, password } = this.form;
      auth.authUser(userName, password)
        .then(setAuthUser)
        .catch((e) => {
          //error handling
        })
        .finally(() => this.submitting = false);
    }
  }
};
</script>

<style scoped>

</style>