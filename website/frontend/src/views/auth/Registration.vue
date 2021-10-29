<template>
  <div class="d-flex vh-100 justify-content-center align-items-center">
    <form @submit="handleSubmit">
      <div class="form-group mb-2">
        <label for="loginInputUsername">Username</label>
        <input v-model="form.userName" type="text" class="form-control" id="loginInputUsername" placeholder="Enter username">
      </div>
      <div class="form-group mb-2">
        <label for="loginInputPassword">Password</label>
        <input v-model="form.password" type="password" class="form-control" id="loginInputPassword" placeholder="Password">
      </div>
      <div class="form-group mb-2">
        <label for="loginInputPassword2">Password</label>
        <input type="password" class="form-control" id="loginInputPassword2" placeholder="Repeat password">
      </div>
      <div class="form-group d-flex  justify-content-around">
        <router-link type="submit" class="btn btn-secondary" to="/">Go back</router-link>
        <button type="submit" class="btn btn-primary" :disabled="submitting">Register</button>
      </div>
    </form>
  </div>
</template>

<script>
import auth from "@/api/Auth";
import { setAuthUser } from "@/utils";

export default {
  name: "Registration",
  data() {
    return {
      form: {
        userName: "",
        password: "",
        passwordRepeat: "",
      },
      errors: {
        userName: "",
        password: "",
        passwordRepeat: "",
      },
      submitting: false
    }
  },
  methods: {
    handleSubmit(e) {
      e.preventDefault();
      this.submitting = true;
      const { userName, password } = this.form;
      auth.registrateUser(userName, password)
        .then(setAuthUser)
        .then(() => this.$router.push({ name: "home" }))
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