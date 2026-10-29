import { useId, useState } from 'react'
import { useDebouncedValue, usePagedList } from '@/hooks/usePagedList'
import { Input } from './input'
import { Button } from './button'
import { Pagination } from './Pagination'

type Row = { id: string }

export function PagedSelect<T extends Row>({ label, path, params = {}, value, onChange, getLabel, disabled = false }: {
  label: string
  path: string
  params?: Record<string, string | number | undefined>
  value: string
  onChange: (value: string, row: T | null) => void
  getLabel: (row: T) => string
  disabled?: boolean
}) {
  const id = useId()
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(1)
  const [selected, setSelected] = useState<T | null>(null)
  const debouncedSearch = useDebouncedValue(search)
  const query = usePagedList<T>(path, { ...params, search: debouncedSearch || undefined, page, pageSize: 10 })
  const retained = selected?.id === value && !query.items.some(row => row.id === value) ? selected : null
  return (
    <div className="min-w-0 space-y-2">
      <label htmlFor={id} className="text-xs font-semibold text-muted-foreground">{label}</label>
      <Input aria-label={`Buscar ${label.toLowerCase()}`} placeholder="Buscar en todas las opciones..." maxLength={100} value={search}
        onKeyDown={event => { if (event.key === 'Enter') event.preventDefault() }} disabled={disabled} onChange={event => { setSearch(event.target.value); setPage(1) }} className="text-xs" />
      <select id={id} value={value} disabled={disabled || query.loading || !!query.error} className="w-full min-w-0 rounded-md border border-input bg-background px-2.5 py-2 text-xs"
        onChange={event => {
          const row = query.items.find(item => item.id === event.target.value) ?? null
          setSelected(row)
          onChange(event.target.value, row)
        }}>
        <option value="">{query.loading ? 'Cargando opciones…' : 'Seleccione una opción'}</option>
        {retained && <option value={retained.id}>{getLabel(retained)}</option>}
        {query.items.map(row => <option key={row.id} value={row.id}>{getLabel(row)}</option>)}
      </select>
      {query.error ? <div className="text-xs text-destructive" role="alert">No se pudieron cargar las opciones. <Button type="button" variant="outline" size="sm" onClick={() => void query.load()}>Reintentar</Button></div>
        : query.loading ? <p role="status" className="text-xs text-muted-foreground">Consultando opciones…</p>
          : query.totalItems === 0 && <p className="text-xs text-muted-foreground">No hay opciones con esa búsqueda.</p>}
      <Pagination compact currentPage={query.page} totalPages={query.totalPages} totalItems={query.totalItems} pageSize={10}
        disabled={disabled || query.loading} onPageChange={setPage} />
    </div>
  )
}
