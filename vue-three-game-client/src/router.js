import { createWebHistory, createRouter } from "vue-router";
import store from "@/store";

const routes = [
  {
    name: "login",
    path: "/login",
    component: () => import("@/pages/Login.vue"),
  },
  {
    path: "/entry",
    component: () => import("@/pages/Entry/Entry.vue"),
    children: [
      {
        name: "entry.gameMode",
        path: "gameMode",
        component: () => import("@/pages/Entry/GameMode.vue"),
      },
      {
        name: "entry.gameLevel",
        path: "gameLevel",
        component: () => import("@/pages/Entry/GameLevel.vue"),
      },
      {
        name: "entry.gameVehicle",
        path: "gameVehicle",
        component: () => import("@/pages/Entry/GameVehicle.vue"),
        props: true,
      },
    ],
  },
  {
    path: "/gameScene",
    component: () => import("@/pages/gameScene/GameScene.vue"),
    children: [
      {
        name: "scene.Universe",
        path: "Universe",
        component: () => import("@/pages/gameScene/Universe.vue"),
      },
      {
        name: "scene.Earth",
        path: "Earth",
        component: () => import("@/pages/gameScene/Earth.vue"),
      },
      {
        name: "scene.Mars",
        path: "Mars",
        component: () => import("@/pages/gameScene/Mars.vue"),
      },
    ],
  },

  {
    path: "/:catchAll(.*)",
    redirect: "/entry/gameMode",
  },
];
const router = createRouter({
  history: createWebHistory(),
  routes,
});
router.beforeEach((to, from, next) => {
  if (to.path === "/login") {
    next();
    return;
  }
  if (!store.state.isLogin) {
    next({ path: "/login" });
    return;
  }
  next();
});
export default router;
