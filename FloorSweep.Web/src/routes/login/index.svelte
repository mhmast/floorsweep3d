<script lang="ts">
  import { init } from "./login";
</script>

<svelte:head>
  <title>Login</title>
</svelte:head>
  {#await init then loginUrl}
    <iframe title="login" src={loginUrl} />
  {:catch error}
    <p>{error}</p>
  {/await}
