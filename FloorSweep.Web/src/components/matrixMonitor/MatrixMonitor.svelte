<script lang="ts">
  import Authenticated from "../authenticated/authenticated.svelte";
import Matrix from "../matrix/Matrix.svelte";
import { matrixStore, init } from "./matrixMonitor";
  const initialize = (()=>init())();
</script>

<matrixMonitor>
  {#await initialize}
    <p>Loading...</p>
  {:then} 
    {#each $matrixStore as m}
      <Matrix {...m}/>
    {/each}
  {/await}
</matrixMonitor>