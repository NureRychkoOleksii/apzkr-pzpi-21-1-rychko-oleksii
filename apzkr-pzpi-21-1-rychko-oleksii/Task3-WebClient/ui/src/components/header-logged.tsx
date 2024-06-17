"use client";

import { Box, Flex, Text, Spacer, Heading, Button } from "@chakra-ui/react";
import NextLink from "next/link";
import { useRouter } from "next/navigation";
import { LanguagePicker } from "./language-picker";
import { useTranslation } from "react-i18next";

const HeaderLogged = () => {
  const router = useRouter();
  const logout = () => localStorage.removeItem("token");

  const handleLogout = () => {
    logout();
    window.dispatchEvent(new Event("storage"));
    router.push("/login");
  };

  const { t } = useTranslation();

  return (
    <Box bg="teal.500" p={15}>
      <Flex alignItems="center">
        <NextLink href="/" passHref>
          <Heading size="md" color="white">
            StarOfLife
          </Heading>
        </NextLink>
        <Spacer />
        <Button onClick={handleLogout} colorScheme="red" variant="solid" mr={4}>
          {t("logout")}
        </Button>
        <LanguagePicker />
      </Flex>
    </Box>
  );
};

export default HeaderLogged;
