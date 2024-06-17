"use client";

import React, { useEffect, useState } from "react";
import {
  Box,
  Button,
  Table,
  Tbody,
  Td,
  Th,
  Thead,
  Tr,
  VStack,
  Input,
  useToast,
  Checkbox,
} from "@chakra-ui/react";
import axiosInstance from "@/utils/axios-instance";
import { useRouter } from "next/navigation";
import { useTranslation } from "react-i18next";

export interface SensorSettings {
  id: number;
  highCriticalThreshold: number;
  lowCriticalThreshold: number;
  highEdgeThreshold: number;
  lowEdgeThreshold: number;
  samplingFrequency: number;
  isActive: boolean;
}

const SensorSettingsManagement = () => {
  const [sensorSettings, setSensorSettings] = useState<SensorSettings[]>([]);
  const [newSensorSettings, setNewSensorSettings] = useState<
    Omit<SensorSettings, "id">
  >({
    highCriticalThreshold: 0,
    lowCriticalThreshold: 0,
    highEdgeThreshold: 0,
    lowEdgeThreshold: 0,
    samplingFrequency: 0,
    isActive: false,
  });

  const { t } = useTranslation();

  const router = useRouter();
  const toast = useToast();

  useEffect(() => {
    const fetchSensorSettings = async () => {
      try {
        const response = await axiosInstance.get("/SensorSettings");
        setSensorSettings(response.data);
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

    fetchSensorSettings();
  }, [t, toast]);

  const handleDelete = async (id: number) => {
    try {
      await axiosInstance.delete(`/SensorSettings/${id}`);
      setSensorSettings(sensorSettings.filter((sensor) => sensor.id !== id));
    } catch (error) {
      console.error("Error deleting sensor settings:", error);
    }
  };

  const handleAddSettings = async () => {
    try {
      await axiosInstance.post("/SensorSettings", newSensorSettings);
      setNewSensorSettings({
        highCriticalThreshold: 0,
        lowCriticalThreshold: 0,
        highEdgeThreshold: 0,
        lowEdgeThreshold: 0,
        samplingFrequency: 0,
        isActive: false,
      });
    } catch (error) {
      console.error("Error adding sensor settings:", error);
    }
  };

  return (
    <VStack spacing={4} align="stretch">
      <Box>
        <Input
          placeholder={t("high-critical-threshold")}
          value={newSensorSettings.highCriticalThreshold}
          onChange={(e) =>
            setNewSensorSettings({
              ...newSensorSettings,
              highCriticalThreshold: e.target.value as any,
            })
          }
          mb={2}
        />
        <Input
          placeholder={t("high-edge-threshold")}
          value={newSensorSettings.highEdgeThreshold}
          onChange={(e) =>
            setNewSensorSettings({
              ...newSensorSettings,
              highEdgeThreshold: e.target.value as any,
            })
          }
          mb={2}
        />
        <Input
          placeholder={t("low-critical-threshold")}
          value={newSensorSettings.lowCriticalThreshold}
          onChange={(e) =>
            setNewSensorSettings({
              ...newSensorSettings,
              lowCriticalThreshold: e.target.value as any,
            })
          }
          mb={2}
        />
        <Input
          placeholder={t("low-edge-threshold")}
          value={newSensorSettings.lowEdgeThreshold}
          onChange={(e) =>
            setNewSensorSettings({
              ...newSensorSettings,
              lowEdgeThreshold: e.target.value as any,
            })
          }
          mb={2}
        />
        <Input
          placeholder={t("sampling-frequency")}
          value={newSensorSettings.samplingFrequency}
          onChange={(e) =>
            setNewSensorSettings({
              ...newSensorSettings,
              samplingFrequency: e.target.value as any,
            })
          }
          mb={2}
        />
        <Checkbox
          size="md"
          colorScheme="green"
          isChecked={newSensorSettings.isActive}
          onChange={() =>
            setNewSensorSettings({
              ...newSensorSettings,
              isActive: !newSensorSettings.isActive,
            })
          }
        >
          {t("is-active")}
        </Checkbox>
        <br />
        <br />
        <Button colorScheme="blue" mb={4} onClick={handleAddSettings}>
          {t("add-new-sensor-settings")}
        </Button>
        <Table variant="simple">
          <Thead>
            <Tr>
              <Th>ID</Th>
              <Th>{t("high-critical-threshold")}</Th>
              <Th>{t("high-edge-threshold")}</Th>
              <Th>{t("low-critical-threshold")}</Th>
              <Th>{t("low-edge-threshold")}</Th>
              <Th>{t("sampling-frequency")}</Th>
              <Th>{t("is-active")}</Th>
              <Th>{t("actions")}</Th>
            </Tr>
          </Thead>
          <Tbody>
            {sensorSettings.map((setting) => (
              <Tr key={setting.id}>
                <Td>{setting.id}</Td>
                <Td>{setting.highCriticalThreshold}</Td>
                <Td>{setting.highEdgeThreshold}</Td>
                <Td>{setting.lowCriticalThreshold}</Td>
                <Td>{setting.lowEdgeThreshold}</Td>
                <Td>{setting.samplingFrequency}</Td>
                <Td>
                  <Checkbox
                    size="md"
                    colorScheme="green"
                    isChecked={setting.isActive}
                    disabled
                  />
                </Td>
                <Td
                  style={{
                    display: "flex",
                    gap: "5px",
                  }}
                >
                  {/* TODO: getting newborns for parents + statistics */}
                  <Button
                    colorScheme="green"
                    onClick={() =>
                      router.push(`/admin/sensor-settings/${setting.id}`)
                    }
                  >
                    {t("edit-sensor-settings")}
                  </Button>
                  <Button
                    colorScheme="red"
                    onClick={() => handleDelete(setting.id)}
                  >
                    {t("delete")}
                  </Button>
                </Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </Box>
    </VStack>
  );
};

export default SensorSettingsManagement;
