import styled from 'styled-components';
import { Card } from '../../styles/components';

const StyledCard = styled(Card)<{ variant?: 'income' | 'expense' | 'neutral' }>`
  border-left: 4px solid ${props => {
    switch (props.variant) {
      case 'income': return props.theme.colors.income;
      case 'expense': return props.theme.colors.expense;
      default: return props.theme.colors.primary;
    }
  }};
`;
const Title = styled.h3`
  font-size: ${props => props.theme.fontSizes.sm};
  font-weight: ${props => props.theme.fontWeights.medium};
  color: ${props => props.theme.colors.gray600};
  margin-bottom: ${props => props.theme.spacing.xs};
  text-transform: uppercase;
  letter-spacing: 0.05em;
`;
const Value = styled.div<{ variant?: 'income' | 'expense' | 'neutral' }>`
  font-size: ${props => props.theme.fontSizes.xxxl};
  font-weight: ${props => props.theme.fontWeights.bold};
  color: ${props => {
    switch (props.variant) {
      case 'income': return props.theme.colors.income;
      case 'expense': return props.theme.colors.expense;
      default: return props.theme.colors.gray900;
    }
  }};
`;

const Subtitle = styled.div`
  font-size: ${props => props.theme.fontSizes.sm};
  color: ${props => props.theme.colors.gray500};
  margin-top: ${props => props.theme.spacing.xs};
`;

interface StatCardProps {
  title: string;
  value: string;
  subtitle?: string;
  variant?: 'income' | 'expense' | 'neutral';
}

const StatCard = ({ title, value, subtitle, variant = 'neutral' }: StatCardProps) => {
  return (
    <StyledCard variant={variant}>
      <Title>{title}</Title>
      <Value variant={variant}>{value}</Value>
      {subtitle && <Subtitle>{subtitle}</Subtitle>}
    </StyledCard>
  );
};

export default StatCard;