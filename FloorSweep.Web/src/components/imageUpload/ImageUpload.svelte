<script lang="typescript">
  export let clickHandler: (data: string | ArrayBuffer) => void;
  let avatar, fileinput, img;

  const onFileSelected = (e) => {
    let image = e.target.files[0];
    let reader = new FileReader();
    reader.readAsDataURL(image);
    reader.onload = (e) => {
      avatar = e.target.result;
      img.src = avatar;
    };
  };
</script>

<imageUpload>
  <h3>Upload Image</h3>

  <img
    class="avatar upload"
    src="https://cdn4.iconfinder.com/data/icons/small-n-flat/24/user-alt-512.png"
    alt=""
    bind:this={img}
    on:click={() => {
      fileinput.click();
    }}
  />

  <input
    style="display:none"
    type="file"
    accept=".jpg, .jpeg, .png"
    on:change={(e) => onFileSelected(e)}
    bind:this={fileinput}
  />
  {#if avatar}
    <button on:click={(e) => clickHandler(avatar)}>Select</button>
  {/if}
</imageUpload>

<style>
  imageUpload {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-flow: column;
  }

  .upload {
    display: flex;
    height: 50px;
    width: 50px;
    cursor: pointer;
  }
  .avatar {
    display: flex;
    height: 200px;
    width: 200px;
  }
</style>
