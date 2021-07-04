<script lang="ts">
  import TreeView from "svelte-json-tree-auto";
  export let status: object;
</script>

<sessionStatus class="status">
  <TreeView value={status} />
</sessionStatus>

<style>
</style>
