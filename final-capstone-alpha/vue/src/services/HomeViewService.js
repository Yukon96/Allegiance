import axios from "axios"
export default{
getView() {
    return axios.get('home')
  },
getAllViews(){
    return axios.get('home/views')
},
postView(view) {
    return axios.post('home', view)
}, 
putDisplayView(id){
    return axios.put(`home/${id}`)
},
deleteView(id){
    return axios.delete(`home/${id}`)
},
}