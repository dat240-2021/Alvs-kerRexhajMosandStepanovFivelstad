import axios from "axios";
import * as signalR from "@microsoft/signalr";
import { Game } from "@/typings";

let handlers: any[] = [];

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/games")
  .build();

connection.on("GameCreated", (game: Game) => {
  handlers.forEach((handler) => handler(game));
});

connection.on("GameRoomUpdated", (game: Game) => {
  console.log(game);
  handlers.forEach((handler) => handler(game));
});

connection.start();

export const createGame = async (settings: any) => {
  const {
    data: { data: id },
  } = await axios.post("/api/game", settings);
  return id;
};

export const fetchWaitingRooms = async (): Promise<Game[]> => {
  const { data } = await axios.get("/api/games");
  return data as Game[];
};

export const joinGameRoom = async (id: string) => {
  await axios.post(`api/games/${id}/join`);
};

export const subscribeToGameRooms = (cb: any) => {
  handlers = [...handlers, cb];
};
