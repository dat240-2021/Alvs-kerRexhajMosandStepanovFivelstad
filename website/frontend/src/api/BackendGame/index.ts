import axios from "axios";
import {
  Category,
  Game,
  GameSlotUpdateNotification,
  subscribeToGameRoomsCreationCb,
  subscribeToGameRoomsUpdateCb,
} from "@/typings";
import * as signalR from "@microsoft/signalr";

let createGameHandlers: subscribeToGameRoomsCreationCb[] = [];
let updateSlotsHandlers: subscribeToGameRoomsUpdateCb[] = [];

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

export const createGame = async (settings: any) => {
  const {
    data: { data: id },
  } = await axios.post("/api/games", settings);
  return id;
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

export const fetchCategories = async (): Promise<Category[]> => {
  const {
    data: { data: categories },
  } = await axios.get("/api/categories");
  return categories as Category[];
};

export const subscribeToGameRoomsCreation = (
  cb: subscribeToGameRoomsCreationCb
) => {
  createGameHandlers = [...createGameHandlers, cb];
};

export const subscribeToGameRoomsUpdates = (
  cb: subscribeToGameRoomsUpdateCb
) => {
  updateSlotsHandlers = [...updateSlotsHandlers, cb];
};

export const unsubscribeToGameRoomsCreation = (
  cb: subscribeToGameRoomsCreationCb
) => {
  createGameHandlers = createGameHandlers.filter((h) => h !== cb);
};

export const unsubscribeToGameRoomsUpdates = (
  cb: subscribeToGameRoomsUpdateCb
) => {
  updateSlotsHandlers = updateSlotsHandlers.filter((h) => h !== cb);
};
