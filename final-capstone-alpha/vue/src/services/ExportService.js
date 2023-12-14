import axios from 'axios';

export default {
    
  export(user) {
    return axios.post('/export', user)
  },
}
