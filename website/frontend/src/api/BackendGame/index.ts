import axios from "axios";
import * as signalR from "@microsoft/signalr";
import {
  Category,
  Game,
  GameSlotUpdateNotification,
  subscribeToGameRoomsCreationCb,
  subscribeToGameRoomsUpdateCb,
} from "@/typings";

let createGameHandlers: subscribeToGameRoomsCreationCb[] = [];
let updateSlotsHandlers: subscribeToGameRoomsUpdateCb[] = [];

const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hub/games")
  .build();

connection.on("GameCreated", (data) => {
  const { game, occupiedSlotsCount } = data;
  createGameHandlers.forEach((handler) =>
    handler({ ...game, occupiedSlotsCount })
  );
});

connection.on("TEST", (data) => {
  console.log(data);
});

connection.on("GameRoomUpdated", (data: GameSlotUpdateNotification) => {
  updateSlotsHandlers.forEach((handler) => handler(data));
});

connection.start();

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
