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
import { useTranslation } from "react-i18next";

const Page = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const router = useRouter();

  const { t } = useTranslation();

  const handleSignUp = async () => {
    try {
      console.log({ email, password, name });
      const response = await axiosInstance.post("/User/signup", {
        email,
        password,
        username: name,
      });

      if (response.status === 200) {
        router.push("/login");
      } else {
        console.error("Sign up failed");
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  return (
    <Box p={6}>
      <VStack spacing={4} align="flex-start">
        <Heading>Sign Up</Heading>
        <Text>{t("welcome")}</Text>
        <FormControl id="name" isRequired>
          <FormLabel>{t("name")}</FormLabel>
          <Input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
        </FormControl>
        <FormControl id="email" isRequired>
          <FormLabel>{t("email-address")}</FormLabel>
          <Input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </FormControl>
        <FormControl id="password" isRequired>
          <FormLabel>{t("password")}</FormLabel>
          <Input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </FormControl>
        <Button colorScheme="teal" onClick={handleSignUp}>
          {t("sign-up")}
        </Button>
      </VStack>
    </Box>
  );
};

export default Page;
