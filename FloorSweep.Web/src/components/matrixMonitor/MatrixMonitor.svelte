<script lang="ts">
  import Authenticated from "../authenticated/authenticated.svelte";
import Matrix from "../matrix/Matrix.svelte";
import { matrixStore, init } from "./matrixMonitor";
  const initialize = (()=>init())();
</script>

<matrixMonitor>
  <h2>Matrices</h2>
  {#await initialize}
    <p>Loading...</p>
  {:then connection} 
    {#each $matrixStore as m}
      <Matrix init={m} connection={connection}/>
    {/each}
  {/await}
</matrixMonitor>