import { createGlobalStyle } from 'styled-components';

export const GlobalStyles = createGlobalStyle`
  * {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }

  body {
    font-family: ${props => props.theme.fonts.body};
    font-size: ${props => props.theme.fontSizes.md};
    color: ${props => props.theme.colors.gray900};
    background-color: ${props => props.theme.colors.gray50};
    line-height: 1.5;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
  }

  h1, h2, h3, h4, h5, h6 {
    font-family: ${props => props.theme.fonts.heading};
    font-weight: ${props => props.theme.fontWeights.bold};
    line-height: 1.2;
  }

  h1 {
    font-size: ${props => props.theme.fontSizes.xxxl};
  }

  h2 {
    font-size: ${props => props.theme.fontSizes.xxl};
  }

  h3 {
    font-size: ${props => props.theme.fontSizes.xl};
  }

  button {
    font-family: ${props => props.theme.fonts.body};
    cursor: pointer;
    border: none;
    outline: none;
    
    &:disabled {
      cursor: not-allowed;
      opacity: 0.6;
    }
  }

  input, textarea, select {
    font-family: ${props => props.theme.fonts.body};
    font-size: ${props => props.theme.fontSizes.md};
  }

  a {
    color: ${props => props.theme.colors.primary};
    text-decoration: none;
    
    &:hover {
      text-decoration: underline;
    }
  }
`;