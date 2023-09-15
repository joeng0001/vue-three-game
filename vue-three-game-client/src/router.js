import { createWebHistory, createRouter } from "vue-router";
import LogIn from "@/pages/LogIn.vue";
import HelloWorld from "@/pages/HelloWorld.vue";
const routes = [
  {
    path: "/",
    component: HelloWorld,
  },
  {
    path: "/login",
    component: LogIn,
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
