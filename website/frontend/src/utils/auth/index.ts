import { User } from "@/typings";
import { logout } from "@/api/Auth";
import initWebsockets from "@/api/websockets";

export const setCurrentUser = (user: User) => {
  localStorage.setItem("user", JSON.stringify(user));
  if (user.isAuth) {
    initWebsockets();
  }
};

const getStoredUser = (): User => {
  const dataString = localStorage.getItem("user");

  if (!dataString) {
    throw new Error("User is supposed to be stored");
  }

  return JSON.parse(dataString) as User;
};

export const logoutUser = async () => {
  await logout();
  localStorage.setItem("user", JSON.stringify({ isAuth: false }));
};

export const isUserAuth = (): boolean => {
  const user = getStoredUser();
  return user !== null && user.isAuth;
};
