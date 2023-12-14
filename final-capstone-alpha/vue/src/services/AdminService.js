import axios from "axios";


export default {
    listUsers(sessionToken) {
        return axios.get('/admin/users', { headers: { Authorization: `Bearer ${sessionToken}` } });
    },

    generateOTP(sessionToken, userId) {
        return axios.put(`/admin/resetpassword/${userId}`, { headers: { Authorization: `Bearer ${sessionToken}` } });
    },
}