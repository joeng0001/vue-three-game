<template>
    <div class="container">
        <v-progress-circular indeterminate color="purple" :size="100">
            Loading model...
        </v-progress-circular>
    </div>
</template>

<script>
export default {
}
</script>

<style scoped>
.container {
    position: absolute;
    top: 50%;
    left: 50%;
    text-align: center;
}
</style>