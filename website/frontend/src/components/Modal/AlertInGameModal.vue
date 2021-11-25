<template>
  <div class="modal d-block" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Round finished!</h5>
        </div>
        <div class="modal-body">
          <div class="alert" :class="alertType">{{ this.alertMessage }}</div>
          <div v-if="imageSlices?.length" class="d-flex w-100">
            <div class="position-relative mx-auto" style="width: 400px; height: 400px">
              <img
                v-for="im in imageSlices"
                :key="im.id"
                :src="'data:image/png;base64,' + im.imageData"
                :id="im.id"
                style="width: 100%; height: 100%; object-fit: contain"
                class="position-absolute top-0 start-0"
              />
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button  type="button" class="btn btn-secondary" @click="closeModal">Close ({{ autoCloseIn }})</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue'
import { ImageSlice } from "@/typings";

export default defineComponent({
  name: "AlertInGameModal",
  created() {
    this.startTimer();
  },
  data() {
    return {
      autoCloseIn: 5
    }
  },
  methods: {
    startTimer() {
      setTimeout(() => {
        this.autoCloseIn--;
        if (this.autoCloseIn) {
          this.startTimer();
          return;
        }
        this.closeModal();
      }, 1000);
    },
    closeModal() {
      this.$emit("update:alert", null);
    },
  },
  emits: ['update:alert'],
  props: {
    alertMessage: {
      type: String,
      required: true
    },
    alertType: {
      type: Boolean,
      required: true,
    },
    imageSlices: {
      type: Object as PropType<ImageSlice[]>,
      required: false,
      default: [] as ImageSlice[]
    },
  },
});
</script>