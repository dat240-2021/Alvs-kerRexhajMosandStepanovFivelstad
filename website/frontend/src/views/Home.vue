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
                <th scope="col">#</th>
                <th scope="col">Game Type</th>
                <th scope="col">Capacity</th>
                <th scope="col">Join</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(room, id) in gameRooms" :key="room.id">
                <td>{{ id + 1 }}</td>
                <td>Some type here</td>
                <td>Some capacity here </td>
                <td>
                  <router-link class="btn" :to="'/game/' + room.id">
                    <i class="bi bi-box-arrow-in-right"></i>
                  </router-link>
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
import { fetchWaitingRooms, subscribeToGameRooms } from "@/api/BackendGame";

export default defineComponent({
  name: "Home",
  created() {
    this.fetchGameRooms();
    this.subscribeToGames();
  },
  data() {
    return {
      leaderBoard: [],
      gameRooms: [] as any[],
    };
  },
  methods: {
    fetchGameRooms() {
      fetchWaitingRooms()
        .then(rooms => this.gameRooms = rooms);
    },
    addGameRoom(room: any) {
      console.log(room);
      this.gameRooms = [...this.gameRooms, room];
    },
    subscribeToGames() {
      subscribeToGameRooms(this.addGameRoom);
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
