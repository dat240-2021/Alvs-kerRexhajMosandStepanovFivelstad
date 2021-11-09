<template>
  <div class="container vh-100 py-5">
    <div class="row mt-5 h-25 justify-content-center">
      <div class="col-8 d-flex">
        <div class="leaderboard border border-secondary flex-grow-1">
          Leaderboard
        </div>
      </div>
    </div>
    <div class="row mt-3 justify-content-center">
      <div class="col d-flex justify-content-center">
        <router-link class="btn btn-primary" to="/game">
          Create game
        </router-link>
      </div>
    </div>
    <div class="row h-50 mt-5 justify-content-center">
      <div class="col text-center d-flex flex-column h-100">
        <h4>Available games</h4>
        <div
          class="
            games-list
            border border-secondary
            d-flex
            h-100
            overflow-scroll
          "
        >
          <table class="table">
            <thead>
              <tr>
                <th scope="col">Game Type</th>
                <th scope="col">Capacity</th>
                <th scope="col">Join</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="game in visibleGameRooms" :key="game.id">
                <td>Some type here</td>
                <td>{{ game.occupiedSlotsCount }} / {{ game.settings.playersCount }}</td>
                <td>
                  <button class="btn" @click="joinGame(game.id)">
                    <i class="bi bi-box-arrow-in-right"></i>
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { fetchWaitingRooms, subscribeToGameRoomsCreation, subscribeToGameRoomsUpdates } from "@/api/BackendGame";
import { joinGameRoom } from "@/api/BackendGame";
import {
  Game,
  GameSlotUpdateNotification
} from "@/typings";

export default defineComponent({
  name: "Home",
  created() {
    this.fetchGameRooms();
    this.subscribeToGames();
  },
  data() {
    return {
      leaderBoard: [],
      gameRooms: [] as Game[],
    };
  },
  methods: {
    fetchGameRooms() {
      fetchWaitingRooms()
        .then(rooms => this.gameRooms = rooms);
    },
    updateGameRoom(data: GameSlotUpdateNotification) {
      const game = this.gameRooms.find((g: Game) => g.id === data.gameId);
      if (!game) {
        throw new Error(`Game with id ${data.gameId} is not found`);
      }
      game.occupiedSlotsCount = data.occupiedSlotsCount;
    },
    storeGameRoom(game: Game) {
      this.gameRooms = [...this.gameRooms, game];
    },
    subscribeToGames() {
      subscribeToGameRoomsUpdates(this.updateGameRoom);
      subscribeToGameRoomsCreation(this.storeGameRoom);
    },
    joinGame(id: string) {
      joinGameRoom(id)
        .then(() => this.$router.push({ name: "Game", params: { id } }));
    },
  },
  computed: {
    visibleGameRooms(): Game[] {
      return this.gameRooms.filter((game: Game) => game.occupiedSlotsCount < game.settings.playersCount);
    }
  },
});
</script>

<style scoped>
.leaderboard,
.games-list {
  background: #e7efbd;
}
</style>
