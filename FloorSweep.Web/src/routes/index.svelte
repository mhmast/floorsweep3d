<script>
  import Authenticated from "../components/authenticated/authenticated.svelte";
import PathFindControlBar from "../components/pathFindControlBar/PathFindControlBar.svelte";
</script>

<svelte:head>
  <title>Floorsweep Monitor</title>
</svelte:head>

<Authenticated>
  <div>
    <PathFindControlBar/>

  </div>
</Authenticated>
