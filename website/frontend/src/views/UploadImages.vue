<template>
  <div class="container vh-100 py-5">
    <div class="row">
      <div class="col d-flex justify-content-around mb-5">
        <router-link class="btn btn-primary" to="/home">
          Return home
        </router-link>
        <button class="btn btn-outline-primary" type="button" @click="clear">
          Clear Input
        </button>
      </div>
    </div>

    <div class="col d-flex justify-content-center mb-5">
      <h1>Upload Image</h1>
    </div>
    <div class="row">
      <form @submit.prevent="onUploadImage" class="form-control border-0">
        <div class="col d-flex justify-content-around mb-5">
          <Input
            type="file"
            error=""
            @change="onImagesSelected"
            id="inputUploadField"
          ></Input>
          <div class="input-group-prepend">
            <button class="btn btn-outline-primary" type="submit">
              UploadFiles
            </button>
          </div>
          <div v-if="!loading">
            <p v-if="images.length > 1">{{ images.length }} Files selected</p>
          </div>
          <div v-else class="d-flex">
            <div
              class="spinner-border text-primary align-self-center"
              role="status"
            ></div>
            <div class="align-self-center">
              <p class="m-2">Processing {{ images.length }} files</p>
            </div>
          </div>
        </div>
      </form>
    </div>
    <div>
      <p v-if="successText.length > 2" class="text-success">
        {{ successText }}
      </p>
      <p v-if="error.length > 2" class="text-danger">{{ error }}</p>
    </div>
    <div>
      <table class="table table-hover">
        <thead>
          <tr class="text-center">
            <th scope="col">Delete</th>
            <th scope="col">#</th>
            <th scope="col">Filename</th>
            <th scope="col">Solution</th>
            <th scope="col">Category</th>
            <th scope="col">Manual Slicing</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="i in imagesSorted()"
            :key="i.id"
            scope="row"
            class="table-hover"
          >
            <td>
              <button
                class="btn btn-outline-danger"
                style="width: 100%"
                type="button"
                @click="deleteImage(i.id)"
              >
                Delete
              </button>
            </td>
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
            <td>
              <button
                :class="
                  i.sliceFile == ''
                    ? 'btn btn-outline-primary'
                    : 'btn btn-success'
                "
                style="width: 100%"
                type="button"
                @click="manualSlicing(i)"
              >
                Slice
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
  <ManualSlicingModal
    v-on:closeModal="showModal = false"
    v-on:saveAndExit="saveSlicesData"
    v-if="showModal"
    :modalImage="modalImage"
  />
</template>

<script lang="ts">
import { defineComponent } from "vue";
import Input from "@/components/Form/Input.vue";
import { ImageFile, Category } from "@/typings";
import { fetchCategories } from "@/api/Lobby";
import { uploadImages } from "@/api/Images";
import ManualSlicingModal from "@/components/Modal/ManualSlicingModal.vue";

declare interface BaseComponentData {
  images: ImageFile[];
  imagesSelected: any;
  categories: Category[];
  error: string;
  successText: string;
  loading: boolean;
  showModal: boolean;
  modalImage: ImageFile | null;
}

export default defineComponent({
  name: "ImageUpload",
  components: {
    Input,
    ManualSlicingModal,
  },

  data(): BaseComponentData {
    return {
      images: [],
      imagesSelected: null,
      categories: [],
      error: "",
      loading: false,
      successText: "",
      showModal: false,
      modalImage: null,
    };
  },
  mounted() {
    this.loadCategories();
  },
  methods: {
    onImagesSelected(event: any) {
      this.error = "";

      for (let i = 0; i < event.target.files.length; i++) {
        this.loadFile(event.target.files[i], this.images.length+i)
      }
    },

    loadFile(file: any, i: number) {
      let reader = new FileReader();
      reader.onloadend = () => {
        this.images = [
          ...this.images,
          {
            id: i,
            name: file.name,
            file: reader.result,
            sliceFile: "",
            sliceColors: [],
            category: "",
            label: "",
          } as ImageFile,
        ];
      };
      reader.readAsDataURL(file);
    },
    async onUploadImage() {
      if (this.images.length < 1) {
        this.error = "Upload at least one file!";
      }
      let imagesContainEmptyfields = this.images.find(
        (x) => x.label == "" && x.category == ""
      );
      if (imagesContainEmptyfields != undefined) {
        this.error = "Please fill in all fields!";
        return;
      }

      //do the actual upload
      this.loading = true;
      await uploadImages(this.images);
      this.successText = this.images.length + " files were uploaded";
      this.clear();
    },
    clear() {
      this.loading = false;
      this.images = [];
      this.imagesSelected = null;
      this.error = "";
    },
    loadCategories() {
      fetchCategories().then((categories) => {
        // let categoryIds = categories.map(c => c.id);
        this.categories = categories;
      });
    },
    manualSlicing(image: ImageFile) {
      this.modalImage = image;
      this.showModal = true;
    },
    saveSlicesData(object: any) {
      this.showModal = false;
      let image = this.images.find((x) => x.id == object.id);
      if (image != null) {
        image.sliceFile = object.data;
        image.sliceColors = object.colors;
      }
    },
    imagesSorted() {
      return this.images.sort((a, b) => a.id - b.id);
    },
    deleteImage(id: number) {
      this.images = this.images.filter((x) => x.id != id);
    },
  },
});
</script>
