/** Utilidades de color para la identidad visual (hex #RRGGBB). */

const HEX = /^#[0-9a-f]{6}$/i;
export const esHex = (v: string | null | undefined): v is string => !!v && HEX.test(v);

function rgb(hex: string): [number, number, number] {
  return [1, 3, 5].map(i => parseInt(hex.slice(i, i + 2), 16)) as [number, number, number];
}
function aHex([r, g, b]: number[]): string {
  return '#' + [r, g, b].map(v => Math.round(Math.min(255, Math.max(0, v))).toString(16).padStart(2, '0')).join('').toUpperCase();
}

/** Mezcla `a` con `b` (t = 0 → a, t = 1 → b). */
export function mezclar(a: string, b: string, t: number): string {
  const [x, y] = [rgb(a), rgb(b)];
  return aHex(x.map((v, i) => v + (y[i] - v) * t));
}
export const aclarar = (hex: string, t: number) => mezclar(hex, '#FFFFFF', t);
export const oscurecer = (hex: string, t: number) => mezclar(hex, '#000000', t);

function luminancia(hex: string): number {
  const [r, g, b] = rgb(hex).map(v => v / 255).map(v => (v <= 0.03928 ? v / 12.92 : ((v + 0.055) / 1.055) ** 2.4));
  return 0.2126 * r + 0.7152 * g + 0.0722 * b;
}

/** Relación de contraste WCAG entre dos colores. */
export function contraste(a: string, b: string): number {
  const [x, y] = [luminancia(a), luminancia(b)].sort((p, q) => q - p);
  return (x + 0.05) / (y + 0.05);
}

/** Oscurece el color hasta que el texto blanco sobre él alcance contraste AA (4.5:1). */
export function asegurarContrasteConBlanco(hex: string): string {
  let c = hex.toUpperCase();
  for (let i = 0; i < 40 && contraste(c, '#FFFFFF') < 4.5; i++) c = oscurecer(c, 0.06);
  return c;
}

/** Texto legible (blanco u oscuro) sobre un fondo dado. */
export const textoSobre = (fondo: string) => (contraste(fondo, '#FFFFFF') >= 3 ? '#FFFFFF' : '#1B2024');
