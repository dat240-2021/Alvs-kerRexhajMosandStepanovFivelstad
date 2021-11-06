import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { fetchAuthUser, isUserAuth, setAuthUser } from "@/utils/auth";
import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/games")
  .build();

connection.on("ReceiveMessage", function (game) {
  console.log(game);
});

connection.start();

// setTimeout(() => {
//   connection
//     .invoke("SendMessage", "user name", "user message")
//     .catch(function (err) {
//       return console.error(err.toString());
//     });
// }, 5000);

const initApp = async () => {
  if (!isUserAuth()) {
    const user = await fetchAuthUser();
    setAuthUser(user);
  }

  createApp(App).use(router).mount("#app");
};

initApp();
