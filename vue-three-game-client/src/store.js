const store = {
  strict: true,
  state: {
    isLogin: null,
    userID: null,
    spaceShipProfile: [],
    marsRoverProfile: [],
  },
  mutations: {
    setLoginStatus(state, status) {
      state.isLogin = status;
    },
    setUserID(state, status) {
      state.userID = status;
    },
    setSpaceShipProfile(state, status) {
      state.spaceShipProfile = status;
    },
    setMarsRoverProfile(state, status) {
      state.marsRoverProfile = status;
    },
  },
  actions: {
    setLoginStatus({ commit }, status) {
      commit("setLoginStatus", status);
    },
    setUserID({ commit }, status) {
      commit("setUserID", status);
    },
    setSpaceShipProfile({ commit }, status) {
      commit("setSpaceShipProfile", status);
    },
    setMarsRoverProfile({ commit }, status) {
      commit("setMarsRoverProfile", status);
    },
  },
};
export default store;
