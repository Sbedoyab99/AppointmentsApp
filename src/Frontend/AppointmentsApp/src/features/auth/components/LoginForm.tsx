import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

const loginSchema = z.object({
  email: z.email('Ingresa un correo válido'),
  password: z.string().min(8, 'La contraseña debe tener mínimo 8 caracteres'),
})

export type LoginFormValues = z.infer<typeof loginSchema>

interface LoginFormProps {
  isSubmitting: boolean
  backendError?: string
  onSubmit: (values: LoginFormValues) => Promise<void>
}

export function LoginForm({ isSubmitting, backendError, onSubmit }: Readonly<LoginFormProps>) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  })

  return (
    <form className="auth-card" onSubmit={handleSubmit(onSubmit)} noValidate>
      <h1>Ingreso Administrativo</h1>
      <p className="subtitle">Accede a tu panel para gestionar citas y disponibilidad.</p>

      <label htmlFor="email">Correo</label>
      <input id="email" type="email" autoComplete="email" {...register('email')} />
      {errors.email && <small className="field-error">{errors.email.message}</small>}

      <label htmlFor="password">Contraseña</label>
      <input id="password" type="password" autoComplete="current-password" {...register('password')} />
      {errors.password && <small className="field-error">{errors.password.message}</small>}

      {backendError && <div className="error-banner">{backendError}</div>}

      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Validando...' : 'Continuar'}
      </button>
    </form>
  )
}
