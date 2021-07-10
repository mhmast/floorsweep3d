<script lang="typescript">
  import type { HubConnection } from "@microsoft/signalr";
  import type MatrixInitMessage from "../../models/messages/MatrixInitMessage";

  import { initializeCanvas } from "./matrix";
  export let init: MatrixInitMessage;
  export let connection: HubConnection;
  function canvas(node: HTMLCanvasElement) {
    initializeCanvas(init, connection, node);
  }
</script>

<matrix>
  <span>{init.name}</span>
  <canvas use:canvas />
</matrix>

<style lang="scss">
  @import "./matrix.scss";
</style>
