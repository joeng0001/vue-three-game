import store from "@/store.js";

const endPt = "https://localhost:7155";

const fetchFnc = (path, method, data) => {
  const config = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  if (store.state.isLogin) {
    config.credentials = "include";
  }
  if (method) {
    config.method = method;
  }
  if (data) {
    config.body = JSON.stringify(data);
  }

  return fetch(endPt + path, config);
};

export default {
  test() {
    return fetchFnc("/api/Users");
  },

  register(data) {
    return fetchFnc("/api/Users/register", "POST", data);
  },
  login(data) {
    return fetchFnc("/api/Users/login", "POST", data);
  },
};
