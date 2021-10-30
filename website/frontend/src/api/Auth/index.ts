import axios from "axios";
import { User } from "@/typings";
import { isUserAuth } from "@/utils/auth";

// const getAuthUser = async (): Promise<User | null> =>
//   (await axios.get("/me")) as User;

const authUser = async (userName: string, password: string): Promise<User> =>
  await axios.post("/api/login", {
    userName,
    password,
  });

const registrateUser = async (
  userName: string,
  password: string
): Promise<User> =>
  await axios.post("/api/register", {
    userName,
    password,
  });

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
