
<script lang="ts">
    import {init} from "authenticated.ts";
</script>
<authenticated>
    {#await init}
        
    {:then } 
        
    {/await}
</authenticated>