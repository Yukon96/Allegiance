import axios from "axios";
export default {
    generateSequences(sessionToken, id) {
        return axios.patch(`/generator/${id}`, {headers: {Authorization: `Bearer ${sessionToken}`}})
    }
}