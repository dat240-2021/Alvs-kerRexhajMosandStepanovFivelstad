<template>
    <div class="container vh-100 py-5">
        
        <div class="col d-flex justify-content-end">
            <router-link class="btn btn-primary" to="/home">
                Return home
            </router-link>
        </div>
        <div class="col d-flex justify-content-center">
            <h1>Upload Image</h1>
        </div>         
    </div>
</template>

<script lang="ts">
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";


declare interface BaseComponentData {

}

export default {
    name: "UploadImages",
    components: {
        Input,
        Submit,
    },
    
    data() : BaseComponentData {
        return {
        };
    },
    methods:{      
    }
};
</script>

<style scoped>
.row {
    margin: 1em;
}
</style>