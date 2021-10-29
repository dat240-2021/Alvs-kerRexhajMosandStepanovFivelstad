import { NavigationGuardNext, RouteLocationNormalized } from "vue-router";
import { isUserAuth } from "@/utils";

export const isUserAuthenticated = (
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) => {
  if (isUserAuth()) {
    next("/home");
    return;
  }
  next();
};

export const isUserNotAuthenticated = (
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) => {
  if (!isUserAuth()) {
    next("/");
    return;
  }
  next();
};
