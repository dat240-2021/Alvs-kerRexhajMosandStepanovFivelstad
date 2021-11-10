<template>
  some game here
  <button class="btn btn-primary" @click="handleLeaveGame">Leave game</button>
</template>

<script>
import { leaveGameRoom} from "@/api/BackendGame";

export default {
  name: "Game",
  methods: {
    handleLeaveGame() {
      const { id } = this.$route.params;
      leaveGameRoom(id).then(() => {
        this.$router.push({ name: "Home" })
      });
    }
  }
};
</script>

<style scoped>

</style>