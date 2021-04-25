<script lang="ts">
  import ImageUpload from "../imageUpload/ImageUpload.svelte";
  import { initializeCanvas, error } from "./mapSimulator";
  let mapData;
  let robotInited = false;
  const canvas = (e: HTMLCanvasElement) => {
    initializeCanvas(e, mapData, () => (robotInited = true));
  };
</script>

<mapSimulator>
  <h2>Map Simulator</h2>
  {#if $error}
    <p class="error">{$error}</p>
  {/if}
  {#if !mapData}
    <ImageUpload clickHandler={(d) => (mapData = d)} />
  {:else}
    {#if !robotInited}
      <h3>Place the robot</h3>
    {/if}
    <div class="map">
      <canvas use:canvas class:robotPlace={!robotInited} />
    </div>
  {/if}
</mapSimulator>

<style lang="scss">
  @import "./mapSimulator.scss";
</style>
