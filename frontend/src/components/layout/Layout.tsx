import styled from 'styled-components';
import Navbar from './Navbar';
import type { ReactNode } from 'react';

const LayoutWrapper = styled.div`
  min-height: 100vh;
  display: flex;
  flex-direction: column;
`;

const Main = styled.main`
  flex: 1;
`;

interface LayoutProps {
  children: ReactNode;
}

const Layout = ({ children }: LayoutProps) => {
  return (
    <LayoutWrapper>
      <Navbar />
      <Main>{children}</Main>
    </LayoutWrapper>
  );
};

export default Layout;