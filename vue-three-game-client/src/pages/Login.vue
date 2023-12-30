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
                <v-alert type="error" v-if="loginFail.status" />
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
export default {
    name: "Login",
    data() {
        return {
            user: {
                name: "",
                password: ""
            },
            loginFail: {
                status: false,
                msg: ""
            },
            loading: false,
            page: 'login'
        };
    },
    methods: {
        login() {
            http.test().then(res => {
                console.log("test")
            }).catch(err => {
                console.error("error in test")
            })
        },
        register() {
            console.log("calling register")
            http.register({ name: this.user.name, pw: this.user.password }).then(res => {
                console.log("register")
            }).catch(err => {
                console.error("error in register")
            })
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