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
                                v-model="newGuess"
                                error=""
                                type="text"
                                model-value="Guesser Input"
                                id="guessing"                               
                            />
                            <Submit v-on:click="Check">Check</Submit>
                        </div>                          
                    </form> 
                </div>                         
            </div>               
        </div>
        <div class="row -flex justify-content-sm-between">
            <div class="col-sm">
                    <div class="row">                 
                        <div class="border border-success">                         
                        
                        <div class="col-sm">
                            <div>
                                <table>
                                    <tr>
                                        <th>Guesses:</th>
                                    </tr>
                                    <tr v-for="g in guesses" :key="g">
                                        <td>{{g.Guess}}</td>
                                    </tr>
                                </table>
                                {{newGuess}}
                            </div>
                        </div> 
                    </div>
                    </div>                         
            </div>
        
            <div class="col-md">
                <div class="row">                 
                    <div class="col">                         
                        Here comes an image
                    </div> 
                                                             
                </div>                         
            </div>     
           
            <div class="col-md">
                <div class="row">                 
                    <div class="border border-success">                         
                    
                     <div class="sm-2">
                        <div>
                            <table>
                                <tr>
                                    <th>Player:</th>
                                    <th>Score:</th>
                                </tr>
                                <tr v-for="p in players.sort((a, b) => b.Score-a.Score )" :key="p.Name">
                                    <td >
                                        {{ p.Name }}
                                    </td>
                                    <td >
                                        {{p.Score}}
                                    </td>
                                </tr>
                                    
                            </table>
                            
                        </div>
                    </div> 
                    </div>                                      
                </div>                         
            </div>       
        </div>



        
    </div>
</template>

<script lang="ts">
import Input from "@/components/Form/Input.vue";
import Submit from "@/components/Form/Submit.vue";

export class Player {
    Name: string;
    Score: number;
    PlayerId: string;

    constructor(Name: string, Score: number, PlayerId: string) {
        this.Name = Name;
        this.Score = Score;
        this.PlayerId =PlayerId;
    }

}
export class Guess {
    Guess: string;

    constructor(guess: string) {
        this.Guess = guess;
    }
}

//export class NewGuess {
//    newGuess: string;
//
//    constructor(newguess: string) {
//        this.newGuess = newguess;
//    }
//}

declare interface BaseComponentData{
    players : Player[],
    guesses : Guess[],
    imageSlices: string[],
    newGuess: string,
    correct: string,
}



export default {
    name: "InGame",
    components: {
        Input,
        Submit,

    },
    data() : BaseComponentData {
        return {
            
            players: [
                new Player("Jamie Lannister", 10, "1"), 
                new Player("The Hound", 11, "2"),
                new Player("Rob Stark", 9, "3"),
                new Player("Elvis", 12, "4"),
                new Player("Santa Claus", 4, "5"),
                new Player("Madonna", 2, "6"),
                new Player("Lady Gaga", 0, "7")
                
            ] as Player[],

            guesses: [
                new Guess("Monkey"),
                new Guess("Donkey"),
                new Guess("Cat"),

            ] as Guess[],
                
            imageSlices: [],
            //incorrect: true,
            correct: "Fish",
            newGuess: "",
            //player: '',
       
        };
    },
    methods:{
        Check() {
            //this.guesses.push(new Guess(this.newGuess))
            //console.log(this.guesses)
            //if (this.newGuess != this.correct) {
            //    this.guesses.push(new Guess(this.newGuess))
            //}
            
        }
    }

};


</script>

<style scoped>


.row {
    margin: 1em;
}

</style>