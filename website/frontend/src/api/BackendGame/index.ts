import axios from "axios";
import * as signalR from "@microsoft/signalr";

let handlers: any[] = [];

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/games")
  .build();

connection.on("GameCreated", (game) => {
  handlers.forEach((handler) => handler(game));
});

connection.start();

export const createGame = async (settings: any) => {
  const {
    data: { data: id },
  } = await axios.post("/api/game", settings);
  return id;
};

export const fetchWaitingRooms = async () => {
  const { data } = await axios.get("/api/games");
  return data;
};

export const subscribeToGameRooms = (cb: any) => {
  handlers = [...handlers, cb];
};
