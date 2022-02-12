import Layout from "../components/Layout";
import { Button, Input, styled } from "@nextui-org/react";
import { client } from "../utils/client";
import { JWT } from "../utils/jwt";

const AboutPage = () => (
  <Layout title="About | Next.js + TypeScript Example">
    <h1>Login</h1>
    <Form
      onSubmit={(event) => {
        event.preventDefault();
        console.log("SUBMIT");

        const form = event.target;

        const data = {
          username: form.username.value,
          password: form.password.value,
        };

        client<{ token: string }>("users/authenticate", { data }).then(
          (res) => {
            const token = res.token;
            JWT.setToken(token);
          }
        );
      }}
    >
      <Input bordered labelPlaceholder="Username" name="username" />
      <Input
        bordered
        labelPlaceholder="Password"
        name="password"
        type="password"
      />
      <Button type="submit">Submit</Button>
    </Form>
  </Layout>
);

const Form = styled("form", {
  display: "flex",
  gap: "$14",
  flexDirection: "column",
  marginTop: "$14",
});

export default AboutPage;
