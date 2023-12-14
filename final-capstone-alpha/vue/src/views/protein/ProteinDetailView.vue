<template>
  <div class="card">
    <div class="top-bar">
      <div class="p-name">
        <h1>{{ protein.sequenceName }}</h1>
      </div>
    </div>
    <div class="p-description">
      <h2>{{ protein.description }}</h2>
    </div>
    <div class="p-sequence">
      <h2>Original Sequence</h2>
      <h2>{{ original_sequence }}</h2>
    </div>
    <form class="p-generate" @submit.prevent="generateSequences()" v-show="protein.blueSequence[0] == ''">
      <button id="submit" type="submit">Generate Sequences</button>
    </form>
    <div class="generatedCard" v-show="protein.blueSequence[0] !== ''">
      <span v-if="show_copy" class="copied">Copied!</span>

      <div class="card-labels">
      </div>
      <div class="sequences">
        <div class="blue-div">
          <h3><a class="fast">Fast</a></h3>
          <div class="sequence1" v-for="(protein, index) in protein.blueSequence" v-bind:key="index">
            <h2 class="protein-display" v-on:click="change_display(protein)">{{ protein.substring(0, 6) }}...</h2>

            <span class="speed-details">
              Very rapid cleaving (possible losses during wash steps)
            </span>
          </div>
          <button @click="copy_blue">Copy Blue</button>
          <button @click="download_blue">Download Blue</button>
        </div>
        <div class="green-div">
          <h3><a class="medium">Medium</a></h3>
          <div class="sequence2" v-for="(protein, index) in protein.greenSequence" v-bind:key="index">
            <h2 class="protein-display" v-on:click="change_display(protein)">{{ protein.substring(0, 6) }}...</h2>
            <span class="speed-details">
              80-90% cleaved in 5 hours at room temperature
            </span>
          </div>
          <button @click="copy_green">Copy Green</button>
          <button @click="download_green">Download Green</button>
        </div>
        <div class="yellow-div">
          <h3><a class="slow">Slow</a></h3>
          <div class="sequence3" v-for="(protein, index) in protein.yellowSequence" v-bind:key="index">
            <span class="speed-details">
              80-90% cleaved overnight at room temperature
            </span>
            <h2 class="protein-display" v-on:click="change_display(protein)">{{ protein.substring(0, 6) }}...</h2>
          </div>
          <button @click="copy_yellow">Copy Yellow</button>
          <button @click="download_yellow">Download Yellow</button>
        </div>
      </div>
      <div id="singleSelect">
        <div>
          <button @click="copyToClipboard">Copy To Clipboard</button>
          <button @click="download">Download</button>
          <button @click="reset">Reset</button>
        </div>
          <div class="p-sequence">
            <h2>Generated Sequence</h2>
            <h2><a class="gen_display">{{ display_sequence }}</a></h2>
          </div>
        <div id="formatType">
        <label for="data-format">Data Format:</label>
        <select v-model="format_type" id="data-format" name="data-format" @change="update_display_format">
          <option value="1">1</option>
          <option value="2">2</option>
          <option value="3" selected>3</option>
        </select>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ProteinService from '../../services/ProteinService.js';
import GenerateService from '../../services/GenerateService.js'
import FormatService from '../../services/FormatService.js';
export default {
  props: ['id'],

  data() {
    return {
      protein: {
        sequenceName: '',
        description: '',
        proteinSequence: '',
        blueSequence: [],
        greenSequence: [],
        yellowSequence: []
      },
      color: "",
      display_sequence: "",
      original_sequence: "",
      format_type: 1,
      show_copy: false,
      timer: 0,
    };
  },
  created() {
    this.loadProtein();
  },
  methods: {
    loadProtein() {
      const token = this.$store.state.token;
      const protein_id = this.$route.params.id;
      ProteinService.getProtein(token, protein_id)
        .then(response => {
          this.protein = response.data;
          this.display_sequence = this.protein.proteinSequence;
          this.original_sequence = this.protein.proteinSequence;
        })
        .catch(error => {
          console.log(error);
        });
    },
    generateSequences() {
      const token = this.$store.state.token;
      const protein_id = this.$route.params.id;
      GenerateService.generateSequences(token, protein_id)
        .then(response => {
          this.protein = response.data;
          this.display_sequence = this.protein.proteinSequence;
        })
        .catch(error => {
          console.log(error);
        })
    },
    download() {
      const link = document.createElement('a');
      link.href = 'data:text/plain;charset=utf-8,' + encodeURIComponent(this.display_sequence);
      link.download = this.protein.sequenceName + '.txt';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    },

    download_blue() {
      this.download_array(this.protein.blueSequence, '_blue');
    },

    download_green() {
      this.download_array(this.protein.greenSequence, '_green');
    },

    download_yellow() {
      this.download_array(this.protein.yellowSequence, '_yellow');
    },

    download_array(arry, file_name) {
      let buffer = ''

      for (let i = 0; i < arry.length; i++) {
        buffer += FormatService.format(this.format_type, arry[i])
        buffer += '\n----------------------------------------\n'
      }

      const link = document.createElement('a');
      link.href = 'data:text/plain;charset=utf-8,' + encodeURIComponent(buffer);
      link.download = this.protein.sequenceName + file_name + '.txt';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    },

    copyToClipboard() {
      const el = document.createElement('textarea');
      el.value = this.display_sequence;
      document.body.appendChild(el);
      el.select();
      document.execCommand('copy');
      document.body.removeChild(el);
    },

    copy_blue() {
      this.copy_array_to_clipboard(this.protein.blueSequence);
    },

    copy_green() {
      this.copy_array_to_clipboard(this.protein.greenSequence);
    },

    copy_yellow() {
      this.copy_array_to_clipboard(this.protein.yellowSequence);
    },

    copy_array_to_clipboard(arry) {
      let buffer = ''

      for (let i = 0; i < arry.length; i++) {
        buffer += FormatService.format(this.format_type, arry[i])
        buffer += '\n----------------------------------------\n'
      }

      const el = document.createElement('textarea');
      el.value = buffer;
      document.body.appendChild(el);
      el.select();
      document.execCommand('copy');
      document.body.removeChild(el);
    },

    change_display(data) {
      this.display_sequence = FormatService.format(this.format_type, data);
      this.copyToClipboard();
      this.show_copy_message();
    },

    update_display_format() {
      this.display_sequence = FormatService.format(this.format_type, this.display_sequence);
    },

    reset() {
      this.display_sequence = this.protein.proteinSequence;
      this.format_type = 1;
    },
    show_copy_message() {
      this.show_copy = true;
      this.timer = setTimeout(() => {
        this.show_copy = false;
      }, 3000);
    },

  },
}
</script>

<style scoped>
body {
  align-items: center;
  justify-content: center;
  height: 100vh;
  width: 100vw;
  margin: 0;
}

#singleSelect {
  margin: auto;
}

.card {
  height: fit-content;
  margin-left: auto;
  margin-right: auto;
  width: 70vw;
  color: black;
  border-radius: 2.5vh;
  background-color: aliceblue;
  border-color: black;
  border-style: solid;
  text-align: center;
  grid-area: card;
  display: grid;
  grid-template-columns: 1fr 1fr;
  align-items: center;
  grid-template-areas:
    "top-bar top-bar"
    "p-description p-description"
    "p-sequence p-sequence";
}

.top-bar {
  margin: auto;
  grid-area: top-bar;
  grid-template-columns: 1fr;
  grid-template-areas:
    "p-name";
}

.p-name {
  grid-area: p-name;
  display: inline-block;
  font-weight: bold;
}

.p-description {
  margin: auto;
  grid-area: p-description;
}

.p-sequence {
  margin: auto;
  margin-top: 10px;
  grid-area: p-sequence;
  word-break: break-all;
  margin-left: 25px;
  margin-right: 50px;
  border: black 2px;
  border-left: 0px;
  border-right: 0px;
  border-style: solid;
  margin-bottom: 20px;
  font-size: 11px;
}

#submit {
  align-items: right;
  justify-content: right;
  align-content: right;
  justify-self: right;
}

.p-generate {
  align-items: center;
  justify-content: center;
  align-content: center;
  width: 70vw;
}

.generatedCard {
  display: grid;
  justify-content: center;
  align-items: center;
  width: 70vw;
  color: black;
}

.card-labels {
  display: inline-flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  width: 70vw;
  margin: auto;
}

.fast {
  color: black;
  margin-right: auto;
  word-wrap: break-word;
  width: 33%;
}

.medium {
  color: black;
  word-wrap: break-word;
  width: 33%;
}

.slow {
  color: black;
  margin-left: auto;
  word-wrap: break-word;
  width: 33%;
}

.sequences {
  display: flex;
  width: 100%;
}

.blue-div {
  width: 33%;
  padding-left: 10px;
}

.sequence1 {
  word-wrap: break-word;
  margin-right: auto;
  border: black 2px;
  border-radius: 2.5vw;
  border-style: solid;
  background-color: rgba(0, 0, 255, .5);
  width: 100%;
  overflow: hidden;
  align-content: stretch;
  margin-bottom: 10px;
}

.sequence1:hover {
  display: block;
  cursor: pointer;
}

.green-div {
  padding-left: 10px;
  width: 33%;
  padding-right: 10px;
}

.sequence2 {
  word-wrap: break-word;
  margin-right: auto;
  border: black 2px;
  border-radius: 2.5vw;
  border-style: solid;
  background-color: rgba(0, 128, 0, .5);
  width: 100%;
  overflow: hidden;
  align-content: stretch;
  margin-bottom: 10px;

}

.sequence2:hover {
  display: block;
  cursor: pointer;
}

.yellow-div {
  width: 33%;
  padding-left: .4%;
  padding-bottom: 10px;
  padding-right: 10px;
  background-color: rgba(0, 0, 0, 0);
}

.sequence3 {
  word-wrap: break-word;
  margin-right: auto;
  border: black 2px;
  border-radius: 2.5vw;
  border-style: solid;
  background-color: rgba(255, 255, 0, .5);
  width: 100%;
  overflow: hidden;
  align-content: stretch;
  margin-bottom: 10px;

}

.sequence3:hover {
  display: block;
  cursor: pointer;
}

span.showSequence {
  position: absolute;
  display: none;
  padding: 3px;
  padding-left: 5px;
  background: #c0c0c0;
  border: 2px solid #000000;
  color: #000000;
  text-align: center;
  width: 105px;
  height: auto;
  white-space: break-spaces;
  word-wrap: break-word;
  top: 107px;
}

.protein-display {
  border: none;
}

div.sequence1:hover {
  background-color: rgba(0, 0, 255, .2);
}

div.sequence2:hover {
  background-color: rgba(0, 255, 0, .2);
}

div.sequence3:hover {
  background-color: rgba(255, 255, 0, .2);
}
#formatType{
  margin-bottom: 16px;
}
span.speed-details {
  position: absolute;
  display: none;
  padding: 5px;
  background: #c0c0c0;
  border: 2px solid #000000;
  color: #000000;
  text-align: center;
  width: 24vw;
  height: auto;
  white-space: break-spaces;
  word-wrap: break-word;
  top: 50vh;
}

div.sequence1:hover span.speed-details,
div.sequence2:hover span.speed-details,
div.sequence3:hover span.speed-details {
  display: block;
  transform: scale(1);
}

.gen_display {
  word-wrap: break-word;
  white-space: pre-line;
  text-align: left;
  color: #000;
  display: block;
  font-size: 98%;
}
</style>
