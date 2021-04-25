<script lang="ts">
  import type { RobotStatusMessage } from "../../models/messages/RobotStatusMessage";
  import RobotAction from "./RobotAction.svelte";
  export let status: RobotStatusMessage;
</script>

<robotStatus class="status">
  <h2>Local Robot Status</h2>
  <article>
    <h3>Data</h3>
    <p>
      <span>Distance to Object: </span><span>{status.distanceToObject}</span>
    </p>
  </article>
  <RobotAction action={status.currentAction} />
</robotStatus>

<style lang="scss">
  @import "./status.scss";
</style>
