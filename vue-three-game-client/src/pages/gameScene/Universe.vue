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
            control: {
                'shoot': false
            },
            speed: {
                value: 0.05,
                factor: 1,
                rotateSpeed: 0.01,
                maxSideRotation: 0.2,
                minSideRotation: -0.2,
                maxFrontRotation: 0.2,
                minFrontRotation: 0.2,
                bulletSpeed: 0.3
            }
        }
    },
    methods: {
        initScene() {
            const movementKey = this.movementKey
            const spaceShipUrl = new URL('@/assets/model/spaceShip.glb', import.meta.url)
            const meteorUrl = new URL('@/assets/model/meteor.glb', import.meta.url)
            const meteor2Url = new URL('@/assets/model/meteor2.glb', import.meta.url)
            const meteor3Url = new URL('@/assets/model/meteor3.glb', import.meta.url)
            const shootBulletUrl = new URL('@/assets/model/lazer_bullet.glb', import.meta.url)

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

            const directionalLight = new THREE.DirectionalLight(0xffffff, 1);
            directionalLight.position.set(1, 1, 1);
            scene.add(directionalLight);

            const ambientLight = new THREE.AmbientLight(0xFFFFFF, 1);
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

            let rings = [], meteors = [], shootBullets = []
            const ringMaterial = new THREE.MeshBasicMaterial({ color: 0x90EE90, side: THREE.DoubleSide, });
            for (let i = 0; i < 30; i++) {
                const ringMesh = new THREE.Mesh(ringGeometry, ringMaterial);
                scene.add(ringMesh);
                ringMesh.position.set(Math.random() * (200) + -100, Math.random() * (200) + -100, -30 - i * 10)
                rings.push({ model: ringMesh, box: new THREE.Box3().setFromObject(ringMesh), collisionHappened: false })
            }

            for (let i = 0; i < 10; i++) {
                gltfLoader.load(meteorUrl.href, function (gltf) {
                    const model = gltf.scene;
                    scene.add(model);
                    model.position.set((Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50), (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50), (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50))
                    const sceleFactor = Math.random() * (2 - 0.5) + 0.5;
                    model.scale.set(sceleFactor, sceleFactor, sceleFactor)
                    meteors.push({
                        model, box: new THREE.Box3().setFromObject(model),
                        collisionHappened: false, positionTooFar: false, gotShoot: false,
                        rotationSpeed: Math.random() * (0.01 - 0.001) + 0.001,
                        rotationX: Math.random() < 0.5, rotationY: Math.random() < 0.5, rotationZ: Math.random() < 0.5,
                        movementSpeed: Math.random() * (0.01 - 0.001) + 0.001,
                        movementX: Math.random() < 0.5, movementY: Math.random() < 0.5, movementZ: Math.random() < 0.5
                    })
                    console.log("meteor loaded")
                });
                gltfLoader.load(meteor2Url.href, function (gltf) {
                    const model = gltf.scene;
                    scene.add(model);
                    model.position.set((Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50), (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50), (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50))
                    const sceleFactor = Math.random() * (2 - 0.5) + 0.5;
                    model.scale.set(sceleFactor, sceleFactor, sceleFactor)
                    meteors.push({
                        model, box: new THREE.Box3().setFromObject(model),
                        collisionHappened: false, positionTooFar: false, gotShoot: false,
                        rotationSpeed: Math.random() * (0.01 - 0.001) + 0.001,
                        rotationX: Math.random() < 0.5, rotationY: Math.random() < 0.5, rotationZ: Math.random() < 0.5,
                        movementSpeed: Math.random() * (0.01 - 0.001) + 0.001,
                        movementX: Math.random() < 0.5, movementY: Math.random() < 0.5, movementZ: Math.random() < 0.5
                    })
                    console.log("meteor loaded")
                });
                gltfLoader.load(meteor3Url.href, function (gltf) {
                    const model = gltf.scene;
                    scene.add(model);
                    model.position.set((Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50), (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50), (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50))
                    const sceleFactor = Math.random() * (0.05 - 0.01)
                    model.scale.set(sceleFactor, sceleFactor, sceleFactor)
                    meteors.push({
                        model, box: new THREE.Box3().setFromObject(model),
                        collisionHappened: false, positionTooFar: false, gotShoot: false,
                        rotationSpeed: Math.random() * (0.01 - 0.001) + 0.001,
                        rotationX: Math.random() < 0.5, rotationY: Math.random() < 0.5, rotationZ: Math.random() < 0.5,
                        movementSpeed: Math.random() * (0.01 - 0.001) + 0.001,
                        movementX: Math.random() < 0.5, movementY: Math.random() < 0.5, movementZ: Math.random() < 0.5
                    })
                    console.log("meteor loaded")
                });
            }
            let spaceShip = null

            gltfLoader.load(spaceShipUrl.href, function (gltf) {
                const model = gltf.scene;
                scene.add(model);
                spaceShip = model
            });
            let detectCollisionLock = false, detectPositionTooFarLock = false, detectShootLock = false


            const collisionHandler = () => {
                if (!spaceShip || detectCollisionLock) return
                detectCollisionLock = true
                //remove collision
                rings.forEach(ring => {
                    if (ring.collisionHappened) {
                        scene.remove(ring.model)
                        scene.remove(ring.box)
                        rings = rings.filter(obj => obj != ring)
                    }
                })

                meteors.forEach(meteor => {
                    if (meteor.collisionHappened) {
                        const newPos = [
                            spaceShip.position.x + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50),
                            spaceShip.position.y + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50),
                            spaceShip.position.z + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50)]
                        setObjPos(meteor, newPos[0], newPos[1], newPos[2])
                        meteor.collisionHappened = false
                    }
                })

                meteors.forEach(meteor => {
                    if (meteor.gotShoot) {
                        const newPos = [
                            spaceShip.position.x + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50),
                            spaceShip.position.y + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50),
                            spaceShip.position.z + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50)]
                        setObjPos(meteor, newPos[0], newPos[1], newPos[2])
                        meteor.gotShoot = false
                    }
                })

                shootBullets.forEach(bullet => {
                    if (bullet.collisionHappened) {
                        scene.remove(bullet.model)
                        scene.remove(bullet.box)
                        shootBullets = shootBullets.filter(obj => obj != bullet)
                    }
                })
                detectCollisionLock = false
            }

            const detectPositionTooFarHandler = () => {
                if (!spaceShip || detectPositionTooFarLock) return
                detectPositionTooFarLock = true
                meteors.forEach(meteor => {
                    if (meteor.positionTooFar) {
                        const newPos = [
                            spaceShip.position.x + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50),
                            spaceShip.position.y + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50),
                            spaceShip.position.z + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50)]
                        setObjPos(meteor, newPos[0], newPos[1], newPos[2])
                        meteor.positionTooFar = false
                    }
                })

                shootBullets.forEach(bullet => {
                    if (bullet.positionTooFar) {
                        scene.remove(bullet.model)
                        scene.remove(bullet.box)
                        shootBullets = shootBullets.filter(obj => obj != bullet)
                    }
                })
                detectPositionTooFarLock = false
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

                shootBullets.forEach(bullet => {
                    meteors.forEach(meteor => {
                        if (bullet.box.intersectsBox(meteor.box)) {
                            console.log("detect bnullet intersert wwith meteor")
                            bullet.collisionHappened = true
                            meteor.gotShoot = true
                        }
                    })
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
                    if (distance > 80 || meteor.model.position.z > spaceShip.position.z + 20) {
                        meteor.positionTooFar = true
                    }
                })
            }

            const detectBulletPos = () => {
                if (!spaceShip) return
                shootBullets.forEach(bullet => {
                    const distance = Math.sqrt(
                        Math.pow(bullet.model.position.x - spaceShip.position.x, 2) +
                        Math.pow(bullet.model.position.y - spaceShip.position.y, 2) +
                        Math.pow(bullet.model.position.z - spaceShip.position.z, 2)
                    );
                    if (distance > 100) {
                        bullet.positionTooFar = true
                    }
                })
            }

            const moveMeteor = () => {
                if (!spaceShip) return
                meteors.forEach(meteor => {
                    if (meteor.rotationX) meteor.model.rotation.x += meteor.rotationSpeed
                    if (meteor.rotationY) meteor.model.rotation.y += meteor.rotationSpeed
                    if (meteor.rotationZ) meteor.model.rotation.z += meteor.rotationSpeed
                    if (meteor.movementX) { meteor.model.position.x += meteor.movementSpeed }
                    else { meteor.model.position.x -= meteor.movementSpeed }
                    if (meteor.movementY) { meteor.model.position.y += meteor.movementSpeed }
                    else { meteor.model.position.y -= meteor.movementSpeed }
                    if (meteor.movementZ) { meteor.model.position.z += meteor.movementSpeed }
                    else { meteor.model.position.z -= meteor.movementSpeed }
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
            const detectShoot = () => {
                if (detectShootLock) return
                detectShootLock = true
                if (this.control.shoot) {
                    const movementSpeed = this.speed.bulletSpeed
                    gltfLoader.load(shootBulletUrl.href, function (gltf) {
                        const model = gltf.scene;
                        scene.add(model);
                        model.scale.set(0.01, 0.01, 0.01)
                        model.position.set(spaceShip.position.x, spaceShip.position.y, spaceShip.position.z)
                        shootBullets.push({
                            model, box: new THREE.Box3().setFromObject(model), movementSpeed,
                            positionTooFar: false, collisionHappened: false,
                        })
                    });
                    this.control.shoot = false
                }
                detectShootLock = false
            }
            const moveShootBullet = () => {
                shootBullets.forEach(bullet => {
                    setObjPos(bullet, bullet.model.position.x, bullet.model.position.y, bullet.model.position.z -= bullet.movementSpeed)
                })
            }

            setInterval(() => {
                collisionHandler()
                detectPositionTooFarHandler()

                detectShoot()

                detectCollisions()
                detectMeteorPos()
                detectBulletPos()
            }, 100)
            function animate() {
                moveSpaceShip(camera)
                moveMeteor()
                moveShootBullet()
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

            window.addEventListener('contextmenu', (e) => {
                e.preventDefault()
                this.control['shoot'] = true
            })
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
