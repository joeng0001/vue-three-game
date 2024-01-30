const store = {
  strict: true,
  state: {
    isLogin: null,
    userID: null,
  },
  mutations: {
    setLoginStatus(state, status) {
      state.isLogin = status;
    },
    setUserID(state, status) {
      state.userID = status;
    },
  },
  actions: {
    setLoginStatus({ commit }, status) {
      commit("setLoginStatus", status);
    },
    setUserID({ commit }, status) {
      commit("setUser", status);
    },
  },
};
export default store;
