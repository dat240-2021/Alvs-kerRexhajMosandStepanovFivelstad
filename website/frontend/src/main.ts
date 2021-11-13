import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { fetchAuthUser, isUserAuth, setAuthUser } from "@/utils/auth";

const initApp = async () => {
  if (!isUserAuth()) {
    const user = await fetchAuthUser();
    setAuthUser(user);
  }

  createApp(App).use(router).mount("#app");
};

initApp();
