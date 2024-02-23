import store from "@/store.js";

const endPt = "http://localhost:5136";

const fetchFnc = (path, method, data) => {
  const config = {
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "include",
  };

  if (store.state.isLogin) {
    config.headers.Authorization = `bearer ${getCookie("token")}`;
  }
  if (method) {
    config.method = method;
  }
  if (data) {
    config.body = JSON.stringify(data);
  }

  return fetch(endPt + path, config).then(async (res) => {
    if (!res.ok) {
      throw new Error(await res.text());
    }
    return res.text();
  });
};

function getCookie(cookieName) {
  var name = cookieName + "=";
  var decodedCookie = decodeURIComponent(document.cookie);
  var cookieArray = decodedCookie.split(";");
  for (var i = 0; i < cookieArray.length; i++) {
    var cookie = cookieArray[i];
    while (cookie.charAt(0) == " ") {
      cookie = cookie.substring(1);
    }
    if (cookie.indexOf(name) == 0) {
      return cookie.substring(name.length, cookie.length);
    }
  }
  return "";
}

export default {
  test() {
    return fetchFnc("/WeatherForecast");
  },
  register(data) {
    return fetchFnc("/api/Users/register", "POST", data);
  },
  login(data) {
    return fetchFnc("/api/Users/login", "POST", data);
  },
  logout() {
    return fetchFnc("/api/Users/logout", "POST", {});
  },
  getMarsConfig(lv) {
    return fetchFnc(`/api/Config/Mars/${lv}`);
  },
  getUniverseConfig(lv) {
    return fetchFnc(`/api/Config/Universe/${lv}`);
  },
  addSpaceShipProfile(userId, data) {
    return fetchFnc("/api/Config/addSpaceShipProfile/" + userId, "POST", data);
  },
  addMarsRoverProfile(userId, data) {
    return fetchFnc("/api/Config/addMarsRoverProfile/" + userId, "POST", data);
  },
  updateSpaceShipProfile(userId, profileId, data) {
    return fetchFnc(
      `/api/Config/updateSpaceShipProfile/${userId}/${profileId}`,
      "PUT",
      data
    );
  },
  updateMarsRoverProfile(userId, profileId, data) {
    return fetchFnc(
      `/api/Config/updateMarsRoverProfile/${userId}/${profileId}`,
      "PUT",
      data
    );
  },
  getSpaceShipProfile(userId) {
    return fetchFnc("/api/Config/getSpaceShipProfile/" + userId);
  },
  getMarsRoverProfile(userId) {
    return fetchFnc("/api/Config/getMarsRoverProfile/" + userId);
  },
};
