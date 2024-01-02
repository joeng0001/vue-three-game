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
