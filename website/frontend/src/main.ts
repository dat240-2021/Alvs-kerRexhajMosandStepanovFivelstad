import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";

import { createApp } from "vue";

import App from "./App.vue";
import router from "./router";
import { setCurrentUser } from "@/utils/auth";
import { getCurrentUser } from "@/api/Auth";

const initApp = async () => {
  const user = await getCurrentUser();
  setCurrentUser(user);

  createApp(App).use(router).mount("#app");
};

initApp();
