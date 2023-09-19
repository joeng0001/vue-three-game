<template>
    <div>
        <canvas ref="three"></canvas>
    </div>
</template>

<script>

import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';

import starsTexture from '@/assets/img/space2.jpg';



export default {
    mounted() {

        console.log(this.$refs)
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

        camera.position.set(-90, 140, 140);
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

        const textureLoader = new THREE.TextureLoader()
        const sunGeo = new THREE.SphereGeometry(100, 30, 30);
        const sunMat = new THREE.MeshBasicMaterial({
            map: textureLoader.load(starsTexture)
        });
        const sun = new THREE.Mesh(sunGeo, sunMat);
        scene.add(sun);



        function animate() {
            renderer.render(scene, camera);
        }

        renderer.setAnimationLoop(animate);

        window.addEventListener('resize', function () {
            camera.aspect = window.innerWidth / window.innerHeight;
            camera.updateProjectionMatrix();
            renderer.setSize(window.innerWidth, window.innerHeight);
        });

    },
    data() {
        return {

        }
    },

}
</script>


<style scoped></style>
