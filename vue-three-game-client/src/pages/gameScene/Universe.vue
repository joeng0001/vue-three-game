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
                rings.push({ model: ringMesh, box: new THREE.Box3().setFromObject(ringMesh), collisionHappened: false })
            }

            for (let i = 0; i < 20; i++) {
                gltfLoader.load(meteorUrl.href, function (gltf) {
                    const model = gltf.scene;
                    scene.add(model);
                    model.position.set(Math.random() * 40 - 20, Math.random() * 40 - 20, Math.random() * -40)
                    const sceleFactor = Math.random() * (2 - 0.5) + 0.5;
                    model.scale.set(sceleFactor, sceleFactor, sceleFactor)
                    meteors.push({
                        model, box: new THREE.Box3().setFromObject(model),
                        collisionHappened: false, positionTooFar: false,
                        rotationSpeed: Math.random() * (0.01 - 0.001) + 0.001,
                        rotationX: Math.random() < 0.5, rotationY: Math.random() < 0.5, rotationZ: Math.random() < 0.5,
                        movementSpeed: Math.random() * (0.01 - 0.001) + 0.001,
                        movementX: Math.random() < 0.5, movementY: Math.random() < 0.5, movementZ: 0.05

                    })
                });
            }
            let spaceShip = null

            gltfLoader.load(spaceShipUrl.href, function (gltf) {
                const model = gltf.scene;
                scene.add(model);
                spaceShip = model
            });
            let lock = false, lock2 = false


            const collisionHandler = () => {
                if (!spaceShip || lock) return
                lock = true
                //remove collision
                rings.forEach(ring => {
                    if (ring.collisionHappened) {
                        scene.remove(ring.model)
                        scene.remove(ring.box)
                        rings = rings.filter(obj => obj != ring)
                        console.log("removed rings")
                    }
                })

                meteors.forEach(meteor => {
                    if (meteor.collisionHappened) {
                        console.log("meteor collision detect")
                        const newPos = [
                            spaceShip.position.x + Math.floor(Math.random() * 61) - 30,
                            spaceShip.position.y + Math.floor(Math.random() * 41) - 20,
                            spaceShip.position.z - Math.floor(Math.random() * (80 - 30 + 1)) + 30]
                        setObjPos(meteor, newPos[0], newPos[1], newPos[2])
                        meteor.collisionHappened = false
                    }
                })
                lock = false
            }

            const reRenderMeteorHandler = () => {
                if (!spaceShip || lock2) return
                lock2 = true
                meteors.forEach(meteor => {
                    if (meteor.positionTooFar) {
                        const newPos = [
                            spaceShip.position.x + Math.floor(Math.random() * 61) - 30,
                            spaceShip.position.y + Math.floor(Math.random() * 41) - 20,
                            spaceShip.position.z - Math.floor(Math.random() * (80 - 30 + 1)) + 30]
                        setObjPos(meteor, newPos[0], newPos[1], newPos[2])
                        meteor.positionTooFar = false
                    }
                })
                lock2 = false
            }


            const detectCollisions = () => {
                if (!spaceShip) return
                const spaceShipBox = new THREE.Box3().setFromObject(spaceShip)
                meteors.map(meteor => {
                    if (spaceShipBox.intersectsBox(meteor.box)) {
                        meteor.collisionHappened = true
                    }
                })
                rings.map(ring => {
                    if (spaceShipBox.intersectsBox(ring.box)) {
                        ring.collisionHappened = true
                    }
                })
            }

            const detectMeteorPos = () => {
                if (!spaceShip) return
                meteors.forEach(meteor => {
                    const distance = Math.sqrt(
                        Math.pow(meteor.model.position.x - spaceShip.position.x, 2) +
                        Math.pow(meteor.model.position.y - spaceShip.position.y, 2) +
                        Math.pow(meteor.model.position.z - spaceShip.position.z, 2)
                    );
                    if (distance > 80) {
                        console.log("meteor leave spaceship")
                        meteor.positionTooFar = true
                    }
                })
            }

            const moveMeteor = () => {
                if (!spaceShip) return
                meteors.forEach(meteor => {
                    //need to set box model as well
                    if (meteor.rotationX) meteor.model.rotation.x += meteor.rotationSpeed
                    if (meteor.rotationY) meteor.model.rotation.y += meteor.rotationSpeed
                    if (meteor.rotationZ) meteor.model.rotation.z += meteor.rotationSpeed



                    meteor.model.position.z += meteor.movementSpeed
                    if (meteor.movementX) { meteor.model.position.x += meteor.movementSpeed }
                    else { meteor.model.position.x -= meteor.movementSpeed }
                    if (meteor.movementY) { meteor.model.position.y += meteor.movementSpeed }
                    else { meteor.model.position.y -= meteor.movementSpeed }
                    setObjPos(meteor, meteor.model.position.x, meteor.model.position.y, meteor.model.position.z)
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

            const setObjPos = (obj, x, y, z) => {
                obj.model.position.set(x, y, z)
                const translation = new THREE.Vector3(x, y, z).sub(obj.box.getCenter(new THREE.Vector3()));
                obj.box.min.add(translation);
                obj.box.max.add(translation);
            }


            setInterval(() => {
                collisionHandler()
                reRenderMeteorHandler()
                detectCollisions()
                detectMeteorPos()
            }, 100)
            function animate() {
                moveSpaceShip(camera)
                moveMeteor()
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
