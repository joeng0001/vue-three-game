<template>
    <div>
        <canvas ref="three"></canvas>
    </div>
</template>

<script>
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';

import * as CANNON from "cannon-es"
export default {
    data() {
        return {

        }
    },
    mounted() {
        this.initScene()
    },
    methods: {
        initScene() {
            const gltfLoader = new GLTFLoader();
            const mapUrl = new URL('@/assets/model/marsMap.glb', import.meta.url)

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

            const addWireFrame = (model) => {
                if (model.isMesh) {
                    if (Array.isArray(model.material)) {
                        model.material.forEach(function (material) { material.wireframe = true; });
                    } else {
                        model.material.wireframe = true;
                    }
                }

                model.children.forEach(function (child) {
                    if (child.children.length > 0) {
                        addWireFrame(child);
                    } else if (child.isMesh) {
                        if (Array.isArray(child.material)) {
                            child.material.forEach(function (material) { material.wireframe = true; });
                        } else {
                            child.material.wireframe = true;
                        }
                    }
                });
            }
            gltfLoader.load(mapUrl.href, function (gltf) {
                const model = gltf.scene;
                model.scale.set(10, 10, 10)
                scene.add(model);
                addWireFrame(model)
            });



            const boxGeo = new THREE.BoxGeometry(2, 2, 2);
            const boxMat = new THREE.MeshBasicMaterial({
                color: 0x00ff00,
                wireframe: true,
            });
            const boxMesh = new THREE.Mesh(boxGeo, boxMat);
            scene.add(boxMesh);

            const sphereGeo = new THREE.SphereGeometry(2);
            const sphereMat = new THREE.MeshBasicMaterial({
                color: 0xff0000,
                wireframe: true,
                side: THREE.DoubleSide,
            });
            const sphereMesh = new THREE.Mesh(sphereGeo, sphereMat);
            scene.add(sphereMesh);

            // a plane
            const groundGeo = new THREE.PlaneGeometry(30, 30);
            const groundMat = new THREE.MeshBasicMaterial({
                color: 0xffffff,
                side: THREE.DoubleSide,
                wireframe: true,
            });
            const groundMesh = new THREE.Mesh(groundGeo, groundMat);
            scene.add(groundMesh);

            const world = new CANNON.World({
                //force in x,y,z direction
                gravity: new CANNON.Vec3(0, -9.81, 0),
            });

            const groundPhysMat = new CANNON.Material();
            //make a body,need to match geometory in three js,i.e. plane -> create infinite ground
            const groundBody = new CANNON.Body({
                //plane is infinte,-> no falling even out of border -> change it to box model

                //shape: new CANNON.Plane(),
                //mass: 10 //mass default is 0,that refer to static body -> static means mass 0
                shape: new CANNON.Box(new CANNON.Vec3(15, 15, 0.1)),
                type: CANNON.Body.STATIC,
                material: groundPhysMat,
            });
            world.addBody(groundBody);
            //use cannon rotate the ground from verticval to horizontal
            groundBody.quaternion.setFromEuler(-Math.PI / 2, 0, 0);

            const boxPhysMat = new CANNON.Material();

            const boxBody = new CANNON.Body({
                mass: 1,
                // value in CANNON.Vec3() need to be half of setting in three js->const boxGeo
                shape: new CANNON.Box(new CANNON.Vec3(1, 1, 1)),
                position: new CANNON.Vec3(1, 20, 0), //set it falling from sky
                material: boxPhysMat,
            });
            world.addBody(boxBody);

            //set rotation speed and set it linear to slow down
            boxBody.angularVelocity.set(0, 10, 0);
            boxBody.angularDamping = 0.5;

            const groundBoxContactMat = new CANNON.ContactMaterial(
                groundPhysMat,
                boxPhysMat,
                { friction: 0.04 } // set friction to these 2 material
            );

            world.addContactMaterial(groundBoxContactMat);

            const spherePhysMat = new CANNON.Material();

            const sphereBody = new CANNON.Body({
                mass: 4,
                shape: new CANNON.Sphere(2),
                position: new CANNON.Vec3(0, 10, 0),
                material: spherePhysMat,
            });
            world.addBody(sphereBody);
            //make the falling speed and moving become linear to similar resistance and mass
            sphereBody.linearDamping = 0.21;

            const groundSphereContactMat = new CANNON.ContactMaterial(
                groundPhysMat,
                spherePhysMat,
                { restitution: 0.9 } // make it bounce like a ball
            );

            world.addContactMaterial(groundSphereContactMat);
            //control time elaspe
            const timeStep = 1 / 60;

            function animate() {
                //cannon js auto handle coolisiion

                world.step(timeStep);
                //update three js body by cannon js body
                groundMesh.position.copy(groundBody.position);
                //update orientation by cannon js
                groundMesh.quaternion.copy(groundBody.quaternion);

                boxMesh.position.copy(boxBody.position);
                boxMesh.quaternion.copy(boxBody.quaternion);

                sphereMesh.position.copy(sphereBody.position);
                sphereMesh.quaternion.copy(sphereBody.quaternion);

                renderer.render(scene, camera);
            }

            renderer.setAnimationLoop(animate);
        }
    }
}
</script>

<style scoped></style>