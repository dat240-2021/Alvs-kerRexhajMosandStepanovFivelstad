<template>
  <div class="d-flex vh-100 justify-content-center align-items-center">
    <form @submit="handleSubmit">
      <div class="form-group mb-2">
        <label for="loginInputUsername">Username</label>
        <input v-model="form.userName" type="text" :class="{ 'form-control': true, 'is-invalid': validatorErrors.userName.length }" id="loginInputUsername" placeholder="Username">
        <div class="invalid-feedback">{{ validatorErrors.userName }}</div>
      </div>
      <div class="form-group mb-2">
        <label for="loginInputPassword">Password</label>
        <input v-model="form.password" type="password" :class="{ 'form-control': true, 'is-invalid': validatorErrors.password.length }" id="loginInputPassword" placeholder="Password">
        <div class="invalid-feedback">{{ validatorErrors.password }}</div>
      </div>
      <div class="form-group mb-2">
        <label for="loginInputPassword2">Repeat password</label>
        <input v-model="form.passwordRepeat" type="password" :class="{ 'form-control': true, 'is-invalid': validatorErrors.passwordRepeat.length }" id="loginInputPassword2" placeholder="Password">
        <div class="invalid-feedback">{{ validatorErrors.passwordRepeat }}</div>
      </div>
      <div v-show="responseError" class="alert alert-danger" role="alert">
        {{ responseError }}
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
import validators from "@/utils/validators";

const ERROR_LABEL_BY_CODE: {[index: string]:any} = {
  422: "Username is already in use",
  default: "Something went wrong. Try again later"
};

const VALIDATION_ERRORS = {
  passwordMismatch: "Passwords must match",
  weakPassword: "Password must have length 8, be alphanumeric with special chars and have both letter cases.",
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
        .then(() => this.$router.push({ name: "Home" }))
        .catch((e) => {
          const status = e.response.status as string;
          this.responseError = ERROR_LABEL_BY_CODE[status] ?? ERROR_LABEL_BY_CODE.default;
        })
        .finally(() => this.submitting = false);
    },
    validateForm() {
      let hasErrors = false;

      const { userName, password, passwordRepeat } = this.form;

      if (validators.isEmpty(userName)) {
        hasErrors = true;
        this.validatorErrors.userName = VALIDATION_ERRORS.emptyUsername;
      }
      if (validators.passwordValidators.some(validator => !validator(password))) {
        hasErrors = true;
        this.validatorErrors.password = VALIDATION_ERRORS.weakPassword;
      }

      if (!validators.areEqual(password, passwordRepeat)) {
        hasErrors = true;
        this.validatorErrors.passwordRepeat = VALIDATION_ERRORS.passwordMismatch;
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
    'form.passwordRepeat': function() {
      if (this.formHasErrors) {
        this.validatorErrors.passwordRepeat = "";
        this.responseError = "";
      }
    },
  }
});
</script>

<style scoped>

</style>