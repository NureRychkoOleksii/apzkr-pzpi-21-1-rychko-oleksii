"use client";

import { useState } from "react";
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  VStack,
  Heading,
  Text,
} from "@chakra-ui/react";
import { useRouter } from "next/navigation";
import axiosInstance from "@/utils/axios-instance";
import { jwtDecode } from "jwt-decode";
import { useTranslation } from "react-i18next";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const router = useRouter();
  const { t } = useTranslation();

  const handleLogin = async () => {
    try {
      const response = await axiosInstance.post("/User/login", {
        username,
        password,
      });

      if (response.status === 200) {
        localStorage.setItem("token", response.data);
        const decodedToken = jwtDecode(response.data) as any;
        localStorage.setItem(
          "id",
          decodedToken[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
          ]
        );
        window.dispatchEvent(new Event("storage"));
        router.push("/");
      } else {
        console.error("Login failed");
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  return (
    <Box p={6}>
      <VStack spacing={4} align="flex-start">
        <Heading>{t("login")}</Heading>
        <Text>{t("welcome-login")}</Text>
        <FormControl id="username" isRequired>
          <FormLabel>{t("username")}</FormLabel>
          <Input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </FormControl>
        <FormControl id="password" isRequired>
          <FormLabel>Password</FormLabel>
          <Input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </FormControl>
        <Button colorScheme="teal" onClick={handleLogin}>
          {t("login")}
        </Button>
      </VStack>
    </Box>
  );
};

export default Login;
