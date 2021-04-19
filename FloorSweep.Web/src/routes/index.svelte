<script>
  import Authenticated from "../components/authenticated/authenticated.svelte";
import MatrixMonitor from "../components/matrixMonitor/MatrixMonitor.svelte";
import StatusMonitor from "../components/statusMonitor/StatusMonitor.svelte";
</script>

<svelte:head>
  <title>Floorsweep Monitor</title>
</svelte:head>

<Authenticated>
  <div>
    <StatusMonitor/>
    <MatrixMonitor/>
  </div>
</Authenticated>
