<template>
    <div>this login page</div>
    <button @click="addId">add </button>
    <button @click="removeId">remove </button>
    <div>{{ this.$store.state.userID }}</div>
    <v-btn>vuetify</v-btn>
</template>

<script>

export default {
    mounted() {

    },
    methods: {
        addId() {
            this.$store.dispatch('setUserID', 1)
        },
        removeId() {
            this.$store.dispatch('setUserID', 0)
        }
    }
}

</script>