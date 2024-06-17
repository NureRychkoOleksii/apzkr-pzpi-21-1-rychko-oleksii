"use client";

import React, { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import {
  Box,
  Button,
  Heading,
  Text,
  VStack,
  Table,
  Tbody,
  Tr,
  Th,
  Td,
  useToast,
} from "@chakra-ui/react";
import axiosInstance from "@/utils/axios-instance";
import moment from "moment";
import { useTranslation } from "react-i18next";

export interface Parent {
  id: number;
  name: string;
  contractInfo: string;
}

interface Newborn {
  id: number;
  name: string;
  dateOfBirth: string;
}

const ParentProfile: React.FC = () => {
  const [parent, setParent] = useState<Parent | null>(null);
  const [newborns, setNewborns] = useState<Newborn[]>([]);
  const router = useRouter();
  const toast = useToast();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchParentData = async () => {
      if (parent === null) {
        try {
          const response = await axiosInstance.get(
            `/parent-user/${localStorage.getItem("id")}`
          );
          setParent(response.data);
          console.log(response.data);
        } catch (error) {
          console.error("Error fetching parent data:", error);
          toast({
            title: t("error"),
            status: "error",
            duration: 5000,
          });
        }
      }
    };

    const fetchNewborns = async () => {
      if (!parent) return;
      try {
        const response = await axiosInstance.get(
          `/Parent/newborns/${parent.id}`
        );
        setNewborns(response.data);
      } catch (error) {
        console.error("Error fetching newborns:", error);
        toast({
          title: t("error"),
          status: "error",
          duration: 5000,
        });
      }
    };

    fetchParentData();
    fetchNewborns();
  }, [parent, t, toast]);

  const handleEdit = () => {
    router.push(`/parent/edit/${parent?.id}`);
  };

  return (
    <VStack spacing={4} align="stretch">
      {parent && (
        <Box>
          <Heading as="h2" size="lg">
            {t("parent-profile")}
          </Heading>
          <Text>
            {t("name")}: {parent.name}
          </Text>
          <Text>
            {t("contract-info")}: {parent.contractInfo}
          </Text>
          <Button colorScheme="blue" onClick={handleEdit} mt={4}>
            {t("edit-profile")}
          </Button>
        </Box>
      )}

      <Box>
        <Heading as="h3" size="md" mt={8}>
          {t("newborns")}
        </Heading>
        {newborns.length > 0 ? (
          <Table variant="simple" mt={4}>
            <thead>
              <Tr>
                <Th>{t("id")}</Th>
                <Th>{t("name")}</Th>
                <Th>{t("date-of-birth")}</Th>
                <Th>{t("actions")}</Th>
              </Tr>
            </thead>
            <Tbody>
              {newborns.map((newborn) => (
                <Tr key={newborn.id}>
                  <Td>{newborn.id}</Td>
                  <Td>{newborn.name}</Td>
                  <Td>
                    {moment(newborn.dateOfBirth).format(
                      "MMMM Do YYYY, h:mm:ss a"
                    )}
                  </Td>
                  <Td>
                    <Button
                      colorScheme="teal"
                      onClick={() =>
                        router.push(`/parent/newborns/${newborn.id}`)
                      }
                    >
                      {t("view-statistics")}
                    </Button>
                  </Td>
                </Tr>
              ))}
            </Tbody>
          </Table>
        ) : (
          <Text mt={4}>{t("no-newborns")}</Text>
        )}
      </Box>
    </VStack>
  );
};

export default ParentProfile;
