<template>
    <v-dialog fullscreen v-model="dialog" persistent>
        <v-form class="ml-1 w-100" @submit.prevent="submitProfile">
            <v-card>
                <v-card-title class="SceneTitle">
                    config your vehicle
                </v-card-title>

                <v-card-item class="card-content">
                    <div class="d-flex">
                        <canvas ref="vehicle" class="canvas-bg"></canvas>

                        <v-container>
                            <v-row>
                                <v-progress-linear indeterminate color="yellow-darken-2"
                                    v-show="loading"></v-progress-linear>
                                <v-col cols="12">
                                    <v-tabs bg-color="primary" v-model="profile_index">
                                        <v-tab v-for="(_, i) in profile_list" :value="i">profile {{ i + 1 }}</v-tab>
                                    </v-tabs>

                                </v-col>
                            </v-row>
                            <div v-if="$route.query.scene == 'Universe'">
                                <v-row>
                                    <v-col cols="2">
                                        Ammo
                                    </v-col>
                                    <v-col cols="9">
                                        <v-slider min="0" max="50" :step="1" name="ammo"
                                            v-model="profile_list[profile_index].ammo" />
                                    </v-col>
                                    <v-col cols="1">
                                        {{ profile_list[profile_index].ammo }}
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="2">
                                        Energy
                                    </v-col>
                                    <v-col cols="9">
                                        <v-slider min="0" max="50" :step="1" name="energy"
                                            v-model="profile_list[profile_index].energy" />
                                    </v-col>
                                    <v-col cols="1">
                                        {{ profile_list[profile_index].energy }}
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="2">
                                        Life
                                    </v-col>
                                    <v-col cols="9">
                                        <v-slider min="0" max="60" :step="1" name="life"
                                            v-model="profile_list[profile_index].life" />
                                    </v-col>
                                    <v-col cols="1">
                                        {{ profile_list[profile_index].life }}
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="2">
                                        Energy consumption
                                    </v-col>
                                    <v-col cols="9">
                                        <v-slider min="0" max="0.001" :step="0.0001" name="energyConsume"
                                            v-model="profile_list[profile_index].energyConsume" />
                                    </v-col>
                                    <v-col cols="1">
                                        {{ profile_list[profile_index].energyConsume }}
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="2">
                                        Life consumption
                                    </v-col>
                                    <v-col cols="9">
                                        <v-slider min="0" max="0.0001" :step="0.00001" name="lifeConsume"
                                            v-model="profile_list[profile_index].lifeConsume" />
                                    </v-col>
                                    <v-col cols="1">
                                        {{ profile_list[profile_index].lifeConsume }}
                                    </v-col>
                                </v-row>
                            </div>
                            <div v-else>
                                <v-row>
                                    <v-col cols="2">
                                        Oil
                                    </v-col>
                                    <v-col cols="9">
                                        <v-slider min="0" max="10" :step="1" name="ammo"
                                            v-model="profile_list[profile_index].oil" />
                                    </v-col>
                                    <v-col cols="1">
                                        {{ profile_list[profile_index].oil }}
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="2">
                                        Energy
                                    </v-col>
                                    <v-col cols="9">
                                        <v-slider min="0" max="10" :step="1" name="energy"
                                            v-model="profile_list[profile_index].energy" />
                                    </v-col>
                                    <v-col cols="1">
                                        {{ profile_list[profile_index].energy }}
                                    </v-col>
                                </v-row>
                            </div>
                        </v-container>

                    </div>
                </v-card-item>

                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn v-if="profile_index != profile_list.length - 1 || no_more_profile"
                        @click="() => $router.push(`/gameScene/${$route.query.scene}?level=${$route.query.level}&profileID=${profile_list[profile_index].id}&scene=${$route.query.scene}`)"
                        class="mr-6">Start</v-btn>
                    <v-btn class="mr-6" v-if="profile_index == profile_list.length - 1 && !no_more_profile"
                        type="submit">create</v-btn>
                    <v-btn class="mr-6" v-else type="submit">update</v-btn>
                    <v-btn @click="() => $router.push(`/entry/gameLevel?scene=${$route.query.scene}`)" class="mr-6">Back
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-form>
        <v-snackbar v-model="snackbar.open" :color="snackbar.color">
            {{ snackbar.text }}
            <template v-slot:actions>
                <v-btn color="red" variant="text" @click="snackbar.open = false">
                    Close
                </v-btn>
            </template>
        </v-snackbar>
    </v-dialog>
</template>

<script>
import * as THREE from "three"
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { GLTFLoader } from "three/examples/jsm/loaders/GLTFLoader";
import store from "@/store"
import http from "@/http";
export default {
    mounted() {
        this.initModelDisplay()
    },
    beforeUnmount() {
        if (this.renderer) {
            this.renderer.setAnimationLoop(null)
        }
    },
    data() {
        return {
            dialog: true,
            profile_index: 0,
            profile_list: [],
            profile_template: {},
            no_more_profile: false,
            snackbar: {
                open: false,
                text: "",
                color: "blue-grey"
            },
            loading: false,
            renderer: null,
        }
    },
    created() {
        this.initProfileList()
    },
    methods: {
        initModelDisplay() {
            const gltfLoader = new GLTFLoader()
            const scene = new THREE.Scene();
            const canvas = this.$refs.vehicle
            const renderer = new THREE.WebGLRenderer({ canvas, antialias: true, alpha: true });
            this.renderer = renderer
            const spaceShipUrl = new URL('@/assets/model/spaceShip.glb', import.meta.url)
            renderer.setSize(500, 500);
            const camera = new THREE.PerspectiveCamera(
                45,
                window.innerWidth / window.innerHeight,
                0.1,
                1000
            );
            const orbit = new OrbitControls(camera, renderer.domElement);
            const clock = new THREE.Clock();
            camera.position.set(5, 5, -1);
            orbit.update();
            const ambientLight = new THREE.AmbientLight();
            scene.add(ambientLight);

            let modelAnimation = null
            let spaceShip = null
            gltfLoader.load(spaceShipUrl.href, function (gltf) {
                const model = gltf.scene;
                scene.add(model);
                spaceShip = model
                model.position.set(0, 0, 0)

                modelAnimation = new THREE.AnimationMixer(model);
                modelAnimation.timeScale = 2.0
                const clips = gltf.animations;

                clips.forEach(function (clip) {
                    const action = modelAnimation.clipAction(clip);
                    action.play();
                });

            });

            const playAnimation = () => {
                if (!modelAnimation) return
                const delta = clock.getDelta();
                modelAnimation.update(delta);
            }

            const playRotation = () => {
                if (!spaceShip) return
                spaceShip.rotateY(0.004);
            }
            function animate() {
                playAnimation()
                playRotation()
                renderer.render(scene, camera);
            }

            renderer.setAnimationLoop(animate);
        },
        initProfileList() {
            if (this.$route.query.scene == "Universe") {
                this.profile_list = JSON.parse(JSON.stringify(store.state.spaceShipProfile))
                this.profile_template = {
                    ammo: 0,
                    energy: 0,
                    life: 0,
                    lifeConsume: 0,
                    energyConsume: 0
                }

            } else if (this.$route.query.scene == "Mars") {
                this.profile_list = JSON.parse(JSON.stringify(store.state.marsRoverProfile))
                this.profile_template = {
                    oil: 0,
                    energy: 0,
                }
            }

            if (this.profile_list.length != 5) {
                this.profile_list.push(this.profile_template)
            } else {
                this.no_more_profile = true
            }
        },
        submitProfile() {
            this.loading = true
            const Func = () => {
                if (this.profile_index == this.profile_list.length - 1 && !this.no_more_profile) {
                    return (this.$route.query.scene == "Universe" ?
                        http.addSpaceShipProfile(store.state.userID, this.profile_list[this.profile_index]) :
                        http.addMarsRoverProfile(store.state.userID, this.profile_list[this.profile_index]))
                } else {
                    return (this.$route.query.scene == "Universe" ?
                        http.updateSpaceShipProfile(store.state.userID, this.profile_list[this.profile_index].id, this.profile_list[this.profile_index]) :
                        http.updateMarsRoverProfile(store.state.userID, this.profile_list[this.profile_index].id, this.profile_list[this.profile_index]))
                }
            }

            console.log(store.state.userID, this.profile_list[this.profile_index].id, this.profile_list[this.profile_index])
            Func()
                .then(async res => {
                    await this.refetchProfile()
                    this.snackbar = {
                        open: true,
                        text: res,
                        color: "light-green"
                    }
                })
                .catch(res => {
                    this.snackbar = {
                        open: true,
                        text: res,
                        color: "pink"
                    }
                }).finally(_ => {
                    this.loading = false
                })

        },
        async refetchProfile() {
            if (this.$route.query.scene == "Universe") {
                await http.getSpaceShipProfile(store.state.userID).then(async res => {
                    res = JSON.parse(res)
                    console.log(res)
                    await this.$store.dispatch('setSpaceShipProfile', res)
                    this.initProfileList()
                }).catch(res => {
                    this.snackbar = {
                        open: true,
                        text: res,
                        color: "pink"
                    }
                })
            } else {
                await http.getMarsRoverProfile(store.state.userID).then(async res => {
                    res = JSON.parse(res)
                    console.log(res)
                    await this.$store.dispatch('setMarsRoverProfile', res)
                    this.initProfileList()
                }).catch(res => {
                    this.snackbar = {
                        open: true,
                        text: res,
                        color: "pink"
                    }
                })
            }
        }
    }
}

</script>

<style scoped>
.SceneTitle {
    margin-bottom: 20px;
    position: relative;
    display: inline-block;
    padding: 10px 20px;
    color: #03e9f4;
    font-size: 16px;
    text-decoration: none;
    text-transform: uppercase;
    text-align: center;
    overflow: hidden;
    transition: .5s;
    margin-top: 40px;
    letter-spacing: 4px;
}

.SceneTitle span {
    position: absolute;
    display: block;
}

.v-card {
    background-color: rgba(255, 255, 255, 0);
    color: cyan;
}

.v-card--variant-elevated {
    box-shadow: none;
}

.v-dialog>.v-overlay__content>.v-card {
    box-shadow: none;
}



.card-content {
    height: 60vh;
    /* background-color: rgba(224, 223, 223, 0.5); */
}

.canvas-bg {
    /*background-image: url('@/assets/img/stars.jpg');*/
}
</style>