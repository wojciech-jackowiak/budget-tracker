import styled from 'styled-components';

// Button Components
export const Button = styled.button<{ variant?: 'primary' | 'secondary' | 'danger' | 'outline' }>`
  padding: ${props => props.theme.spacing.sm} ${props => props.theme.spacing.md};
  border-radius: ${props => props.theme.borderRadius.md};
  font-weight: ${props => props.theme.fontWeights.medium};
  font-size: ${props => props.theme.fontSizes.md};
  transition: all 0.2s;
  display: inline-flex;
  align-items: center;
  gap: ${props => props.theme.spacing.sm};
  
  ${props => {
    switch (props.variant) {
      case 'primary':
        return `
          background-color: ${props.theme.colors.primary};
          color: ${props.theme.colors.white};
          &:hover:not(:disabled) {
            background-color: ${props.theme.colors.primaryHover};
          }
        `;
      case 'secondary':
        return `
          background-color: ${props.theme.colors.secondary};
          color: ${props.theme.colors.white};
          &:hover:not(:disabled) {
            background-color: ${props.theme.colors.secondaryHover};
          }
        `;
      case 'danger':
        return `
          background-color: ${props.theme.colors.danger};
          color: ${props.theme.colors.white};
          &:hover:not(:disabled) {
            background-color: ${props.theme.colors.dangerHover};
          }
        `;
      case 'outline':
        return `
          background-color: transparent;
          color: ${props.theme.colors.primary};
          border: 1px solid ${props.theme.colors.primary};
          &:hover:not(:disabled) {
            background-color: ${props.theme.colors.primaryLight};
          }
        `;
      default:
        return `
          background-color: ${props.theme.colors.primary};
          color: ${props.theme.colors.white};
          &:hover:not(:disabled) {
            background-color: ${props.theme.colors.primaryHover};
          }
        `;
    }
  }}
`;

// Card Component
export const Card = styled.div`
  background-color: ${props => props.theme.colors.white};
  border-radius: ${props => props.theme.borderRadius.lg};
  box-shadow: ${props => props.theme.shadows.md};
  padding: ${props => props.theme.spacing.lg};
`;

// Input Component
export const Input = styled.input`
  width: 100%;
  padding: ${props => props.theme.spacing.sm} ${props => props.theme.spacing.md};
  border: 1px solid ${props => props.theme.colors.gray300};
  border-radius: ${props => props.theme.borderRadius.md};
  font-size: ${props => props.theme.fontSizes.md};
  transition: all 0.2s;
  
  &:focus {
    outline: none;
    border-color: ${props => props.theme.colors.primary};
    box-shadow: 0 0 0 3px ${props => props.theme.colors.primaryLight};
  }
  
  &::placeholder {
    color: ${props => props.theme.colors.gray400};
  }
  
  &:disabled {
    background-color: ${props => props.theme.colors.gray100};
    cursor: not-allowed;
  }
`;

// Select Component
export const Select = styled.select`
  width: 100%;
  padding: ${props => props.theme.spacing.sm} ${props => props.theme.spacing.md};
  border: 1px solid ${props => props.theme.colors.gray300};
  border-radius: ${props => props.theme.borderRadius.md};
  font-size: ${props => props.theme.fontSizes.md};
  background-color: ${props => props.theme.colors.white};
  transition: all 0.2s;
  
  &:focus {
    outline: none;
    border-color: ${props => props.theme.colors.primary};
    box-shadow: 0 0 0 3px ${props => props.theme.colors.primaryLight};
  }
`;

// Label Component
export const Label = styled.label`
  display: block;
  font-weight: ${props => props.theme.fontWeights.medium};
  font-size: ${props => props.theme.fontSizes.sm};
  color: ${props => props.theme.colors.gray700};
  margin-bottom: ${props => props.theme.spacing.xs};
`;

// Container
export const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 ${props => props.theme.spacing.md};
  
  @media (min-width: ${props => props.theme.breakpoints.tablet}) {
    padding: 0 ${props => props.theme.spacing.lg};
  }
`;

// Grid
export const Grid = styled.div<{ columns?: number; gap?: string }>`
  display: grid;
  grid-template-columns: repeat(${props => props.columns || 1}, 1fr);
  gap: ${props => props.gap || props.theme.spacing.md};
  
  @media (min-width: ${props => props.theme.breakpoints.tablet}) {
    grid-template-columns: repeat(${props => Math.min(props.columns || 2, 2)}, 1fr);
  }
  
  @media (min-width: ${props => props.theme.breakpoints.desktop}) {
    grid-template-columns: repeat(${props => props.columns || 3}, 1fr);
  }
`;

// Flex
export const Flex = styled.div<{ 
  direction?: 'row' | 'column'; 
  justify?: string; 
  align?: string;
  gap?: string;
}>`
  display: flex;
  flex-direction: ${props => props.direction || 'row'};
  justify-content: ${props => props.justify || 'flex-start'};
  align-items: ${props => props.align || 'stretch'};
  gap: ${props => props.gap || '0'};
`;