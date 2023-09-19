import { createWebHistory, createRouter } from "vue-router";
import Entry from "@/pages/Entry.vue";
import HelloWorld from "@/pages/HelloWorld.vue";
import GameMode from "@/components/Entry/GameMode.vue";
import GameLevel from "@/components/Entry/GameLevel.vue";
import Universe from "@/pages/gameScene/Universe";

const routes = [
  {
    path: "/",
    component: HelloWorld,
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
        path: "gameLevel",
        component: GameLevel,
      },
    ],
  },
  {
    path: "/:catchAll(.*)",
    redirect: "/login",
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
  next();
});
export default router;
