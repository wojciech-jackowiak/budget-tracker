import { useState, useEffect } from 'react';
import styled from 'styled-components';
import { X } from 'lucide-react';
import { Button, Input, Select, Label, Card } from '../../styles/components';
import { transactionsApi } from '../../api/transactions';
import { categoriesApi } from '../../api/categories';
import type { Category, CreateTransactionRequest } from '../../types';

const Overlay = styled.div`
  position: fixed;
  inset: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 200;
  padding: ${props => props.theme.spacing.md};
`;

const Modal = styled(Card)`
  width: 100%;
  max-width: 480px;
  max-height: 90vh;
  overflow-y: auto;
`;

const ModalHeader = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: ${props => props.theme.spacing.lg};
`;

const ModalTitle = styled.h2`
  font-size: ${props => props.theme.fontSizes.xl};
  font-weight: ${props => props.theme.fontWeights.bold};
  color: ${props => props.theme.colors.gray900};
`;

const CloseButton = styled.button`
  background: transparent;
  border: none;
  cursor: pointer;
  color: ${props => props.theme.colors.gray500};
  padding: ${props => props.theme.spacing.xs};
  border-radius: ${props => props.theme.borderRadius.md};
  display: flex;
  align-items: center;

  &:hover {
    background-color: ${props => props.theme.colors.gray100};
    color: ${props => props.theme.colors.gray900};
  }
`;

const TypeToggle = styled.div`
  display: flex;
  border: 1px solid ${props => props.theme.colors.gray300};
  border-radius: ${props => props.theme.borderRadius.md};
  overflow: hidden;
  margin-bottom: ${props => props.theme.spacing.lg};
`;

const TypeButton = styled.button<{ $active: boolean; $type: 'income' | 'expense' }>`
  flex: 1;
  padding: ${props => props.theme.spacing.sm} ${props => props.theme.spacing.md};
  border: none;
  font-weight: ${props => props.theme.fontWeights.medium};
  font-size: ${props => props.theme.fontSizes.md};
  cursor: pointer;
  transition: all 0.2s;

  ${props => props.$active
    ? props.$type === 'income'
      ? `background-color: ${props.theme.colors.income}; color: white;`
      : `background-color: ${props.theme.colors.expense}; color: white;`
    : `background-color: transparent; color: ${props.theme.colors.gray600};`
  }
`;

const FormField = styled.div`
  margin-bottom: ${props => props.theme.spacing.md};
`;

const ErrorText = styled.p`
  color: ${props => props.theme.colors.danger};
  font-size: ${props => props.theme.fontSizes.sm};
  margin-top: ${props => props.theme.spacing.xs};
`;

const ModalFooter = styled.div`
  display: flex;
  gap: ${props => props.theme.spacing.sm};
  justify-content: flex-end;
  margin-top: ${props => props.theme.spacing.lg};
`;

interface Props {
  onClose: () => void;
  onSuccess: () => void;
}

const AddTransactionForm = ({ onClose, onSuccess }: Props) => {
  const [type, setType] = useState<'income' | 'expense'>('expense');
  const [categories, setCategories] = useState<Category[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState('');

  const today = new Date().toISOString().split('T')[0];
  const [form, setForm] = useState<{
    categoryId: string;
    amount: string;
    description: string;
    date: string;
  }>({
    categoryId: '',
    amount: '',
    description: '',
    date: today,
  });

  useEffect(() => {
    categoriesApi.getAll().then(setCategories).catch(() => {});
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setForm(prev => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (!form.categoryId) {
      setError('Please select a category');
      return;
    }
    if (!form.amount || Number(form.amount) <= 0) {
      setError('Please enter a valid amount');
      return;
    }

    const payload: CreateTransactionRequest = {
      categoryId: Number(form.categoryId),
      amount: Number(form.amount),
      type: type === 'income' ? 0 : 1,
      description: form.description,
      date: form.date,
    };

    setIsSubmitting(true);
    try {
      if (type === 'income') {
        await transactionsApi.createIncome(payload);
      } else {
        await transactionsApi.createExpense(payload);
      }
      onSuccess();
    } catch (err) {
      const detail = (err as { response?: { data?: { detail?: string } } }).response?.data?.detail;
      setError(detail ?? 'Failed to add transaction');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Overlay onClick={onClose}>
      <Modal onClick={e => e.stopPropagation()}>
        <ModalHeader>
          <ModalTitle>Add Transaction</ModalTitle>
          <CloseButton onClick={onClose}>
            <X size={20} />
          </CloseButton>
        </ModalHeader>

        <TypeToggle>
          <TypeButton
            type="button"
            $active={type === 'income'}
            $type="income"
            onClick={() => setType('income')}
          >
            Income
          </TypeButton>
          <TypeButton
            type="button"
            $active={type === 'expense'}
            $type="expense"
            onClick={() => setType('expense')}
          >
            Expense
          </TypeButton>
        </TypeToggle>

        <form onSubmit={handleSubmit}>
          <FormField>
            <Label htmlFor="categoryId">Category</Label>
            <Select
              id="categoryId"
              name="categoryId"
              value={form.categoryId}
              onChange={handleChange}
            >
              <option value="">Select category...</option>
              {categories.map(cat => (
                <option key={cat.id} value={cat.id}>
                  {cat.iconName} {cat.name}
                </option>
              ))}
            </Select>
          </FormField>

          <FormField>
            <Label htmlFor="amount">Amount</Label>
            <Input
              id="amount"
              name="amount"
              type="number"
              min="0.01"
              step="0.01"
              placeholder="0.00"
              value={form.amount}
              onChange={handleChange}
            />
          </FormField>

          <FormField>
            <Label htmlFor="description">Description</Label>
            <Input
              id="description"
              name="description"
              type="text"
              placeholder="Optional description"
              value={form.description}
              onChange={handleChange}
            />
          </FormField>

          <FormField>
            <Label htmlFor="date">Date</Label>
            <Input
              id="date"
              name="date"
              type="date"
              value={form.date}
              onChange={handleChange}
            />
          </FormField>

          {error && <ErrorText>{error}</ErrorText>}

          <ModalFooter>
            <Button type="button" variant="outline" onClick={onClose}>
              Cancel
            </Button>
            <Button
              type="submit"
              variant={type === 'income' ? 'secondary' : 'danger'}
              disabled={isSubmitting}
            >
              {isSubmitting ? 'Saving...' : `Add ${type === 'income' ? 'Income' : 'Expense'}`}
            </Button>
          </ModalFooter>
        </form>
      </Modal>
    </Overlay>
  );
};

export default AddTransactionForm;
