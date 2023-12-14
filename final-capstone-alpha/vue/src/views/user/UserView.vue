<template>
  <div>
    <h2>Welcome {{ name }}</h2>
    <ul>
      <li><router-link v-bind:to="{ name: 'change_password' }">Change Password</router-link></li>
      <li><router-link v-bind:to="{ name: 'view_proteins' }">View your proteins</router-link></li>
    </ul>
  </div>
</template>


<script>
export default {
  data() {
    return {
      name:'',
    };
  },
  methods: {

  },
  created() {
    this.name = this.$store.state.user.username;
  }

};
</script>

<style scoped>
</style>
