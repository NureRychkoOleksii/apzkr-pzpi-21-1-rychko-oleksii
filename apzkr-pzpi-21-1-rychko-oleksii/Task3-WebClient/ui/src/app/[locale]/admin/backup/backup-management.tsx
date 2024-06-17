"use client";

import axiosInstance from "@/utils/axios-instance";
import { Box, Button, useToast } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";

const BackupManagement = () => {
  const toast = useToast();
  const { t } = useTranslation();

  const handleBackup = async () => {
    try {
      await axiosInstance.post("/backup/save");

      toast({
        title: t("success"),
        status: "success",
        duration: 5000,
      });
    } catch (error) {
      console.error("Error saving backup:", error);
      toast({
        title: t("error"),
        status: "error",
        duration: 5000,
      });
    }
  };

  const handleDownload = async () => {
    try {
      const response = await axiosInstance.post("/backup/download", null, {
        responseType: "blob",
      });

      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", "database_backup.zip");
      document.body.appendChild(link);
      link.click();
      link.remove();

      toast({
        title: t("success"),
        status: "success",
        duration: 5000,
      });
    } catch (error) {
      console.error("Error downloading backup:", error);
      toast({
        title: t("error"),
        status: "error",
        duration: 5000,
      });
    }
  };

  return (
    <Box
      style={{
        display: "flex",
        justifyContent: "center",
        flexDirection: "column",
        gap: "10px",
      }}
    >
      <Button colorScheme="blue" onClick={handleBackup}>
        {t("save-backup")}
      </Button>
      <Button colorScheme="blue" onClick={handleDownload}>
        {t("download-backup")}
      </Button>
    </Box>
  );
};

export default BackupManagement;
