import { JWT } from "./jwt";

type Config = {
  data?: unknown;
  token?: string | null;
  method?: "GET" | "POST" | "PUT" | "DELETE" | "OPTIONS" | "PATCH";
  headers?: HeadersInit;
  customConfig?: RequestInit;
  json?: boolean;
  api?: APIType;
  type?: "json" | "text";
  signal?: AbortSignal;
};

type APIType = "JWT" | "API";

const APIMapping = {
  JWT: process.env.JWT_API_URL,
  API: process.env.APP_API_URL,
};
console.log(APIMapping);

/*
  To call the api.

  To `get` an item
  ```ts
  client("item")
  ```
  Get is the default method if no data provided.

  If you want `POST` an item, you need to provide data.
  If Data was provided, default method is `POST`
  ```ts
  client("item", {data: {name: "John"}})
  ```

  To `patch` / `delete` an item
  ```ts
  client("item/1", {method: "PATCH"})
  ```

  To provide a Token :
  ```ts
  client("item/1", {method: "PATCH", token: "SomeToken"})
  ```

  TODO: You can see example in `client.test.tsx`
*/

type ClientType = <T>(endpoint: string, config?: Config) => Promise<T>;

const client: ClientType = async <T>(
  endpoint: string,
  {
    data = undefined,
    method = undefined,
    token = JWT.getToken(),
    headers: customHeaders = undefined,
    customConfig = undefined,
    api = "API",
    type = "json",
    signal = undefined,
  } = {}
) => {
  const config: RequestInit = {
    signal,
    method: method || (data ? "POST" : "GET"),
    body: data ? JSON.stringify(data) : null,
    headers: {
      Authorization: token ? `Bearer ${token}` : "",
      "Content-Type": data ? "application/json" : "",
      Accept: "application/json",
      ...customHeaders,
    },
    ...customConfig,
  };
  console.log(config);

  return window
    .fetch(`${APIMapping[api]}/${endpoint}`, config)
    .then(async (response) => {
      // statusCode "401" is for unauthenticated request
      if (response.status === 401) {
        return Promise.reject({ message: "Please re-authenticate." });
      }

      let data;

      if (type === "json") {
        data = await response.json();
      } else {
        data = await response.text();
      }

      if (response.ok) {
        return Promise.resolve(data);
      } else {
        return Promise.reject(data);
      }
    });
};

export { client };
