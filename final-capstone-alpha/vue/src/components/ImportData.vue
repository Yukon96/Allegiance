<template>
  <h1>Import data</h1>
  <div id="data-input">
    <form id="text-form">
      <div class="name">
        <label for="nameText" id="nameLabel">Name</label>
        <input type="text" id="nameText" v-model="protein.name" required />
      </div>
      <div class="note">
        <label for="noteText" id="noteLabel">Note</label>
        <input type="text" id="noteText" v-model="protein.description" />
      </div>
      <div class="data">
        <label for="dataField" id="dataLabel">Sequence</label>
        <textarea rows="20" cols="70" id="dataField" v-model="protein.data" required></textarea>
      </div>
    </form>
    <div class="import">
      <form id="api-form" @submit.prevent="getProteinAPIfromNCBI()">
        <label for="apiText" id="apiLabel">Get Info From NCBI</label>
        <input type="text" id="apiText" v-model="apiDataNCBI" />
        <button type="submit" id="apiSubmit">Import Data</button>
      </form>
      <form id="RCSB-form" @submit.prevent="getProteinAPIfromRCSB()">
        <label for="apiText" id="apiLabel">Get Info From RCSB</label>
        <input type="text" id="apiText" v-model="apiDataRCSB" />
        <button type="submit" id="apiSubmit">Import Data</button>
      </form>
      <form class="file">
        <label for="fileInput" id="fileLabel">Upload From File</label>
        <input type="file" id="fileInput" v-on:change="importFile" />
      </form>
      <form class="finished" @submit.prevent="importTextArea">
        <button id="clearForm" v-on:click="clearForm()">Clear Form</button>
        <button id="submit" type="submit">Submit Data</button>
      </form>
    </div>
  </div>
</template>

<script>
import ProteinService from '../services/ProteinService.js';
export default {
  data() {
    return {
      protein: {
        name: '',
        description: '',
        data: '',
        proteinId: ''
      },
      apiDataNCBI: '',
      apiDataRCSB: '',
      proteinName: '',
      proteinDataBox: '',
      file: null,
    };
  },
  methods: {
    clearForm() {
      this.protein = {};
      this.apiData = '';
      this.apiDataNCBI = '';
      this.apiDataRCSB = '';
    },
    getProteinAPIfromNCBI() {
      ProteinService.ncbiAPI(this.$store.state.token, this.apiDataNCBI).then((respsonse) => this.assignProtein(respsonse)
      )
    },
    getProteinAPIfromRCSB() {
      ProteinService.rcsbAPI(this.$store.state.token, this.apiDataRCSB).then((respsonse) => this.assignProtein(respsonse)
      )
    },

    assignProtein(response) {
      this.protein.name = response.data.sequenceName;
      this.protein.data = response.data.proteinSequence;
      this.protein.description = response.data.description;
      this.protein.proteinId = response.data.proteinId;
    },
    importTextArea() {
      const token = this.$store.state.token;
      const protein_data = {
        SequenceName: this.protein.name,
        ProteinSequence: this.protein.data,
        Description: this.protein.description,
        FormatType: 0,
        UserId: 0
      };
      if (!this.validateEntries()) {
        alert("Fields cannot be blank.")
      } else if (!this.validateLength()){
        alert("Please enter a valid protein.") 
      } else {
        ProteinService.createProtein(token, protein_data)
        .then(response => {
          if (response.status === 201) {
            alert("Protein created successfully");
            this.$router.push(`/protein/${response.data.proteinId}`);
          }
        })
        .catch(error => {
          const response = error.response;
          if (response.status === 401) {
            alert("Invalid password");
          }
        });
      }
    },
    importFile(evt) {
      const file = evt.target.files[0];
      if (file.name.includes(".txt")) {
        const reader = new FileReader();
        reader.onload = e => {
          this.protein.name = file.name.replace(".txt", "");
          this.protein.data = e.target.result;
        }
        reader.readAsText(file);
      }
      else {
        alert("Your file must be uploaded in .txt format.");
      }
    },
    validateEntries() {
      if(this.protein.name === "" || this.protein.description === "" || this.protein.data === "") {
        return false;
      }
      return true;
    },
    validateLength() {
      if (this.protein.data.length < 20) {
        return false;
      }
      return true;
    }
  }
};
</script>

<style scoped>
h1 {
  text-align: center;
}

#data-input {
  height: fit-content;
  margin-left: auto;
  margin-right: auto;
  width: 60%;
  height: 90%;
  color: black;
  border-radius: 2.5vh;
  background-color: aliceblue;
  border-color: black;
  border-style: solid;
  text-align: center;
  grid-area: data-input;
  display: grid;
  column-gap: 50px;
  grid-row-gap: 20px;
  grid-template-columns: 1fr 1fr 1fr 1fr;
  align-items: center;
  grid-template-areas:
    "text-form text-form text-form text-form"
    "text-form text-form text-form text-form"
    "text-form text-form text-form text-form"
    "text-form text-form text-form text-form"
    "import import import import"
}

#text-form {
  margin: auto;
  grid-area: text-form;
  display: grid;
  column-gap: 5px;
  align-items: center;
  grid-row-gap: 20px;
  grid-template-columns: 1fr 1fr;
  grid-template-areas:
    "name note"
    "data data"
    "data data"
    "data data"
}

.name {
  object-fit: contain;
  display: block;
  grid-area: name;
  grid-template-columns: 1fr 1fr;
  grid-template-areas:
    "nameLabel" "NameText"
}

#nameLabel {
  margin: auto;
  width: auto;
  grid-area: nameLabel;
}

#nameText {
  margin: auto;
  width: auto;
  grid-area: nameText;
}

.note {
  margin-left: 1px;
  margin-right: 1px;
  object-fit: contain;
  display: block;
  grid-area: note;
  grid-template-columns: 1fr 1fr;
  grid-template-areas:
    "noteLabel" "noteText";
}

#noteLabel {
  grid-area: noteLabel;
}

#noteText {
  margin: auto;
  width: auto;
  grid-area: noteText;
}

.data {
  margin: auto;
  grid-area: data;
  grid-template-columns: 1fr;
  grid-template-areas:
    "dataField";
}

#dataField {
  grid-area: dataField;
  height: 100%;
  width: 100%;
}

.import {
  margin: auto;
  grid-area: import;
  grid-row-gap: 5px;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr;
  grid-template-areas:
    "api-form api-form file file submit"
}

#api-form {
  margin: auto;
  grid-area: api-form;
  grid-template-columns: 1fr 1fr 1fr;
  grid-template-areas:
    "apiLabel apiText apiSubmit";
}

#apiLabel {
  grid-area: apiLabel;
}

#apiText {
  margin: auto;
  width: auto;
  grid-area: apiText;
}

#apiSubmit {
  grid-area: apiSubmit;
}

.file {
  grid-area: file;
  grid-template-columns: 1fr 1fr;
  grid-template-areas:
    "fileLabel fileSubmit";
}

.finished {
  display: flex;
  align-items: center;
  justify-content: center;
  margin: auto;
  margin-top: 15px;
  margin-bottom: 10px;
}
#submit {
  align-items: right;
  justify-content: right;
  margin: auto;
}

#clearForm {
  margin: auto;
}
</style>