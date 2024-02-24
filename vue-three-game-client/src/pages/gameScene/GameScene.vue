<template>
    <div>
        <div class="header">
            <v-btn @click="() => $router.push({ name: 'entry.gameMode' })" class="mx-2">Home </v-btn>
            <v-btn @click="logout" class="mx-2"> Logout </v-btn>
            <v-btn @click="() => dialog = true" class="mx-2">Control</v-btn>
        </div>
        <Tutorial :subTitle="$route.query.scene" :dialog="dialog" :closeDialog="() => dialog = false"></Tutorial>
        <router-view></router-view>
    </div>
</template>


<script>
import http from "@/http"
import Tutorial from "@/components/Tutorial"
export default {
    components: { Tutorial },
    data() {
        return {
            dialog: false
        }
    },
    methods: {
        logout() {
            http.logout()
            this.$store.dispatch('setLoginStatus', false)
            this.$store.dispatch('setUserID', null)
            this.$store.dispatch('setSpaceShipProfile', [])
            this.$store.dispatch('setMarsRoverProfile', [])
            this.$router.push("login")
        }
    }
}
</script>

<style scoped>
.header {
    position: absolute;
    top: 0;
    right: 0;
    background-color: rgba(255, 255, 255, 0.5);
    z-index: 99 !important;
}
</style>