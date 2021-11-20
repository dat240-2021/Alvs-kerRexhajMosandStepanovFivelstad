import { lobbyHubConnection } from "./Lobby";
import { gameHubConnection } from "./Game";

export default () => {
  lobbyHubConnection.start();
  gameHubConnection.start();
};
