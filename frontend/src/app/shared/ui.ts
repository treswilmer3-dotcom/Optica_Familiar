import { MatDialog } from '@angular/material/dialog';
import { Observable, map } from 'rxjs';
import { ConfirmData, ConfirmDialogComponent } from './confirm-dialog.component';

/** Abre un diálogo de confirmación; emite true si el usuario acepta. */
export function confirmar(dialog: MatDialog, data: ConfirmData): Observable<boolean> {
  return dialog.open(ConfirmDialogComponent, { data, width: '420px' }).afterClosed().pipe(map(r => r === true));
}

export function claseEstado(estado?: string | null): string {
  switch (estado) {
    case 'PAGADA': case 'ENTREGADA': case 'ACTIVO': case 'TERMINADA': return 'ok';
    case 'PENDIENTE': case 'CREADA': return 'warn';
    case 'EN_PRODUCCION': case 'EN_LABORATORIO': return 'info';
    case 'ANULADA': case 'INACTIVO': return 'bad';
    default: return '';
  }
}

const ETIQUETAS: Record<string, string> = { EN_PRODUCCION: 'En producción' };

export function etiquetaEstado(estado?: string | null): string {
  const e = estado ?? '';
  return ETIQUETAS[e] ?? e.replace(/_/g, ' ').toLowerCase().replace(/^./, c => c.toUpperCase());
}

/** Convierte un Date del datepicker a ISO (fecha a medianoche UTC) o null. */
export function fechaIso(d: Date | null | undefined): string | null {
  if (!d) return null;
  return new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate())).toISOString();
}

/** Convierte un número de formulario ('' / null) en number | null. */
export function num(v: unknown): number | null {
  if (v === '' || v === null || v === undefined) return null;
  const n = Number(v);
  return Number.isFinite(n) ? n : null;
}

/** '' -> null para campos de texto opcionales. */
export function txt(v: unknown): string | null {
  const s = (v ?? '').toString().trim();
  return s === '' ? null : s;
}

