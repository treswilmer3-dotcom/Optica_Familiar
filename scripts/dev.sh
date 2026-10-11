#!/usr/bin/env bash
# Levanta/detiene el entorno de desarrollo (API .NET + frontend Angular) dentro de WSL.
# Uso: ./scripts/dev.sh start | stop | status | logs
set -u
RAIZ="$(cd "$(dirname "$0")/.." && pwd)"
RUN="$RAIZ/.run"
API_URL="http://localhost:5130"
WEB_URL="http://localhost:4200"
mkdir -p "$RUN"

vivo() { [ -f "$RUN/$1.pid" ] && kill -0 "$(cat "$RUN/$1.pid")" 2>/dev/null; }

esperar() { # nombre url intentos
  for _ in $(seq 1 "$3"); do
    curl -s -o /dev/null "$2" && return 0
    sleep 2
  done
  return 1
}

iniciar() {
  if vivo api; then echo "API ya está en ejecución."; else
    echo "Compilando e iniciando la API..."
    ( cd "$RAIZ/backend/OpticaFamiliar.API" && ASPNETCORE_ENVIRONMENT=Development \
      setsid nohup dotnet run --urls "$API_URL" < /dev/null > "$RUN/api.log" 2>&1 & echo $! > "$RUN/api.pid" )
  fi
  if vivo web; then echo "Frontend ya está en ejecución."; else
    echo "Iniciando el frontend..."
    ( cd "$RAIZ/frontend" && [ -d node_modules ] || npm install --no-audit --no-fund
      setsid nohup npx ng serve --port 4200 < /dev/null > "$RUN/web.log" 2>&1 & echo $! > "$RUN/web.pid" )
  fi
  esperar API "$API_URL/swagger/v1/swagger.json" 60 && echo "API lista:       $API_URL/swagger" || echo "La API no respondió; revise: ./scripts/dev.sh logs"
  esperar WEB "$WEB_URL" 60 && echo "Frontend listo:  $WEB_URL" || echo "El frontend no respondió; revise: ./scripts/dev.sh logs"
}

detener() {
  for n in api web; do
    if vivo $n; then
      pid="$(cat "$RUN/$n.pid")"
      pkill -P "$pid" 2>/dev/null; kill "$pid" 2>/dev/null
      echo "Detenido: $n"
    fi
    rm -f "$RUN/$n.pid"
  done
  # procesos hijos que pudieran haber quedado
  pkill -f "[O]pticaFamiliar.API --urls" 2>/dev/null
  pkill -f "[n]g serve --port 4200" 2>/dev/null
  return 0
}

case "${1:-}" in
  start)  iniciar ;;
  stop)   detener ;;
  status) vivo api && echo "API: en ejecución ($API_URL)" || echo "API: detenida"
          vivo web && echo "Frontend: en ejecución ($WEB_URL)" || echo "Frontend: detenido" ;;
  logs)   tail -n 30 "$RUN/api.log" "$RUN/web.log" ;;
  *) echo "Uso: $0 start | stop | status | logs"; exit 1 ;;
esac
