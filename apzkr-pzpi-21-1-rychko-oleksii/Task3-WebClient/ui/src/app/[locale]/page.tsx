"use client";

import { VStack, Heading, Button, Box, Text } from "@chakra-ui/react";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";

function Home() {
  const router = useRouter();
  const [token, setToken] = useState("");

  const { t } = useTranslation();

  useEffect(() => {
    setToken(localStorage.getItem("token") ?? "");

    const handleStorageChange = () => {
      const newToken = localStorage.getItem("token") ?? "";
      setToken(newToken);
    };

    window.addEventListener("storage", handleStorageChange);

    return () => window.removeEventListener("storage", handleStorageChange);
  }, []);

  const handleGetStarted = () => {
    router.push("/login");
  };

  return (
    <Box p={6} textAlign="center">
      <VStack spacing={6}>
        <Heading size="2xl">{t("welcome-main")}</Heading>
        {!token ? (
          <>
            <Text fontSize="xl">{t("partners")}</Text>
            <Button colorScheme="teal" size="lg" onClick={handleGetStarted}>
              {t("get-started")}
            </Button>
          </>
        ) : (
          <>
            <Text fontSize="xl">{t("see-your-info")}</Text>
            <Button
              colorScheme="teal"
              size="lg"
              onClick={() => router.push("/parent")}
            >
              {t("get-started")}
            </Button>
          </>
        )}
      </VStack>
    </Box>
  );
}

export default Home;
