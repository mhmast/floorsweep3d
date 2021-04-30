<script lang="ts">
  import type { RobotCommandMessage } from "../../models/messages/RobotStatusMessage";
  import { RobotCommandType } from "../../models/messages/RobotStatusMessage";

  export let action: RobotCommandMessage;
</script>

<robotAction>
  <p><span>Type: </span><span>{RobotCommandType[action.type]}</span></p>
  <p><span>Data: </span><span>{action.data}</span></p>
</robotAction>
