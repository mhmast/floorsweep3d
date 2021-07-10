<script lang="typescript">
  import { init } from "./token";
  const initialize = (async () => await init())();
</script>

<svelte:head>
  <title>Redirecting</title>
</svelte:head>
<div>
  {#await initialize}
    <p>redirecting...</p>
  {:catch error}
    <p>{error}</p>
  {/await}
</div>
