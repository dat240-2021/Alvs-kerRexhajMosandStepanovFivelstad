<template>
  <div class="modal d-block" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Game is about to start</h5>
        </div>
        <div class="modal-body">
          <p>Waiting for players to join the game.</p>
          <div class="progress">
            <div class="progress-bar" role="progressbar" :style="{ width: `${progressPercentage}%` }" aria-valuenow="30">
              {{ game.occupiedSlotsCount }} / {{ game.settings.guessersCount }}
            </div>
          </div>
          <div class="d-flex justify-content-center pt-4">
            <div v-if="!isCreator && areSlotsFull" class="spinner-border" role="status" />
          </div>
        </div>
        <div class="modal-footer">
          <button :disabled="leavingDisabled" type="button" class="btn btn-secondary" @click="handleGameLeave">Leave</button>
          <button v-if="isCreator" :disabled="!game.occupiedSlotsCount" class="btn btn-primary" @click="handleStartGame">Start game</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue'
import { Game } from "@/typings";
import { leaveGameRoom, startGame } from "@/api/BackendGame";

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
    isCreator: {
      type: Boolean,
      default: false
    },
  },
  methods: {
    handleGameLeave() {
      this.leavingDisabled = true;
      leaveGameRoom(this.game.id).then(() => this.$emit("update:game", null));
    },
    handleStartGame() {
      //startGame(this.game.id).then(() => this.$router.push({ name: "InGame" }));
      startGame(this.game.id);
      this.$router.push({ name: "InGame" });
    }
  },
  emits: ['update:game'],
  computed: {
    progressPercentage(): number {
      return Math.floor((this.game.occupiedSlotsCount / this.game.settings.guessersCount) * 100);
    },
    areSlotsFull(): boolean {
      return this.game.occupiedSlotsCount === this.game.settings.guessersCount;
    },
  }
});
</script>

<style scoped>

</style>