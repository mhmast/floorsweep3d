<script lang="ts">
  import RobotStatus from "./RobotStatus.svelte";
  import RobotCommand from "./RobotCommand.svelte";
  import SessionStatus from "./SessionStatus.svelte";

  import { sessionStatusStore, robotCommandStore, init } from "./statusMonitor";
  import { robotStatusStore } from "../../services/statusService";
  const initialize = (() => init())();
</script>

<statusMonitor>
  {#await initialize}
    <p>Loading...</p>
  {:then}
    <div class="grid grid-columns-3">
      <div>
        <h2>Robot Status(Local => Server)</h2>
        <RobotStatus status={$robotStatusStore} />
      </div>
      <div>
        <h2>Robot Command (Server => Robot)</h2>
        <RobotCommand action={$robotCommandStore} />
      </div>
      <div>
        <h2>Session Status(Server)</h2>
        <SessionStatus status={$sessionStatusStore} />
      </div>
    </div>
  {/await}
</statusMonitor>
