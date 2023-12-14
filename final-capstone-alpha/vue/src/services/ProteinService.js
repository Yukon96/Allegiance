import axios from 'axios';

export default {
  getProteins(sessionToken) {
    return axios.get('/protein', {headers: {Authorization: `Bearer ${sessionToken}`}});
  },

  getProtein(sessionToken, proteinId) {
    return axios.get(`/protein/${proteinId}`, {headers: {Authorization: `Bearer ${sessionToken}`}});
  },

  getProteinByName(sessionToken, name) {
    return axios.get(`/protein/name/${name}`, {headers: {Authorization: `Bearer ${sessionToken}`}});
  },

  createProtein(sessionToken, protein) {
    return axios.post('/protein', protein, {headers: {Authorization: `Bearer ${sessionToken}`}});
  },

  updateProtein(sessionToken, protein) {
    return axios.put(`/protein`, protein, {headers: {Authorization: `Bearer ${sessionToken}`}});
  },

  deleteProtein(sessionToken, proteinId) {
    return axios.delete(`/protein/${proteinId}`, {headers: {Authorization: `Bearer ${sessionToken}`}});
  },
  ncbiAPI(sessionToken, name){
    return axios.get(`/api/ncbi/${name}`, {headers: {Authorization: `Bearer ${sessionToken}`}});
  },
  rcsbAPI(sessionToken, name){
    return axios.get(`/api/rcsb/${name}`, {headers: {Authorization: `Bearer ${sessionToken}`}});
  },
}