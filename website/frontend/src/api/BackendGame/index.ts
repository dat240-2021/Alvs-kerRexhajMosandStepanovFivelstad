import axios from "axios";
import * as signalR from "@microsoft/signalr";
import { Game, GameSlotUpdateNotification } from "@/typings";

let createGameHandlers: any[] = [];
let updateSlotsHandlers: any[] = [];

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/games")
  .build();

connection.on("GameCreated", (game: Game) => {
  createGameHandlers.forEach((handler) => handler(game));
});

connection.on("GameRoomUpdated", (data: GameSlotUpdateNotification) => {
  updateSlotsHandlers.forEach((handler) => handler(data));
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

export const leaveGameRoom = async (id: string) => {
  await axios.post(`api/games/${id}/leave`);
};

export const subscribeToGameRoomsCreation = (cb: any) => {
  createGameHandlers = [...createGameHandlers, cb];
};

export const subscribeToGameRoomsUpdates = (cb: any) => {
  updateSlotsHandlers = [...updateSlotsHandlers, cb];
};
