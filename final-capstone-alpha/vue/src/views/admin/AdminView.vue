<template>
        <div>
            <h2>Admin Tools<ProteinExport></ProteinExport></h2>
            <ul>
                <li class="a"><router-link id="admin-otp" v-bind:to="{ name: 'otp' }">OTP Password Reset</router-link></li>
                <li class="a"><router-link id="admin-editHome" v-bind:to="{ name: 'editHome'}">Edit Welcome Screen</router-link></li>
                <ProteinExport></ProteinExport>
            </ul>
        </div>
</template>

<script>
export default {
    data() {
        return {
            components: {
            }
        };
    },
    components: {  }
}
</script>

<style>
</style>
