const store = {
  strict: true,
  state: {
    isLogin: null,
  },
  mutations: {
    setLoginStatus(state, status) {
      state.isLogin = status;
    },
  },
  actions: {
    setLoginStatus({ commit }, status) {
      commit("setLoginStatus", status);
    },
  },
};
export default store;
