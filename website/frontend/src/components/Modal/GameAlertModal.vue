<template>
  <div class="modal d-block" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">{{ title }}</h5>
        </div>
        <div class="modal-body">
          <p>{{ text }}</p>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary">Leave</button>
          <button class="btn btn-primary">Start game</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue'

export default defineComponent({
  name: "GameAlertModal",
  props: {
    title: {
      type: String,
      required: true,
    },
    text: {
      type: String,
      required: true,
    },
    visible: {
      type: Boolean,
      default: true,
    }
  },
  mounted() {
    var self = this;

    setTimeout(function () { 
        self.visible = false; 
    }, 2000);
  }
});
</script>

<style scoped>

</style>