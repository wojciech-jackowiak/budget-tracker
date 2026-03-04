import styled from 'styled-components';
import RegisterForm from '../components/auth/RegisterForm';

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

const RegisterPage = () => {
  return (
    <PageWrapper>
      <FormContainer>
        <Title>Budget Tracker 💰</Title>
        <RegisterForm />
      </FormContainer>
    </PageWrapper>
  );
};

export default RegisterPage;