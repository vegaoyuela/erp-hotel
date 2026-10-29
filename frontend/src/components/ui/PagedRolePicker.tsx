import { PagedSelect } from './PagedSelect'
import { Button } from './button'
import type { RoleOption } from '@/types'

export function PagedRolePicker({ value, onChange }: { value: string[]; onChange: (roles: string[]) => void }) {
  return <div className="space-y-2">
    <PagedSelect<RoleOption> label="Agregar rol autorizado" path="/users/role-options/paged" params={{ assignableOnly: 'true' }} value=""
      onChange={(_, row) => { if (row && !value.includes(row.name)) onChange([...value, row.name]) }} getLabel={row => row.name} />
    <p className="text-xs text-muted-foreground">Solo puede asignar roles cuyos permisos ya tiene.</p>
    <ul aria-label="Roles seleccionados" className="space-y-1">
      {value.map(name => <li key={name} className="flex min-w-0 items-center justify-between gap-2 text-sm">
        <span className="break-all">{name}</span>
        <Button type="button" variant="outline" size="sm" onClick={() => onChange(value.filter(role => role !== name))} aria-label={`Quitar rol ${name}`}>Quitar</Button>
      </li>)}
    </ul>
  </div>
}
