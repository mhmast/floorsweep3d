<script lang="ts">
  import { findPathAsync } from "./pathFindControlBar";
</script>

<pathfindControlBar>
  <ul>
<li>
  <button on:click={async e =>await findPathAsync()}>Find Path</button>
</li>
  </ul>
</pathfindControlBar>