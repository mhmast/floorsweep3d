<script lang="ts">
  import type { RobotStatusMessage } from "../../models/messages/RobotStatusMessage";
  import { RobotStatusType } from "../../models/messages/RobotStatusMessage";

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
  <p>
    <span>Type: </span><span>{RobotStatusType[status.currentAction.type]}</span>
  </p>
  <p><span>Data: </span><span>{status.currentAction.data}</span></p>
</robotStatus>

<style lang="scss">
  @import "./status.scss";
</style>
