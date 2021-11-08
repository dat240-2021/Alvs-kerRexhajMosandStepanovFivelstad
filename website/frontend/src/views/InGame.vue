<template>
    <div class="container vh-100 py-5">
        
        <div class="col d-flex justify-content-end">
            <!--Submit>Leave Game</Submit-->
            <router-link class="btn btn-primary" to="/home">
                Leave Game
            </router-link>
        </div>
        <div class="col d-flex justify-content-center">
            <h1>In Game</h1>
        </div>  

        <div class="row">
            <div class="col">
                <div class="row">
                    <form>
                        <div class="col d-flex justify-content-around">
                            <Submit>Show one more</Submit> 
                            <Input                                
                                error=""
                                type="text"
                                model-value="Guesser Input"
                                id="guessing"                               
                            />       
                            <Submit>Check</Submit>

                        </div>                          
                    </form> 
                </div>                         
            </div>               
        </div>
        <div class="row -flex justify-content-sm-between">
            <div class="col Incorrectguess ">
                    <div class="row ">                 
                        <div class="col">                         
                        </div>
                        <div class="sm-2">
                            <div>
                                <table>
                                    <tr>
                                        <th>Guesses:</th>
                                    </tr>
                                    <tr>
                                        <td>Monkey</td>
                                    </tr>
                                    <tr>
                                        <td>Donkey</td>
                                    </tr>
                                    <tr>
                                        <td>Cat</td>
                                    </tr>
                                </table>
                            </div>
                        </div> 

                    </div>                         
            </div>
        
            <div class="col image">
                <div class="row ">                 
                    <div class="col">                         
                        Here comes an image
                    </div> 
                                                             
                </div>                         
            </div>     
           
            <div class="col Players">
                <div class="row ">                 
                    <div class="col">                         
                    </div>
                     <div class="sm-2">
                        <div>
                            <table>
                                <tr>
                                    <th>Player:</th>
                                    <th>Score:</th>
                                </tr>
                                <tr v-for="p in players" :key="p.playerName">
                                    <td >
                                        {{ p.playerName }}
                                    </td>
                                    <td >
                                        {{p.score}}
                                    </td>
                                </tr>
                                    
                                <!--tr>
                                    <td>Jamie Lannister</td>
                                    <td>10</td>
                                </tr>
                                <tr>
                                    <td>The Hound</td>
                                    <td>11</td>
                                </tr>
                                <tr>
                                    <td>Rob Stark</td>
                                    <td>9</td>
                                </tr-->
                            </table>
                            
                        </div> 
                    </div>                                      
                </div>                         
            </div>       
        </div>



        
    </div>
</template>

<script>
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";

export default {
    name: "InGame",
    components: {
        Input,
        Submit,
    },
    data() {
        return {
            //imagesegments: [],
            players: [
                {playerName: "Jamie Lannister", score: 10, playerId: 1},
                {playerName: "The Hound", score: 11, playerId: 2},
                {playerName: "Rob Stark", score: 9, playerId: 3},
            ],
            guess: "",
            proposer: '',
            image: '',
            creator: '',
       
        };
    },
    methods:{
        makePropser: function(){	
			//const idx = Math.floor(Math.random() * this.players.length);
            //this.proposer = this.players[idx]
		},

        TwoPlayerModeFindProposer: function(creatorId){
            //if (this.players.length==2) {
            //    if (this.player[2] == creatorId) {
            //        this.creator = this.player[2]
            //    }
//
            //}
        },//
    }
};


</script>

<style scoped>
.Incorrectguess {
  background: #e7efbd;
}

.image {
    background: #efbde4;
}
.Players {
    background: #e7efbd
}

.row {
    margin: 1em;
}

</style>