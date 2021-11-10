<template>
    <div class="container vh-100 py-5">
        
        <div class="col">
            <router-link class="btn btn-primary" to="/home">
                Return home
            </router-link>
        </div>
        <div class="col">
            <h1>Upload Image</h1>
        </div>
        <div class="row" >
            <div class="row d-flex justify-content-start" v-for="i in 10" :key="i"><!--this loop must be changed to iterate over images-->
                <div class="col border border-success" >
                    image {{i}} goes here
                    
                </div>
                    <div class="col border border-success">
                        <table>
                             <tr>
                                 <th>Image name</th>
                             </tr>
                             <tr>
                                 <td>Image title (what to guess)</td>
                             </tr>
                             <tr>
                                 <td>Category</td>
                             </tr>
                        </table>
                    </div>
            </div>
        </div>         
    </div>
</template>

<script lang="ts">
// There is no need for input and submit here but, keep it for now.
//import Input from "@/components/Form/Input.vue";
//import Submit from "@/components/Form/Submit.vue";

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
}

export default {
    name: "UploadImages",
    components: {
        //Input,
        //Submit,
    },
    
    data() : BaseComponentData {
        return {
            images: [],
        };
    },
    //methods:{      
    //}
};
</script>

<style scoped>
/* I just let this stay for now */
.col {
    margin: 1em;
}
</style>