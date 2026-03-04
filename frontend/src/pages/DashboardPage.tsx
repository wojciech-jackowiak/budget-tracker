import { useState, useEffect } from 'react';
import styled from 'styled-components';
import { Container, Grid, Button } from '../styles/components';
import { transactionsApi } from '../api/transactions';
import type { BudgetSummary } from '../types';
import { formatCurrency, formatPercentage, getCurrentMonthYear, formatMonthYear } from '../utils/formatters';
import StatCard from '../components/dashboard/StatCard';
import CategoryBreakdownCard from '../components/dashboard/CategoryBreakdownCard';

const PageHeader = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: ${props => props.theme.spacing.xl};
  flex-wrap: wrap;
  gap: ${props => props.theme.spacing.md};
`;

const Title = styled.h1`
  font-size: ${props => props.theme.fontSizes.xxxl};
  color: ${props => props.theme.colors.gray900};
`;

const MonthSelector = styled.div`
  display: flex;
  gap: ${props => props.theme.spacing.sm};
  align-items: center;
`;

const MonthDisplay = styled.div`
  padding: ${props => props.theme.spacing.sm} ${props => props.theme.spacing.md};
  background-color: ${props => props.theme.colors.white};
  border: 1px solid ${props => props.theme.colors.gray300};
  border-radius: ${props => props.theme.borderRadius.md};
  font-weight: ${props => props.theme.fontWeights.medium};
  min-width: 150px;
  text-align: center;
`;

const LoadingMessage = styled.div`
  text-align: center;
  padding: ${props => props.theme.spacing.xxl};
  color: ${props => props.theme.colors.gray600};
  font-size: ${props => props.theme.fontSizes.lg};
`;

const ErrorMessage = styled.div`
  text-align: center;
  padding: ${props => props.theme.spacing.xxl};
  color: ${props => props.theme.colors.danger};
  font-size: ${props => props.theme.fontSizes.lg};
`;

const DashboardPage = () => {
  const [monthYear, setMonthYear] = useState(getCurrentMonthYear());
  const [summary, setSummary] = useState<BudgetSummary | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadSummary();
  }, [monthYear]);

  const loadSummary = async () => {
    setIsLoading(true);
    setError('');
    
    try {
      const data = await transactionsApi.getBudgetSummary(monthYear);
      setSummary(data);
    } catch (err) {
      const detail = (err as { response?: { data?: { detail?: string } } }).response?.data?.detail;
      setError(detail ?? 'Failed to load budget summary');
    } finally {
      setIsLoading(false);
    }
  };

  const changeMonth = (offset: number) => {
    const [year, month] = monthYear.split('-').map(Number);
    const date = new Date(year, month - 1 + offset, 1);
    const newMonthYear = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
    setMonthYear(newMonthYear);
  };

  if (isLoading) {
    return (
      <Container>
        <LoadingMessage>Loading budget summary... 📊</LoadingMessage>
      </Container>
    );
  }

  if (error) {
    return (
      <Container>
        <ErrorMessage>{error}</ErrorMessage>
        <div style={{ textAlign: 'center', marginTop: '1rem' }}>
          <Button variant="primary" onClick={loadSummary}>
            Retry
          </Button>
        </div>
      </Container>
    );
  }

  if (!summary) {
    return null;
  }

  return (
    <Container style={{ paddingTop: '2rem', paddingBottom: '2rem' }}>
      <PageHeader>
        <Title>Dashboard 💰</Title>
        <MonthSelector>
          <Button variant="outline" onClick={() => changeMonth(-1)}>
            ←
          </Button>
          <MonthDisplay>{formatMonthYear(monthYear)}</MonthDisplay>
          <Button variant="outline" onClick={() => changeMonth(1)}>
            →
          </Button>
        </MonthSelector>
      </PageHeader>

      <Grid columns={4} gap="1.5rem" style={{ marginBottom: '2rem' }}>
        <StatCard
          title="Total Income"
          value={formatCurrency(summary.totalIncome)}
          subtitle={`${summary.transactionCount} transactions`}
          variant="income"
        />
        <StatCard
          title="Total Expenses"
          value={formatCurrency(summary.totalExpenses)}
          variant="expense"
        />
        <StatCard
          title="Net"
          value={formatCurrency(summary.net)}
          variant={summary.net >= 0 ? 'income' : 'expense'}
        />
        <StatCard
          title="Savings Rate"
          value={formatPercentage(summary.savingsRate)}
          subtitle={summary.net >= 0 ? 'Great job! 🎉' : 'Keep going! 💪'}
        />
      </Grid>

      <CategoryBreakdownCard breakdown={summary.categoryBreakdown} />
    </Container>
  );
};

export default DashboardPage;