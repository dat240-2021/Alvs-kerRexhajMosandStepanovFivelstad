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
            <div class="row d-flex justify-content-start" v-for="i in 10" :key="i">
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
//import Input from "@/components/Form/Input.vue";
//import Submit from "@/components/Form/Submit.vue";


//declare interface BaseComponentData {
//
//}

export default {
    name: "UploadImages",
    components: {
        //Input,
        //Submit,
    },
    
    //data() : BaseComponentData {
    //    return {
    //    };
    //},
    //methods:{      
    //}
};
</script>

<style scoped>
.col {
    margin: 1em;
}
</style>