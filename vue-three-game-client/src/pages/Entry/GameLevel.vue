<template>
    <v-dialog width="800" v-model="dialog" persistent>
        <v-card class="d-flex justify-center">
            <v-card-title class="d-flex justify-center SceneTitle">
                <span></span>
                <span></span>
                <span></span>
                <span></span>
                Pick A Level
            </v-card-title>

            <!--after pick the scene,move the camera the the game scenario-->
            <v-card-actions class="d-flex justify-space-between mt-4">
                <v-container>
                    <v-row>
                        <v-col cols="3">
                            <v-btn class="btn" style="color:Aquamarine;"
                                @click="() => $router.push(`/gameScene/${$route.query.scene}?level=1`)">1</v-btn>
                        </v-col>
                        <v-col cols="3">
                            <v-btn class="btn" style="color:GreenYellow;"
                                @click="() => $router.push(`/gameScene/${$route.query.scene}?level=2`)">2</v-btn>
                        </v-col>
                        <v-col cols="3">
                            <v-btn class="btn" style="color:DeepSkyBlue;"
                                @click="() => $router.push(`/gameScene/${$route.query.scene}?level=3`)">3</v-btn>
                        </v-col>
                        <v-col cols="3">
                            <v-btn class="btn" style="color:Lime;"
                                @click="() => $router.push(`/gameScene/${$route.query.scene}?level=4`)">4</v-btn>
                        </v-col>
                        <v-col cols="3">
                            <v-btn class="btn" style="color:Crimson;"
                                @click="() => $router.push(`/gameScene/${$route.query.scene}?level=5`)">5</v-btn>
                        </v-col>
                        <v-col cols="3">
                            <v-btn class="btn" style="color:darkred;"
                                @click="() => $router.push(`/gameScene/${$route.query.scene}?level=6`)">6</v-btn>
                        </v-col>
                        <v-col cols="3">
                            <v-btn class="btn" style="color:DeepPink;"
                                @click="() => $router.push(`/gameScene/${$route.query.scene}?level=7`)">7</v-btn>
                        </v-col>
                        <v-col cols="3">
                            <v-btn class="btn" style="color:magenta;"
                                @click="() => $router.push(`/gameScene/${$route.query.scene}?level=8`)">8</v-btn>
                        </v-col>
                    </v-row>
                </v-container>


            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>

export default {
    data() {
        return {
            dialog: true
        }
    }
}

</script>

<style scoped>
.btn {
    background-color: rgba(224, 223, 223, 0.5);
    font-weight: 700;
    font-size: 30px;
}

.SceneTitle {
    margin-bottom: 20px;

    position: relative;
    display: inline-block;
    padding: 10px 20px;
    color: #03e9f4;
    font-size: 16px;
    text-decoration: none;
    text-transform: uppercase;
    overflow: hidden;
    transition: .5s;
    margin-top: 40px;
    letter-spacing: 4px;

}

.SceneTitle span {
    position: absolute;
    display: block;
}

.SceneTitle span:nth-child(1) {
    top: 0;
    left: -100%;
    height: 2px;
    width: 100%;
    background: linear-gradient(90deg, transparent, #03e9f4);
    animation: btn-anim1 1s linear infinite;
}

@keyframes btn-anim1 {
    0% {
        left: -100%;
    }

    50%,
    100% {
        left: 100%;
    }
}

.SceneTitle span:nth-child(2) {
    top: -100%;
    right: 0;
    width: 2px;
    height: 100%;
    background: linear-gradient(180deg, transparent, #03e9f4);
    animation: btn-anim2 1s linear infinite;
    animation-delay: .25s;
}

@keyframes btn-anim2 {
    0% {
        top: -100%;
    }

    50%,
    100% {
        top: 100%;
    }
}

.SceneTitle span:nth-child(3) {
    bottom: 0;
    right: -100%;
    width: 100%;
    height: 2px;
    background: linear-gradient(270deg, transparent, #03e9f4);
    animation: btn-anim3 1s linear infinite;
    animation-delay: .5s;
}

@keyframes btn-anim3 {
    0% {
        right: -100%;
    }

    50%,
    100% {
        right: 100%;
    }
}

.SceneTitle span:nth-child(4) {
    bottom: -100%;
    left: 0;
    width: 2px;
    height: 100%;
    background: linear-gradient(360deg, transparent, #03e9f4);
    animation: btn-anim4 1s linear infinite;
    animation-delay: .75s;
}

@keyframes btn-anim4 {
    0% {
        bottom: -100%;
    }

    50%,
    100% {
        bottom: 100%;
    }
}

.v-card {
    background-color: rgba(255, 255, 255, 0);
}

.v-card--variant-elevated {
    box-shadow: none;
}

.v-dialog>.v-overlay__content>.v-card {
    box-shadow: none;
}
</style>