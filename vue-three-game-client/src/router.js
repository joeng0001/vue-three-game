import { createWebHistory, createRouter } from "vue-router";
import Entry from "@/pages/Entry.vue";
import HelloWorld from "@/pages/HelloWorld.vue";
import GameMode from "@/components/Entry/GameMode.vue";
import GameLevel from "@/components/Entry/GameLevel.vue";
import Universe from "@/pages/gameScene/Universe";
import Earth from "@/pages/gameScene/Earth";
import Mars from "@/pages/gameScene/Mars";
const routes = [
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
  next();
});
export default router;
