<template>
    <div class="wrapper">
        <canvas ref="three" class="canvas"></canvas>
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
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';

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
    mounted() {
        this.initBackground()
    },
    methods: {
        login() {
            this.loading = true
            http.login({ Username: this.user.name, Password: this.user.password })
                .then(res => {
                    res = JSON.parse(res)
                    this.controlAlert(true, "success", "login success", 3000)
                    this.$store.dispatch('setLoginStatus', true)
                    this.$store.dispatch('setUserID', res.id)
                    this.$store.dispatch('setSpaceShipProfile', res.spaceShipProfile)
                    this.$store.dispatch('setMarsRoverProfile', res.marsRoverProfile)
                    this.$router.push({ name: 'entry.gameMode' })
                }).catch(err => {
                    this.controlAlert(true, "error", err)
                }).finally(() => {
                    this.loading = false
                })
        },
        register() {
            this.loading = true
            http.register({ Username: this.user.name, Password: this.user.password })
                .then(msg => {
                    this.controlAlert(true, "success", msg, 3000)
                    this.page = 'login'
                }).catch(err => {
                    this.controlAlert(true, "error", err)
                }).finally(() => {
                    this.loading = false
                })
        },
        controlAlert(open, type, msg, timeout) {
            this.alert = { open, type, msg }
            if (timeout) {
                setTimeout(() => { this.alert.open = false }, timeout)
            }
        },
        initBackground() {
            const bgUrl = new URL('@/assets/model/loginBG.glb', import.meta.url)
            const scene = new THREE.Scene();
            const canvas = this.$refs.three
            const renderer = new THREE.WebGLRenderer({ canvas });
            renderer.setSize(window.innerWidth, window.innerHeight);
            const camera = new THREE.PerspectiveCamera(
                50,
                window.innerWidth / window.innerHeight,
                0.1,
                1300
            );
            camera.position.set(431, -87, -26);
            camera.lookAt(0, -20, 0);
            const ambient = new THREE.AmbientLight(0xFFFFFF, 1);
            scene.add(ambient);

            scene.background = new THREE.Color(0xffffff);
            const gltfLoader = new GLTFLoader();
            let bgAnimation = null
            let bgModel = null
            gltfLoader.load(bgUrl.href, function (gltf) {
                const model = gltf.scene;
                model.position.set(0, 50, 0)
                bgModel = model
                const animation = new THREE.AnimationMixer(model);
                bgAnimation = animation
                const clips = gltf.animations;
                const action = animation.clipAction(clips[0]);
                action.play();

                scene.add(model);
            });

            const clock = new THREE.Clock();
            function animate() {
                renderer.render(scene, camera);

                const delta = clock.getDelta();

                if (bgAnimation) {
                    bgAnimation.update(delta);
                }
                if (bgModel) {
                    bgModel.rotation.y += 0.001
                }
            }

            renderer.setAnimationLoop(animate);

            window.addEventListener('resize', function () {
                camera.aspect = window.innerWidth / window.innerHeight;
                camera.updateProjectionMatrix();
                renderer.setSize(window.innerWidth, window.innerHeight);
            });
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
    z-index: 0;
    background-color: rgba(255, 255, 255, 0.9);
}

.canvas {
    width: 100%;
    height: 100%;
    position: absolute;
    inset: 0;
}
</style>