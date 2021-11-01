<template>
  <div class="d-flex vh-100 justify-content-center align-items-center">
    <form @submit.prevent="handleSubmit">
      <Input
        v-model="form.userName"
        id="loginUsername"
        label="Username"
        placeholder="Username"
        :error="validatorErrors.userName"
        @update:error="validatorErrors.userName = $event"
      />
      <Input
        v-model="form.password"
        id="loginPassword"
        label="Password"
        placeholder="Password"
        type="password"
        :error="validatorErrors.password"
        @update:error="validatorErrors.password = $event"
      />
      <div v-show="responseError" class="alert alert-danger" role="alert">
        {{ responseError }}
      </div>
      <div class="form-group d-flex justify-content-around mt-2">
        <Submit :has-go-back-button="true" :disabled="submitting">Login</Submit>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";
import auth from "@/api/Auth";
import { setAuthUser } from "@/utils/auth";
import validators from "@/utils/validators";

const ERROR_LABEL_BY_CODE: { [index: string]: any } = {
  401: "Username or password is wrong.",
  default: "Something went wrong. Try again later",
};

const VALIDATION_ERRORS = {
  emptyPassword: "Password can't be empty",
  emptyUsername: "Username can't be empty",
};

export default defineComponent({
  name: "Login",
  components: {
    Input,
    Submit
  },
  data() {
    return {
      form: {
        userName: "",
        password: "",
      },
      validatorErrors: {
        userName: "",
        password: "",
      },
      responseError: "",
      submitting: false,
    };
  },
  methods: {
    handleSubmit() {
      if (!this.validateForm()) return;

      this.submitting = true;
      const { userName, password } = this.form;

      auth
        .authUser(userName, password)
        .then(setAuthUser)
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
      const { userName, password } = this.form;

      if (validators.isEmpty(userName)) {
        hasErrors = true;
        this.validatorErrors.userName = VALIDATION_ERRORS.emptyUsername;
      }
      if (validators.isEmpty(password)) {
        hasErrors = true;
        this.validatorErrors.password = VALIDATION_ERRORS.emptyPassword;
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
  },
});
</script>

<style scoped></style>
