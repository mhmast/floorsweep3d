<script lang="typescript">
  import { init } from "./login";
  const initialize = (async () => await init())();
</script>

<svelte:head>
  <title>Login</title>
</svelte:head>
<div>
  {#await initialize}
    <p>redirecting to login</p>
  {:catch error}
    <p>{error}</p>
  {/await}
</div>
