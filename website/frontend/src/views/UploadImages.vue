<template>
  <div class="container vh-100 py-5">
    <div class="row">
      <div class="col d-flex justify-content-around">
        <router-link class="btn btn-primary" to="/home">
          Return home
        </router-link>
        <Submit>Start game</Submit>
      </div>
    </div>

    <div class="col d-flex justify-content-center">
      <h1>Upload Image</h1>
    </div>
    <div class="row">
      <form @submit.prevent="onUploadImage" class="form-control border-0">
        <div class="col d-flex justify-content-around">
          <p>Upload Images</p>
          <Input type="file" @change="onImagesSelected"></Input>
          <div class="input-group-prepend">
            <button class="btn btn-outline-primary" type="submit">
              UploadFiles
            </button>
          </div>
          <p>{{ images.length }} files selected</p>
        </div>
      </form>
    </div>
    <div v-if="error.length > 2">
      <p class="text-danger">{{ error }}</p>
    </div>
    <form>
      <div class="table">
        <table>
          <tr>
            <th>#</th>
            <th>Filename</th>
            <th>Title</th>
            <th>Category</th>
          </tr>
          <tr v-for="i in images" :key="i.id">
            <td>{{ i.id }}</td>
            <td>{{ i.name }}</td>
            <td>
              <Input v-model="i.label" error="" type="text" id="imagetitle" />
            </td>
            <td>
              <select v-model="i.category" class="form-select">
                <option
                  v-for="c in categories.sort((a, b) => a.id - b.id)"
                  :key="c.id"
                  :value="c.id"
                >
                  {{ c.name }}
                </option>
              </select>
            </td>
          </tr>
        </table>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";
import { ImageFile, Category } from "@/typings";
import { fetchCategories } from "@/api/BackendGame";
import { uploadImages } from "@/api/Images";

declare interface BaseComponentData {
  images: ImageFile[];
  imagesSelected: any;
  categories: Category[];
  error: string;
}

export default defineComponent({
  name: "ImageUpload",
  components: {
    Input,
    Submit,
  },

  data(): BaseComponentData {
    return {
      images: [],
      imagesSelected: null,
      categories: [],
      error: "",
    };
  },
  mounted() {
    this.loadCategories();
  },
  methods: {
    onImagesSelected(event: any) {
      console.log(event.target.files);
      for (var i = 0; i < event.target.files.length; i++) {
        var name = event.target.files[i].name;
        var reader = new FileReader();
        reader.onloadend = () => {
          this.images.push({
            id: i,
            name: name,
            file: reader.result,
            category: "",
            label: "",
          } as ImageFile);
        };
        reader.readAsDataURL(event.target.files[i]);
      }

      //this.images.Image = event.target.files[0]
      //this.selectedImage = event.target.files[0]
    },
    onUploadImage() {
      if (this.images.length < 1) {
        this.error = "Upload at least one file!";
      }
      for (var i = 0; i < this.images.length; i++) {
        if (this.images[i].category == "" || this.images[i].label == "") {
          this.error = "Please fill in all fields!";
          return;
        }
      }

      //do the actual upload
      console.log(this.images);
      uploadImages(this.images);
    },

    loadCategories() {
      fetchCategories().then((categories) => {
        // var categoryIds = categories.map(c => c.id);
        this.categories = categories;
      });
    },
  },
});
</script>
