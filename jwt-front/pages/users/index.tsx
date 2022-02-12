import Link from "next/link";
import { useEffect, useState } from "react";

import Layout from "../../components/Layout";
import List from "../../components/List";
import { client } from "../../utils/client";
import { User } from "../../interfaces";

const WithStaticProps = () => {
  const [users, setUsers] = useState<User[]>([]);

  useEffect(() => {
    const abortController = new AbortController();
    const signal = abortController.signal;
    client<User[]>("users", { signal })
      .then((data) => {
        setUsers(data);
      })
      .catch(() => {
        setUsers([]);
      });

    return () => {
      abortController.abort();
    };
  }, []);

  return (
    <Layout title="Users List | Next.js + TypeScript Example">
      <h1>Users List</h1>
      <p>
        Example fetching data from inside <code>getStaticProps()</code>.
      </p>
      <p>You are currently on: /users</p>
      <List items={users} />
      <p>
        <Link href="/">
          <a>Go home</a>
        </Link>
      </p>
    </Layout>
  );
};

export default WithStaticProps;
