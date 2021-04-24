<script>
  import Authenticated from "../components/authenticated/authenticated.svelte";
  import MapMonitor from "../components/mapMonitor/MapMonitor.svelte";
  import MapSimulator from "../components/mapSimulator/MapSimulator.svelte";
  import StatusMonitor from "../components/statusMonitor/StatusMonitor.svelte";
</script>

<svelte:head>
  <title>Floorsweep Monitor</title>
</svelte:head>

<Authenticated>
  <div class="grid grid-rows-2">
    <StatusMonitor />
    <div class="grid grid-columns-2">
      <MapSimulator />
      <MapMonitor />
    </div>
  </div>
</Authenticated>
