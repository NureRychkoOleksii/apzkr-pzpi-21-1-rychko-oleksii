"use client";

import React, { useEffect, useState } from "react";
import { Box, Button, Input, Select, useToast } from "@chakra-ui/react";
import axiosInstance from "@/utils/axios-instance";
import { useRouter } from "next/navigation";
import { Sensor, SensorType } from "@/components/management/sensor-management";
import { useTranslation } from "react-i18next";

const EditSensor = ({ params }: { params: { id: number } }) => {
  const [editableSensor, setEditableSensor] = useState<
    Sensor & { type: number }
  >({
    id: 0,
    type: SensorType.ElectroCardiogram,
    newbornId: 0,
    sensorSettingsId: 0,
  });
  const router = useRouter();
  const toast = useToast();

  const { t } = useTranslation();

  useEffect(() => {
    const fetchSensor = async () => {
      try {
        const response = await axiosInstance.get(`/Sensor/${params.id}`);
        setEditableSensor({
          ...response.data,
          type: response.data.sensorType,
        });
      } catch (error) {
        console.error("Error fetching sensor:", error);
        toast({
          title: t("error"),
          status: "error",
          duration: 5000,
        });
      }
    };

    fetchSensor();
  }, [params.id, t, toast]);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setEditableSensor({ ...editableSensor, [name]: value });
  };

  const handleSaveSensor = async () => {
    try {
      await axiosInstance.put(`/Sensor/${params.id}`, {
        newbornId: editableSensor.newbornId,
        sensorType: editableSensor.type,
        sensorSettingsId: editableSensor.sensorSettingsId,
      });
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

  if (!editableSensor) {
    return <div>{t("loading")}</div>;
  }

  return (
    <Box>
      <Input
        placeholder={t("newborn-id")}
        value={editableSensor.newbornId}
        onChange={handleInputChange}
        name="newbornId"
        mb={2}
      />
      <Input
        placeholder={t("sensor-settings-id")}
        value={editableSensor.sensorSettingsId}
        onChange={handleInputChange}
        name="sensorSettingsId"
        mb={2}
      />
      <Button colorScheme="blue" onClick={handleSaveSensor}>
        {t("save-sensor")}
      </Button>
      <Button colorScheme="gray" onClick={handleCancelEdit} ml={2}>
        {t("cancel")}
      </Button>
    </Box>
  );
};

export default EditSensor;
