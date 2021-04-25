<script lang="ts">
  import type { RobotAction } from "../../models/messages/RobotStatusMessage";

  export let action: RobotAction;
</script>

<robotAction>
  <p><span>Type: </span><span>{action.type}</span></p>
  <p><span>Data: </span><span>{action.data}</span></p>
</robotAction>
