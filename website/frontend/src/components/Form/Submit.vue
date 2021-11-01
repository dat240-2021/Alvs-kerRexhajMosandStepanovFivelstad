<template>
  <button v-if="hasGoBackButton" type="button" class="btn btn-secondary" @click="$router.go(-1)"
  >Go back</button
  >
  <button :disabled="disabled" type="submit" class="btn btn-primary">
    <slot></slot>
  </button>
</template>

<script>
export default {
  name: "Submit",
  props: {
    disabled: {
      type: Boolean,
      required: false,
      default: false
    },
    hasGoBackButton: {
      type: Boolean,
      required: false,
      default: false
    },
  }
};
</script>

<style scoped>

</style>