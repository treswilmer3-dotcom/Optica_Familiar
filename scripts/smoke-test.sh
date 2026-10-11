#!/usr/bin/env bash
# Verificación rápida de extremo a extremo contra el entorno local (API + PostgreSQL + frontend).
# Requiere: API en marcha (./scripts/dev.sh start), curl y python3.
# Por defecto es de SOLO LECTURA. Con SMOKE_WRITE=1 además recorre el flujo completo creando registros nuevos.
# Las contraseñas se leen de 'dotnet user-secrets' (Seed:DemoPassword / Seed:SuperAdminPassword); no se guardan aquí.
set -u
RAIZ="$(cd "$(dirname "$0")/.." && pwd)"
API="${API_URL:-http://localhost:5130}/api"
WEB="${WEB_URL:-http://localhost:4200}"
H='Content-Type: application/json'
FALLAS=0

secreto() { dotnet user-secrets list --project "$RAIZ/backend/OpticaFamiliar.API" 2>/dev/null | sed -n "s/^$1 = //p"; }
DEMO_PW="${DEMO_PASSWORD:-$(secreto Seed:DemoPassword)}"
SUPER_PW="${SUPER_PASSWORD:-$(secreto Seed:SuperAdminPassword)}"
[ -n "$DEMO_PW" ] || { echo "Falta Seed:DemoPassword (user-secrets) o DEMO_PASSWORD."; exit 2; }

json() { python3 -c "import sys,json;d=json.load(sys.stdin);print($1)" 2>/dev/null; }
codigo() { curl -s -o /dev/null -w "%{http_code}" "$@"; }
chkmin() { if [ "$3" -ge "$2" ] 2>/dev/null; then echo "  OK    $1"; else echo "  FALLA $1 (mínimo: $2, obtenido: $3)"; FALLAS=$((FALLAS+1)); fi; }
chk() { if [ "$2" = "$3" ]; then echo "  OK    $1"; else echo "  FALLA $1 (esperado: $2, obtenido: $3)"; FALLAS=$((FALLAS+1)); fi; }
token() { curl -s -X POST "$API/auth/login" -H "$H" -d "{\"codigoEmpresa\":\"$1\",\"username\":\"$2\",\"password\":\"$3\"}" | json "d['token']"; }
get() { curl -s "$API/$2" -H "Authorization: Bearer $1"; }
cuenta() { get "$1" "$2" | json "len(d)"; }
status() { codigo "$API/$3" -H "Authorization: Bearer $1" ${2:+-X "$2"}; }

echo "== Servicios"
chk "API responde (Swagger)" 200 "$(codigo "${API%/api}/swagger/v1/swagger.json")"
chk "CORS permite el origen del frontend" 204 "$(codigo -X OPTIONS "$API/auth/login" -H "Origin: $WEB" -H "Access-Control-Request-Method: POST")"
if curl -s -o /dev/null "$WEB"; then chk "Frontend sirve la aplicación" 1 "$(curl -s "$WEB" | grep -c '<app-root>')"; else echo "  (frontend no está en ejecución: se omite)"; fi

echo "== Autenticación"
chk "Sin token -> 401" 401 "$(codigo "$API/clientes")"
chk "Clave incorrecta -> 401" 401 "$(codigo -X POST "$API/auth/login" -H "$H" -d '{"codigoEmpresa":"DEMO-ANDINA","username":"admin","password":"incorrecta"}')"
chk "Usuario de otra empresa -> 401" 401 "$(codigo -X POST "$API/auth/login" -H "$H" -d "{\"codigoEmpresa\":\"DEMO-SIERRA\",\"username\":\"vend.quito\",\"password\":\"$DEMO_PW\"}")"
A=$(token DEMO-ANDINA admin "$DEMO_PW"); S=$(token DEMO-SIERRA admin "$DEMO_PW")
VQ=$(token DEMO-ANDINA vend.quito "$DEMO_PW"); VG=$(token DEMO-ANDINA vend.guayaquil "$DEMO_PW"); OQ=$(token DEMO-ANDINA opto.quito "$DEMO_PW")
chk "Login admin Andina" 1 "$([ -n "$A" ] && echo 1 || echo 0)"
chk "Login admin Sierra" 1 "$([ -n "$S" ] && echo 1 || echo 0)"
chk "Login vendedor y optometrista" 1 "$([ -n "$VQ" ] && [ -n "$OQ" ] && echo 1 || echo 0)"

echo "== Datos demo visibles"
chkmin "Andina: >=3 sucursales" 3 "$(cuenta "$A" sucursales)"
chkmin "Andina: >=12 clientes" 12 "$(cuenta "$A" 'clientes?tamano=100')"
chkmin "Andina: >=9 productos" 9 "$(cuenta "$A" productos)"
chkmin "Andina: >=9 ventas" 9 "$(cuenta "$A" 'ventas?tamano=100')"
chkmin "Andina: >=8 órdenes de trabajo" 8 "$(cuenta "$A" ordenes-trabajo)"
chkmin "Andina: >=7 usuarios" 7 "$(cuenta "$A" usuarios)"
chkmin "Sierra: >=6 clientes" 6 "$(cuenta "$S" 'clientes?tamano=100')"
chkmin "Sierra: >=4 ventas" 4 "$(cuenta "$S" 'ventas?tamano=100')"
chk "Sierra: 1 sucursal" 1 "$(cuenta "$S" sucursales)"
chk "Empresa: Ecuador / USD / IVA 15" "EC USD 15.0" "$(get "$A" empresas/actual | json "d['pais'],d['moneda'],d['ivaPorcentaje']" | tr -d "(),'" )"

echo "== Aislamiento entre empresas"
CID=$(get "$A" 'clientes?buscar=Mora&tamano=5' | json "d[0]['id']")
chk "Sierra no accede a un cliente de Andina (404)" 404 "$(status "$S" GET clientes/$CID)"
chk "Sierra no accede a su historial (404)" 404 "$(status "$S" GET clientes/$CID/historial)"
chk "Misma cédula en ambas empresas = 2 clientes distintos" "1 1" "$(cuenta "$A" 'clientes?buscar=1700000011') $(cuenta "$S" 'clientes?buscar=1700000011')"
chk "Andina no ve sucursales ni usuarios de Sierra" 0 "$(get "$A" sucursales | json "len([s for s in d if s['codigo']=='AMBATO'])")"

echo "== Identidad visual por empresa"
chk "Andina tiene su color principal" "#2E6F4E" "$(get "$A" empresas/actual/marca | json "d['colorPrimario']")"
chk "Sierra tiene un color distinto" "#8A4B14" "$(get "$S" empresas/actual/marca | json "d['colorPrimario']")"
chk "Vendedor no modifica la identidad visual (403)" 403 "$(status "$VQ" PUT empresas/actual/marca)"

echo "== Permisos por rol y por sucursal"
chk "Vendedor no registra exámenes (403)" 403 "$(status "$VQ" POST examenes-visuales)"
chk "Optometrista no ve ventas (403)" 403 "$(status "$OQ" GET ventas)"
chk "Admin de empresa no lista empresas (403)" 403 "$(status "$A" GET empresas)"
chk "Vendedor de Guayaquil: solo ventas de su sucursal" 1 "$(get "$VG" 'ventas?tamano=100' | json "len(set(v['sucursalId'] for v in d))")"
chk "Admin ve las ventas de todas las sucursales" 3 "$(get "$A" 'ventas?tamano=100' | json "len(set(v['sucursalId'] for v in d))")"
if [ -n "$SUPER_PW" ]; then
  SA=$(token PLATAFORMA superadmin "$SUPER_PW")
  chk "Superadmin lista empresas (>=4)" 1 "$(get "$SA" empresas | json "1 if len(d)>=4 else 0")"
  chk "Superadmin NO accede a clientes (403)" 403 "$(status "$SA" GET clientes)"
fi

if [ "${SMOKE_WRITE:-0}" = "1" ]; then
  echo "== Flujo completo (escribe datos)"
  ID="19$(date +%s | tail -c 9)"
  CL=$(curl -s -X POST "$API/clientes" -H "Authorization: Bearer $VQ" -H "$H" -d "{\"numeroIdentificacion\":\"$ID\",\"nombres\":\"Prueba\",\"apellidos\":\"Smoke\"}" | json "d['id']")
  EX=$(curl -s -X POST "$API/examenes-visuales" -H "Authorization: Bearer $OQ" -H "$H" -d "{\"clienteId\":$CL,\"motivoConsulta\":\"smoke\"}" | json "d['id']")
  RE=$(curl -s -X POST "$API/recetas" -H "Authorization: Bearer $OQ" -H "$H" -d "{\"consultaId\":$EX,\"odEsfera\":-1}" | json "d['id']")
  OT=$(curl -s -X POST "$API/ordenes-trabajo" -H "Authorization: Bearer $VQ" -H "$H" -d "{\"recetaId\":$RE}" | json "d['id']")
  PR=$(get "$VQ" 'productos?buscar=SER-001' | json "d[0]['id']")
  VE=$(curl -s -X POST "$API/ventas" -H "Authorization: Bearer $VQ" -H "$H" -d "{\"clienteId\":$CL,\"ordenTrabajoId\":$OT,\"detalles\":[{\"productoId\":$PR,\"cantidad\":1}],\"pagos\":[]}" | json "d['estado']")
  chk "Cliente -> examen -> receta -> orden -> venta" "1 1 1 1 PENDIENTE" "$([ -n "$CL" ] && echo 1) $([ -n "$EX" ] && echo 1) $([ -n "$RE" ] && echo 1) $([ -n "$OT" ] && echo 1) $VE"
fi

echo
if [ "$FALLAS" = 0 ]; then echo "TODAS LAS VERIFICACIONES OK"; else echo "$FALLAS VERIFICACIONES FALLARON"; exit 1; fi
