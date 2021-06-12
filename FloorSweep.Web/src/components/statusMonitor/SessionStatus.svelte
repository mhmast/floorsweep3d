<script lang="ts">
  import TreeView from "svelte-json-tree-view";
  export let status: Object;
</script>

<sessionStatus class="status">
  <TreeView value={status} />
</sessionStatus>
