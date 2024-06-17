"use client";

import { useState, useEffect } from "react";
import axiosInstance from "@/utils/axios-instance";
import { useRouter } from "next/navigation";
import { VStack, Box, Input, Select, Button, useToast } from "@chakra-ui/react";
import { Role, User } from "@/components/management/user-management";
import { useTranslation } from "react-i18next";

const EditUser = ({ params }: { params: { id: number } }) => {
  const [editableUser, setEditableUser] = useState<User & { password: string }>(
    {
      id: 0,
      username: "",
      email: "",
      role: Role.Parent.toString(),
      password: "",
    }
  );
  const router = useRouter();
  const toast = useToast();

  const { t } = useTranslation();

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await axiosInstance.get(`/User/${params.id}`);
        setEditableUser({
          ...response.data,
          role: Role[response.data.role],
        });
      } catch (error) {
        console.error("Error fetching user:", error);
        toast({
          title: t("error"),
          status: "error",
          duration: 5000,
        });
      }
    };

    fetchUser();
  }, [params.id, t, toast]);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setEditableUser({ ...editableUser, [name]: value });
  };

  const handleSaveUser = async () => {
    console.log({ editableUser });
    try {
      await axiosInstance.put(`/User/${params.id}`, {
        username: editableUser.username,
        email: editableUser.email,
        role: Role[editableUser.role as any],
        password: editableUser.password,
      });
      toast({
        title: t("success"),
        status: "success",
        duration: 5000,
      });
      router.push("/admin");
    } catch (error) {
      console.error("Error updating user:", error);
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

  if (!editableUser) {
    return <div>{t("loading")}</div>;
  }

  return (
    <VStack spacing={4} align="stretch">
      <Box>
        <Input
          placeholder={t("username")}
          value={editableUser.username}
          onChange={handleInputChange}
          name="username"
          mb={2}
        />
        <Input
          placeholder={t("email")}
          value={editableUser.email}
          onChange={handleInputChange}
          name="email"
          mb={2}
        />
        <Select
          placeholder={t("select-role")}
          value={editableUser.role}
          onChange={(e) => {
            setEditableUser({
              ...editableUser,
              role: e.target.value,
            });
          }}
          mb={2}
        >
          {Object.keys(Role)
            .filter((key: string) => !isNaN(+key))
            .map((key: string) => (
              <option key={key} value={Role[+key]}>
                {Role[+key]}
              </option>
            ))}
        </Select>
        <Button colorScheme="blue" onClick={handleSaveUser} mr={2}>
          {t("save-changes")}
        </Button>
        <Button colorScheme="gray" onClick={handleCancelEdit}>
          {t("cancel")}
        </Button>
      </Box>
    </VStack>
  );
};

export default EditUser;
