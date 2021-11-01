<template>
  <div class="form-group has-validation">
    <label v-if="label" :for="id">{{ label }}</label>
    <input
      :value="this.modelValue"
      @input="handleInputChange"
      :type="this.type"
      :class="{
        'is-invalid': this.error !== ''
      }"
      class="form-control"
      :id="id"
      :placeholder="this.placeholder"
     />
    <div class="invalid-feedback">{{ error }}</div>
  </div>
</template>

<script>
export default {
  name: "Input",
  methods: {
    handleInputChange(e) {
      this.$emit('update:modelValue', e.target.value);

      if(this.error) {
        this.$emit('update:error', '');
      }
    }
  },
  props: {
    id: {
      required: true,
    },
    class: {
      required: false,
      type: String,
      default: "",
    },
    type: {
      required: false,
      type: String,
      default: "text",
    },
    label: {
      required: false,
      type: String,
    },
    placeholder: {
      required: false,
      type: String,
    },
    modelValue: {
      required: true,
      type: String,
    },
    error: {
      required: true,
      type: String
    }
  },
  emits: ['update:modelValue', 'update:error'],
};
</script>

<style scoped></style>
