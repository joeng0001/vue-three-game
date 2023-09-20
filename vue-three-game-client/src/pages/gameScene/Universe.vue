<template>
    <div>
        <canvas ref="three"></canvas>
    </div>
</template>

<script>

import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
import starsTexture from '@/assets/img/space2.jpg';



export default {
    mounted() {
        this.initScene(this.movementKey)



    },
    data() {
        return {
            movementKey: {
                'w': false,
                'a': false,
                's': false,
                'd': false,
                ' ': false,
                'Alt': false
            }
        }
    },
    methods: {
        initScene(movementKey) {
            const spaceShipUrl = new URL('@/assets/model/spaceShip.glb', import.meta.url)
            const gltfLoader = new GLTFLoader();
            let spaceShip = null
            gltfLoader.load(spaceShipUrl.href, function (gltf) {
                const model = gltf.scene;
                scene.add(model);
                spaceShip = model
            });

            const scene = new THREE.Scene();

            const canvas = this.$refs.three

            const renderer = new THREE.WebGLRenderer({ canvas, antialias: true });

            renderer.setSize(window.innerWidth, window.innerHeight);

            const camera = new THREE.PerspectiveCamera(
                45,
                window.innerWidth / window.innerHeight,
                0.1,
                1000
            );

            const orbit = new OrbitControls(camera, renderer.domElement);

            camera.position.set(0, 50, 50);
            orbit.update();

            const ambientLight = new THREE.AmbientLight(0xFFFFFF);
            scene.add(ambientLight);

            const cubeTextureLoader = new THREE.CubeTextureLoader();
            scene.background = cubeTextureLoader.load([
                starsTexture,
                starsTexture,
                starsTexture,
                starsTexture,
                starsTexture,
                starsTexture
            ]);

            const axesHelper = new THREE.AxesHelper(100);
            scene.add(axesHelper);

            function moveSpaceShip(movementKey) {
                if (movementKey['w']) {
                    spaceShip.position.z -= 0.01
                    if (spaceShip.rotation.x < 0.3) {
                        spaceShip.rotation.x += 0.01
                    }

                }
                if (movementKey['s']) {
                    spaceShip.position.z += 0.01
                    if (spaceShip.rotation.x > -0.3) {
                        spaceShip.rotation.x -= 0.01
                    }
                }
                if (movementKey['a']) {
                    spaceShip.position.x -= 0.01
                    spaceShip.rotation.z += 0.02
                }
                if (movementKey['d']) {
                    spaceShip.position.x += 0.01
                    spaceShip.rotation.z -= 0.02
                }
                if (movementKey[' ']) {
                    spaceShip.position.y += 0.01
                }
                if (movementKey['Alt']) {
                    spaceShip.position.y -= 0.01
                }
            }

            function animate() {
                moveSpaceShip(movementKey)
                renderer.render(scene, camera);
            }



            renderer.setAnimationLoop(animate);

            this.addSpaceShipMovementListener(movementKey)
            this.addCanvasResizeListener(camera, renderer)
        },
        addSpaceShipMovementListener(movementKey) {
            window.addEventListener('keydown', (e) => {
                console.log(e)
                e.preventDefault()
                movementKey[e.key] = true;
            }, false);
            window.addEventListener('keyup', (e) => {
                e.preventDefault()
                movementKey[e.key] = false;
            }, false);

        },
        addCanvasResizeListener(camera, renderer) {
            window.addEventListener('resize', function () {
                camera.aspect = window.innerWidth / window.innerHeight;
                camera.updateProjectionMatrix();
                renderer.setSize(window.innerWidth, window.innerHeight);
            });
        }
    }

}
</script>


<style scoped></style>
