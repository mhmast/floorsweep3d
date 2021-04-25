<script lang="ts">
  import RobotStatus from "./RobotStatus.svelte";
  import RobotAction from "./RobotAction.svelte";
  import LocationStatus from "./LocationStatus.svelte";
  import { locationStatusStore, robotActionStore, init } from "./statusMonitor";
  import { robotStatusStore } from "../../services/statusService";
  const initialize = (() => init())();
</script>

<statusMonitor>
  {#await initialize}
    <p>Loading...</p>
  {:then}
    <div class="grid grid-columns-3">
      <div>
        <h2>Robot (Local)</h2>
        <RobotStatus status={$robotStatusStore} />
      </div>
      <div>
        <h2>Robot (Server)</h2>
        <RobotAction action={$robotActionStore} />
      </div>
      <div>
        <h2>Location Status(Server)</h2>
        <LocationStatus status={$locationStatusStore} />
      </div>
    </div>
  {/await}
</statusMonitor>
