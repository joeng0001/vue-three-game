const endPt = "https://localhost:7155";

export default {
  test() {
    return fetch(endPt + "/api/Users");
  },

  register(data) {
    return fetch(endPt + "/api/Users/register", {
      method: "POST",
      body: data,
    });
  },
};
