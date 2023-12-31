<template>
    <div class="wrapper">
        <v-card class="card" :loading="loading">

            <v-card-title>
                <v-tabs v-model="page" bg-color="primary">
                    <v-tab value="login"> <v-icon>mdi-login</v-icon>Login</v-tab>
                    <v-tab value="register"><v-icon>mdi-account-plus</v-icon>Register</v-tab>
                </v-tabs>
            </v-card-title>
            <v-card-text>
                <v-alert :type="alert.type" v-if="alert.open">{{ alert.msg }}</v-alert>
                <v-form class="mx-2">
                    <v-text-field prepend-icon="person" type="text" v-model="user.name" label="Name"></v-text-field>
                    <v-text-field prepend-icon="lock" type="password" v-model="user.password"
                        label="Password"></v-text-field>
                </v-form>
            </v-card-text>
            <v-card-actions class="d-flex justify-end">
                <v-btn color="primary" @click="login" v-if="page === 'login'">Login</v-btn>
                <v-btn color="primary" @click="register" v-else>Register</v-btn>
            </v-card-actions>
        </v-card>
    </div>
</template>
<script>
import http from '@/http'
import store from '@/store'
export default {
    name: "Login",
    data() {
        return {
            user: {
                name: "",
                password: ""
            },
            alert: {
                openAlert: false,
                type: "error",
                msg: ""
            },
            loading: false,
            page: 'login'
        };
    },
    methods: {
        login() {
            http.login({ Username: this.user.name, Password: this.user.password })
                .then(async res => {
                    if (!res.ok) {
                        throw new Error(await res.text());
                    }
                    return res.text()
                })
                .then(res => {
                    this.controlAlert(true, "success", res, 3000)
                    this.$store.dispatch('setLoginStatus', true)
                }).catch(err => {
                    this.controlAlert(true, "error", err)
                })
        },
        register() {
            http.register({ Username: this.user.name, Password: this.user.password })
                .then(async res => {
                    if (!res.ok) {
                        throw new Error(await res.text());
                    }
                    return res.text()
                })
                .then(res => {
                    this.controlAlert(true, "success", res, 3000)
                }).catch(err => {
                    this.controlAlert(true, "error", err)
                })
        },
        controlAlert(open, type, msg, timeout) {
            this.alert = { open, type, msg }
            if (timeout) {
                setTimeout(() => { this.alert.open = false }, timeout)
            }
        }
    }
};
</script>

<style scoped>
.wrapper {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    min-height: 100vh;
}


.card {
    width: 70%;
}
</style>