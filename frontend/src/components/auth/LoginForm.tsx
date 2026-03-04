import { useState } from 'react';
import styled from 'styled-components';
import { useNavigate } from 'react-router-dom';
import { Button, Input, Label, Card } from '../../styles/components';
import axios from 'axios';
import { useAuth } from '../../hooks/useAuth';

const Form = styled.form`
  display: flex;
  flex-direction: column;
  gap: ${props => props.theme.spacing.md};
`;

const FormGroup = styled.div`
  display: flex;
  flex-direction: column;
  gap: ${props => props.theme.spacing.xs};
`;

const ErrorMessage = styled.div`
  color: ${props => props.theme.colors.danger};
  font-size: ${props => props.theme.fontSizes.sm};
  margin-top: ${props => props.theme.spacing.xs};
`;

const LinkText = styled.p`
  text-align: center;
  margin-top: ${props => props.theme.spacing.md};
  font-size: ${props => props.theme.fontSizes.sm};
  color: ${props => props.theme.colors.gray600};

  a {
    color: ${props => props.theme.colors.primary};
    font-weight: ${props => props.theme.fontWeights.medium};
    
    &:hover {
      text-decoration: underline;
    }
  }
`;

const LoginForm = () => {
  const navigate = useNavigate();
  const { login } = useAuth();
  
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();
  setError('');
  setIsLoading(true);
  try {
    await login({ email, password });
    navigate('/dashboard');
  } catch (err: unknown) {
    if (axios.isAxiosError(err)) {
      setError(err.response?.data?.detail || 'Login failed. Please try again.');
    } else {
      setError('Login failed. Please try again.');
    }
  } finally {
    setIsLoading(false);
  }
};

  return (
    <Card>
      <h2 style={{ marginBottom: '1.5rem' }}>Welcome Back! 👋</h2>
      
      <Form onSubmit={handleSubmit}>
        <FormGroup>
          <Label htmlFor="email">Email</Label>
          <Input
            id="email"
            type="email"
            placeholder="your@email.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            disabled={isLoading}
          />
        </FormGroup>

        <FormGroup>
          <Label htmlFor="password">Password</Label>
          <Input
            id="password"
            type="password"
            placeholder="Enter your password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            disabled={isLoading}
          />
        </FormGroup>

        {error && <ErrorMessage>{error}</ErrorMessage>}

        <Button 
          type="submit" 
          variant="primary" 
          disabled={isLoading}
          style={{ marginTop: '0.5rem' }}
        >
          {isLoading ? 'Logging in...' : 'Login'}
        </Button>
      </Form>

      <LinkText>
        Don't have an account? <a href="/register">Sign up</a>
      </LinkText>
    </Card>
  );
};

export default LoginForm;