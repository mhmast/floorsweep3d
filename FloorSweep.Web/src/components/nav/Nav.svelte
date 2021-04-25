<script lang="ts">
  export let segment: string;
</script>

<nav>
  <ul>
    <li>
      <a aria-current={segment === undefined ? "page" : undefined} href="."
        >Home</a
      >
    </li>

    <!-- for the blog link, we're using rel=prefetch so that Sapper prefetches
		     the blog data when we hover over the link or tap it on a touchscreen -->
    <li>
      <a
        rel="prefetch"
        aria-current={segment === "logout" ? "page" : undefined}
        href="logout">Logout</a
      >
    </li>
  </ul>
</nav>

<style lang="scss">
  @import "nav.scss";
</style>
