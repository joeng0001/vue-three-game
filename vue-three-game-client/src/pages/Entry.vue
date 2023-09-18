<template>
    <!-- <div>this login page</div>
    <button @click="addId">add </button>
    <button @click="removeId">remove </button>
    <div>{{ this.$store.state.userID }}</div>
    <v-btn>vuetify</v-btn> -->
    <div>
        <canvas ref="three"></canvas>
        <router-view></router-view>

    </div>
</template>

<script>
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import starsTexture from '@/assets/stars.jpg';
import sunTexture from '@/assets/img/sun.jpg'
import gsap from 'gsap'
export default {
    mounted() {

        const spaceStationUrl = new URL('@/assets/space_station_3.glb', import.meta.url)
        const galaxyUrl = new URL('@/assets/galaxy.glb', import.meta.url)
        const asteroidUrl = new URL('@/assets/asteroid.glb', import.meta.url)
        const solarSystemUrl = new URL('@/assets/solarSystem.glb', import.meta.url)

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

        const axesHelper = new THREE.AxesHelper(100);
        scene.add(axesHelper);

        const ambient = new THREE.AmbientLight(0xFFFFFF, 1);
        scene.add(ambient);

        const pointLight = new THREE.PointLight(0xffffff, 200);
        scene.add(pointLight);

        // const cubeTextureLoader = new THREE.CubeTextureLoader();
        // cubeTextureLoader.load([
        //     starsTexture,
        //     starsTexture,
        //     starsTexture,
        //     starsTexture,
        //     starsTexture,
        //     starsTexture
        // ], function (texture) {
        //     console.log("texture load successfully")
        //     // Texture loaded successfully
        //     scene.background = texture;
        // }, undefined, function (error) {
        //     // Error occurred while loading textures
        //     console.error('Error loading textures:', error);
        // });

        // const dLight = new THREE.DirectionalLight(0xFFFFFF, 1);
        // scene.add(dLight);
        // const dLightHelper = new THREE.DirectionalLightHelper(dLight)
        // scene.add(dLightHelper);

        // const dLightShadowHelper = new THREE.CameraHelper(dLight.shadow.camera)
        // scene.add(dLightShadowHelper);

        const textureLoader = new THREE.TextureLoader()
        // const sunGeo = new THREE.SphereGeometry(16, 30, 30);
        // const sunMat = new THREE.MeshBasicMaterial({
        //     map: textureLoader.load(sunTexture)
        // });
        // const sun = new THREE.Mesh(sunGeo, sunMat);
        // scene.add(sun);
        // sun.position.set(-100, -100, -100)



        scene.background = textureLoader.load(starsTexture)
        const spotLight = new THREE.SpotLight(0xffffff, 0.9, 0, Math.PI / 8, 1)
        spotLight.position.set(-30, 40, 30)
        spotLight.target.position.set(0, 0, 0)
        spotLight.angle = 0.2
        scene.add(spotLight)

        const spotLightHelper = new THREE.SpotLightHelper(spotLight)
        scene.add(spotLightHelper)


        const gltfLoader = new GLTFLoader();

        renderer.outputEncoding = THREE.sRGBEncoding;
        renderer.toneMapping = THREE.ACESFilmicToneMapping;



        gltfLoader.load(galaxyUrl.href, function (gltf) {
            const model = gltf.scene;
            scene.add(model);

            model.position.set(0, 0, 0)
        });


        gltfLoader.load(spaceStationUrl.href, function (gltf) {
            const model = gltf.scene;
            scene.add(model);
        });
        for (let i = 0; i < 10; i++) {
            gltfLoader.load(asteroidUrl.href, function (gltf) {
                const model = gltf.scene;
                scene.add(model);
                model.position.set(Math.random() * (100 - -100) + -100, Math.random() * (100 - -100) + -100, Math.random() * (100 - -100) + -100)
            });

        }
        gltfLoader.load(solarSystemUrl.href, function (gltf) {
            const model = gltf.scene;
            scene.add(model);
            model.position.set(-100, -100, 100)
        });


        let position = 0;
        function moving() {
            switch (position) {
                case 0:
                    moveCamera(10, 10, 10);
                    lookAtOrigin();
                    position = 1;
                    break;
                case 1:
                    moveCamera(2.8, 0, 3.6);
                    lookAtOrigin();
                    position = 2;
                    break;
                case 2:
                    moveCamera(2.5, -0.9, 12.2);
                    lookAtOrigin();
                    position = 3;
                    break;
                case 3:
                    moveCamera(-2.7, 0.6, 3.7);
                    lookAtOrigin();
                    position = 4;
                    break;
                case 4:
                    moveCamera(-1.7, 0, 8.7);
                    lookAtOrigin();
                    position = 5;
                    break;
                case 5:
                    moveCamera(0.5, 0.8, 10);
                    lookAtOrigin();
                    position = 0;
            }

        };

        function moveCamera(x, y, z) {
            gsap.to(camera.position, {
                x,
                y,
                z,
                duration: 3
            });
        }

        function lookAtOrigin() {
            gsap.to({}, {
                duration: 3,
                onUpdate: () => {
                    camera.lookAt(0, 0, 0);
                }
            });
        }

        // setInterval(() => {
        //     moving()
        // }, 3000)
        function animate() {
            renderer.render(scene, camera);
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