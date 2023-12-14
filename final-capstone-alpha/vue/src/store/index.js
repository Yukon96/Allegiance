import { createStore as _createStore } from 'vuex';
import axios from 'axios';

export function createStore(currentToken, currentUser) {
  let store = _createStore({
    state: {
      token: currentToken || '',
      user: currentUser || {}, 
      protein: {
        sequenceName: '',
        description: '',
        proteinSequence: '',
        blueSequence: [],
        greenSequence: [],
        yellowSequence: []
      },
      color: '',
    },
    mutations: {
      SET_AUTH_TOKEN(state, token) {
        state.token = token;
        localStorage.setItem('token', token);
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      },
      SET_USER(state, user) {
        state.user = user;
        localStorage.setItem('user', JSON.stringify(user));
      },
      LOGOUT(state) {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        state.token = '';
        state.user = {};
        axios.defaults.headers.common = {};
      },
      PASSPROTEIN(state, protein){
        state.protein = protein;
        localStorage.setItem('protein', JSON.stringify(protein));
      },
      CLEARPROTEIN(state){
        state.protein = {};
        state.color = {};
      },
      GETPROTEIN(state){
        state.protein = JSON.parse(localStorage.getItem('protein'));
      }
    },
  });
  return store;
}
