<template>
    <!-- <div>this login page</div>
    <button @click="addId">add </button>
    <button @click="removeId">remove </button>
    <div>{{ this.$store.state.userID }}</div>
    <v-btn>vuetify</v-btn> -->
    <canvas ref="three"></canvas>
</template>

<script>
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import starsTexture from '@/assets/stars.jpg';

import gsap from 'gsap'
export default {
    mounted() {

        const spaceStationUrl = new URL('@/assets/space_station_3.glb', import.meta.url)
        const spaceUrl = new URL('@/assets/space.glb', import.meta.url)

        const scene = new THREE.Scene();
        const canvas = this.$refs.three
        const renderer = new THREE.WebGLRenderer({ canvas, antialias: true });
        renderer.setSize(window.innerWidth, window.innerHeight);
        const camera = new THREE.PerspectiveCamera(
            50,
            window.innerWidth / window.innerHeight,
            0.1,
            1300
        );

        renderer.setClearColor(0xFFFFFF);
        const orbit = new OrbitControls(camera, renderer.domElement);
        orbit.update();
        camera.position.set(-1.8, 1.6, 5);
        camera.lookAt(0, 0, 0);

        const axesHelper = new THREE.AxesHelper(100); // 5 represent the length of axis
        scene.add(axesHelper);// add the help to the scene

        // const ambient = new THREE.AmbientLight(0xffffff, 3);
        // scene.add(ambient);

        // const pointLight = new THREE.PointLight(0xffffff, 200);
        // scene.add(pointLight);

        const cubeTextureLoader = new THREE.CubeTextureLoader();
        cubeTextureLoader.load([
            starsTexture,
            starsTexture,
            starsTexture,
            starsTexture,
            starsTexture,
            starsTexture
        ], function (texture) {
            console.log("texture load successfully")
            // Texture loaded successfully
            scene.background = texture;
        }, undefined, function (error) {
            // Error occurred while loading textures
            console.error('Error loading textures:', error);
        });




        const gltfLoader = new GLTFLoader();

        renderer.outputEncoding = THREE.sRGBEncoding;
        renderer.toneMapping = THREE.ACESFilmicToneMapping;

        let position = 0;

        gltfLoader.load(spaceUrl.href, function (gltf) {
            const model = gltf.scene;
            scene.add(model);

            model.position.set(0, 0, 0)
        });


        gltfLoader.load(spaceStationUrl.href, function (gltf) {
            const model = gltf.scene;
            scene.add(model);

            // window.addEventListener('click', function () {
            //     switch (position) {
            //         case 0:
            //             moveCamera(0, 0, 0);
            //             rotateCamera(0, 0.1, 0);
            //             position = 1;
            //             break;
            //         case 1:
            //             moveCamera(2.8, 0, 3.6);
            //             rotateCamera(0, -2, 0);
            //             position = 2;
            //             break;
            //         case 2:
            //             moveCamera(2.5, -0.9, 12.2);
            //             rotateCamera(0.9, 0.6, -0.6);
            //             position = 3;
            //             break;
            //         case 3:
            //             moveCamera(-2.7, 0.6, 3.7);
            //             rotateCamera(0.6, 1.9, -0.6);
            //             position = 4;
            //             break;
            //         case 4:
            //             moveCamera(-1.7, 0, 8.7);
            //             rotateCamera(0, 4.7, 0);
            //             position = 5;
            //             break;
            //         case 5:
            //             moveCamera(0.5, 0.8, 10);
            //             rotateCamera(0.3, 1.65, -0.3);
            //             position = 0;
            //     }

            // });

            function moveCamera(x, y, z) {
                gsap.to(camera.position, {
                    x,
                    y,
                    z,
                    duration: 3
                });
            }

            function rotateCamera(x, y, z) {
                gsap.to(camera.rotation, {
                    x,
                    y,
                    z,
                    duration: 3.2
                });
            }
        });

        //const clock = new THREE.Clock();
        function animate() {
            renderer.render(scene, camera);
            //controls.update(clock.getDelta());
        }

        renderer.setAnimationLoop(animate);

        window.addEventListener('resize', function () {
            camera.aspect = window.innerWidth / window.innerHeight;
            camera.updateProjectionMatrix();
            renderer.setSize(window.innerWidth, window.innerHeight);
        });
    },
    data() {
        return {
            dialog: true
        }
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