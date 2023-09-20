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
import * as CANNON from "cannon-es";


export default {
    mounted() {
        this.initScene()
    },
    data() {
        return {
            movementKey: {
                'w': false,
                'a': false,
                's': false,
                'd': false,
                ' ': false,
                'Alt': false,
                'Shift': false
            },
            speed: {
                value: 0.05,
                factor: 1,
                rotateSpeed: 0.01,
                maxSideRotation: 0.2,
                minSideRotation: -0.2,
                maxFrontRotation: 0.2,
                minFrontRotation: 0.2,
            }
        }
    },
    methods: {
        initScene() {
            const movementKey = this.movementKey
            const spaceShipUrl = new URL('@/assets/model/spaceShip.glb', import.meta.url)
            const meteorUrl = new URL('@/assets/model/meteor.glb', import.meta.url)
            const gltfLoader = new GLTFLoader();

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
            camera.position.set(0, 20, 30);
            orbit.update();

            const ambientLight = new THREE.AmbientLight(0xFFFFFF);
            scene.add(ambientLight);

            const axesHelper = new THREE.AxesHelper(100);
            scene.add(axesHelper);

            const cubeTextureLoader = new THREE.CubeTextureLoader();
            scene.background = cubeTextureLoader.load([
                starsTexture,
                starsTexture,
                starsTexture,
                starsTexture,
                starsTexture,
                starsTexture
            ]);


            const ringGeometry = new THREE.TorusGeometry(
                2,
                0.5,
                64, 64
            );

            let rings = [], meteors = []
            const ringMaterial = new THREE.MeshBasicMaterial({ color: 0x90EE90, side: THREE.DoubleSide, });
            for (let i = 0; i < 30; i++) {
                const ringMesh = new THREE.Mesh(ringGeometry, ringMaterial);
                scene.add(ringMesh);
                ringMesh.position.set(0, 0, -30 - i * 10)
                rings.push(new THREE.Box3().setFromObject(ringMesh))
            }

            for (let i = 0; i < 10; i++) {
                gltfLoader.load(meteorUrl.href, function (gltf) {
                    const model = gltf.scene;
                    scene.add(model);
                    model.position.set(Math.random() * (10 - -10) + -10, Math.random() * (10 - -10) + -10, Math.random() * (10 - -10) + -10)
                    const sceleFactor = Math.floor(Math.random() * 0.1) + 1
                    model.scale.set(sceleFactor, sceleFactor, sceleFactor)
                    meteors.push(new THREE.Box3().setFromObject(model))
                });
            }
            let spaceShip = null

            gltfLoader.load(spaceShipUrl.href, function (gltf) {
                const model = gltf.scene;
                scene.add(model);
                spaceShip = model
            });


            function detectCollisions() {
                if (!spaceShip) return
                const spaceShipBox = new THREE.Box3().setFromObject(spaceShip)
                meteors.forEach(meteor => {
                    var collision = spaceShipBox.intersectsBox(meteor);
                    if (collision) {
                        console.log("collision with rocks")
                    }
                })
                rings.forEach(ring => {
                    var collision = spaceShipBox.intersectsBox(ring);
                    if (collision) {
                        console.log("collision with rings")
                    }
                })

            }






            const moveSpaceShip = (camera) => {
                const speed = this.speed

                if (!camera || !spaceShip) return

                if (movementKey['Shift']) {
                    if (speed.factor < 2) {
                        speed.factor += 0.1
                    }
                } else {
                    speed.factor = 1
                }
                if (movementKey['w']) {
                    spaceShip.position.z -= speed.value * speed.factor
                    camera.position.z -= speed.value * speed.factor
                    if (spaceShip.rotation.x < speed.maxFrontRotation) {
                        spaceShip.rotation.x += 0.01
                    }
                } else if (movementKey['s']) {
                    spaceShip.position.z += speed.value * speed.factor
                    camera.position.z += speed.value * speed.factor
                    if (spaceShip.rotation.x > speed.minFrontRotation) {
                        spaceShip.rotation.x -= 0.01
                    }
                }
                if (movementKey['a']) {
                    spaceShip.position.x -= speed.value * speed.factor
                    camera.position.x -= speed.value * speed.factor
                    if (spaceShip.rotation.z < speed.maxSideRotation)
                        spaceShip.rotation.z += speed.rotateSpeed
                } else if (movementKey['d']) {
                    spaceShip.position.x += speed.value * speed.factor
                    camera.position.x += speed.value * speed.factor
                    if (spaceShip.rotation.z > speed.minSideRotation)
                        spaceShip.rotation.z -= speed.rotateSpeed
                }
                if (movementKey['Alt']) {
                    spaceShip.position.y -= speed.value * speed.factor
                    camera.position.y -= speed.value * speed.factor
                } else if (movementKey[' ']) {
                    spaceShip.position.y += speed.value * speed.factor
                    camera.position.y += speed.value * speed.factor
                }
                orbit.target.copy(new THREE.Vector3(spaceShip.position.x, spaceShip.position.y, spaceShip.position.z))
                orbit.update();
            }

            function animate() {
                detectCollisions()
                moveSpaceShip(camera)
                renderer.render(scene, camera);
            }



            renderer.setAnimationLoop(animate);

            this.addSpaceShipMovementListener(movementKey)
            this.addCanvasResizeListener(camera, renderer)
        },
        addSpaceShipMovementListener(movementKey) {
            window.addEventListener('keydown', (e) => {
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
