<template>
  <div class="d-flex vh-100 justify-content-center align-items-center">
    <form @submit="handleSubmit">
      <div class="form-group mb-2 has-validation">
        <label for="loginUsername">Username</label>
        <input v-model="form.userName" type="text" :class="{ 'form-control': true, 'is-invalid': validatorErrors.userName.length }" id="loginUsername" placeholder="Enter username">
        <div class="invalid-feedback">{{ validatorErrors.userName }}</div>
      </div>
      <div class="form-group mb-2">
        <label for="loginPassword">Password</label>
        <input v-model="form.password" type="password" :class="{ 'form-control': true, 'is-invalid': validatorErrors.password.length }" id="loginPassword" placeholder="Password">
        <div class="invalid-feedback">{{ validatorErrors.password }}</div>
      </div>
      <div v-show="responseError" class="alert alert-danger" role="alert">
        {{ responseError }}
      </div>
      <div class="form-group d-flex justify-content-around">
        <router-link type="submit" class="btn btn-secondary" to="/">Go back</router-link>
        <button :disabled="submitting" type="submit" class="btn btn-primary">Login</button>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

import auth from "@/api/Auth";
import { setAuthUser } from "@/utils/auth";

const ERROR_LABEL_BY_CODE: {[index: string]:any} = {
  401: "Username or password is wrong.",
  default: "Something went wrong. Try again later"
};

const VALIDATION_ERRORS = {
  emptyPassword: "Password can't be empty",
  emptyUsername: "Username can't be empty"
}

export default defineComponent({
  name: "Login",
  data() {
    return {
      form: {
        userName: "",
        password: "",
      },
      validatorErrors: {
        userName: "",
        password: ""
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
      auth.authUser(userName, password)
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

      return !hasErrors;
    },
  },
  watch: {
    'form.userName': function() {
      if (this.formHasErrors) {
        this.validatorErrors.userName = "";
        this.responseError = "";
      }
    },
    'form.password': function() {
      if (this.formHasErrors) {
        this.validatorErrors.password = "";
        this.responseError = "";
      }
    },
  }
});
</script>

<style scoped></style>