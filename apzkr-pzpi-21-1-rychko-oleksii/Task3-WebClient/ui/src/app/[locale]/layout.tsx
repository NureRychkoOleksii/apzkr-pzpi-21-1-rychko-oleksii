import DynamicHeader from "@/components/dynamic-header";
import { PropsWithChildren, ReactNode } from "react";
import { TranslationsProvider } from "./locale-provider";
import { ChakraProvider } from "@chakra-ui/react";

interface RootLayoutProps {
  children: ReactNode;
  params: {
    locale: string;
  };
}

const RootLayout = ({
  children,
  params: { locale },
}: Readonly<RootLayoutProps>) => {
  return (
    <html lang={locale}>
      <body>
        <ChakraProvider>
          <TranslationsProvider locale={locale}>
            <>
              <DynamicHeader />
              <main
                style={{
                  padding: "30px",
                }}
              >
                {children}
              </main>
            </>
          </TranslationsProvider>
        </ChakraProvider>
      </body>
    </html>
  );
};

export default RootLayout;
