import axiosInstance from "@/utils/axios-instance";
import {
  VStack,
  Button,
  Table,
  Thead,
  Tr,
  Th,
  Tbody,
  Td,
  Box,
  Input,
  Select,
} from "@chakra-ui/react";
import { useRouter } from "next/navigation";

import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";

export interface User {
  id: number;
  username: string;
  email: string;
  role: string;
}

export enum Role {
  Newborn = 0,
  Admin = 1,
  Doctor = 2,
  Sensor = 3,
  Parent = 4,
}

const UserManagement: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [newUser, setNewUser] = useState<{
    username: string;
    password: string;
    email: string;
    role: string;
  }>({
    username: "",
    password: "",
    email: "",
    role: Role.Parent.toString(),
  });

  const { t } = useTranslation();
  const router = useRouter();

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await axiosInstance.get("/User");
        setUsers(response.data);
      } catch (error) {
        console.error("Error fetching users:", error);
      }
    };

    fetchUsers();
  }, []);

  const handleDelete = async (id: number) => {
    try {
      await axiosInstance.delete(`/User/${id}`);
      setUsers(users.filter((user) => user.id !== id));
    } catch (error) {
      console.error("Error deleting user:", error);
    }
  };

  const handleEditUser = (id: number) => {
    router.push(`/admin/users/${id}`);
  };

  const handleAddUser = async () => {
    try {
      console.log(newUser.role);
      await axiosInstance.post("/User", newUser);
      setNewUser({
        username: "",
        email: "",
        role: Role.Parent.toString(),
        password: "",
      });
    } catch (error) {
      console.error("Error adding user:", error);
    }
  };

  return (
    <VStack spacing={4} align="stretch">
      <Box>
        <Input
          placeholder={t("username")}
          value={newUser.username}
          onChange={(e) => setNewUser({ ...newUser, username: e.target.value })}
          mb={2}
        />
        <Input
          placeholder={t("password")}
          value={newUser.password}
          onChange={(e) => setNewUser({ ...newUser, password: e.target.value })}
          mb={2}
        />
        <Input
          placeholder={t("email")}
          value={newUser.email}
          onChange={(e) => setNewUser({ ...newUser, email: e.target.value })}
          mb={2}
        />
        <Select
          placeholder={t("select-role")}
          onChange={(e) => {
            setNewUser({
              ...newUser,
              role: Role[e.target.value as any],
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
        <Button
          colorScheme="blue"
          mb={4}
          onClick={async () => await handleAddUser()}
        >
          {t("add-new-user")}
        </Button>
        <Table variant="simple">
          <Thead>
            <Tr>
              <Th>ID</Th>
              <Th>{t("name")}</Th>
              <Th>{t("email")}</Th>
              <Th>{t("actions")}</Th>
            </Tr>
          </Thead>
          <Tbody>
            {users.map((user) => (
              <Tr key={user.id}>
                <Td>{user.id}</Td>
                <Td>{user.username}</Td>
                <Td>{user.email}</Td>
                <Td
                  style={{
                    display: "flex",
                    gap: "5px",
                  }}
                >
                  <Button
                    colorScheme="green"
                    onClick={() => handleEditUser(user.id)}
                  >
                    {t("edit-user")}
                  </Button>
                  <Button
                    colorScheme="red"
                    onClick={() => handleDelete(user.id)}
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

export default UserManagement;
