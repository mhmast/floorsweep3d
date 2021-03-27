<script lang="ts">
  import { init } from "./authenticated";
</script>

<authenticated>
  {#await init}
    <slot />
  {:catch error}
    <p>{error}</p>
  {/await}
</authenticated>
