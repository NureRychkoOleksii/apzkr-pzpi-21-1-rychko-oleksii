interface Ii18nConfig {
  prefixDefault: string;
  locales: string[];
  defaultLocale: string;
}

export const i18nConfig: Ii18nConfig = {
  defaultLocale: "en",
  locales: ["en", "ua"],
  prefixDefault: "en",
};
