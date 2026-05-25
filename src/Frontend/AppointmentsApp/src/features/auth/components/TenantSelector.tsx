import type { LoginCandidateTenantDto } from '../types'

interface TenantSelectorProps {
  tenants: LoginCandidateTenantDto[]
  isSubmitting: boolean
  error?: string
  onSelectTenant: (tenant: LoginCandidateTenantDto) => Promise<void>
}

export function TenantSelector({ tenants, isSubmitting, error, onSelectTenant }: Readonly<TenantSelectorProps>) {
  return (
    <section className="auth-card">
      <h1>Selecciona tu negocio</h1>
      <p className="subtitle">Encontramos múltiples perfiles asociados a tu cuenta.</p>

      <div className="tenant-list">
        {tenants.map((tenant) => (
          <button
            key={tenant.tenantId}
            className="tenant-item"
            type="button"
            onClick={() => onSelectTenant(tenant)}
            disabled={isSubmitting}
          >
            <span>{tenant.tenantName}</span>
            <small>{tenant.isOwner ? 'Propietario' : 'Staff'}</small>
          </button>
        ))}
      </div>

      {error && <div className="error-banner">{error}</div>}
    </section>
  )
}
