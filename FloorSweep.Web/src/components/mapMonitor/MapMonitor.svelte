<script lang="typescript">
  import Matrix from "../matrix/Matrix.svelte";
  import { mapStore, init } from "./mapMonitor";
  let mapOnly = true;

  const initialize = (() => init(mapOnly))();
</script>

<mapMonitor>
  <h2>Maps</h2>
  <span>
    Base map only <input
      type="checkbox"
      bind:checked={mapOnly}
      on:change={async () => await init(mapOnly)}
    />
  </span>
  {#await initialize}
    <p>Loading...</p>
  {:then connection}
    {#each $mapStore as m}
      <Matrix init={m} {connection} />
    {/each}
  {/await}
</mapMonitor>
