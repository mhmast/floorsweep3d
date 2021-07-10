<script lang="typescript">
  import { init } from "./authenticated";
</script>

<authenticated>
  {#await init then A}
    <slot />
  {:catch error}
    <p>{error}</p>
  {/await}
</authenticated>
