const store = {
  strict: true,
  state: {
    token: null,
  },
  mutations: {
    setToken(state, token) {
      state.token = token;
      state.loginStatus = !!token;
    },
  },
  actions: {
    setToken({ commit }, token) {
      commit("setToken", token);
    },
  },
};
export default store;
