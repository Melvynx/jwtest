const LOCAL_STORAGE_TOKEN_KEY = "jwt_token";

const setToken = (token: string) => {
  localStorage.setItem(LOCAL_STORAGE_TOKEN_KEY, token);
};

const getToken = () => {
  return localStorage.getItem(LOCAL_STORAGE_TOKEN_KEY);
};

const JWT = {
  setToken,
  getToken,
};

export { JWT };
