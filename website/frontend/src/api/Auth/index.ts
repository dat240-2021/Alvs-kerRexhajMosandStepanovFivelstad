import axios from "axios";
import { User } from "@/typings";

const authUser = async (userName: string, password: string): Promise<User> => {
  const { data }: { data: User } = await axios.post("/api/login", {
    userName,
    password,
  });
  return { ...data, isAuth: true };
};

const registrateUser = async (
  userName: string,
  password: string
): Promise<User> => {
  const { data }: { data: User } = await axios.post("/api/register", {
    userName,
    password,
  });
  return { ...data, isAuth: true };
};

export const getCurrentUser = async (): Promise<User> => {
  const {
    data: { data: userData },
  } = await axios.get("/api/me");
  return userData.username ? { ...userData, isAuth: true } : { isAuth: false };
};

export const logout = async () => {
  await axios.post("/api/logout");
};

export default {
  getCurrentUser,
  authUser,
  registrateUser,
  logout,
};
