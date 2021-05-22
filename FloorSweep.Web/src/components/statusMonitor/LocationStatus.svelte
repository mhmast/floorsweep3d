<script lang="ts">
  import type { LocationStatusMessage } from "../../models/messages/LocationStatusMessage";
  import { LocationDeterminationStatus } from "../../models/messages/LocationStatusMessage";

  export let status: LocationStatusMessage;
</script>

<locationStatus class="status">
  <p>
    <span>Avg Speed Pixels/Second: </span><span
      >{status.avgSpeedPixelsPerSecond}</span
    >
  </p>
  <p>
    <span>Avg Speed mm/Second: </span><span>{status.avgSpeedMmPerSecond}</span>
  </p>
  <p>
    <span>Status: </span><span
      >{LocationDeterminationStatus[status.locationDeterminationStatus]}</span
    >
  </p>
</locationStatus>

<style lang="scss">
  @import "./status.scss";
</style>
