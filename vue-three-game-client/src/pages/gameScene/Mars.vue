<template>
    <div>
        <canvas ref="three"></canvas>
        <div class="marksPanel">
            <div class="bar">
                <div class="marks"> Marks: &nbsp;{{ marks }}/10 </div>
                <div style="display:flex;align-items: center;">
                    Life: &nbsp;
                    <v-progress-linear :model-value="life" max="10" bg-color="black" color="success" class="lifeBar" />

                </div>


            </div>
        </div>
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
import testTexture from '@/assets/img/earth.jpg'
import { GUI } from 'dat.gui'
class Car {
    constructor(scene, world) {
        this.scene = scene;
        this.world = world;
        this.car = {};
        this.chassis = {};
        this.wheels = [];
        this.cannonBody = null
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
        this.cannonBody = chassisBody
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
        const maxForce = 200;
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
            marks: 0,
            life: 10
        }
    },
    mounted() {
        this.initScene()
    },
    methods: {
        async initScene() {
            console.log("loading")
            let stats = new Stats();
            stats.showPanel(0);
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

            const ambientLight = new THREE.AmbientLight(0xffffff, 0.8)
            scene.add(ambientLight)

            const axesHelper = new THREE.AxesHelper(100);
            scene.add(axesHelper);

            const cubeTextureLoader = new THREE.CubeTextureLoader()
            const cubeEnvironmentMapTexture = cubeTextureLoader.load([
                starsTexture, starsTexture, starsTexture, starsTexture, starsTexture, starsTexture
            ])
            scene.background = cubeEnvironmentMapTexture

            const sizes = {
                width: window.innerWidth,
                height: window.innerHeight
            }

            window.addEventListener('resize', () => {
                sizes.width = window.innerWidth
                sizes.height = window.innerHeight

                camera.aspect = sizes.width / sizes.height
                camera.updateProjectionMatrix()

                renderer.setSize(sizes.width, sizes.height)
                renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2))
            })



            let matrix = [];
            let sizeX = 256,
                sizeY = 256;

            for (let i = 0; i < sizeX; i++) {
                matrix.push([]);
                for (let j = 0; j < sizeY; j++) {
                    let height = Math.cos(i / sizeX * Math.PI * 5) * Math.cos(j / sizeY * Math.PI * 5) * 3 + 2;
                    if (i === 0 || i === sizeX - 1 || j === 0 || j === sizeY - 1)
                        height = 5;
                    matrix[i].push(height);
                }
            }

            let hfShape = new CANNON.Heightfield(matrix, {
                elementSize: 200 / sizeX
            });
            let hfBody = new CANNON.Body({ mass: 0 });
            hfBody.addShape(hfShape);
            hfBody.position.set(-sizeX * hfShape.elementSize / 2, -4, sizeY * hfShape.elementSize / 2);
            hfBody.quaternion.setFromAxisAngle(new CANNON.Vec3(1, 0, 0), -Math.PI / 2);
            world.addBody(hfBody);

            let geometry = new THREE.BufferGeometry();

            // Generate vertices and faces based on the height field matrix
            let vertices = [];
            let faces = [];
            const colors = [];
            const color = new THREE.Color();
            const preDefineColors = [


                [0.7647058823529411, 0.3137254901960784, 0.1411764705882353],  // Mars Orange
                [0.8274509803921568, 0.4117647058823529, 0.19607843137254902], // Dusty Brown

            ]
            for (let i = 0; i < sizeX; i++) {
                for (let j = 0; j < sizeY; j++) {
                    let x = i * hfShape.elementSize - sizeX * hfShape.elementSize / 2;
                    let y = matrix[i][j];
                    let z = -j * hfShape.elementSize + sizeY * hfShape.elementSize / 2;
                    vertices.push(x, y, z);
                    const color = new THREE.Color();
                    const randomIndex = Math.floor(Math.random() * preDefineColors.length)
                    color.setRGB(preDefineColors[randomIndex][0], preDefineColors[randomIndex][1], preDefineColors[randomIndex][2]);

                    colors.push(color.r, color.g, color.b);

                    if (i < sizeX - 1 && j < sizeY - 1) {
                        let a = i * sizeY + j;
                        let b = i * sizeY + j + 1;
                        let c = (i + 1) * sizeY + j;
                        let d = (i + 1) * sizeY + j + 1;
                        faces.push(a, b, c);
                        faces.push(b, d, c);
                    }
                }
            }

            // Set the vertex and index attributes of the geometry
            geometry.setAttribute('position', new THREE.Float32BufferAttribute(vertices, 3));
            geometry.setAttribute('color', new THREE.Float32BufferAttribute(colors, 3));
            geometry.setIndex(faces);
            //const material = new THREE.MeshPhongMaterial({ side: THREE.DoubleSide, wireframe: true })
            let material = new THREE.MeshBasicMaterial({ side: THREE.DoubleSide, vertexColors: true });


            //let material = new THREE.MeshBasicMaterial({ color: 0x00ff00, wireframe: true });
            // Create mesh using the geometry and material
            let mesh = new THREE.Mesh(geometry, material);
            mesh.position.y = -4
            // Add the mesh to the scene
            scene.add(mesh);

            //iinit trasure
            const treasure = new URL('@/assets/model/treasure1.glb', import.meta.url)
            const gltfLoader = new GLTFLoader()
            const treasureList = []
            let treasureListLoadedCount = 0
            for (let i = 0; i < 10; i++) {
                gltfLoader.load(treasure.href, function (gltf) {
                    const model = gltf.scene;
                    scene.add(model);
                    const boxBody = new CANNON.Body({
                        mass: 1,
                        shape: new CANNON.Box(new CANNON.Vec3(1, 1, 1)),
                        position: new CANNON.Vec3(Math.random() * 100 - 50, 50, Math.random() * 100 - 50), //set it falling from sky
                    });
                    world.addBody(boxBody);
                    treasureList.push({
                        model,
                        box: new THREE.Box3().setFromObject(model),
                        cannonBody: boxBody,
                        needRemove: false
                    })
                    treasureListLoadedCount += 1
                });
            }
            const camera = new THREE.PerspectiveCamera(60, sizes.width / sizes.height, 0.1, 10000)
            camera.position.set(0, 10, -10)
            scene.add(camera)


            const rocksList = []
            const rockUrl = new URL('@/assets/model/rock.glb', import.meta.url)
            setInterval(() => {
                for (let i = 0; i < 10; i++) {
                    gltfLoader.load(rockUrl.href, function (gltf) {
                        const model = gltf.scene;
                        scene.add(model);
                        model.scale.set(Math.random() * (0.005 - 0.001) + 0.001, Math.random() * (0.005 - 0.001) + 0.001, Math.random() * (0.005 - 0.001) + 0.001)
                        model.position.set(Math.random() * (200) - 100, Math.random() * (200) - 100, Math.random() * (200) - 100)
                        const rockShape = new CANNON.Box(new CANNON.Vec3(0.5, 0.5, 0.3))
                        const rockBody = new CANNON.Body({ mass: 100 })
                        rockBody.addShape(rockShape)
                        rockBody.position.x = model.position.x
                        rockBody.position.y = model.position.y
                        rockBody.position.z = model.position.z
                        world.addBody(rockBody)

                        rocksList.push({ model, cannonBody: rockBody, touchedGround: false })
                    });
                }
            }, 5000)


            const gui = new GUI()

            const positionFolder = gui.addFolder('Position')
            positionFolder.add({
                resetPosition: () => {

                    console.log("reseting", car.cannonBody)
                    var newPosition = new CANNON.Vec3(car.cannonBody.position.x, 10, car.cannonBody.position.z); // Adjust x, y, z as needed
                    car.cannonBody.position.copy(newPosition);
                    car.cannonBody.velocity.set(0, 0, 0);
                    car.cannonBody.angularVelocity.set(0, 0, 0);
                }
            }, 'resetPosition').name('Reset');


            const controls = new OrbitControls(camera, canvas)
            controls.enableDamping = true


            const renderer = new THREE.WebGLRenderer({
                canvas: canvas
            })
            renderer.setSize(sizes.width, sizes.height)
            renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2))


            world.addEventListener('beginContact', (e) => {

                const bodyA = e.bodyA; // First colliding body
                const bodyB = e.bodyB; // Second colliding body

                //if (bodyA === hfBody || bodyB === hfBody) return
                treasureList.forEach(treasure => {
                    if ((bodyB === treasure.cannonBody && bodyA === car.cannonBody) || (bodyA === treasure.cannonBody && bodyB === car.cannonBody)) {
                        treasure.needRemove = true
                        this.marks += 1
                    }
                })


                rocksList.forEach(rock => {
                    if ((bodyA === rock.cannonBody && bodyB === car.cannonBody || bodyB === rock.cannonBody && bodyA === car.cannonBody) && !rock.touchedGround) {
                        console.log("rock hit car")
                        this.life -= 1
                        console.log(this.life)
                        rock.touchedGround = true
                    }
                    if (bodyA === rock.cannonBody && bodyB === hfBody || bodyB === rock.cannonBody && bodyA === hfBody) {
                        //console.log("rock touch ground")
                        rock.touchedGround = true
                    }
                })


            });
            const timeStep = 1 / 60
            const tick = () => {
                if (!camera || !car.chassis.position || treasureListLoadedCount != 10) return

                try {
                    stats.begin();
                    controls.update()

                    world.step(timeStep)
                    treasureList.map(treasure => {
                        if (treasure.needRemove) {
                            scene.remove(treasure.model)
                            world.removeBody(treasure.cannonBody)
                        } else {
                            treasure.model.position.copy(new CANNON.Vec3(treasure.cannonBody.position.x, treasure.cannonBody.position.y - 0.8, treasure.cannonBody.position.z))
                        }
                    })

                    rocksList.forEach(rock => {
                        rock.model.position.set(
                            rock.cannonBody.position.x,
                            rock.cannonBody.position.y,
                            rock.cannonBody.position.z
                        )
                        rock.model.quaternion.set(
                            rock.cannonBody.quaternion.x,
                            rock.cannonBody.quaternion.y,
                            rock.cannonBody.quaternion.z,
                            rock.cannonBody.quaternion.w
                        )
                    })



                    camera.parent = car.chassis
                    camera.lookAt(car.chassis.position)

                    renderer.render(scene, camera)
                    stats.end();
                } catch (e) {
                    console.error(e)
                }
            }
            renderer.setAnimationLoop(tick);
        }
    }
}
</script>

<style scoped>
.marksPanel {
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

.marks {
    background-color: white;
}

.lifeBar {
    height: 44px;
    width: 60px;
    margin-top: 5px;
}
</style>