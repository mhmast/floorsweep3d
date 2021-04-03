<script lang="ts">

import {draw} from "./matrix"
  
  export let data;
  export let width;
  export let height;
  export let isBinary;
  export let name;
  function canvas(node)
  {
    draw(width,height,data,isBinary,node);
  }
</script>

<matrix>  
  <span>{name}</span>
  <canvas use:canvas />
</matrix>



<style lang="scss">
  @import "./matrix.scss";
</style>