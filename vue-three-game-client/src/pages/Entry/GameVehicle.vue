<template>
    <v-dialog fullscreen v-model="dialog" persistent>
        <!--
            left show model spinning, right let user config the params of the flight(base value only, still afftect by level)
            show user pass configed profile as well
            each setting file is a record in database
        -->
        <v-card>
            <v-card-title class="SceneTitle">
                config your vehicle
            </v-card-title>

            <v-card-item class="card-content">
                <div class="d-flex">
                    <canvas ref="vehicle" class="canvas-bg"></canvas>
                    <v-form class="ml-1 w-100">
                        <v-container>
                            <v-row>
                                <v-col cols="12">
                                    <v-tabs bg-color="primary">
                                        <v-tab v-for="i in [1, 2, 3, 4, 5]" :value="i">profile {{ i }}</v-tab>
                                    </v-tabs>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field v-model="firstname" :rules="nameRules" :counter="10" label="First name"
                                        required hide-details></v-text-field>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-form>
                </div>
            </v-card-item>

            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn @click="isActive.value = false" class="mr-6">create </v-btn>
                <v-btn @click="isActive.value = false" class="mr-6">Close </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import * as THREE from "three"
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { GLTFLoader } from "three/examples/jsm/loaders/GLTFLoader";
export default {
    mounted() {
        this.initModelDisplay()
    },
    data() {
        return {
            dialog: true
        }
    },
    methods: {
        initModelDisplay() {
            const gltfLoader = new GLTFLoader()
            const scene = new THREE.Scene();
            const canvas = this.$refs.vehicle
            const renderer = new THREE.WebGLRenderer({ canvas, antialias: true, alpha: true });
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