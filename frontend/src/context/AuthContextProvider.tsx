import { useState, type ReactNode } from 'react';
import { AuthContext } from './AuthContext';
import { authApi } from '../api/auth';
import type { User, LoginRequest, RegisterRequest } from '../types';

const getStoredUser = (): User | null => {
  const token = localStorage.getItem('accessToken');
  const storedUser = localStorage.getItem('user');

  if (token && storedUser) {
    try {
      return JSON.parse(storedUser);
    } catch (error) {
      console.error('Failed to parse stored user', error);
      localStorage.removeItem('user');
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      return null;
    }
  }
  return null;
};

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(getStoredUser);

  const login = async (data: LoginRequest) => {
    const response = await authApi.login(data);

    localStorage.setItem('accessToken', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);

    const userObj: User = {
      userId: response.userId,
      username: response.username,
      email: response.email,
    };
    localStorage.setItem('user', JSON.stringify(userObj));

    setUser(userObj);
  };

  const register = async (data: RegisterRequest) => {
    const response = await authApi.register(data);

    localStorage.setItem('accessToken', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);

    const userObj: User = {
      userId: response.userId,
      username: response.username,
      email: response.email,
    };
    localStorage.setItem('user', JSON.stringify(userObj));

    setUser(userObj);
  };

  const logout = () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
    setUser(null);
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        isLoading: false,
        login,
        register,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};