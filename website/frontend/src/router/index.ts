import { createRouter, createWebHashHistory, RouteRecordRaw } from "vue-router";
import {
  isUserAuthenticated,
  isUserNotAuthenticated,
} from "@/router/middleware";

import Home from "@/views/Home.vue";
import Index from "@/views/Index.vue";
import Login from "@/views/auth/Login.vue";
import Registration from "@/views/auth/Registration.vue";

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
];

const router = createRouter({
  history: createWebHashHistory(process.env.BASE_URL),
  routes,
});

export default router;