import { apiClient } from './client';
import type { Transaction, CreateTransactionRequest, BudgetSummary } from '../types';

export const transactionsApi = {
  // Get all transactions with filters
  getAll: async (params?: {
    monthYear?: string;
    categoryId?: number;
    type?: 'Income' | 'Expense';
  }): Promise<Transaction[]> => {
    const response = await apiClient.get<Transaction[]>('/transactions', { params });
    return response.data;
  },

  // Get transaction by ID
  getById: async (id: number): Promise<Transaction> => {
    const response = await apiClient.get<Transaction>(`/transactions/${id}`);
    return response.data;
  },

  // Create expense
  createExpense: async (data: CreateTransactionRequest): Promise<number> => {
    const response = await apiClient.post<number>('/transactions/expenses', data);
    return response.data;
  },

  // Create income
  createIncome: async (data: CreateTransactionRequest): Promise<number> => {
    const response = await apiClient.post<number>('/transactions/income', data);
    return response.data;
  },

  // Update transaction
  update: async (id: number, data: CreateTransactionRequest & { id: number }): Promise<void> => {
    await apiClient.put(`/transactions/${id}`, data);
  },

  // Delete transaction
  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/transactions/${id}`);
  },

  // Get budget summary
  getBudgetSummary: async (monthYear: string): Promise<BudgetSummary> => {
    const response = await apiClient.get<BudgetSummary>('/budget/summary', {
      params: { monthYear },
    });
    return response.data;
  },
};