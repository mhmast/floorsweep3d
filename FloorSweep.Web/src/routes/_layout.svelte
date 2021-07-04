<script lang="ts">
  import Nav from "../components/nav/Nav.svelte";
  import { init } from "./_layout";
  let initialize = (async () => await init())();
  export let segment: string;
</script>

{#await initialize}
  <p>Loading..</p>
{:then result}
  <Nav {segment} />
  {#if !result.error}
    <main>
      <slot />
    </main>
  {:else}
    <p>{result.error}</p>
  {/if}
{:catch ex}
  <p>{ex}</p>
{/await}

<style lang="scss" global>
  @import "../style/global.scss";
</style>
