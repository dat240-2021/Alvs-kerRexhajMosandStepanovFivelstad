<template>
  <div class="d-flex justify-content-end">
    <button class="btn btn-primary m-2" @click="handleLogout">Logout</button>
  </div>
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
                <td>{{ game.occupiedSlotsCount }} / {{ game.settings.guessersCount }}</td>
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
  <teleport to="body">
    <LoadingGameModal v-if="joinedGame" v-model:game="joinedGame" />
  </teleport>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { fetchWaitingRooms, joinGameRoom, startGame } from "@/api/BackendGame";
import * as ws from "@/api/BackendGame/subscriptions";
import {
  Game,
  GameSlotUpdateNotification
} from "@/typings";
import { logoutUser } from "@/utils/auth";
import LoadingGameModal from "@/components/Modal/LoadingGameModal.vue";

export default defineComponent({
  name: "Home",
  components: {
    LoadingGameModal
  },
  created() {
    this.fetchGameRooms();
    this.subscribeToGames();
  },
  unmounted() {
    ws.unsubscribeFromGameRoomsUpdates(this.updateGameRoom);
    ws.unsubscribeFromGameRoomsCreation(this.storeGameRoom);
    ws.unsubscribeFromGameRoomsDeletes(this.deleteGameRoom);
    ws.unsubscribeFromGameStart(this.startGame);
  },
  data() {
    return {
      leaderBoard: [],
      gameRooms: [] as Game[],
      joinedGame: null as Game | null,
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
    deleteGameRoom(id: string) {
      this.gameRooms = this.gameRooms.filter(game => game.id !== id);

      if (this.joinedGame?.id === id) {
        this.joinedGame = null;
      }
    },
    subscribeToGames() {
      ws.subscribeToGameRoomsUpdates(this.updateGameRoom);
      ws.subscribeToGameRoomsCreation(this.storeGameRoom);
      ws.subscribeToGameRoomsDeletes(this.deleteGameRoom);
      ws.subscribeToGameStart(this.startGame);
    },
    joinGame(id: string) {
      joinGameRoom(id).then(() => {
        const game = this.gameRooms.find(g => g.id === id);
        if (!game) {
          throw new Error("Game was not found!");
        }
        this.joinedGame = game;
      });
    },
    startGame() {
      this.$router.push({ name: "InGame" });
    },
    handleLogout() {
      logoutUser().then(() => this.$router.push({ name: "Index" }));
    }
  },
  computed: {
    visibleGameRooms(): Game[] {
      return this.gameRooms.filter((game: Game) => game.occupiedSlotsCount < game.settings.guessersCount);
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
