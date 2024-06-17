"use client";

import { Box, Heading, VStack } from "@chakra-ui/react";
import BackupManagement from "./backup-management";
import { useTranslation } from "react-i18next";

const BackupPage = () => {
  const { t } = useTranslation();
  return (
    <VStack spacing={4} align="stretch">
      <Box>
        <Heading as="h1" size="lg" mb={4}>
          {t("backup-management")}
        </Heading>
        <BackupManagement />
      </Box>
    </VStack>
  );
};

export default BackupPage;
