<template>
  <div class="home">
    <h1 id="company">{{ header }}</h1>
    <p id="app">{{ body }}</p>
    <img id="HomeViewImage" v-bind:src="image"/>
    <div class="process">
      <p id="ProcessInfo">Protein Purification Process Using the <i>iCapTag</i>&trade; Technology</p>
      <img id="ProcessPic" src="./../../img/Process.png" /> 
    </div>
  </div>
</template>

<script>
import HomeViewService from '../../services/HomeViewService'
export default {
  created(){
this.view();
  },
  data(){
    return{
      body:'',
      header: '',
      image: '',
    }

  },
  methods:{
    view(){
     return HomeViewService
      .getView()
      .then((response) => {
            if (response.status == 200) {
              this.body = response.data.body;
              this.header = response.data.header;
              this.image = response.data.image;
              }})
          .catch((error) => {
            const response = error.response;
            this.registrationErrors = true;
            if (response.status === 400) {
              this.registrationErrorMsg = 'Bad Request: Validation Errors';
            }
      })
      

  },
  },
  computed:{
  },
}
</script>

<style scoped>
.home {
  font-family: Playfair Display;
  font-weight: 400;
}
#company {
  text-align: center;
  height: auto;
}
#app {
  text-align: center;
  font-size: 20px;
}
#HomeViewImage {
  display:block;
  margin: auto;
  max-width: 100%;
}
#ProcessInfo {
  text-align: center;
  font-size: 30px;
  height: auto; 
  font-style: normal;
}
#ProcessPic {
  display: block;
  margin:auto;
  width: 70%;
  margin-top: 0px;
}
</style>