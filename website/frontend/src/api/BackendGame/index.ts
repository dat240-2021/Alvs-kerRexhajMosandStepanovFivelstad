import axios from "axios";
import { Category, Game, GameSlotUpdateNotification } from "@/typings";
import * as signalR from "@microsoft/signalr";
import {
  createGameHandlers,
  deleteGameHandlers,
  updateSlotsHandlers,
  startGameHandlers,
} from "@/api/BackendGame/subscriptions";

export const gameHubConnection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/games")
  .build();

gameHubConnection.on("GameCreated", (data) => {
  const { game, occupiedSlotsCount } = data;
  createGameHandlers.forEach((handler) =>
    handler({ ...game, occupiedSlotsCount })
  );
});

gameHubConnection.on("GameRoomUpdated", (data: GameSlotUpdateNotification) => {
  updateSlotsHandlers.forEach((handler) => handler(data));
});

gameHubConnection.on("GameDeleted", (id: string) => {
  deleteGameHandlers.forEach((handler) => handler(id));
});

gameHubConnection.on("GameStarted", () => {
  startGameHandlers.forEach((handler) => handler());
});

export const createGame = async (settings: any): Promise<Game> => {
  const { data } = await axios.post("/api/games", settings);
  return data.data;
};

export const fetchWaitingRooms = async (): Promise<Game[]> => {
  const {
    data: { data },
  } = await axios.get("/api/games");
  return data as Game[];
};

export const joinGameRoom = async (id: string) => {
  await axios.post(`api/games/${id}/join`);
};

export const leaveGameRoom = async (id: string) => {
  await axios.post(`api/games/${id}/leave`);
};

export const startGame = async (id: string) => {
  await axios.post(`api/games/${id}/start`);
};

export const fetchCategories = async (): Promise<Category[]> => {
  const {
    data: { data: categories },
  } = await axios.get("/api/categories");
  return categories as Category[];
};
