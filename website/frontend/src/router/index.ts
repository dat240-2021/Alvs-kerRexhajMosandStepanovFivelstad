import { createRouter, createWebHashHistory, RouteRecordRaw } from "vue-router";
import {
  isUserAuthenticated,
  isUserNotAuthenticated,
} from "@/router/middleware";

import Home from "@/views/Home.vue";
import Index from "@/views/Index.vue";
import Login from "@/views/auth/Login.vue";
import Registration from "@/views/auth/Registration.vue";
import NewGame from "@/views/NewGame.vue";
import UploadImages from "@/views/UploadImages.vue";
import Game from "@/views/Game.vue";
import InGame from "@/views/InGame.vue";


const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "Index",
    component: Index,
    beforeEnter: isUserAuthenticated,
  },
  {
    path: "/home",
    name: "Home",
    component: Home,
    beforeEnter: isUserNotAuthenticated,
  },
  {
    path: "/login",
    name: "Login",
    component: Login,
    beforeEnter: isUserAuthenticated,
  },
  {
    path: "/registration",
    name: "Registration",
    component: Registration,
    beforeEnter: isUserAuthenticated,
  },
  {
    path: "/game",
    name: "NewGame",
    component: NewGame,
    beforeEnter: isUserNotAuthenticated,
  },
  {
    path: "/uploadImages",
    name: "UploadImages",
    component: UploadImages,
  },
  {
<<<<<<< HEAD
    path: "/game",
    name: "Game",
    component: Game,
  },
  {
=======
>>>>>>> main
    path: "/ingame/",
    name: "InGame",
    component: InGame,
    beforeEnter: isUserNotAuthenticated,
  },
];

const router = createRouter({
  history: createWebHashHistory(process.env.BASE_URL),
  routes,
});

export default router;
