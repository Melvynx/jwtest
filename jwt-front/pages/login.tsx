import Layout from "../components/Layout";
import { Button, Input, styled, Switch, Text } from "@nextui-org/react";
import { client } from "../utils/client";
import { JWT } from "../utils/jwt";
import { useState } from "react";

const AboutPage = () => {
  const [isLogin, setIsLogin] = useState(true);

  return (
    <Layout title="About | Next.js + TypeScript Example">
      <h1>Login</h1>
      <Form
        onSubmit={(event) => {
          event.preventDefault();

          const form = event.target;

          const data = {
            name: isLogin ? "" : form.username.value,
            email: form.email.value,
            password: form.password.value,
          };

          const requestUrl = form.isLogin.checked ? "login" : "register";

          client<{ token?: string; result: boolean; message?: string }>(
            `auth/${requestUrl}`,
            { data }
          ).then((res) => {
            const token = res.token;
            if (res.result) {
              JWT.setToken(token);
            }
          });
        }}
      >
        <SwitchLabel>
          <Text
            css={{
              color: "white",
              textGradient: !isLogin
                ? "45deg, $blue500 -20%, $pink500 50%"
                : "45deg, $gray600 -20%, $gray600 50%",
            }}
            weight="bold"
          >
            Register
          </Text>
          <Switch
            bordered
            shadow
            color="primary"
            onChange={(e) => setIsLogin(e.target.checked)}
            checked={isLogin}
          />
          <input
            type="checkbox"
            readOnly
            style={{ display: "none" }}
            checked={isLogin}
            name="isLogin"
          />
          <Text
            css={{
              color: "white",
              textGradient: isLogin
                ? "45deg, $blue500 -20%, $pink500 50%"
                : "45deg, $gray600 -20%, $gray600 50%",
            }}
            weight="bold"
          >
            Login
          </Text>
        </SwitchLabel>

        {!isLogin && <Input bordered placeholder="Name" name="username" />}

        <Input bordered placeholder="Email" name="email" />
        <Input
          bordered
          placeholder="Password"
          name="password"
          type="password"
        />
        <Button type="submit">Submit</Button>
      </Form>
    </Layout>
  );
};

const Form = styled("form", {
  display: "flex",
  gap: "$14",
  flexDirection: "column",
  marginTop: "$14",
});

const SwitchLabel = styled("label", {
  display: "flex",
  gap: "$12",
  margin: "auto",
});

export default AboutPage;
