<script>
  import Authenticated from "../components/authenticated/authenticated.svelte";
import MatrixMonitor from "../components/matrixMonitor/MatrixMonitor.svelte";
import PathFindControlBar from "../components/pathFindControlBar/PathFindControlBar.svelte";
</script>

<svelte:head>
  <title>Floorsweep Monitor</title>
</svelte:head>

<Authenticated>
  <div>
    <PathFindControlBar/>
<MatrixMonitor/>
  </div>
</Authenticated>
