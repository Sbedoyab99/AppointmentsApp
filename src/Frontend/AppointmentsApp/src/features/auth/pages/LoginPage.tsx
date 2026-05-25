import { AxiosError } from 'axios'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { loginRequest, selectTenantRequest } from '../api/authApi'
import { LoginForm, type LoginFormValues } from '../components/LoginForm'
import { TenantSelector } from '../components/TenantSelector'
import type { LoginCandidateTenantDto } from '../types'
import { saveAuthSession } from '../types'

type Step = 'credentials' | 'tenant-selection'

function getErrorMessage(error: unknown, fallback: string): string {
  if (error instanceof AxiosError) {
    return error.response?.data?.message ?? fallback
  }

  return fallback
}

export function LoginPage() {
  const navigate = useNavigate()
  const [step, setStep] = useState<Step>('credentials')
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [backendError, setBackendError] = useState<string | undefined>(undefined)
  const [availableTenants, setAvailableTenants] = useState<LoginCandidateTenantDto[]>([])
  const [selectedUserId, setSelectedUserId] = useState<string>('')

  const submitCredentials = async (values: LoginFormValues) => {
    setIsSubmitting(true)
    setBackendError(undefined)

    try {
      const response = await loginRequest(values)
      const loginResult = response.data

      if (!loginResult) {
        throw new Error('Respuesta de login inválida')
      }

      setSelectedUserId(loginResult.userId)
      setAvailableTenants(loginResult.tenants)

      if (loginResult.requiresTenantSelection) {
        setStep('tenant-selection')
      } else if (loginResult.tenants.length === 1) {
        await submitTenantSelection(loginResult.tenants[0], loginResult.userId)
      } else {
        setBackendError('No se encontraron negocios asociados para esta cuenta.')
      }
    } catch (error) {
      setBackendError(getErrorMessage(error, 'No fue posible validar tus credenciales.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  const submitTenantSelection = async (tenant: LoginCandidateTenantDto, explicitUserId?: string) => {
    setIsSubmitting(true)
    setBackendError(undefined)

    try {
      const response = await selectTenantRequest({
        userId: explicitUserId ?? selectedUserId,
        tenantId: tenant.tenantId,
      })

      const session = response.data
      if (!session) {
        throw new Error('No se recibió sesión válida')
      }

      saveAuthSession({
        accessToken: session.accessToken,
        refreshToken: session.refreshToken,
        expiresAtUtc: session.expiresAtUtc,
        tenantId: session.tenantId,
        tenantName: tenant.tenantName,
      })

      navigate('/admin')
    } catch (error) {
      setBackendError(getErrorMessage(error, 'No se pudo iniciar sesión para el negocio seleccionado.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <main className="auth-shell">
      {step === 'credentials' && (
        <LoginForm isSubmitting={isSubmitting} backendError={backendError} onSubmit={submitCredentials} />
      )}

      {step === 'tenant-selection' && (
        <TenantSelector
          tenants={availableTenants}
          isSubmitting={isSubmitting}
          error={backendError}
          onSelectTenant={submitTenantSelection}
        />
      )}
    </main>
  )
}
