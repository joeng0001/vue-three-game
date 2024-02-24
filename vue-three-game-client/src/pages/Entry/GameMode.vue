<template>
    <v-dialog width="800" v-model="dialog" persistent>
        <v-card class="d-flex justify-center">
            <v-card-title class="d-flex justify-center SceneTitle">
                <span></span>
                <span></span>
                <span></span>
                <span></span>
                Pick A Scene
            </v-card-title>

            <!--after pick the scene,move the camera the the game scenario-->
            <v-card-actions class="d-flex justify-space-between mt-4">
                <v-btn class="btn" style="color:Aquamarine;"
                    @click="() => $router.push('/entry/gameLevel?scene=Universe')">
                    <v-tooltip activator="parent" location="bottom">Universe
                    </v-tooltip>
                    <canvas ref="universe"></canvas></v-btn>

                <!--in development-->
                <v-btn class="btn" style="color:GreenYellow;" @click="() => $router.push('/entry/gameLevel?scene=Earth')">
                    <v-tooltip activator="parent" location="bottom">Earth
                    </v-tooltip>
                    <canvas ref="earth"></canvas>
                </v-btn>


                <v-btn class="btn" style="color:orange;" @click="() => $router.push('/entry/gameLevel?scene=Mars')">
                    <v-tooltip activator="parent" location="bottom">Mars
                    </v-tooltip>
                    <canvas ref="mars"></canvas>
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import * as THREE from "three"
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { GLTFLoader } from "three/examples/jsm/loaders/GLTFLoader";
import earthTexture from '@/assets/img/earth.jpg'
import marsTexture from '@/assets/img/mars.jpg'
export default {
    mounted() {
        this.initUniverseBtn()
        this.initEarthBtn()
        this.initMarsBtn()
    },
    data() {
        return {
            dialog: true,
            renderer: []
        }
    },
    beforeUnmount() {
        if (this.renderer) {
            this.renderer.forEach(renderer => {
                renderer.setAnimationLoop(null)
            })
        }
    },
    methods: {
        initUniverseBtn() {
            const gltfLoader = new GLTFLoader()
            const scene = new THREE.Scene();
            const canvas = this.$refs.universe
            const renderer = new THREE.WebGLRenderer({ canvas, antialias: true, alpha: true });
            this.renderer.push(renderer)
            const blackholeUrl = new URL('@/assets/model/blackhole.glb', import.meta.url)
            renderer.setSize(100, 100);
            const camera = new THREE.PerspectiveCamera(
                45,
                window.innerWidth / window.innerHeight,
                0.1,
                1000
            );
            const orbit = new OrbitControls(camera, renderer.domElement);
            const clock = new THREE.Clock();
            camera.position.set(0, 15, 0);
            orbit.update();
            const ambientLight = new THREE.AmbientLight(0x333333);
            scene.add(ambientLight);

            let blackholeAnimation = null
            gltfLoader.load(blackholeUrl.href, function (gltf) {
                const model = gltf.scene;
                scene.add(model);
                model.position.set(0, 0, 0)

                blackholeAnimation = new THREE.AnimationMixer(model);
                blackholeAnimation.timeScale = 2.0
                const clips = gltf.animations;

                // Play all animations at the same time
                clips.forEach(function (clip) {
                    const action = blackholeAnimation.clipAction(clip);
                    action.play();
                });

            });

            const playBlackholeAnimation = () => {
                if (!blackholeAnimation) return
                const delta = clock.getDelta();
                blackholeAnimation.update(delta);
            }
            function animate() {
                playBlackholeAnimation()
                renderer.render(scene, camera);
            }

            renderer.setAnimationLoop(animate);
        },
        initEarthBtn() {
            const scene = new THREE.Scene();
            const canvas = this.$refs.earth
            const renderer = new THREE.WebGLRenderer({ canvas, antialias: true, alpha: true });
            this.renderer.push(renderer)
            renderer.setSize(100, 100);
            const camera = new THREE.PerspectiveCamera(
                45,
                window.innerWidth / window.innerHeight,
                0.1,
                1000
            );
            const orbit = new OrbitControls(camera, renderer.domElement);
            camera.position.set(25, 25, 25);
            orbit.update();
            const ambientLight = new THREE.AmbientLight(0x333333);
            scene.add(ambientLight);
            const textureLoader = new THREE.TextureLoader();
            const earthGeo = new THREE.SphereGeometry(16, 30, 30);
            const earthMat = new THREE.MeshBasicMaterial({
                map: textureLoader.load(earthTexture)
            });
            const earth = new THREE.Mesh(earthGeo, earthMat);
            scene.add(earth);
            function animate() {
                earth.rotateY(0.004);
                renderer.render(scene, camera);
            }

            renderer.setAnimationLoop(animate);
        },
        initMarsBtn() {
            const scene = new THREE.Scene();
            const canvas = this.$refs.mars
            const renderer = new THREE.WebGLRenderer({ canvas, antialias: true, alpha: true });
            this.renderer.push(renderer)
            renderer.setSize(100, 100);
            const camera = new THREE.PerspectiveCamera(
                45,
                window.innerWidth / window.innerHeight,
                0.1,
                1000
            );
            const orbit = new OrbitControls(camera, renderer.domElement);
            camera.position.set(25, 25, 25);
            orbit.update();
            const ambientLight = new THREE.AmbientLight(0x333333);
            scene.add(ambientLight);
            const textureLoader = new THREE.TextureLoader();
            const marsGeo = new THREE.SphereGeometry(16, 30, 30);
            const marsMat = new THREE.MeshBasicMaterial({
                map: textureLoader.load(marsTexture)
            });
            const mars = new THREE.Mesh(marsGeo, marsMat);
            scene.add(mars);
            function animate() {
                mars.rotateY(0.008);
                renderer.render(scene, camera);
            }

            renderer.setAnimationLoop(animate);
        },
    }
}

</script>

<style scoped>
.btn {
    background-color: rgba(224, 223, 223, 0.5);
    font-weight: 700;
    font-size: 30px;
}

.v-btn.v-btn--density-default {
    height: 100px;
    width: 100px
}

.btn[data-v-55552302] {
    background: none;
}

.SceneTitle {
    margin-bottom: 20px;
    position: relative;
    display: inline-block;
    padding: 10px 20px;
    color: #03e9f4;
    font-size: 16px;
    text-decoration: none;
    text-transform: uppercase;
    overflow: hidden;
    transition: .5s;
    margin-top: 40px;
    letter-spacing: 4px;

}

.SceneTitle span {
    position: absolute;
    display: block;
}

.SceneTitle span:nth-child(1) {
    top: 0;
    left: -100%;
    height: 2px;
    width: 100%;
    background: linear-gradient(90deg, transparent, #03e9f4);
    animation: btn-anim1 1s linear infinite;
}

@keyframes btn-anim1 {
    0% {
        left: -100%;
    }

    50%,
    100% {
        left: 100%;
    }
}

.SceneTitle span:nth-child(2) {
    top: -100%;
    right: 0;
    width: 2px;
    height: 100%;
    background: linear-gradient(180deg, transparent, #03e9f4);
    animation: btn-anim2 1s linear infinite;
    animation-delay: .25s;
}

@keyframes btn-anim2 {
    0% {
        top: -100%;
    }

    50%,
    100% {
        top: 100%;
    }
}

.SceneTitle span:nth-child(3) {
    bottom: 0;
    right: -100%;
    width: 100%;
    height: 2px;
    background: linear-gradient(270deg, transparent, #03e9f4);
    animation: btn-anim3 1s linear infinite;
    animation-delay: .5s;
}

@keyframes btn-anim3 {
    0% {
        right: -100%;
    }

    50%,
    100% {
        right: 100%;
    }
}

.SceneTitle span:nth-child(4) {
    bottom: -100%;
    left: 0;
    width: 2px;
    height: 100%;
    background: linear-gradient(360deg, transparent, #03e9f4);
    animation: btn-anim4 1s linear infinite;
    animation-delay: .75s;
}

@keyframes btn-anim4 {
    0% {
        bottom: -100%;
    }

    50%,
    100% {
        bottom: 100%;
    }
}


.v-card {
    background-color: rgba(255, 255, 255, 0);
}

.v-card--variant-elevated {
    box-shadow: none;
}

.v-dialog>.v-overlay__content>.v-card {
    box-shadow: none;
}
</style>