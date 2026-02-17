import { environment } from '../../../environments/environment';

export const ApiEndpoints = {
  auth: {
    baseUrl: 'https://localhost:7xxx/api/auth',
    login: `${environment.apiUrl}/auth/login`,
    register: `${environment.apiUrl}/auth/register`,
    forgotPassword: 'https://localhost:7xxx/api/auth/forgot-password',
    resetPassword: 'https://localhost:7xxx/api/auth/reset-password'
  },
  properties: {
    base: `${environment.apiUrl}/property`,
    types: `${environment.apiUrl}/property/types`, // الرابط الجديد هنا
    details: (id: number) => `${environment.apiUrl}/property/${id}`,
    media: (id: number) => `${environment.apiUrl}/property/${id}/media`,
  },
  users: {
    base: `${environment.apiUrl}/Users`,
  },
  chat: {
    conversations: `${environment.apiUrl}/chat/conversations`,
    messages: (id: number) => `${environment.apiUrl}/chat/conversations/${id}/messages`,
    send: `${environment.apiUrl}/chat/send`,
  },
  contact: {
    base: `${environment.apiUrl}/contact`
  }
};