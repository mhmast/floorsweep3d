<script lang="ts">
import RobotStatus from "./RobotStatus.svelte";
import LocationStatus from "./LocationStatus.svelte";
import { locationStatusStore, robotStatusStore, init } from "./statusMonitor";
  const initialize = (()=>init())();
</script>

<statusMonitor>
  {#await initialize}
    <p>Loading...</p>
  {:then} 
    <RobotStatus status={$robotStatusStore}/>
    <LocationStatus status={$locationStatusStore}/>
  {/await}
</statusMonitor>