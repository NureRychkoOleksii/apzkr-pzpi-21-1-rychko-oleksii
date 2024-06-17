"use client";

import React, { useEffect, useState } from "react";
import {
  Box,
  Button,
  Checkbox,
  Input,
  Select,
  useToast,
} from "@chakra-ui/react";
import axiosInstance from "@/utils/axios-instance";
import { useRouter } from "next/navigation";
import { SensorSettings } from "@/components/management/sensor-settings-management";
import { useTranslation } from "react-i18next";

const EditSensorSettings = ({ params }: { params: { id: number } }) => {
  const [editableSensorSettings, setEditableSensorSettings] = useState<
    Omit<SensorSettings, "id">
  >({
    highCriticalThreshold: 0,
    lowCriticalThreshold: 0,
    highEdgeThreshold: 0,
    lowEdgeThreshold: 0,
    samplingFrequency: 0,
    isActive: false,
  });
  const router = useRouter();
  const toast = useToast();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchSensor = async () => {
      try {
        const response = await axiosInstance.get(
          `/SensorSettings/${params.id}`
        );
        setEditableSensorSettings({
          ...response.data,
          isActive: response.data.isActive === true,
        });
        console.log(response.data);
      } catch (error) {
        console.error("Error fetching sensor settings:", error);
        toast({
          title: t("error"),
          status: "error",
          duration: 5000,
        });
      }
    };

    fetchSensor();
  }, [params.id, t, toast]);

  const handleSaveSensorSettings = async () => {
    try {
      await axiosInstance.put(
        `/SensorSettings/${params.id}`,
        editableSensorSettings
      );
      toast({
        title: t("success"),
        status: "success",
        duration: 5000,
      });
      router.push("/admin");
    } catch (error) {
      console.error("Error updating sensor:", error);
      toast({
        title: t("error"),
        status: "error",
        duration: 5000,
      });
    }
  };

  const handleCancelEdit = () => {
    router.push("/admin");
  };

  if (!editableSensorSettings) {
    return <div>{t("loading")}</div>;
  }

  return (
    <Box>
      <Input
        placeholder={t("high-critical-threshold")}
        value={editableSensorSettings.highCriticalThreshold}
        onChange={(e) =>
          setEditableSensorSettings({
            ...editableSensorSettings,
            highCriticalThreshold: e.target.value as any,
          })
        }
        mb={2}
      />
      <Input
        placeholder={t("high-edge-threshold")}
        value={editableSensorSettings.highEdgeThreshold}
        onChange={(e) =>
          setEditableSensorSettings({
            ...editableSensorSettings,
            highEdgeThreshold: e.target.value as any,
          })
        }
        mb={2}
      />
      <Input
        placeholder={t("low-critical-threshold")}
        value={editableSensorSettings.lowCriticalThreshold}
        onChange={(e) =>
          setEditableSensorSettings({
            ...editableSensorSettings,
            lowCriticalThreshold: e.target.value as any,
          })
        }
        mb={2}
      />
      <Input
        placeholder={t("low-edge-threshold")}
        value={editableSensorSettings.lowEdgeThreshold}
        onChange={(e) =>
          setEditableSensorSettings({
            ...editableSensorSettings,
            lowEdgeThreshold: e.target.value as any,
          })
        }
        mb={2}
      />
      <Input
        placeholder={t("sampling-frequency")}
        value={editableSensorSettings.samplingFrequency}
        onChange={(e) =>
          setEditableSensorSettings({
            ...editableSensorSettings,
            samplingFrequency: e.target.value as any,
          })
        }
        mb={2}
      />
      <Checkbox
        size="md"
        colorScheme="green"
        isChecked={editableSensorSettings.isActive}
        onChange={() =>
          setEditableSensorSettings({
            ...editableSensorSettings,
            isActive: !editableSensorSettings.isActive,
          })
        }
      >
        {t("is-active")}
      </Checkbox>
      <br />
      <br />
      <Button colorScheme="blue" onClick={handleSaveSensorSettings}>
        {t("save-sensor-settings")}
      </Button>
      <Button colorScheme="gray" onClick={handleCancelEdit} ml={2}>
        {t("cancel")}
      </Button>
    </Box>
  );
};

export default EditSensorSettings;
