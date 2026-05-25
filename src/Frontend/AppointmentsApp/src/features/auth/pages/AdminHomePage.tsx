import { useQuery } from '@tanstack/react-query'
import { httpClient } from '../../../shared/api/httpClient'

interface AdminHealthResponse {
  status: string
}

async function fetchAdminHealth(): Promise<AdminHealthResponse> {
  const response = await httpClient.get<AdminHealthResponse>('/api/admin/health')
  return response.data
}

export function AdminHomePage() {
  const { data, error, isLoading } = useQuery({
    queryKey: ['admin-health'],
    queryFn: fetchAdminHealth,
  })

  return (
    <main className="auth-shell">
      <section className="auth-card">
        <h1>Panel Administrativo</h1>
        <p className="subtitle">Validación mínima de acceso con JWT y tenant activo.</p>

        {isLoading && <p>Validando acceso...</p>}

        {error && <div className="error-banner">No fue posible validar el acceso administrativo.</div>}

        {data && (
          <div className="success-box">
            <strong>Estado:</strong> {data.status}
          </div>
        )}
      </section>
    </main>
  )
}
