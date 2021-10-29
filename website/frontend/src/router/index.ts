import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import {
  isUserAuthenticated,
  isUserNotAuthenticated,
} from "@/router/middleware";

import Home from "@/views/Home.vue";
import Index from "@/views/Index.vue";

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
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
