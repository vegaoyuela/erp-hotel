import { useQuery } from '@tanstack/react-query'
import { useEffect, useState } from 'react'
import api from '@/lib/axios'
import type { PagedResult } from '@/types/api'

export function useDebouncedValue(value: string, delay = 250) {
  const [debounced, setDebounced] = useState(value)
  useEffect(() => {
    const timer = window.setTimeout(() => setDebounced(value), delay)
    return () => window.clearTimeout(timer)
  }, [value, delay])
  return debounced
}

export function usePagedList<T>(path: string, params: Record<string, string | number | undefined>) {
  const query = useQuery({
    queryKey: [path, params],
    queryFn: async ({ signal }) => (await api.get<PagedResult<T>>(path, { params, signal })).data,
    retry: false,
  })
  return {
    items: query.data?.items ?? [],
    totalItems: query.data?.totalItems ?? 0,
    totalPages: query.data?.totalPages ?? 1,
    page: query.data?.page ?? Number(params.page ?? 1),
    loading: query.isFetching,
    error: query.error,
    load: () => query.refetch(),
  }
}
