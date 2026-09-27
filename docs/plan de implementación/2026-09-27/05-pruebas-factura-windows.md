# 05 — Pruebas graduales de factura en Windows 11

Este protocolo permite que el propietario pruebe la factura poco a poco. Las etapas W0–W4 utilizan datos y rangos sintéticos. W5 solo se habilita con perfil, autorización y aprobación contable reales.

## Reglas de seguridad

- Nunca usar un CAI o correlativo real en desarrollo, captura, soporte o una prueba repetible.
- Marcar todo documento de W0–W4 como `PRUEBA — SIN VALOR FISCAL`.
- Conservar el mismo `PrintJobId` para reintentar una impresión.
- Un error de impresora no revierte ni vuelve a emitir una factura confirmada.
- Guardar fotografía o PDF únicamente si no contiene datos personales reales.
- Registrar cada intento en [registro-pruebas-factura-windows.md](registro-pruebas-factura-windows.md).

## W0. Vista previa sin impresora

### Preparación

- Perfil y CAI sintéticos.
- Factura fixture con líneas 15 %, 18 %, exenta, exonerada, descuento y turismo.
- Nota de crédito y débito vinculadas.

### Verificación

- identidad, RTN, establecimiento y punto;
- CAI, rango, fecha límite y correlativo;
- fecha/hora hondureña;
- cliente/RTN o consumidor final;
- detalle, cantidad, precio, descuento y total por línea;
- bases, ISV 15 %, ISV 18 %, exento, exonerado, turismo y total;
- medio/condición de pago sin alterar el documento emitido;
- original/copia y tipo correcto de nota;
- contenido estable después de cambiar catálogos.

### Salida

El contador aprueba contenido y orden lógico antes de usar papel.

## W1. Conexión del agente y prueba técnica

### Preparación

- Print Agent instalado como Windows Service.
- Impresora instalada con nombre estable y agregada a la lista permitida.
- Comunicación API → agente autenticada.

### Pruebas

- enumeración de impresoras autorizadas;
- prueba de conexión;
- agente detenido, clave incorrecta y trabajo repetido;
- impresora fuera de línea y recuperación;
- usuario no autorizado recibe 403.

### Salida

La caída del agente queda visible y no crea documento/correlativo adicional.

## W2. Regla y calibración de papel

- Imprimir la regla de 100 columnas.
- Registrar ancho de papel, ancho observado, DPI, driver, modelo y puerto.
- Configurar caracteres por línea, corte, margen, logo y densidad.
- Probar tildes, ñ, símbolos `L`, `%`, `/`, `-` y números largos.
- Verificar que CAI, rango y correlativo no se corten.
- Repetir hasta obtener una plantilla estable y guardar la configuración por impresora.

## W3. Factura sintética completa

### Casos mínimos

1. consumidor final;
2. cliente con RTN;
3. una línea al 15 %;
4. una línea al 18 %;
5. mezcla 15/18/exenta/exonerada;
6. turismo cuando aplique;
7. descuento autorizado;
8. pago pendiente, efectivo, tarjeta, transferencia y mixto;
9. factura con muchas líneas;
10. valores cercanos al máximo esperado.

### Evidencia

- comparar preview, PDF y papel contra el mismo snapshot/hash;
- recalcular con oráculo independiente;
- revisar original y copia;
- fotografiar muestra sintética y anotar defectos.

## W4. Notas, reimpresión y fallos

- Nota de crédito parcial y total.
- Nota de débito.
- Reimpresión después de cambiar cliente/configuración: debe conservar snapshot.
- Papel agotado, impresora apagada y spooler detenido.
- Respuesta perdida después de confirmar factura.
- Dos clics y dos solicitudes concurrentes con la misma intención.
- Reinicio de API, agente y Windows con trabajo pendiente.
- Verificar un solo documento, un solo correlativo, un asiento y un trabajo recuperable.

## W5. Ensayo controlado con configuración real

### Requisitos previos obligatorios

- G0–G9 aprobadas.
- Perfil fiscal real aprobado por gerente/contador.
- Autorización y rango verificados contra documentación vigente.
- Copia/impresión y conservación aprobadas.
- Respaldo inmediato y restauración reciente aprobados.
- Plan de contingencia y responsables presentes.

### Ejecución

1. Registrar versión, hora, último correlativo y hash del respaldo.
2. Consultar autorización vigente sin modificar el numerador.
3. Si el contador confirma que procede una emisión real controlada, crear una única operación acordada.
4. Comparar pantalla, base, snapshot, asiento, PDF y papel.
5. Confirmar el correlativo consumido y el siguiente esperado.
6. Registrar resultado y prohibir repetir la emisión como mecanismo de prueba.
7. Ante un defecto, conservar el documento y aplicar el flujo fiscal autorizado; nunca borrarlo o reutilizar su número.

## Puerta G10

- W0–W4 aprobadas por propietario y QA.
- W5 aprobada únicamente si contador y gerente autorizaron el ensayo real.
- Preview, PDF y papel coinciden con el snapshot.
- Reintentos no duplican documentos o correlativos.
- Fallos de impresora se recuperan mediante el mismo trabajo.
- Configuración de impresora y evidencia quedan versionadas sin secretos ni datos personales.
