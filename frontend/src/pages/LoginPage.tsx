import styled from 'styled-components';
import LoginForm from '../components/auth/LoginForm';

const PageWrapper = styled.div`
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: ${props => props.theme.spacing.md};
`;

const FormContainer = styled.div`
  width: 100%;
  max-width: 400px;
`;

const Title = styled.h1`
  text-align: center;
  color: ${props => props.theme.colors.primary};
  margin-bottom: ${props => props.theme.spacing.xl};
  font-size: ${props => props.theme.fontSizes.xxxl};
`;

const LoginPage = () => {
  return (
    <PageWrapper>
      <FormContainer>
        <Title>Budget Tracker 💰</Title>
        <LoginForm />
      </FormContainer>
    </PageWrapper>
  );
};

export default LoginPage;