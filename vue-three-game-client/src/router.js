import { createWebHistory, createRouter } from "vue-router";
import Entry from "@/pages/Entry.vue";
import store from "@/store";

import GameMode from "@/components/Entry/GameMode.vue";
import GameLevel from "@/components/Entry/GameLevel.vue";
import Universe from "@/pages/gameScene/Universe.vue";
import Earth from "@/pages/gameScene/Earth.vue";
import Mars from "@/pages/gameScene/Mars.vue";

import Login from "@/pages/Login.vue";

const routes = [
  {
    name: "login",
    path: "/login",
    component: Login,
  },
  {
    path: "/entry",
    component: Entry,
    children: [
      {
        name: "entry.gameMode",
        path: "gameMode",
        component: GameMode,
      },
      {
        name: "entry.gameLevel",
        path: "gameLevel",
        component: GameLevel,
      },
    ],
  },
  {
    path: "/gameScene",
    children: [
      {
        name: "scene.Universe",
        path: "Universe",
        component: Universe,
      },
      {
        name: "scene.Earth",
        path: "Earth",
        component: Earth,
      },
      {
        name: "scene.Mars",
        path: "Mars",
        component: Mars,
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
