"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import { jwtDecode } from "jwt-decode";

export const useAdminAuth = () => {
  const token = localStorage.getItem("token");
  const router = useRouter();

  useEffect(() => {
    console.log({ token });
    if (token !== null) {
      const decodedToken = jwtDecode(token);
      if (
        (decodedToken as any)[
          "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        ] !== "Admin"
      ) {
        router.push("/");
      }
    } else {
      router.push("/login");
    }
  }, [router, token]);
};
