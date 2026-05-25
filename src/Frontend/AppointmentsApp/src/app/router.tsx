import { createBrowserRouter } from 'react-router-dom'
import { AdminHomePage } from '../features/auth/pages/AdminHomePage'
import { LoginPage } from '../features/auth/pages/LoginPage'

export const router = createBrowserRouter([
  {
    path: '/',
    element: <LoginPage />,
  },
  {
    path: '/admin',
    element: <AdminHomePage />,
  },
])
