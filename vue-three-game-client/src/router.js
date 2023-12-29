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
    path: "/login",
    component: Login,
  },
  {
    path: "/entry",
    component: Entry,
    children: [
      {
        path: "gameMode",
        component: GameMode,
      },
      {
        path: "gameLevel",
        component: GameLevel,
      },
    ],
  },
  {
    path: "/gameScene",
    children: [
      {
        path: "Universe",
        component: Universe,
      },
      {
        path: "Earth",
        component: Earth,
      },
      {
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
  // routing middleware
  //   const banned_list = ["/user", "/room_list"];

  //   if (to.path === "/" || to.path === "/registration") {
  //     if (vuex_store.state.loginStatus) {
  //       router.push("/home");
  //       return;
  //     }
  //   }
  if (to.path === "/login") {
    next();
    return;
  }
  if (!store.state.token) {
    next({ path: "/login" });
    return;
  }
  next();
});
export default router;
