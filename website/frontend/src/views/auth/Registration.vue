<template>
  <div class="d-flex vh-100 justify-content-center align-items-center">
    <form @submit.prevent="handleSubmit">
      <Input
        v-model="form.userName"
        id="registerUsername"
        label="Username"
        placeholder="Username"
        :error="validatorErrors.userName"
        @update:error="validatorErrors.userName = $event"
      />
      <Input
        v-model="form.password"
        id="registerPassword"
        label="Password"
        placeholder="Password"
        type="password"
        :error="validatorErrors.password"
        @update:error="validatorErrors.password = $event"
      />
      <Input
        v-model="form.passwordRepeat"
        id="registerRepeatPassword"
        label="Password"
        placeholder="Password"
        type="password"
        :error="validatorErrors.passwordRepeat"
        @update:error="validatorErrors.passwordRepeat = $event"
      />
      <div v-show="responseError" class="alert alert-danger" role="alert">
        {{ responseError }}
      </div>
      <div class="form-group d-flex justify-content-around mt-2">
        <Submit :has-go-back-button="true" :disabled="submitting">Register</Submit>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import auth from "@/api/Auth";
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";
import { setCurrentUser } from "@/utils/auth";

import validators from "@/utils/validators";

const ERROR_LABEL_BY_CODE: { [index: string]: any } = {
  422: "Username is already in use",
  default: "Something went wrong. Try again later",
};

const VALIDATION_ERRORS = {
  passwordMismatch: "Passwords must match",
  weakPassword:
    "Password be at least 6 characters long, contain at least one letter and number",
  emptyUsername: "Username can't be empty",
};

export default defineComponent({
  name: "Registration",
  components: {
    Input,
    Submit
  },
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
      submitting: false,
    };
  },
  methods: {
    handleSubmit(e: Event) {
      if (!this.validateForm()) return;

      this.submitting = true;

      const { userName, password } = this.form;

      auth
        .registrateUser(userName, password)
        .then(() => auth.authUser(userName, password))
        .then(setCurrentUser)
        .then(() => this.$router.push({ name: "Home" }))
        .catch((e) => {
          const status = e.response.status as string;
          this.responseError =
            ERROR_LABEL_BY_CODE[status] ?? ERROR_LABEL_BY_CODE.default;
        })
        .finally(() => (this.submitting = false));
    },
    validateForm() {
      let hasErrors = false;

      const { userName, password, passwordRepeat } = this.form;

      if (validators.isEmpty(userName)) {
        hasErrors = true;
        this.validatorErrors.userName = VALIDATION_ERRORS.emptyUsername;
      }
      if (
        validators.passwordValidators.some((validator) => !validator(password))
      ) {
        hasErrors = true;
        this.validatorErrors.password = VALIDATION_ERRORS.weakPassword;
      }

      if (!validators.areEqual(password, passwordRepeat)) {
        hasErrors = true;
        this.validatorErrors.passwordRepeat =
          VALIDATION_ERRORS.passwordMismatch;
      }

      return !hasErrors;
    },
  },
  watch: {
    "form.userName": function () {
      if (this.responseError) {
        this.responseError = "";
      }
    },
    "form.password": function () {
      if (this.responseError) {
        this.responseError = "";
      }
    },
    "form.passwordRepeat": function () {
      if (this.responseError) {
        this.responseError = "";
      }
    },
  },
});
</script>

<style scoped></style>
