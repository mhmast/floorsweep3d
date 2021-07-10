<script lang="typescript">
  import { init } from "./logout";
  const initialize = (async () => await init())();
</script>

<svelte:head>
  <title>Logout</title>
</svelte:head>
<div>
  {#await initialize}
    <p>Logging out</p>
  {:catch error}
    <p>{error}</p>
  {/await}
</div>
