<template>
    <div>
        <canvas ref="three"></canvas>
    </div>
</template>

<script>
import * as THREE from 'three'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import * as CANNON from 'cannon-es'
import Stats from 'stats.js';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader';
import { DRACOLoader } from 'three/examples/jsm/loaders/DRACOLoader.js';
import starsTexture from '@/assets/img/space2.jpg';
class Car {
    constructor(scene, world) {
        this.scene = scene;
        this.world = world;

        this.car = {};
        this.chassis = {};
        this.wheels = [];
        this.chassisDimension = {
            x: 2.65,
            y: 1.25,
            z: 4.66
        };
        this.chassisModelPos = {
            x: 0,
            y: -0.9499999999999993,
            z: 0.02
        };
        this.wheelScale = {
            frontWheel: 0.01,
            hindWheel: 0.01
        };
        this.mass = 250;
    }

    init() {
        this.loadModels();
        this.setChassis();
        this.setWheels();
        this.controls();
        this.update()
    }

    loadModels() {
        const gltfLoader = new GLTFLoader();
        const dracoLoader = new DRACOLoader();

        dracoLoader.setDecoderConfig({ type: 'js' })
        dracoLoader.setDecoderPath('https://www.gstatic.com/draco/v1/decoders/');

        const chassisUrl = new URL('@/assets/model/marsCar.glb', import.meta.url)
        const wheelUrl = new URL('@/assets/model/wheel.glb', import.meta.url)

        gltfLoader.setDRACOLoader(dracoLoader);

        gltfLoader.load(chassisUrl.href, gltf => {
            this.chassis = gltf.scene;
            this.scene.add(this.chassis);
        })

        this.wheels = [];
        for (let i = 0; i < 4; i++) {
            gltfLoader.load(wheelUrl.href, gltf => {
                const model = gltf.scene;
                this.wheels[i] = model;
                if (i === 1 || i === 3)
                    this.wheels[i].scale.set(-1 * this.wheelScale.frontWheel, 1 * this.wheelScale.frontWheel, -1 * this.wheelScale.frontWheel);
                else
                    this.wheels[i].scale.set(1 * this.wheelScale.frontWheel, 1 * this.wheelScale.frontWheel, 1 * this.wheelScale.frontWheel);
                this.scene.add(this.wheels[i]);
            })
        }
    }

    setChassis() {
        const chassisShape = new CANNON.Box(new CANNON.Vec3(this.chassisDimension.x * 0.5, this.chassisDimension.y * 0.5, this.chassisDimension.z * 0.5));
        const chassisBody = new CANNON.Body({ mass: this.mass, material: new CANNON.Material({ friction: 0 }) });
        chassisBody.addShape(chassisShape);

        this.car = new CANNON.RaycastVehicle({
            chassisBody,
            indexRightAxis: 0,
            indexUpAxis: 1,
            indexForwardAxis: 2
        });
        this.car.addToWorld(this.world);
    }

    setWheels() {
        this.car.wheelInfos = [];
        this.car.addWheel({
            radius: 0.57,
            directionLocal: new CANNON.Vec3(0, -1, 0),
            suspensionStiffness: 55,
            suspensionRestLength: 0.5,
            frictionSlip: 30,
            dampingRelaxation: 2.3,
            dampingCompression: 4.3,
            maxSuspensionForce: 10000,
            rollInfluence: 0.01,
            axleLocal: new CANNON.Vec3(-1, 0, 0),
            chassisConnectionPointLocal: new CANNON.Vec3(0.5899999999999999, 0.1, -1.75),
            maxSuspensionTravel: 1,
            customSlidingRotationalSpeed: 30,
        });
        this.car.addWheel({
            radius: 0.57,
            directionLocal: new CANNON.Vec3(0, -1, 0),
            suspensionStiffness: 55,
            suspensionRestLength: 0.5,
            frictionSlip: 30,
            dampingRelaxation: 2.3,
            dampingCompression: 4.3,
            maxSuspensionForce: 10000,
            rollInfluence: 0.01,
            axleLocal: new CANNON.Vec3(-1, 0, 0),
            chassisConnectionPointLocal: new CANNON.Vec3(-0.5899999999999999, 0.1, -1.75),
            maxSuspensionTravel: 1,
            customSlidingRotationalSpeed: 30,
        });
        this.car.addWheel({
            radius: 0.57,
            directionLocal: new CANNON.Vec3(0, -1, 0),
            suspensionStiffness: 55,
            suspensionRestLength: 0.5,
            frictionSlip: 30,
            dampingRelaxation: 2.3,
            dampingCompression: 4.3,
            maxSuspensionForce: 10000,
            rollInfluence: 0.01,
            axleLocal: new CANNON.Vec3(-1, 0, 0),
            chassisConnectionPointLocal: new CANNON.Vec3(0.5899999999999999, 0.1, 1.75),
            maxSuspensionTravel: 1,
            customSlidingRotationalSpeed: 30,
        });
        this.car.addWheel({
            radius: 0.57,
            directionLocal: new CANNON.Vec3(0, -1, 0),
            suspensionStiffness: 55,
            suspensionRestLength: 0.5,
            frictionSlip: 30,
            dampingRelaxation: 2.3,
            dampingCompression: 4.3,
            maxSuspensionForce: 10000,
            rollInfluence: 0.01,
            axleLocal: new CANNON.Vec3(-1, 0, 0),
            chassisConnectionPointLocal: new CANNON.Vec3(-0.5899999999999999, 0.1, 1.75),
            maxSuspensionTravel: 1,
            customSlidingRotationalSpeed: 30,
        });

        this.car.wheelInfos.forEach(function (wheel, index) {
            const cylinderShape = new CANNON.Cylinder(wheel.radius, wheel.radius, wheel.radius / 2, 20)
            const wheelBody = new CANNON.Body({
                mass: 1,
                material: new CANNON.Material({ friction: 0 }),
            })
            const quaternion = new CANNON.Quaternion().setFromEuler(-Math.PI / 2, 0, 0)
            wheelBody.addShape(cylinderShape, new CANNON.Vec3(), quaternion)
            // this.wheels[index].wheelBody = wheelBody;
        }.bind(this));
    }

    controls() {
        const maxSteerVal = 0.2;
        const maxForce = 10;
        const brakeForce = 36;
        const slowDownCar = 19.6;
        const keysPressed = [];

        window.addEventListener('keydown', (e) => {
            // e.preventDefault();
            if (!keysPressed.includes(e.key.toLowerCase())) keysPressed.push(e.key.toLowerCase());
            hindMovement();
        });
        window.addEventListener('keyup', (e) => {
            // e.preventDefault();
            keysPressed.splice(keysPressed.indexOf(e.key.toLowerCase()), 1);
            hindMovement();
        });

        const hindMovement = () => {
            if (keysPressed.includes("r") || keysPressed.includes("r")) resetCar();

            if (!keysPressed.includes(" ") && !keysPressed.includes(" ")) {
                this.car.setBrake(0, 0);
                this.car.setBrake(0, 1);
                this.car.setBrake(0, 2);
                this.car.setBrake(0, 3);

                if (keysPressed.includes("a") || keysPressed.includes("arrowleft")) {
                    this.car.setSteeringValue(maxSteerVal * 1, 2);
                    this.car.setSteeringValue(maxSteerVal * 1, 3);
                }
                else if (keysPressed.includes("d") || keysPressed.includes("arrowright")) {
                    this.car.setSteeringValue(maxSteerVal * -1, 2);
                    this.car.setSteeringValue(maxSteerVal * -1, 3);
                }
                else stopSteer();

                if (keysPressed.includes("w") || keysPressed.includes("arrowup")) {
                    this.car.applyEngineForce(maxForce * -1, 0);
                    this.car.applyEngineForce(maxForce * -1, 1);
                    this.car.applyEngineForce(maxForce * -1, 2);
                    this.car.applyEngineForce(maxForce * -1, 3);
                }
                else if (keysPressed.includes("s") || keysPressed.includes("arrowdown")) {
                    this.car.applyEngineForce(maxForce * 1, 0);
                    this.car.applyEngineForce(maxForce * 1, 1);
                    this.car.applyEngineForce(maxForce * 1, 2);
                    this.car.applyEngineForce(maxForce * 1, 3);
                }
                else stopCar();
            }
            else
                brake();
        }

        const resetCar = () => {
            this.car.chassisBody.position.set(0, 4, 0);
            this.car.chassisBody.quaternion.set(0, 0, 0, 1);
            this.car.chassisBody.angularVelocity.set(0, 0, 0);
            this.car.chassisBody.velocity.set(0, 0, 0);
        }

        const brake = () => {
            this.car.setBrake(brakeForce, 0);
            this.car.setBrake(brakeForce, 1);
            this.car.setBrake(brakeForce, 2);
            this.car.setBrake(brakeForce, 3);
        }

        const stopCar = () => {
            this.car.setBrake(slowDownCar, 0);
            this.car.setBrake(slowDownCar, 1);
            this.car.setBrake(slowDownCar, 2);
            this.car.setBrake(slowDownCar, 3);
        }

        const stopSteer = () => {
            this.car.setSteeringValue(0, 2);
            this.car.setSteeringValue(0, 3);
        }
    }

    update() {
        const updateWorld = () => {
            if (this.car.wheelInfos && this.chassis.position && this.wheels[0].position) {
                this.chassis.position.set(
                    this.car.chassisBody.position.x + this.chassisModelPos.x,
                    this.car.chassisBody.position.y + this.chassisModelPos.y,
                    this.car.chassisBody.position.z + this.chassisModelPos.z
                );
                this.chassis.quaternion.copy(this.car.chassisBody.quaternion);
                for (let i = 0; i < 4; i++) {
                    if (this.car.wheelInfos[i]) {
                        this.car.updateWheelTransform(i);
                        this.wheels[i].position.copy(this.car.wheelInfos[i].worldTransform.position);
                        this.wheels[i].quaternion.copy(this.car.wheelInfos[i].worldTransform.quaternion);
                    }
                }
            }
        }
        this.world.addEventListener('postStep', updateWorld);
    }
}


export default {
    data() {
        return {

        }
    },
    mounted() {
        this.initScene()
    },
    methods: {
        async initScene() {
            var stats = new Stats();
            stats.showPanel(0); // 0: fps, 1: ms, 2: mb, 3+: custom
            document.body.appendChild(stats.dom);
            const canvas = this.$refs.three
            const scene = new THREE.Scene()
            const world = new CANNON.World({
                gravity: new CANNON.Vec3(0, -9.82, 0), // m/s²
            })
            world.broadphase = new CANNON.SAPBroadphase(world);


            const car = new Car(scene, world);
            car.init();

            const bodyMaterial = new CANNON.Material();
            const groundMaterial = new CANNON.Material();
            const bodyGroundContactMaterial = new CANNON.ContactMaterial(
                bodyMaterial,
                groundMaterial,
                {
                    friction: 0.1,
                    restitution: 0.3
                }
            )
            world.addContactMaterial(bodyGroundContactMaterial)

            const ambientLight = new THREE.AmbientLight(0xffffff, 0.4)
            const spotLight = new THREE.SpotLight(0x29dfff, 2, 0, 0.9, 1, 0);
            spotLight.position.set(7, 1.291, -6);
            const spotLight2 = new THREE.SpotLight(0x943dff, 2, 0, 0.9, 1, 0);
            spotLight2.position.set(-7, 1.291, -6);
            const spotLight3 = new THREE.SpotLight(0xd5f8ff, 2, 0, 0.9, 1, 0);
            spotLight3.position.set(0, 1.291, 7);
            scene.add(spotLight, spotLight2, spotLight3);

            /**
             * Cube Texture Loader
             */
            const cubeTextureLoader = new THREE.CubeTextureLoader()
            const cubeEnvironmentMapTexture = cubeTextureLoader.load([
                starsTexture, starsTexture, starsTexture, starsTexture, starsTexture, starsTexture
            ])
            // scene.background = cubeEnvironmentMapTexture
            scene.environment = cubeEnvironmentMapTexture

            /**
             * Sizes
             */
            const sizes = {
                width: window.innerWidth,
                height: window.innerHeight
            }

            window.addEventListener('resize', () => {
                // Update sizes
                sizes.width = window.innerWidth
                sizes.height = window.innerHeight

                // Update camera
                camera.aspect = sizes.width / sizes.height
                camera.updateProjectionMatrix()

                // Update renderer
                renderer.setSize(sizes.width, sizes.height)
                renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2))
            })

            /**
             * Floor
             */
            const floorGeo = new THREE.PlaneGeometry(100, 100);
            const floorMesh = new THREE.Mesh(
                floorGeo,
                new THREE.MeshStandardMaterial({
                    color: 0xffffff,
                    roughness: 0.5,
                    metalness: 0,
                    emissive: 0xffffff,
                    emissiveIntensity: -0.36,
                })
            )
            floorMesh.rotation.x = -Math.PI * 0.5;
            scene.add(floorMesh)

            const floorS = new CANNON.Plane();
            const floorB = new CANNON.Body();
            floorB.mass = 0;

            floorB.addShape(floorS);
            world.addBody(floorB);

            floorB.quaternion.setFromAxisAngle(
                new CANNON.Vec3(-1, 0, 0),
                Math.PI * 0.5
            );


            const camera = new THREE.PerspectiveCamera(50, sizes.width / sizes.height, 0.1, 10000)
            camera.position.set(0, 4, 6)
            scene.add(camera)

            // Controls
            const controls = new OrbitControls(camera, canvas)
            controls.enableDamping = true

            /**
             * Renderer
             */
            const renderer = new THREE.WebGLRenderer({
                canvas: canvas
            })
            renderer.setSize(sizes.width, sizes.height)
            renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2))

            /**
             * Animate
             */
            const timeStep = 1 / 60 // seconds
            let lastCallTime = null
            function sleep(ms) {
                return new Promise(resolve => setTimeout(resolve, ms));
            }
            await sleep(5000)
            const tick = () => {
                try {


                    stats.begin();
                    // Update controls
                    controls.update()




                    const time = performance.now() / 1000 // seconds
                    if (!lastCallTime) {
                        world.step(timeStep)
                    } else {
                        const dt = time - lastCallTime
                        world.step(timeStep, dt)
                    }
                    world.step(timeStep)

                    lastCallTime = time

                    // Render
                    renderer.render(scene, camera)
                    stats.end();

                    // Call tick again on the next frame
                    window.requestAnimationFrame(tick)
                } catch (e) {
                    console.error(e)
                }
            }


            tick()

        }
    }
}
</script>

<style scoped></style>