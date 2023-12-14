import axios from "axios"

export default {
  changePassword(user) {
    return axios.put('user/changepassword', user)
  },
  viewProteins(username, sessionToken) {
    return axios.get(`protein/user/${username}`, {headers: {Authorization: `Bearer ${sessionToken}`}})
  }

}
