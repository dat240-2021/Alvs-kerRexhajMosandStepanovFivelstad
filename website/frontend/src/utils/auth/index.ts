import { User } from "@/typings";
import auth from "@/api/Auth";

export const fetchAuthUser = async (): Promise<User> => {
  const user = await auth.getAuthUser();
  return user ? { ...user, isAuth: true } : ({ isAuth: false } as User);
};

export const setAuthUser = (user: User) => {
  localStorage.setItem("user", JSON.stringify(user));
};

const getStoredUser = (): User | null => {
  const dataString = localStorage.getItem("user");

  if (!dataString) {
    return null;
  }

  return JSON.parse(dataString) as User;
};

export const isUserAuth = (): boolean => {
  const user = getStoredUser();
  return user !== null && user.isAuth;
};
