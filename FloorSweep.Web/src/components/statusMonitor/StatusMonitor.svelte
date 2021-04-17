<script lang="ts">
import Status from "./Status.svelte";
import { statusStore, init } from "./statusMonitor";
  const initialize = (()=>init())();
</script>

<statusMonitor>
  {#await initialize}
    <p>Loading...</p>
  {:then} 
    <Status status={$statusStore}/>
  {/await}
</statusMonitor>