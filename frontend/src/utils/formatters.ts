import { format, parseISO } from 'date-fns';

export const formatCurrency = (amount: number): string => {
  return new Intl.NumberFormat('pl-PL', {
    style: 'currency',
    currency: 'PLN',
  }).format(amount);
};

export const formatDate = (dateString: string): string => {
  try {
    const date = parseISO(dateString);
    return format(date, 'dd MMM yyyy');
  } catch {
    return dateString;
  }
};

export const formatMonthYear = (monthYear: string): string => {
  try {
    const [year, month] = monthYear.split('-');
    const date = new Date(parseInt(year), parseInt(month) - 1);
    return format(date, 'MMMM yyyy');
  } catch {
    return monthYear;
  }
};

export const getCurrentMonthYear = (): string => {
  return format(new Date(), 'yyyy-MM');
};

export const formatPercentage = (value: number): string => {
  return `${value.toFixed(1)}%`;
};