// Auth Types
export interface User {
  userId: number;
  username: string;
  email: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface AuthResponse {
  userId: number;
  username: string;
  email: string;
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAt: string;
  refreshTokenExpiresAt: string;
}

// Transaction Types
export interface Transaction {
  id: number;
  userId: number;
  categoryId: number;
  categoryName: string;
  categoryIcon: string;
  categoryColor: string;
  amount: number;
  signedAmount: number;
  type: 'Income' | 'Expense';
  description: string;
  date: string;
  monthYear: string;
  isFromRecurring: boolean;
  recurringTransactionId?: number;
}

export interface CreateTransactionRequest {
  categoryId: number;
  amount: number;
  type: 0 | 1; // 0 = Income, 1 = Expense
  description: string;
  date: string;
}


export interface BudgetSummary {
  userId: number;
  monthYear: string;
  totalIncome: number;
  totalExpenses: number;
  net: number;
  savingsRate: number;
  transactionCount: number;
  categoryBreakdown: CategoryBreakdown[];
}

export interface CategoryBreakdown {
  categoryId: number;
  categoryName: string;
  categoryIcon: string;
  categoryColor: string;
  income: number;
  expenses: number;
  net: number;
  transactionCount: number;
}

// Recurring Transaction Types
export interface RecurringTransaction {
  id: number;
  userId: number;
  categoryId: number;
  categoryName: string;
  categoryIcon: string;
  categoryColor: string;
  amount: number;
  type: 'Income' | 'Expense';
  description: string;
  frequency: 'Monthly' | 'Yearly';
  dayOfMonth: number;
  startDate: string;
  endDate?: string;
  isActive: boolean;
  lastProcessedMonth?: string;
  generatedTransactionsCount: number;
  createdAt: string;
}

export interface CreateRecurringRequest {
  categoryId: number;
  amount: number;
  type: 0 | 1;
  description: string;
  dayOfMonth: number;
  startDate: string;
  endDate?: string;
  frequency?: 0 | 1; // 0 = Monthly, 1 = Yearly
}

// Category Types
export interface Category {
  id: number;
  name: string;
  iconName: string;
  colorCode: string;
  description?: string;
  isSystemDefault: boolean;
}