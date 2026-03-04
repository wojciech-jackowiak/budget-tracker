import { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import styled from 'styled-components';
import { LogOut, Menu, X, LayoutDashboard, Receipt, Repeat } from 'lucide-react';
import { useAuth } from '../../context/AuthContext';
import { Button } from '../../styles/components';

const Nav = styled.nav`
  background-color: ${props => props.theme.colors.white};
  border-bottom: 1px solid ${props => props.theme.colors.gray200};
  box-shadow: ${props => props.theme.shadows.sm};
  position: sticky;
  top: 0;
  z-index: 100;
`;

const NavContainer = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 ${props => props.theme.spacing.md};
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 64px;
`;

const Logo = styled(Link)`
  font-size: ${props => props.theme.fontSizes.xl};
  font-weight: ${props => props.theme.fontWeights.bold};
  color: ${props => props.theme.colors.primary};
  text-decoration: none;
  display: flex;
  align-items: center;
  gap: ${props => props.theme.spacing.sm};

  &:hover {
    text-decoration: none;
    opacity: 0.8;
  }
`;

const NavLinks = styled.div<{ $isOpen: boolean }>`
  display: flex;
  align-items: center;
  gap: ${props => props.theme.spacing.lg};

  @media (max-width: ${props => props.theme.breakpoints.tablet}) {
    position: fixed;
    top: 64px;
    left: 0;
    right: 0;
    background-color: ${props => props.theme.colors.white};
    flex-direction: column;
    padding: ${props => props.theme.spacing.lg};
    border-bottom: 1px solid ${props => props.theme.colors.gray200};
    box-shadow: ${props => props.theme.shadows.lg};
    transform: translateY(${props => props.$isOpen ? '0' : '-100%'});
    opacity: ${props => props.$isOpen ? '1' : '0'};
    transition: all 0.3s ease;
    pointer-events: ${props => props.$isOpen ? 'all' : 'none'};
  }
`;

const NavLink = styled(Link)<{ $isActive: boolean }>`
  display: flex;
  align-items: center;
  gap: ${props => props.theme.spacing.xs};
  padding: ${props => props.theme.spacing.sm} ${props => props.theme.spacing.md};
  border-radius: ${props => props.theme.borderRadius.md};
  font-weight: ${props => props.theme.fontWeights.medium};
  color: ${props => props.$isActive 
    ? props.theme.colors.primary 
    : props.theme.colors.gray700};
  background-color: ${props => props.$isActive 
    ? props.theme.colors.primaryLight 
    : 'transparent'};
  text-decoration: none;
  transition: all 0.2s;

  &:hover {
    background-color: ${props => props.theme.colors.primaryLight};
    color: ${props => props.theme.colors.primary};
    text-decoration: none;
  }

  svg {
    width: 20px;
    height: 20px;
  }

  @media (max-width: ${props => props.theme.breakpoints.tablet}) {
    width: 100%;
    justify-content: flex-start;
  }
`;

const UserSection = styled.div`
  display: flex;
  align-items: center;
  gap: ${props => props.theme.spacing.md};

  @media (max-width: ${props => props.theme.breakpoints.tablet}) {
    width: 100%;
    flex-direction: column;
  }
`;

const UserInfo = styled.div`
  display: flex;
  align-items: center;
  gap: ${props => props.theme.spacing.sm};
  padding: ${props => props.theme.spacing.sm} ${props => props.theme.spacing.md};
  background-color: ${props => props.theme.colors.gray100};
  border-radius: ${props => props.theme.borderRadius.lg};

  @media (max-width: ${props => props.theme.breakpoints.tablet}) {
    width: 100%;
    justify-content: center;
  }
`;

const UserName = styled.span`
  font-weight: ${props => props.theme.fontWeights.medium};
  color: ${props => props.theme.colors.gray900};
`;

const MobileMenuButton = styled.button`
  display: none;
  background: transparent;
  border: none;
  padding: ${props => props.theme.spacing.xs};
  color: ${props => props.theme.colors.gray700};
  cursor: pointer;

  svg {
    width: 24px;
    height: 24px;
  }

  @media (max-width: ${props => props.theme.breakpoints.tablet}) {
    display: block;
  }
`;

const Navbar = () => {
  const { user, logout } = useAuth();
  const location = useLocation();
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

  const handleLogout = () => {
    logout();
    setIsMobileMenuOpen(false);
  };

  const closeMobileMenu = () => {
    setIsMobileMenuOpen(false);
  };

  return (
    <Nav>
      <NavContainer>
        <Logo to="/dashboard">
          💰 Budget Tracker
        </Logo>

        <MobileMenuButton onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}>
          {isMobileMenuOpen ? <X /> : <Menu />}
        </MobileMenuButton>

        <NavLinks $isOpen={isMobileMenuOpen}>
          <NavLink 
            to="/dashboard" 
            $isActive={location.pathname === '/dashboard'}
            onClick={closeMobileMenu}
          >
            <LayoutDashboard />
            Dashboard
          </NavLink>

          <NavLink 
            to="/transactions" 
            $isActive={location.pathname === '/transactions'}
            onClick={closeMobileMenu}
          >
            <Receipt />
            Transactions
          </NavLink>

          <NavLink 
            to="/recurring" 
            $isActive={location.pathname === '/recurring'}
            onClick={closeMobileMenu}
          >
            <Repeat />
            Recurring
          </NavLink>

          <UserSection>
            <UserInfo>
              <span style={{ fontSize: '1.25rem' }}>👤</span>
              <UserName>{user?.username}</UserName>
            </UserInfo>

            <Button 
              variant="outline" 
              onClick={handleLogout}
              style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}
            >
              <LogOut size={16} />
              Logout
            </Button>
          </UserSection>
        </NavLinks>
      </NavContainer>
    </Nav>
  );
};

export default Navbar;