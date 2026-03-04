import styled from 'styled-components';
import { Card } from '../../styles/components';
import type { CategoryBreakdown } from '../../types';
import { formatCurrency } from '../../utils/formatters';

const BreakdownList = styled.div`
  display: flex;
  flex-direction: column;
  gap: ${props => props.theme.spacing.md};
`;

const BreakdownItem = styled.div`
  display: flex;
  align-items: center;
  gap: ${props => props.theme.spacing.md};
`;

const CategoryIcon = styled.div<{ color: string }>`
  width: 48px;
  height: 48px;
  border-radius: ${props => props.theme.borderRadius.lg};
  background-color: ${props => props.color}20;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: ${props => props.theme.fontSizes.xl};
  flex-shrink: 0;
`;

const CategoryInfo = styled.div`
  flex: 1;
  min-width: 0;
`;

const CategoryName = styled.div`
  font-weight: ${props => props.theme.fontWeights.semibold};
  color: ${props => props.theme.colors.gray900};
  margin-bottom: ${props => props.theme.spacing.xs};
`;

const CategoryStats = styled.div`
  display: flex;
  gap: ${props => props.theme.spacing.md};
  font-size: ${props => props.theme.fontSizes.sm};
  color: ${props => props.theme.colors.gray600};
`;

const CategoryAmount = styled.div<{ isPositive: boolean }>`
  font-size: ${props => props.theme.fontSizes.lg};
  font-weight: ${props => props.theme.fontWeights.bold};
  color: ${props => props.isPositive 
    ? props.theme.colors.income 
    : props.theme.colors.expense};
`;

interface CategoryBreakdownCardProps {
  breakdown: CategoryBreakdown[];
}

const CategoryBreakdownCard = ({ breakdown }: CategoryBreakdownCardProps) => {
  const activeCategories = breakdown.filter(c => c.transactionCount > 0);

  if (activeCategories.length === 0) {
    return (
      <Card>
        <h3 style={{ marginBottom: '1rem' }}>Category Breakdown</h3>
        <p style={{ color: '#9CA3AF', textAlign: 'center', padding: '2rem 0' }}>
          No transactions yet this month
        </p>
      </Card>
    );
  }

  return (
    <Card>
      <h3 style={{ marginBottom: '1.5rem' }}>Category Breakdown</h3>
      <BreakdownList>
        {activeCategories.map((category) => (
          <BreakdownItem key={category.categoryId}>
            <CategoryIcon color={category.categoryColor}>
              {category.categoryIcon}
            </CategoryIcon>
            <CategoryInfo>
              <CategoryName>{category.categoryName}</CategoryName>
              <CategoryStats>
                {category.income > 0 && (
                  <span>Income: {formatCurrency(category.income)}</span>
                )}
                {category.expenses > 0 && (
                  <span>Expenses: {formatCurrency(category.expenses)}</span>
                )}
                <span>• {category.transactionCount} transactions</span>
              </CategoryStats>
            </CategoryInfo>
            <CategoryAmount isPositive={category.net > 0}>
              {formatCurrency(category.net)}
            </CategoryAmount>
          </BreakdownItem>
        ))}
      </BreakdownList>
    </Card>
  );
};

export default CategoryBreakdownCard;