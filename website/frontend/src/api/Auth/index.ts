import axios from "axios";
import { User } from "@/typings";
import { isUserAuth } from "@/utils/auth";

// const getAuthUser = async (): Promise<User | null> =>
//   (await axios.get("/me")) as User;

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
/*
temp for testing
 */

// not auth user
const getAuthUser = async (): Promise<User | null> => null;

// auth user
// const getAuthUser = async (): Promise<User | null> =>
//   ({ isAuth: true } as User);

export default {
  getAuthUser,
  authUser,
  registrateUser,
};
