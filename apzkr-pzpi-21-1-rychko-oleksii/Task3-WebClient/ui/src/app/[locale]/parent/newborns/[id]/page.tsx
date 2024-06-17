"use client";

import React, { useEffect, useState } from "react";
import {
  Box,
  Select,
  VStack,
  Heading,
  Spinner,
  useToast,
  Button,
  Table,
  Tbody,
  Td,
  Th,
  Thead,
  Tr,
} from "@chakra-ui/react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";
import axiosInstance from "@/utils/axios-instance";
import moment from "moment";
import { SensorType } from "@/components/management/sensor-management";
import { useTranslation } from "react-i18next";
import router from "next/router";

interface MedicalData {
  timeSaved: string;
  data: number;
  sensor: number;
}

const NewbornStatistics = ({ params }: { params: { id: number } }) => {
  const [allData, setAllData] = useState<MedicalData[]>([]);
  const [filteredData, setFilteredData] = useState<MedicalData[]>([]);
  const [sensorTypes, setSensorTypes] = useState<number[]>([]);
  const [selectedSensor, setSelectedSensor] = useState("");
  const toast = useToast();

  const { t } = useTranslation();

  useEffect(() => {
    const fetchNewbornData = async () => {
      try {
        const response = await axiosInstance.get(`/Newborn/data/${params.id}`);
        const data = response.data.map((d: MedicalData) => ({
          ...d,
          timeSaved: moment(d.timeSaved).format("MMMM Do YYYY, h:mm:ss a"),
        })) as MedicalData[];

        const sensors = [...new Set(data.map((d: MedicalData) => d.sensor))];
        setSensorTypes(sensors);

        setAllData(data);
        if (sensors.length > 0) {
          setSelectedSensor(SensorType.ElectroCardiogram.toString());
          setFilteredData(
            data.filter((d: MedicalData) => d.sensor === sensors[0])
          );
        }
      } catch (error) {
        console.error("Error fetching newborn data:", error);
        toast({
          title: t("error"),
          status: "error",
          duration: 5000,
        });
      }
    };

    fetchNewbornData();
  }, [params.id, t, toast]);

  const handleSensorChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const sensor = SensorType[event.target.value as any];
    setSelectedSensor(sensor);
    setFilteredData(allData.filter((d: MedicalData) => d.sensor === +sensor));
  };

  return (
    <VStack spacing={4} align="stretch">
      <Box>
        <Heading as="h2" size="lg" mb={4}>
          {t("newborn-statistics")}
        </Heading>
        <Select onChange={handleSensorChange} mb={4}>
          {Object.keys(SensorType)
            .filter((key: string) => !isNaN(+key))
            .filter((key) => sensorTypes.includes(+key))
            .map((key: string) => (
              <option key={key} value={SensorType[+key]}>
                {SensorType[+key]}
              </option>
            ))}
        </Select>
        {filteredData.length > 0 ? (
          <ResponsiveContainer width="100%" height={400}>
            <LineChart data={filteredData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="timeSaved" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Line type="monotone" dataKey="data" stroke="#8884d8" />
            </LineChart>
          </ResponsiveContainer>
        ) : (
          <Spinner />
        )}
      </Box>
      <Table variant="simple">
        <Thead>
          <Tr>
            <Th>{t("sensor")}</Th>
            <Th>{t("data")}</Th>
            <Th>{t("date")}</Th>
          </Tr>
        </Thead>
        <Tbody>
          {allData.map((data, i) => (
            <Tr key={i}>
              <Td>{SensorType[data.sensor]}</Td>
              <Td>{data.data}</Td>
              <Td>{data.timeSaved}</Td>
            </Tr>
          ))}
        </Tbody>
      </Table>
    </VStack>
  );
};

export default NewbornStatistics;
