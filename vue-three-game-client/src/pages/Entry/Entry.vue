<template>
    <div>
        <canvas ref="three"></canvas>
        <router-view></router-view>
    </div>
</template>

<script>
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';

import gsap from 'gsap'
export default {
    data() {
        return {
            renderer: null
        }
    },
    beforeUnmount() {
        if (this.renderer) {
            this.renderer.setAnimationLoop(null)
        }
    },
    mounted() {
        const spaceStationUrl = new URL('@/assets/model/space_station_3.glb', import.meta.url)
        const galaxyUrl = new URL('@/assets/model/galaxy.glb', import.meta.url)
        const asteroidUrl = new URL('@/assets/model/asteroid.glb', import.meta.url)
        const solarSystemUrl = new URL('@/assets/model/solarSystem.glb', import.meta.url)
        const scene = new THREE.Scene();
        const canvas = this.$refs.three
        const renderer = new THREE.WebGLRenderer({ canvas, antialias: true });
        this.renderer = renderer
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

        const ambient = new THREE.AmbientLight(0xFFFFFF, 0.1);
        scene.add(ambient);

        const directionalLight = new THREE.DirectionalLight(0xffffff, 1);
        directionalLight.position.set(-1, 0, 0); // Set the position to (-1, 0, 0)
        scene.add(directionalLight);

        const gltfLoader = new GLTFLoader();

        renderer.toneMapping = THREE.ACESFilmicToneMapping;

        gltfLoader.load(galaxyUrl.href, function (gltf) {
            const model = gltf.scene;
            scene.add(model);
            model.position.set(0, 0, 0)
            model.scale.set(500, 500, 500)

        });

        let mixer;
        gltfLoader.load(spaceStationUrl.href, function (gltf) {
            const model = gltf.scene;
            scene.add(model);
            if (gltf.animations && gltf.animations.length > 0) {
                mixer = new THREE.AnimationMixer(model);
                const clips = gltf.animations;
                clips.forEach(function (clip) {
                    const action = mixer.clipAction(clip);
                    action.play();
                });
            }

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
            model.position.set(-50, -50, -50)
        });


        let pos = 0;
        function moving() {
            switch (pos) {
                case 0:
                    moveCamera(10, 10, 10);
                    rotateCamera(0, 1, 0)
                    pos = 1;
                    break;
                case 1:
                    moveCamera(100, -40, 200);
                    rotateCamera(0, 1, 0)
                    pos = 2;
                    break;
                case 2:
                    moveCamera(70, -20, 30);
                    rotateCamera(1, -2, 0)
                    pos = 3;
                    break;
                case 3:
                    moveCamera(-100, 20, 140);
                    rotateCamera(-0, -1, -3)
                    pos = 4;
                    break;
                case 4:
                    moveCamera(50, 50, -70);
                    rotateCamera(1, 0, 2)
                    pos = 5;
                    break;
                case 5:
                    moveCamera(10, 10, 10);
                    lookAt(0, 0, 0);
                    pos = 0;
            }

        };

        function moveCamera(x, y, z) {
            gsap.to(camera.position, {
                x,
                y,
                z,
                duration: 5
            });
        }

        function lookAt(x, y, z) {
            gsap.to({}, {
                duration: 5,
                onUpdate: () => {
                    camera.lookAt(x, y, z);
                }
            });
        }

        function rotateCamera(x, y, z) {
            gsap.to(camera.rotation, {
                x,
                y,
                z,
                duration: 5
            });
        }

        const CameraInterval = setInterval(() => {
            moving()
        }, 5000)

        window.addEventListener('contextmenu', function () {
            clearInterval(CameraInterval)
        });
        const clock = new THREE.Clock();
        function animate() {
            if (mixer)
                mixer.update(clock.getDelta());
            renderer.render(scene, camera);
        }

        renderer.setAnimationLoop(animate);

        window.addEventListener('resize', function () {
            camera.aspect = window.innerWidth / window.innerHeight;
            camera.updateProjectionMatrix();
            renderer.setSize(window.innerWidth, window.innerHeight);
        });
    },
    onBeforeUnmount() {
        if (this.renderer) {
            this.renderer.setAnimationLoop(null)
        }
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