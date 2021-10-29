import axios from "axios";
import { User } from "@/typings";
import { isUserAuth } from "@/utils";

// const getAuthUser = async (): Promise<User | null> =>
//   (await axios.get("/me")) as User;

/*
temp for testing
 */
// not auth
const getAuthUser = async (): Promise<User | null> => null;

// auth
// const getAuthUser = async (): Promise<User | null> =>
//   ({ isAuth: true } as User);

export default {
  getAuthUser,
};
