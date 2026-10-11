// Genera src/app/core/brand/material-tokens.ts a partir del tema prediseñado de Angular Material.
// Lista las variables CSS que llevan el color primario del tema (azure) para poder reemplazarlas
// en tiempo de ejecución con el color de cada empresa, respetando el ámbito donde Material las define:
//   - las del selector global `html` se reemplazan en el elemento raíz;
//   - las de reglas con clase (.mat-primary, .mat-badge…) se reemplazan con una hoja de estilos equivalente.
// Ejecutar tras actualizar Angular Material:   node scripts/generar-tokens-material.mjs
import fs from 'node:fs';

const css = fs.readFileSync('node_modules/@angular/material/prebuilt-themes/azure-blue.css', 'utf8');
const PRIMARIO = '#005cbb';   // color primario del tema
const CONTENEDOR = '#d7e3ff'; // contenedor claro del primario

function porAmbito(color) {
  const ambitos = {};
  for (const regla of css.matchAll(/([^{}]+)\{([^{}]*)\}/g)) {
    const selector = regla[1].trim();
    const props = [...regla[2].matchAll(/(--(?:mdc|mat)-[a-z0-9-]+)\s*:\s*(#[0-9a-fA-F]{6})\b/g)]
      .filter(p => p[2].toLowerCase() === color).map(p => p[1]);
    if (props.length) ambitos[selector] = [...new Set([...(ambitos[selector] ?? []), ...props])].sort();
  }
  return ambitos;
}

const prim = porAmbito(PRIMARIO);
const cont = porAmbito(CONTENEDOR);
const lista = a => a.map(t => `  '${t}'`).join(',\n');
const scoped = Object.entries(prim).filter(([s]) => s !== 'html')
  .map(([s, a]) => `  '${s}': [${a.map(t => `'${t}'`).join(', ')}]`).join(',\n');

fs.writeFileSync('src/app/core/brand/material-tokens.ts',
`// ARCHIVO GENERADO por scripts/generar-tokens-material.mjs. No editar a mano.
// Variables CSS de Angular Material que contienen el color primario del tema.

/** Definidas en el selector global \`html\`. */
export const TOKENS_PRIMARIO: readonly string[] = [
${lista(prim['html'] ?? [])}
];

/** Contenedor claro del primario (selector global). */
export const TOKENS_PRIMARIO_CLARO: readonly string[] = [
${lista(cont['html'] ?? [])}
];

/** Definidas en reglas con clase: selector -> variables. */
export const TOKENS_PRIMARIO_POR_SELECTOR: Readonly<Record<string, readonly string[]>> = {
${scoped}
};
`);
console.log(`html: ${(prim['html'] ?? []).length} · contenedor: ${(cont['html'] ?? []).length} · con clase: ${Object.keys(prim).length - 1} selectores`);
