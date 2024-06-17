"use client";

import { i18nConfig } from "@/i18n-config";
import { Button } from "@chakra-ui/react";
import { useRouter } from "next/navigation";
import { usePathname } from "next/navigation";
import { useTranslation } from "react-i18next";

export const LanguagePicker = () => {
  const { i18n, t } = useTranslation();
  const currentLocale = i18n.language;
  const router = useRouter();
  const currentPathname = usePathname();

  const handleChange = (locale: string) => {
    if (
      currentLocale === i18nConfig.defaultLocale &&
      !i18nConfig.prefixDefault
    ) {
      router.push("/" + locale + currentPathname);
    } else {
      router.push(currentPathname.replace(`/${currentLocale}`, `/${locale}`));
    }

    router.refresh();
  };

  return (
    <>
      <Button mr={4} onClick={() => handleChange("en")}>
        {t("en")}
      </Button>
      <Button mr={4} onClick={() => handleChange("ua")}>
        {t("ua")}
      </Button>
    </>
  );
};
