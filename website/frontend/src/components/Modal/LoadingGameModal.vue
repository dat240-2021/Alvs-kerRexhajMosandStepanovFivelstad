<template>
  <div class="modal d-block" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Joining</h5>
        </div>
        <div class="modal-body">
          <p>Waiting for players to join the game.</p>
          <div class="progress">
            <div class="progress-bar" role="progressbar" :style="{ width: `${progressPercentage}%` }" aria-valuenow="30">
              {{ game.occupiedSlotsCount }} / {{ game.settings.playersCount }}
            </div>
          </div>
          <div v-if="areSlotsFull" class="d-flex justify-content-center pt-4">
            <div class="spinner-border" role="status" />
          </div>
        </div>
        <div class="modal-footer">
          <button :disabled="leavingDisabled" type="button" class="btn btn-secondary" @click="handleGameLeave">Leave</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue'
import { Game } from "@/typings";
import { logoutUser } from "@/utils/auth";
import { leaveGameRoom } from "@/api/BackendGame";

export default defineComponent({
  name: "LoadingGameModal",
  data() {
    return {
      leavingDisabled: false,
    };
  },
  props: {
    game: {
      type: Object as PropType<Game>,
      required: true,
      default: {} as Game
    },
  },
  methods: {
    handleGameLeave() {
      this.leavingDisabled = true;
      leaveGameRoom(this.game.id).then(() => this.$emit("update:game", null));
    }
  },
  emits: ['update:game'],
  computed: {
    progressPercentage(): number {
      return Math.floor((this.game.occupiedSlotsCount / this.game.settings.playersCount) * 100);
    },
    areSlotsFull(): boolean {
      return this.game.occupiedSlotsCount === this.game.settings.playersCount;
    },
  }
});
</script>

<style scoped>

</style>