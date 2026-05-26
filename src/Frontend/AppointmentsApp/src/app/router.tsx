import { createBrowserRouter } from 'react-router-dom'
import { AdminHomePage } from '../features/auth/pages/AdminHomePage'
import { LoginPage } from '../features/auth/pages/LoginPage'
import { BusinessProfilePage } from '../features/business-profile/pages/BusinessProfilePage'
import { PublicBookingPage } from '../features/public-booking/pages/PublicBookingPage'

export const router = createBrowserRouter([
  {
    path: '/',
    element: <LoginPage />,
  },
  {
    path: '/book/:businessProfileId',
    element: <PublicBookingPage />,
  },
  {
    path: '/admin',
    element: <AdminHomePage />,
  },
  {
    path: '/admin/business-profile',
    element: <BusinessProfilePage />,
  },
])
