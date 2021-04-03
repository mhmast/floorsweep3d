<script lang="ts">
  import { matrixStore, init } from "./matrixMonitor";
  const initialize = (()=>init())();
</script>

<matrixMonitor>
  {#await initialize}
    <p>Loading...</p>
  {:then} 
    {#each $matrixStore as m}
      <div>{m.name}</div>
    {/each}
  {/await}
</matrixMonitor>