"use client";

import { FC, ReactNode, useEffect, useMemo, useState } from "react";
import { I18nextProvider } from "react-i18next";
import { initTranslations } from "../i18n";

interface TranslationsProviderProps {
  children: ReactNode;
  locale: string;
}

type I18nType = Awaited<ReturnType<typeof initTranslations>> | null;

let i18n: I18nType;

export const TranslationsProvider: FC<TranslationsProviderProps> = ({
  children,
  locale,
}) => {
  const [instance, setInstance] = useState<I18nType>(i18n);

  const namespaces = useMemo(() => ["common"], []);

  useEffect(() => {
    const init = async () => {
      if (!i18n) {
        const newInstance = await initTranslations(locale, namespaces);
        i18n = newInstance;
        setInstance(newInstance);
      } else if (i18n.language !== locale) {
        await i18n.changeLanguage(locale);
      }
    };

    init();
  }, [locale, namespaces]);

  if (!instance) {
    return null;
  }

  return <I18nextProvider i18n={instance}>{children}</I18nextProvider>;
};
