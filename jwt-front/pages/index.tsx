import Link from "next/link";
import Layout from "../components/Layout";
import { useState } from "react";
import { Button } from "@nextui-org/react";
import { client } from "../utils/client";

const IndexPage = () => {
  const [user, setUser] = useState<string>("");

  const getUser = (isPayload: boolean) => {
    client<unknown>(`auth/me?isPayload=${isPayload}`)
      .then((res) => {
        setUser(
          JSON.stringify(res)
            .replaceAll(",", ",\n")
            .replace("{", "{\n")
            .replace("}", "\n}")
        );
      })
      .catch(() => {
        setUser("Unauthorized");
      });
  };

  return (
    <Layout title="Home | Next.js + TypeScript Example">
      <h1>Hello JWT</h1>
      <p>
        If you are not connected, please go on{" "}
        <Link href="/login">
          <a>Login</a>
        </Link>
      </p>
      <div style={{ display: "flex", alignItems: "center" }}>
        <p>Payload</p>
        <pre>{user}</pre>
      </div>
      <div>
        <Button onClick={() => getUser(true)}>Get payload</Button>
        <Button onClick={() => getUser(false)}>Get user</Button>
      </div>
    </Layout>
  );
};

export default IndexPage;
