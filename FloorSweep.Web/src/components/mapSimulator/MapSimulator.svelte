<script lang="ts">
  import ImageUpload from "../imageUpload/ImageUpload.svelte";
  import {
    initializeCanvasAsync,
    error,
    startRobotAsync,
    stopRobot,
  } from "./mapSimulator";
  let mapData;
  let robotInited = false;
  const canvas = (e: HTMLCanvasElement) => {
    console.log("place robot");
    initializeCanvasAsync(e, mapData, () => (robotInited = true));
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
      <p>
        <button disabled={!robotInited} on:click={(e) => startRobotAsync()}
          >Start</button
        >
        <button disabled={!robotInited} on:click={(e) => stopRobot()}
          >Stop</button
        >
      </p>
      <canvas use:canvas class:robotPlace={!robotInited} />
    </div>
  {/if}
</mapSimulator>

<style lang="scss">
  @import "./mapSimulator.scss";
</style>
