<template>
  <div class="container vh-100 py-2">
    <div class="d-flex justify-content-end">
      <button class="btn btn-primary mx-2 mt-2" @click="handleLogout">
        Logout
      </button>
    </div>
    <div class="row justify-content-center overflow-hidden" style="height: 40%">
      <h3 class="w-100 text-center" style="height: 10%">Leaderboard Top 10</h3>
      <div
        class="col-8 d-flex overflow-auto border border-2 rounded"
        style="height: 80%"
      >
        <div class="table-responsive w-100">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>Player Name</th>
                <th class="text-end">Score</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="p in sortedPlayers()" :key="p.playername">
                <td>{{ p.playername }}</td>
                <td class="text-end">{{ p.score }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <div class="row mt-2 justify-content-center">
      <div class="col d-flex justify-content-between">
        <router-link class="btn btn-primary" to="/uploadImages">
          Upload Images
        </router-link>
        <h4>Available games</h4>
        <router-link class="btn btn-primary" to="/game">
          Create game
        </router-link>
      </div>
    </div>
    <div class="row mt-2 justify-content-center" style="height: 45%">
      <div class="col text-center d-flex flex-column h-100">
        <div class="border border-2 rounded d-flex h-100 overflow-auto">
          <div class="table-responsive w-100 mx-2">
            <table class="table table-hover">
              <thead>
                <tr>
                  <th scope="col" class="text-start">Game Type</th>
                  <th scope="col" class="text-start">Capacity</th>
                  <th scope="col" class="text-end">Join Game</th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="game in visibleGameRooms"
                  :key="game.id"
                  class="text-start"
                >
                  <td>Some type here</td>
                  <td class="text-start">
                    {{ game.occupiedSlotsCount }} /
                    {{ game.settings.guessersCount }}
                  </td>
                  <td class="text-end">
                    <button
                      class="btn btn-outline-success"
                      @click="joinGame(game.id)"
                    >
                      Join Game
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
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
import { fetchWaitingRooms, joinGameRoom, fetchLeaderBoard } from "@/api/Lobby";
import * as ws from "@/api/Lobby/subscriptions";
import { Score, Game, GameSlotUpdateNotification } from "@/typings";
import { logoutUser } from "@/utils/auth";
import LoadingGameModal from "@/components/Modal/LoadingGameModal.vue";

export default defineComponent({
  name: "Home",
  components: {
    LoadingGameModal,
  },
  created() {
    this.fetchGameRooms();
    this.subscribeToGames();
  },
  mounted() {
    this.fetchPlayerScores();
  },
  unmounted() {
    ws.unsubscribeFromGameRoomsUpdates(this.updateGameRoom);
    ws.unsubscribeFromGameRoomsCreation(this.storeGameRoom);
    ws.unsubscribeFromGameRoomsDeletes(this.deleteGameRoom);
    ws.unsubscribeFromGameStart(this.startGame);
  },
  data() {
    return {
      leaderBoard: [] as Score[],
      gameRooms: [] as Game[],
      joinedGame: null as Game | null,
    };
  },
  methods: {
    fetchGameRooms() {
      fetchWaitingRooms().then((rooms) => (this.gameRooms = rooms));
    },
    fetchPlayerScores() {
      fetchLeaderBoard().then((s) => (this.leaderBoard = s));
    },
    sortedPlayers() {
      return this.leaderBoard.sort((a, b) => b.score - a.score);
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
      this.gameRooms = this.gameRooms.filter((game) => game.id !== id);

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
        const game = this.gameRooms.find((g) => g.id === id);
        if (!game) {
          throw new Error("Game was not found!");
        }
        this.joinedGame = game;
      });
    },
    startGame() {
      this.$router.push({ name: "Game" });
    },
    handleLogout() {
      logoutUser().then(() => this.$router.push({ name: "Index" }));
    },
  },
  computed: {
    visibleGameRooms(): Game[] {
      return this.gameRooms.filter(
        (game: Game) => game.occupiedSlotsCount < game.settings.guessersCount
      );
    },
  },
});
</script>
