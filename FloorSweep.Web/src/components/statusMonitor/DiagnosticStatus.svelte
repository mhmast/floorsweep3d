<script lang="ts">
  import type { DiagnosticStatusMessage } from "../../models/messages/DiagnosticStatusMessage";
  import { DiagnosticStatus } from "../../models/messages/DiagnosticStatusMessage";

  export let status: DiagnosticStatusMessage;
</script>

<diagnosticStatus class="status">
  <p>
    <span>Error: </span><span>{status.error}</span>
  </p>
  <p>
    <span>Status: </span><span>{DiagnosticStatus[status.status]}</span>
  </p>
</diagnosticStatus>

<style lang="scss">
  @import "./status.scss";
</style>
