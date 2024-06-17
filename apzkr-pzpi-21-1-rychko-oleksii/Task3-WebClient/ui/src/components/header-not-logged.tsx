import { Box, Flex, Text, Spacer, Heading } from "@chakra-ui/react";
import NextLink from "next/link";
import { LanguagePicker } from "./language-picker";
import { useTranslation } from "react-i18next";

const HeaderNotLogged = () => {
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
        <NextLink href="/signup" passHref>
          <Text mr={4} color="white" fontWeight="bold">
            {t("sign-up")}
          </Text>
        </NextLink>
        <NextLink href="/login" passHref>
          <Text mr={4} color="white" fontWeight="bold">
            {t("log-in")}
          </Text>
        </NextLink>
        <LanguagePicker />
      </Flex>
    </Box>
  );
};

export default HeaderNotLogged;
