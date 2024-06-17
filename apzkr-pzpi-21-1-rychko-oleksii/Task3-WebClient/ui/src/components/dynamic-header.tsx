"use client";

import React, { useEffect, useState } from "react";
import dynamic from "next/dynamic";

const HeaderNotLogged = dynamic(() => import("./header-not-logged"), {
  ssr: false,
});
const HeaderLogged = dynamic(() => import("./header-logged"), {
  ssr: false,
});

const DynamicHeader: React.FC = () => {
  const [token, setToken] = useState("");

  useEffect(() => {
    setToken(localStorage.getItem("token") ?? "");

    const handleStorageChange = () => {
      const newToken = localStorage.getItem("token") ?? "";
      setToken(newToken);
    };

    window.addEventListener("storage", handleStorageChange);

    return () => window.removeEventListener("storage", handleStorageChange);
  }, []);

  return <>{token ? <HeaderLogged /> : <HeaderNotLogged />}</>;
};

export default DynamicHeader;
