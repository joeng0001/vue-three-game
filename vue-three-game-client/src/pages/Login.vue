<template>
    <div class="wrapper">
        <v-card class="card" :loading="loading">

            <v-card-title>
                <!-- <v-toolbar color="primary" :dark="true">
                    <v-icon>mdi-login</v-icon>
                    <v-toolbar-title>Login</v-toolbar-title>
                </v-toolbar> -->
                <v-tabs v-model="tab" bg-color="primary">
                    <v-tab value="login"> <v-icon>mdi-login</v-icon>Login</v-tab>
                    <v-tab value="register">Register</v-tab>
                </v-tabs>
            </v-card-title>
            <v-card-text>
                <v-alert type="error" v-if="loginFail.status">
                </v-alert>
                <v-form class="mx-2">
                    <v-text-field prepend-icon="person" type="text" v-model="user.name" label="Name"></v-text-field>
                    <v-text-field prepend-icon="lock" type="password" v-model="user.password"
                        label="Password"></v-text-field>
                </v-form>
            </v-card-text>
            <v-card-actions class="d-flex justify-end">
                <v-btn color="primary" @click="login">Login</v-btn>
            </v-card-actions>
        </v-card>
    </div>
</template>
<script>

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
            tab: 'login'
        };
    },
    methods: {
        login() {
            this.cnt += 1;
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