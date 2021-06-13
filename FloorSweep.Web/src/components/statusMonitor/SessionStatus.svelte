<script lang="ts">
  import TreeView from "./TreeView.svelte";
  export let status: string;
</script>

<sessionStatus class="status">
  <TreeView value={status} />
</sessionStatus>
