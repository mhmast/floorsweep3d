<script lang="ts">
  import { init } from "./authenticated";
</script>

<authenticated>
  {#await init then A}
    <slot></slot>
  {:catch error}
    <p>{error}</p>
  {/await}
</authenticated>
