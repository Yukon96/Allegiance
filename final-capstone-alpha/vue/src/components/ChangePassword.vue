<template>
  <div id="password-change">
    <form id="password-change-form" @submit.prevent="changePassword">
      <h1 id="password-change-header">Change Password</h1>
      <div class="form-input-group" id="password-new">
        <label id="password-new-label" for="newPassword">New Password</label>
        <input type="password" id="password-new-pass" v-model="password" />
      </div>
      <div class="form-input-group" id="password-confirm">
        <label id="password-confirm-label" for="confirmPassword">Confirm Password</label>
        <input type="password" id="password-confirm-password" v-model="confirmPassword" />
      </div>
      <button id="password-submit" type="submit">Change Password</button>
    </form>
  </div>
</template>

<script>
import UserService from '../services/UserService';

export default {
  data() {
    return {
      password: "",
      confirmPassword: "",
    };
  },
  methods: {
    changePassword() {
      const session_token = this.$store.state.token;
      if (this.password == "") {
        alert("Password may not be empty.");
        return;
      }
      if (this.password !== this.confirmPassword) {
        alert("Passwords do not match");
        return;
      }
      const login_data = {
        username: this.$store.state.user.username,
        password: this.password,
        confirmPassword: this.confirmPassword
      };
      UserService.changePassword(login_data)
        .then(response => {
          if (response.status === 200) {
            alert("Password changed successfully");
            this.$router.push("/logout");
          }
        })
        .catch(error => {
          const response = error.response;
          if (response.status === 401) {
            alert("Invalid password");
          }
        });

    }
  }
};
</script>

<style scoped>
.form-input-group {
  margin-bottom: 1rem;
}

label {
  margin-right: 0.5rem;
}
</style>