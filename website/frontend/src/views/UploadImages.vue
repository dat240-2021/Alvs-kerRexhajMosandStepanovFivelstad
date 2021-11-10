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
        <div class="row ">
            <form>
            <div class="col d-flex justify-content-around">              
                <p>Upload Images</p>
                    <Input type="file" @change="onImageSelected"></Input>
                    <Submit @click="onUploadImage" > Upload</Submit>
                <p>{{numberOfImages}} files selected</p>
            </div>   
            </form>         
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
                <tr v-for="i in 10" :key="i">
                    <td>{{i}}</td>                
                    <td>imagename.jpg</td>                
                    <td>
                       <Input  
                           v-model="label"
                           error=""
                           type="text"
                           model-value="Image Title"
                           id="imagetitle"                               
                       />
                    </td>              
                    <td>
                       <Input                                                       
                           error=""
                           type="text"
                           model-value="Category"
                           id="category"                               
                       />
                    </td>
                </tr>         
            </table>
        </div>
        </form>      
        </div>         
</template>

<script lang="ts">
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";


export class Image {
    Image: string; //this is the image.
    ImageName: string; // this is the name of the image.
    ImageTitle: string; // this is the image title and also correct answere to the guess.
    Category: string; 
    ImageId: string;

    constructor(image: string, imageName: string, imageTitle: string, category:string, imageId:string) {
        this.Image = image;
        this.ImageName = imageName;
        this.ImageTitle = imageTitle;
        this.Category = category;
        this.ImageId = imageId;
    }

}

declare interface BaseComponentData {
    images : Image[],
    selectedImage: null,
    label : string,
    numberOfImages : number,
}

export default {
    name: "UploadImages",
    components: {
        Input,
        Submit,
    },
    
    data() : BaseComponentData {
        return {
            images: [] as Image[],
            selectedImage: null,
            label: "",
            numberOfImages: 0,
        };
    },
    methods:{
        onImageSelected(event: any) {
            console.log(event)
            //this.images.Image = event.target.files[0]
            //this.selectedImage = event.target.files[0]
        },
        onUploadImage(){
            // This is where we send the images to database.
        }
    }
};
</script>

<style scoped>
.col {
    margin: 1em;
}
</style>