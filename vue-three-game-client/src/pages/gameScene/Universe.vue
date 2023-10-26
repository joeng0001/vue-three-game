<template>
    <div>
        <div style="background-color: white;">{{ CollisionDistance }}</div>
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
                value: 0.15,
                factor: 1,
                rotateSpeed: 0.01,
                maxSideRotation: 0.2,
                minSideRotation: -0.2,
                maxFrontRotation: 0.2,
                minFrontRotation: 0.2,
                bulletSpeed: 0.3
            },
            spaceShipQuaternion: {
                rotationQuaternion: ((new THREE.Quaternion()).setFromAxisAngle(new THREE.Vector3(0, 1, 0), Math.PI)),
                horizontal: {
                    quaternionAngel: 0,
                    direction: new THREE.Vector3(0, 0, -1),
                    axis: new THREE.Vector3(0, 1, 0)
                },
                vertical: {
                    quaternionAngel: 0,
                    direction: new THREE.Vector3(-1, 0, 0),
                    axis: new THREE.Vector3(1, 0, 0)
                },
                side: {
                    quaternionAngel: 0,
                    direction: new THREE.Vector3(0, -1, 0),
                    axis: new THREE.Vector3(0, 0, 1)
                },
            },
            CollisionDistance: null
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
                const randomQuaternion = new THREE.Quaternion();
                randomQuaternion.setFromEuler(new THREE.Euler(
                    Math.random() * 2 * Math.PI,
                    Math.random() * 2 * Math.PI,
                    Math.random() * 2 * Math.PI
                ));
                ringMesh.setRotationFromQuaternion(randomQuaternion);
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

            var angle = Math.PI / 2;
            var rotationMatrix = new THREE.Matrix4().makeRotationY(angle);
            scene.matrix.multiply(rotationMatrix);
            scene.matrixAutoUpdate = false;

            let spaceShip = null
            let spaceShipArrowHelper = null
            gltfLoader.load(spaceShipUrl.href, function (gltf) {
                const model = gltf.scene;
                model.scale.set(0.5, 0.5, 0.5)
                model.rotation.y = Math.PI

                spaceShipArrowHelper = new THREE.ArrowHelper(
                    model.getWorldDirection(new THREE.Vector3()),
                    model.position,
                    100,
                    0xff0000
                );

                scene.add(spaceShipArrowHelper);

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
                    if (meteor.collisionHappened || meteor.gotShoot) {
                        const newPos = [
                            spaceShip.position.x + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50),
                            spaceShip.position.y + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50),
                            spaceShip.position.z + (Math.random() < 0.5 ? Math.random() * 50 : -1 * Math.random() * 50)]
                        setObjPos(meteor, newPos[0], newPos[1], newPos[2])
                        if (meteor.collisionHappened) {
                            meteor.collisionHappened = false
                        } else {
                            meteor.gotShoot = false
                        }
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

            const detectCollisionDistance = () => {
                if (!spaceShip) return
                let min_distance = 9999
                meteors.map(meteor => {
                    const distance = Math.sqrt(
                        Math.pow(meteor.model.position.x - spaceShip.position.x, 2) +
                        Math.pow(meteor.model.position.y - spaceShip.position.y, 2) +
                        Math.pow(meteor.model.position.z - spaceShip.position.z, 2)
                    );

                    if (distance < min_distance) {
                        min_distance = distance
                    }
                })
                this.CollisionDistance = min_distance
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
                if (!camera || !spaceShip) return
                const speed = this.speed
                const originSpaceshipPos = {
                    x: spaceShip.position.x,
                    y: spaceShip.position.y,
                    z: spaceShip.position.z,
                }
                if (movementKey['Shift']) {
                    if (speed.factor < 2) {
                        speed.factor += 0.1
                    }
                } else {
                    speed.factor = 1
                }
                if (movementKey['w'] || movementKey['s']) {
                    let direction = null
                    if (movementKey['w']) {
                        direction = new THREE.Vector3(0, 0, 1);
                    } else {
                        direction = new THREE.Vector3(0, 0, -1);
                    }
                    direction.applyQuaternion(this.spaceShipQuaternion.rotationQuaternion);
                    spaceShip.position.add(direction.multiplyScalar(speed.value * speed.factor));
                }
                if (movementKey['a'] || movementKey['d']) {
                    if (movementKey['a']) {
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.horizontal.axis, this.spaceShipQuaternion.horizontal.quaternionAngel = 0.01));
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.side.axis, this.spaceShipQuaternion.side.quaternionAngel = -0.01));
                    } else {
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.horizontal.axis, this.spaceShipQuaternion.horizontal.quaternionAngel = -0.01));
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.side.axis, this.spaceShipQuaternion.side.quaternionAngel = 0.01));
                    }
                    this.spaceShipQuaternion.horizontal.direction.applyQuaternion(this.spaceShipQuaternion.rotationQuaternion);
                    spaceShip.position.add(this.spaceShipQuaternion.horizontal.direction.multiplyScalar(speed.value * speed.factor));
                    spaceShip.position.add(this.spaceShipQuaternion.side.direction.multiplyScalar(speed.value * speed.factor));
                    spaceShip.setRotationFromQuaternion(this.spaceShipQuaternion.rotationQuaternion);
                }
                if (movementKey['Alt'] || movementKey[' ']) {
                    if (movementKey['Alt']) {
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.vertical.axis, this.spaceShipQuaternion.vertical.quaternionAngel = 0.005));
                    }
                    if (movementKey[' ']) {
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.vertical.axis, this.spaceShipQuaternion.vertical.quaternionAngel = -0.005));
                    }
                    this.spaceShipQuaternion.vertical.direction.applyQuaternion(this.spaceShipQuaternion.rotationQuaternion);
                    spaceShip.position.add(this.spaceShipQuaternion.vertical.direction.multiplyScalar(speed.value * speed.factor));
                    spaceShip.setRotationFromQuaternion(this.spaceShipQuaternion.rotationQuaternion);
                    //camera.position.add(this.spaceShipQuaternion.vertical.direction.multiplyScalar(speed.value * speed.factor))
                }
                camera.position.set(
                    camera.position.x + spaceShip.position.x - originSpaceshipPos.x,
                    camera.position.y + spaceShip.position.y - originSpaceshipPos.y,
                    camera.position.z + spaceShip.position.z - originSpaceshipPos.z
                )
                orbit.target.copy(new THREE.Vector3(spaceShip.position.x, spaceShip.position.y, spaceShip.position.z))
                orbit.update();
            }

            const setObjPos = (obj, x, y, z) => {
                obj.model.position.set(x, y, z)
                const translation = new THREE.Vector3(x, y, z).sub(obj.box.getCenter(new THREE.Vector3()));
                obj.box.min.add(translation);
                obj.box.max.add(translation);
            }
            const detectShoot = async () => {
                if (detectShootLock) return
                detectShootLock = true
                if (this.control.shoot) {
                    const movementSpeed = this.speed.bulletSpeed
                    const quaternion = this.spaceShipQuaternion.rotationQuaternion.clone()
                    //quaternion.setFromAxisAngle(new THREE.Vector3(0, 1, 0), Math.PI)
                    gltfLoader.load(shootBulletUrl.href, function (gltf) {
                        const model = gltf.scene;
                        scene.add(model);
                        model.scale.set(0.01, 0.01, 0.01)
                        model.setRotationFromQuaternion(quaternion);
                        model.position.set(spaceShip.position.x, spaceShip.position.y, spaceShip.position.z)
                        shootBullets.push({
                            model, box: new THREE.Box3().setFromObject(model), movementSpeed,
                            positionTooFar: false, collisionHappened: false, direction: new THREE.Vector3(0, 0, 1),
                            quaternion
                        })
                    });
                    this.control.shoot = false
                }
                detectShootLock = false
            }
            const moveShootBullet = () => {
                shootBullets.forEach(bullet => {
                    bullet.direction = new THREE.Vector3(0, 0, 1)
                    bullet.direction.applyQuaternion(bullet.quaternion);
                    bullet.direction.multiplyScalar(bullet.movementSpeed)
                    bullet.model.position.add(bullet.direction);
                    const translation = new THREE.Vector3(bullet.model.position.x, bullet.model.position.y, bullet.model.position.z)
                        .sub(bullet.box.getCenter(new THREE.Vector3()));
                    bullet.box.min.add(translation);
                    bullet.box.max.add(translation);
                    //setObjPos(bullet, bullet.model.position.x, bullet.model.position.y, bullet.model.position.z -= bullet.movementSpeed)
                })
            }
            const moveSpaceShipArrowHelper = () => {
                if (!spaceShip || !spaceShipArrowHelper) return
                spaceShipArrowHelper.position.copy(spaceShip.position);
                spaceShipArrowHelper.setDirection(new THREE.Vector3(0, 0, 1).applyQuaternion(this.spaceShipQuaternion.rotationQuaternion.clone()));
                //spaceShipArrowHelper.setDirection(spaceShip.getWorldDirection(new THREE.Vector3()));
            }

            setInterval(() => {
                collisionHandler()
                detectPositionTooFarHandler()
                detectShoot()
                detectCollisions()
                detectMeteorPos()
                detectBulletPos()
                detectCollisionDistance()
            }, 100)
            function animate() {
                moveSpaceShip(camera)
                moveMeteor()
                moveShootBullet()
                moveSpaceShipArrowHelper()
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
