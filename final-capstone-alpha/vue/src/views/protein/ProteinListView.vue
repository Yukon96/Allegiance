<template>
  <div class="protein-list-view">
    <div class="table-container">
      <table class="p-table">
        <thead>
            <tr id="header">
              <th id="name">Name</th>
              <th>Note</th>
            </tr>
          </thead>
          <tbody v-for="(p, idx) in filteredProteins" :key="p.proteinId" :class="{'highlight': idx % 2 === 0}">
          <tr>
            <th id="nameRows">
              <router-link :to="{ name: 'protein_detail', params: { id: p.proteinId } }"> {{ p.sequenceName }}</router-link>
            </th>
            <td class="dec-column">{{ p.description }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="search">
      <input class="search_box" type="text" v-model="search.name" placeholder="Search by name">
    </div>
  </div>
</template>

<script>
import ProteinService from '../../services/ProteinService.js';

export default {
  data() {
    return {
      protein: {
        name: '',
        description: '',
        sequence: '',
      },
      proteins: [],
      search: {
        name: '',
      },
    }
  },

  created() {
    this.loadProteins();
  },

  methods:  {
    loadProteins() {
      let token = this.$store.state.token;
      ProteinService.getProteins(token)
        .then(response => {
          this.proteins = response.data;
        })
        .catch(error => {
          console.log(error);
        })
    },


  },

  computed: {
    filteredProteins() {
      let protein_filter = this.proteins;
      const name = this.search.name
      if (name != '') {
        protein_filter = protein_filter.filter(p => {
          return p.sequenceName.toLowerCase().includes(name.toLowerCase())
        })
      }

      return protein_filter
    }
  },
}
</script>

<style scoped>
.protein-list-view {
  padding-top: 25px;
  margin-top: 40px;
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
  grid-area: protein-list-view;
  display: grid;
  grid-template-columns: 1fr 1fr;
  justify-content: center;
  align-items: center;
  grid-template-areas:
    "table-container table-container"
    "search search"
}

.search {
  margin: auto;
  margin-bottom: 10px; /* Optional margin between search box and table */
  width: 100%;
  padding-bottom: 10px;
  grid-area: search;
  justify-content: center;
  align-items: center;
  transform:scale(1.5);
}

.table-container {
  display: flex;
  grid-area: table-container;
  padding-bottom: 2%;
}


.p-table {
  border-collapse: collapse;
  border-top: 2px solid black;
  border-bottom: 2px solid black;
  border-radius: 2.5vw;
  margin: auto;
  color: black;
  width: 100%;
  font-size: 25px;
}

.highlight {
  background-color: rgb(223, 222, 222);
}

tr#header {
  border-bottom: 2px solid black;
}
th#name, th#nameRows {
  border-right: 2px solid black;
}
</style>
