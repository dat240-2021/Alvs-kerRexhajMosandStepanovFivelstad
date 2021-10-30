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
        <button type="submit" class="btn btn-primary" :disabled="submitting">Login</button>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import auth from "@/api/Auth";
import { setAuthUser } from "@/utils/auth";
import { defineComponent } from "vue";

const ERROR_LABEL_BY_CODE: {[index: string]:any} = {
  default: "Something went wrong. Try again later"
};

const VALIDATION_ERRORS = {
  passwordMissmatch: "Passwords must match",
  emptyPassword: "Password can't be empty",
  emptyUsername: "Username can't be empty"
}

export default defineComponent({
  name: "Registration",
  data() {
    return {
      form: {
        userName: "",
        password: "",
        passwordRepeat: "",
      },
      validatorErrors: {
        userName: "",
        password: "",
        passwordRepeat: "",
      },
      responseError: "",
      submitting: false
    }
  },
  computed: {
    formHasErrors(): boolean {
      return this.responseError.length !== 0 || Object.values(this.validatorErrors).some(v => v !== "");
    }
  },
  methods: {
    handleSubmit(e: Event) {
      e.preventDefault();
      if (!this.validateForm()) return;

      this.submitting = true;

      const { userName, password } = this.form;
      auth.registrateUser(userName, password)
        .then(setAuthUser)
        .then(() => this.$router.push({ name: "home" }))
        .catch((e) => {
          const status = e.response.status as string;
          this.responseError = ERROR_LABEL_BY_CODE[status] ?? ERROR_LABEL_BY_CODE.default;
        })
        .finally(() => this.submitting = false);
    },
    validateForm() {
      let hasErrors = false;

      if (!this.form.userName.trim().length) {
        hasErrors = true;
        this.validatorErrors.userName = VALIDATION_ERRORS.emptyUsername;
      }
      if (!this.form.password.trim().length) {
        hasErrors = true;
        this.validatorErrors.password = VALIDATION_ERRORS.emptyPassword;
      }
      if (this.form.password !== this.form.passwordRepeat) {
        hasErrors = true;
        this.validatorErrors.passwordRepeat = VALIDATION_ERRORS.passwordMissmatch;
      }

      return !hasErrors;
    },
  }
});
</script>

<style scoped>

</style>