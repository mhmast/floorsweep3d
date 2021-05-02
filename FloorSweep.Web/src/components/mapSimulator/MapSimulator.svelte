<script lang="ts">
  import ImageUpload from "../imageUpload/ImageUpload.svelte";
  import { RobotStatusType } from "../../models/messages/RobotStatusMessage";
  import {
    initializeCanvasAsync,
    error,
    startRobotAsync,
    stopRobot,
    doNextStep,
    action,
    location,
    rotation,
    autoContinue,
  } from "./mapSimulator";
  let mapData;
  let robotInited = false;
  const canvas = (e: HTMLCanvasElement) => {
    initializeCanvasAsync(e, mapData, () => (robotInited = true));
  };
</script>

<mapSimulator>
  <h2>Map Simulator</h2>
  {#if $error}
    <p class="error">{$error}</p>
  {/if}
  rotation: {$rotation}
  location: x:{$location?.x} y:{$location?.y}

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
        <span disabled={!robotInited}>Autocontinue </span><input
          type="checkbox"
          disabled={!robotInited}
          checked={$autoContinue}
          on:click={() => autoContinue.set(!$autoContinue)}
        />
        <button
          disabled={!robotInited || $autoContinue}
          on:click={(e) => doNextStep()}>Next Step</button
        >
      </p>
      <canvas use:canvas class:robotPlace={!robotInited} />
    </div>
  {/if}
</mapSimulator>

<style lang="scss">
  @import "./mapSimulator.scss";
</style>
