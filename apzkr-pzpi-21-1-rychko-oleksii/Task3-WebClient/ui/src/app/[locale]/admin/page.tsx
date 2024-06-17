"use client";

import React from "react";
import {
  Box,
  Heading,
  VStack,
  Link as ChakraLink,
  Button,
} from "@chakra-ui/react";
import { useAdminAuth } from "@/hooks/use-admin-auth";
import UserManagement from "@/components/management/user-management";
import SensorManagement from "@/components/management/sensor-management";
import SensorSettingsManagement from "@/components/management/sensor-settings-management";
import Link from "next/link";
import { useTranslation } from "react-i18next";

const AdminPanel = () => {
  useAdminAuth();

  const { t } = useTranslation();

  return (
    <Box p={8}>
      <VStack spacing={4} align="stretch">
        <Heading as="h1" size="xl" textAlign="center">
          {t("admin-panel")}
        </Heading>
        <ChakraLink as={Link} href="/admin/backup">
          <Button colorScheme="blue" mb={4}>
            {t("backup-database")}
          </Button>
        </ChakraLink>
        <Heading as="h1" size="xl" textAlign="center">
          {t("users")}
        </Heading>
        <UserManagement />
        <Heading as="h1" size="xl" textAlign="center">
          {t("sensors")}
        </Heading>
        <SensorManagement />
        <SensorSettingsManagement />
      </VStack>
    </Box>
  );
};

export default AdminPanel;
