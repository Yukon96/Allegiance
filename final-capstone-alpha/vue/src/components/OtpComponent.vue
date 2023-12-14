<template>
  <h1>One time Password</h1>
    <div id="otp">
      <table>
        <thead>
          <tr>
            <th></th>
            <th>User Id</th>
            <th>Username</th>
            <th>Email</th>
            <th>Role</th>
          </tr>
        </thead>
        <tbody>
          <td></td>
          <td><input type="text" id="filter_id" v-model="search.userId" placeholder="User ID"></td>
          <td><input type="text" id="filter_name" v-model="search.username" placeholder="Username"></td>
          <td><input type ="text" id="filter_email" v-model="search.email" placeholder="Email"></td>
          <td><input type="text" id="filter_role" v-model="search.role" placeholder="Role"></td>
        </tbody>
        <tbody v-for="user in filteredList" :key="user.userId"
          :class="{'selected': selected_user.userId === user.userId}">
          <tr @click="clickLog(user)">
            <td><input v-model="checked_user" :id="user.userId" type="radio" :value="user.userId" v-on:click="clickLog(user)"/></td>
            <td>{{ user.userId }}</td>
            <td>{{ user.username }}</td>
            <td>{{ user.email }}</td>
            <td>{{ user.role }}</td>
          </tr>
        </tbody>
      </table>
        <div v-show="has_selected_user">
          <h3>Are you sure you want to select this user?</h3>
          <p>Username: {{ selected_user.username }}</p>
          <button @click="confirmUser">Confirm User</button>
          <button @click="cancelUserSelection">Cancel</button>
          
        </div>
        <p v-show="one_time_password">One time password: {{ one_time_password }}</p>
        <button v-show="one_time_password" @click="clearOneTimePass">Clear</button>

    </div>
</template>

<script>
import AdminService from '../services/AdminService';
export default {
  name: 'OtpComponent',
  data() {
    return {
      user: {
        userId:Number,
        username: String,
        email: String,
        role: String,
      },
      search: {
        userId: '',
        username: '',
        email: '',
        role: '',
      },
      selected_user: {},
      user_list: [],
      one_time_password: '',
      checked_user: 0,
    }
  },
  created() {
    this.getUsers();
  },
  methods: {
    getUsers() {
      let session_token = this.$store.state.token;
      AdminService.listUsers(session_token)
        .then(response => {
          this.user_list = response.data;
        })
        .catch(error => {
          console.log(error);
        });
    },
    clickLog(user) {
      this.has_selected_user = true;
      this.selected_user = user;
      this.checked_user = user.userId;
    },
    confirmUser() {
      this.setOneTimePass();
      this.cancelUserSelection();
    },
    cancelUserSelection() {
      this.selected_user = {};
    },

    //Todo(anderson): Handle error checking
    setOneTimePass() {
      let session_token = this.$store.state.token;
      AdminService.generateOTP(session_token, this.selected_user.userId)
        .then(response => {
          this.one_time_password = response.data;
        })
        .catch(error => {
          console.log(error);
        });
    },

    //Todo(anderson): Handle error checking
    clearOneTimePass() {
      this.one_time_password = '';
    }
  },
  computed: {
    filteredList() {
      let user_filter = this.user_list;
      const { userId, username, email, role} = this.search;

      
      if (userId != '') {
        user_filter = user_filter.filter(user => user.userId == userId);
      }
      
      if (username != '') {
        user_filter = user_filter.filter(user =>  user.username.toLowerCase().includes(username.toLowerCase()));
      }

      if (email != '') {
        user_filter = user_filter.filter(user => user.email == email);
      }
      
      if (role != '') {
        user_filter = user_filter.filter(user => user.role.toLowerCase().includes(role.toLowerCase()));
      }

      return user_filter

    },
  }
}

</script>

<style scoped>
.hide {
  display: none;
}


.tr.selected {
  color: #4CAF50;
}
#otp {
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
  justify-items: center;
}
</style>
