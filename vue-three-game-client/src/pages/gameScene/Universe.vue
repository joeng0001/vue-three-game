<template>
    <div>
        <canvas ref="three"></canvas>
        <div class="scorePanel">
            <div class="bar">
                <div class="score"> Score: &nbsp;{{ score }}/{{ maxScore }} </div>
                <div class="panelContent">
                    <span>Life: </span>
                    <v-progress-linear :model-value="life" :max="maxLife" bg-color="white" color="success"
                        class="lifeBar" />
                </div>
                <div class="panelContent">
                    <span>Energy: </span>
                    <v-progress-linear :model-value="energy" :max="maxEnergy" bg-color="white" color="primary"
                        class="lifeBar" />
                </div>
                <div class="panelContent">
                    <span>Ammo: </span>
                    <v-progress-linear :model-value="ammo" :max="maxAmmo" bg-color="white" color="secondary"
                        class="lifeBar" />

                </div>
                <div class="panelContent">
                    <v-btn @click="endGame.dialog = true">end game</v-btn>
                </div>
            </div>
        </div>
        <v-dialog width="500" v-model="endGame.dialog">
            <v-card title="Dialog">
                <v-card-text>
                    {{ endGame.message }}
                </v-card-text>

                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn text="Try Again" v-show="endGame.win"></v-btn>
                    <v-btn text="Back" @click="endGame.dialog = false"></v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>

<script>

import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
import starsTexture from '@/assets/img/space2.jpg';
import Stats from 'three/examples/jsm/libs/stats.module'

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
                rotationQuaternion: ((new THREE.Quaternion())),//.setFromAxisAngle(new THREE.Vector3(0, 1, 0), Math.PI)),
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
            CollisionDistance: null,
            score: 0,
            ammo: 50 + this.getLevel() * 5,
            life: 100 + this.getLevel() * 5,
            energy: 60 - this.getLevel() * 5,
            maxLife: 50 + this.getLevel() * 5,
            maxAmmo: 50 + this.getLevel() * 5,
            maxEnergy: 60 - this.getLevel() * 5,
            maxScore: 20 + this.getLevel() * 2,
            maxMeteorNumber: 10 + this.getLevel() * 15,
            energyConsume: 0.01 * this.getLevel(),
            energyResume: 0.005,
            lifeConsume: 0.001 * this.getLevel(),
            endGame: {
                endGameDialog: false,
                endGameWin: false,
                endGameMessage: ""
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
            let spaceShipAnimationMixer = null
            const clock = new THREE.Clock();

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

            const ambientLight = new THREE.AmbientLight(0xFFFFFF, 1);
            scene.add(ambientLight);

            const axesHelper = new THREE.AxesHelper(100);
            scene.add(axesHelper);

            const stats = new Stats()
            document.body.appendChild(stats.dom)

            const cubeTextureLoader = new THREE.CubeTextureLoader();
            scene.background = cubeTextureLoader.load([
                starsTexture,
                starsTexture,
                starsTexture,
                starsTexture,
                starsTexture,
                starsTexture
            ]);
            const getRandomPosition = (maxDis) => {
                return [
                    Math.random() * maxDis * (Math.random() < 0.5 ? 1 : -1),
                    Math.random() * maxDis * (Math.random() < 0.5 ? 1 : -1),
                    Math.random() * maxDis * (Math.random() < 0.5 ? 1 : -1),
                ]
            }

            const ringGeometry = new THREE.TorusGeometry(
                2,
                0.5,
                64, 64
            );

            let rings = [], meteors = [], shootBullets = []
            const ringMaterial = new THREE.MeshBasicMaterial({ color: 0x90EE90, side: THREE.DoubleSide, });
            for (let i = 0; i < this.maxScore; i++) {
                const ringMesh = new THREE.Mesh(ringGeometry, ringMaterial);
                scene.add(ringMesh);
                ringMesh.position.set(...getRandomPosition(200))
                const randomQuaternion = new THREE.Quaternion();
                randomQuaternion.setFromEuler(new THREE.Euler(
                    Math.random() * 2 * Math.PI,
                    Math.random() * 2 * Math.PI,
                    Math.random() * 2 * Math.PI
                ));
                ringMesh.setRotationFromQuaternion(randomQuaternion);
                rings.push({ model: ringMesh, box: new THREE.Box3().setFromObject(ringMesh), collisionHappened: false })
            }
            for (let i = 0; i < this.maxMeteorNumber; i++) {
                gltfLoader.load(meteorUrl.href, function (gltf) {
                    const model = gltf.scene;
                    scene.add(model);
                    model.position.set(...getRandomPosition(200))
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
                });
                gltfLoader.load(meteor2Url.href, function (gltf) {
                    const model = gltf.scene;
                    scene.add(model);
                    model.position.set(...getRandomPosition(200))
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
                });
                gltfLoader.load(meteor3Url.href, function (gltf) {
                    const model = gltf.scene;
                    scene.add(model);
                    model.position.set(...getRandomPosition(200))
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
                });
            }

            var rotationMatrix = new THREE.Matrix4().makeRotationY(Math.PI / 2);
            scene.matrix.multiply(rotationMatrix);
            scene.matrixAutoUpdate = false;

            let spaceShip = null
            let spaceShipArrowHelper = null
            gltfLoader.load(spaceShipUrl.href, function (gltf) {
                const model = gltf.scene;
                //model.scale.set(0.5, 0.5, 0.5)
                //model.rotation.y = Math.PI

                spaceShipArrowHelper = new THREE.ArrowHelper(
                    model.getWorldDirection(new THREE.Vector3()),
                    model.position,
                    100,
                    0xff0000
                );

                scene.add(spaceShipArrowHelper);
                scene.add(model);
                spaceShip = model

                spaceShipAnimationMixer = new THREE.AnimationMixer(model);
                const clips = gltf.animations;

                // Play all animations at the same time
                clips.forEach(function (clip) {
                    const action = spaceShipAnimationMixer.clipAction(clip);
                    action.play();
                });
            });
            let detectCollisionLock = false, detectPositionTooFarLock = false, detectShootLock = false

            const collisionHandler = async () => {
                if (!spaceShip || detectCollisionLock) return
                detectCollisionLock = true
                //remove collision
                rings.forEach(ring => {
                    if (ring.collisionHappened) {
                        scene.remove(ring.model)
                        scene.remove(ring.box)
                        this.score += 1
                        this.life < this.maxLife - 10 ? this.life += 10 : this.life = this.maxLife
                        this.ammo < this.maxAmmo - 10 ? this.ammo += 10 : this.ammo = this.maxAmmo
                        rings = rings.filter(obj => obj != ring)
                    }
                })

                meteors.forEach(meteor => {
                    if (meteor.collisionHappened || meteor.gotShoot) {
                        setObjPos(meteor, ...getRandomPosition(200))
                        if (meteor.collisionHappened) {
                            meteor.collisionHappened = false
                            this.life -= 10
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

            const detectPositionTooFarHandler = async () => {
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


            const detectCollisions = async () => {
                if (!spaceShip) return
                const spaceShipBox = new THREE.Box3().setFromObject(spaceShip)
                meteors.forEach(meteor => {
                    if (spaceShipBox.intersectsBox(meteor.box)) {
                        meteor.collisionHappened = true
                    }
                })
                rings.forEach(ring => {
                    if (spaceShipBox.intersectsBox(ring.box)) {
                        ring.collisionHappened = true
                    }
                })

                shootBullets.forEach(bullet => {
                    meteors.forEach(meteor => {
                        if (bullet.box.intersectsBox(meteor.box)) {
                            bullet.collisionHappened = true
                            meteor.gotShoot = true
                        }
                    })
                })
            }

            const detectMeteorPos = async () => {
                if (!spaceShip) return
                meteors.forEach(meteor => {
                    if (meteor.model.position.z > 300 || meteor.model.position.z < -300 ||
                        meteor.model.position.y > 300 || meteor.model.position.y < -300 ||
                        meteor.model.position.x > 300 || meteor.model.position.x < -300) {
                        meteor.positionTooFar = true
                    }
                })
            }

            const detectBulletPos = async () => {
                if (!spaceShip) return
                shootBullets.forEach(bullet => {
                    const distance = Math.sqrt(
                        Math.pow(bullet.model.position.x - spaceShip.position.x, 2) +
                        Math.pow(bullet.model.position.y - spaceShip.position.y, 2) +
                        Math.pow(bullet.model.position.z - spaceShip.position.z, 2)
                    );
                    if (distance > 200) {
                        bullet.positionTooFar = true
                    }
                })
            }

            const detectCollisionDistance = async () => {
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

            const moveMeteor = async () => {
                if (!spaceShip) return
                meteors.forEach(meteor => {
                    if (meteor.rotationX) meteor.model.rotation.x += meteor.rotationSpeed
                    if (meteor.rotationY) meteor.model.rotation.y += meteor.rotationSpeed
                    if (meteor.rotationZ) meteor.model.rotation.z += meteor.rotationSpeed
                    meteor.movementX ? meteor.model.position.x += meteor.movementSpeed : meteor.model.position.x -= meteor.movementSpeed
                    meteor.movementY ? meteor.model.position.y += meteor.movementSpeed : meteor.model.position.y -= meteor.movementSpeed
                    meteor.movementZ ? meteor.model.position.z += meteor.movementSpeed : meteor.model.position.z -= meteor.movementSpeed
                    setObjPos(meteor, meteor.model.position.x, meteor.model.position.y, meteor.model.position.z)
                })
            }

            const moveSpaceShip = async (camera) => {
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
                    if (this.energy > 0) {
                        this.energy -= this.energyConsume
                    }
                } else {
                    speed.factor = 1
                    if (this.energy < this.maxEnergy) {
                        this.energy += this.energyResume
                    }

                }
                if (movementKey['w'] || movementKey['s']) {
                    let direction = null
                    if (movementKey['w']) {
                        direction = new THREE.Vector3(0, 0, -1);
                    } else {
                        direction = new THREE.Vector3(0, 0, 1);
                    }
                    direction.applyQuaternion(this.spaceShipQuaternion.rotationQuaternion);
                    spaceShip.position.add(direction.multiplyScalar(speed.value * speed.factor));
                }
                if (movementKey['a'] || movementKey['d']) {
                    if (movementKey['a']) {
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.horizontal.axis, this.spaceShipQuaternion.horizontal.quaternionAngel = 0.01));
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.side.axis, this.spaceShipQuaternion.side.quaternionAngel = 0.01));
                    } else {
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.horizontal.axis, this.spaceShipQuaternion.horizontal.quaternionAngel = -0.01));
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.side.axis, this.spaceShipQuaternion.side.quaternionAngel = -0.01));
                    }
                    this.spaceShipQuaternion.horizontal.direction.applyQuaternion(this.spaceShipQuaternion.rotationQuaternion);
                    spaceShip.position.add(this.spaceShipQuaternion.horizontal.direction.multiplyScalar(speed.value * speed.factor));
                    spaceShip.position.add(this.spaceShipQuaternion.side.direction.multiplyScalar(speed.value * speed.factor));
                    spaceShip.setRotationFromQuaternion(this.spaceShipQuaternion.rotationQuaternion);
                }
                if (movementKey['Alt'] || movementKey[' ']) {
                    if (movementKey['Alt']) {
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.vertical.axis, this.spaceShipQuaternion.vertical.quaternionAngel = -0.005));
                    }
                    if (movementKey[' ']) {
                        this.spaceShipQuaternion.rotationQuaternion.multiply(new THREE.Quaternion().setFromAxisAngle(this.spaceShipQuaternion.vertical.axis, this.spaceShipQuaternion.vertical.quaternionAngel = 0.005));
                    }
                    this.spaceShipQuaternion.vertical.direction.applyQuaternion(this.spaceShipQuaternion.rotationQuaternion);
                    spaceShip.position.add(this.spaceShipQuaternion.vertical.direction.multiplyScalar(speed.value * speed.factor));
                    spaceShip.setRotationFromQuaternion(this.spaceShipQuaternion.rotationQuaternion);
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
                if (detectShootLock || this.ammo <= 0) return
                detectShootLock = true
                if (this.control.shoot) {
                    this.ammo -= 1
                    const movementSpeed = this.speed.bulletSpeed
                    const quaternion = this.spaceShipQuaternion.rotationQuaternion.clone()
                    //quaternion.setFromAxisAngle(new THREE.Vector3(0, 1, 0), Math.PI)
                    gltfLoader.load(shootBulletUrl.href, function (gltf) {
                        const model = gltf.scene;

                        model.traverse(function (child) {
                            if (child instanceof THREE.Mesh) {
                                child.material = new THREE.MeshBasicMaterial({ color: 0xFFBF00 });
                            }
                        });
                        model.scale.set(0.01, 0.01, 0.01)
                        model.setRotationFromQuaternion(quaternion);
                        model.position.set(spaceShip.position.x, spaceShip.position.y, spaceShip.position.z)
                        scene.add(model);
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
            const moveShootBullet = async () => {
                shootBullets.forEach(bullet => {
                    bullet.direction = new THREE.Vector3(0, 0, -1)
                    bullet.direction.applyQuaternion(bullet.quaternion);
                    bullet.direction.multiplyScalar(bullet.movementSpeed)
                    bullet.model.position.add(bullet.direction);
                    const translation = new THREE.Vector3(bullet.model.position.x, bullet.model.position.y, bullet.model.position.z)
                        .sub(bullet.box.getCenter(new THREE.Vector3()));
                    bullet.box.min.add(translation);
                    bullet.box.max.add(translation);
                })
            }
            const moveSpaceShipArrowHelper = async () => {
                if (!spaceShip || !spaceShipArrowHelper) return
                spaceShipArrowHelper.position.copy(spaceShip.position);
                spaceShipArrowHelper.setDirection(new THREE.Vector3(0, 0, -1).applyQuaternion(this.spaceShipQuaternion.rotationQuaternion.clone()));
            }
            const playSpaceShipAnimation = () => {
                if (!spaceShipAnimationMixer) return
                const delta = clock.getDelta();
                spaceShipAnimationMixer.update(delta);
            }
            const lifeComsume = () => {
                this.life -= this.lifeConsume
            }

            const detectWinOrLose = () => {
                if (this.life <= 0) {
                    this.endGame.win = false
                    this.endGame.message = "you lose!"
                    this.endGame.dialog = true
                    console.error("you lose")
                }
                if (this.score >= this.maxScore) {
                    this.endGame.win = true
                    this.endGame.message = "you win!"
                    this.endGame.dialog = true
                    console.error("you win")
                }
            }
            console.log("setting interval")
            setInterval(() => {
                collisionHandler()
                detectPositionTooFarHandler()
                detectShoot()
                detectCollisions()
                detectMeteorPos()
                detectBulletPos()
                detectCollisionDistance()
                detectWinOrLose()
                console.log(this.life, this.energy, this.score, this.ammo)
            }, 200)
            function animate() {
                stats.begin();
                moveSpaceShip(camera)
                moveMeteor()
                moveShootBullet()
                moveSpaceShipArrowHelper()
                playSpaceShipAnimation()
                lifeComsume()
                stats.end();
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
        },
        getLevel() {
            return this.$route.query.level > 8 || this.$route.query.level < 1 ? 1 : parseInt(this.$route.query.level)
        }
    }

}
</script>


<style scoped>
.scorePanel {
    z-index: 99;
    width: 100%;
    display: flex;
    justify-content: center;
}

.bar {
    position: absolute;
    top: 5%;

    font-family: fantasy;
    font-size: 16px;
    font-weight: 300;
    text-align: center;
    margin-left: 10px;
}

.score {
    background-color: white;
}

.lifeBar {
    height: 44px;
    width: 60px;
    margin-top: 5px;
}

.panelContent {
    display: flex;
    align-items: center;
    justify-content: space-between;
    color: white;
}
</style>
