<template>
  <div>
    <canvas ref="three"></canvas>
  </div>
</template>

<script>

import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';

import starsTexture from '@/assets/img/earth.jpg';
import sunTexture from '@/assets/img/sun.jpg';
import mercuryTexture from '@/assets/img/mercury.jpg';
import venusTexture from '@/assets/img/venus.jpg';
import earthTexture from '@/assets/img/earth.jpg';
import marsTexture from '@/assets/img/mars.jpg';
import jupiterTexture from '@/assets/img/jupiter.jpg';
import saturnTexture from '@/assets/img/saturn.jpg';
import saturnRingTexture from '@/assets/img/saturn ring.png';
import uranusTexture from '@/assets/img/uranus.jpg';
import uranusRingTexture from '@/assets/img/uranus ring.png';
import neptuneTexture from '@/assets/img/neptune.jpg';
import plutoTexture from '@/assets/img/pluto.jpg';


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

    const ambientLight = new THREE.AmbientLight(0x333333);
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

    const textureLoader = new THREE.TextureLoader();
    const sunGeo = new THREE.SphereGeometry(16, 30, 30);
    const sunMat = new THREE.MeshBasicMaterial({
      map: textureLoader.load(sunTexture)
    });
    const sun = new THREE.Mesh(sunGeo, sunMat);
    scene.add(sun);


    function createPlanete(size, texture, position, ring) {
      const geo = new THREE.SphereGeometry(size, 30, 30);
      const mat = new THREE.MeshStandardMaterial({
        map: textureLoader.load(texture)
      });
      const mesh = new THREE.Mesh(geo, mat);
      const obj = new THREE.Object3D();
      obj.add(mesh);
      if (ring) {
        //below is just similar createPlanete function to add ring
        const ringGeo = new THREE.RingGeometry(
          ring.innerRadius,
          ring.outerRadius,
          32);
        const ringMat = new THREE.MeshBasicMaterial({
          map: textureLoader.load(ring.texture),
          side: THREE.DoubleSide
        });
        const ringMesh = new THREE.Mesh(ringGeo, ringMat);
        obj.add(ringMesh);
        ringMesh.position.x = position;
        ringMesh.rotation.x = -0.5 * Math.PI;
      }
      scene.add(obj);
      mesh.position.x = position;
      return { mesh, obj }
    }

    const mercury = createPlanete(3.2, mercuryTexture, 28);
    const venus = createPlanete(5.8, venusTexture, 44);
    const earth = createPlanete(6, earthTexture, 62);
    const mars = createPlanete(4, marsTexture, 78);
    const jupiter = createPlanete(12, jupiterTexture, 100);
    const saturn = createPlanete(10, saturnTexture, 138, {
      innerRadius: 10,
      outerRadius: 20,
      texture: saturnRingTexture
    });
    const uranus = createPlanete(7, uranusTexture, 176, {
      innerRadius: 7,
      outerRadius: 12,
      texture: uranusRingTexture
    });
    const neptune = createPlanete(7, neptuneTexture, 200);
    const pluto = createPlanete(2.8, plutoTexture, 216);

    // sun's light ,light emit to all direction , distance in 2 to 300
    const pointLight = new THREE.PointLight(0xFFFFFF, 2, 300);
    scene.add(pointLight);

    function animate() {
      //Self-rotation
      sun.rotateY(0.004);
      mercury.mesh.rotateY(0.004);
      venus.mesh.rotateY(0.002);
      earth.mesh.rotateY(0.02);
      mars.mesh.rotateY(0.018);
      jupiter.mesh.rotateY(0.04);
      saturn.mesh.rotateY(0.038);
      uranus.mesh.rotateY(0.03);
      neptune.mesh.rotateY(0.032);
      pluto.mesh.rotateY(0.008);

      //Around-sun-rotation
      mercury.obj.rotateY(0.04);
      venus.obj.rotateY(0.015);
      earth.obj.rotateY(0.01);
      mars.obj.rotateY(0.008);
      jupiter.obj.rotateY(0.002);
      saturn.obj.rotateY(0.0009);
      uranus.obj.rotateY(0.0004);
      neptune.obj.rotateY(0.0001);
      pluto.obj.rotateY(0.00007);

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
