<script lang="ts">

import type LocationStatusMessage from "../../models/messages/LocationStatusMessage";


  export let status: LocationStatusMessage;
</script>

<locationStatus class="status">  
  <h2>Location Status</h2>
  <article>
    
  <p><span>Avg Speed Pixels/Second: </span><span>{status.avgSpeedPixelsPerSecond}</span></p>
    <p><span>LastUpdate Received: </span><span>{status.lastUpdateReceived}</span></p>
    <p><span>Status: </span><span>{status.locationDeterminationStatus}</span></p>
    </article>
</locationStatus>



<style lang="scss">
  @import "./status.scss";
</style>