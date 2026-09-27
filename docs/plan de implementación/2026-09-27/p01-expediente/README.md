# Expediente P01 — instrucciones de preparación

Esta carpeta contiene una plantilla vacía. No debe completarse dentro del repositorio.

## Procedimiento

1. Copiar [perfil-hotel.template.json](perfil-hotel.template.json) a una ubicación cifrada y respaldada fuera del proyecto.
2. Resolver DEC-01–DEC-15 con gerente y contador. `not_applicable` requiere el mismo sustento que `approved`.
3. Guardar los documentos fuente fuera de Git y registrar en el expediente referencias internas y hashes SHA-256.
4. Ejecutar:

   ```bash
   node scripts/validate-hotel-profile.mjs /ruta/segura/perfil-hotel.json \
     --require-official \
     --output /ruta/segura/validacion-perfil.json
   ```

5. Revisar que N01–N06 aparezcan como `passed` y que `status` sea `passed`.
6. Gerente y contador aprueban el reporte, el hash de entrada y las fuentes externas.
7. Registrar en el ERP únicamente la referencia del expediente validado y su SHA-256. No copiar el archivo real a `docs`.

Una actualización desde una versión anterior deja el perfil fiscal en borrador aunque antes figurara aprobado. Complete este procedimiento y apruebe la nueva versión antes de intentar emitir documentos nuevos.

El modo sin `--require-official` existe solamente para pruebas sintéticas. Nunca habilita la salida productiva ni equivale a aprobación del SAR.

## Controles N01–N06

| Control | Validación automática |
|---|---|
| N01 | Identidad, RTN, contacto, establecimiento y punto de emisión completos |
| N02 | Modalidad, registro y al menos una autorización con referencia SHA-256 |
| N03 | Obligaciones con responsable, fuente, prueba y tratamiento |
| N04 | Especificación documental, hash y caso firmado |
| N05 | Impresora, papel, copias y conservación |
| N06 | DEC-01–DEC-15 resueltas, sustentadas y aprobadas por gerente y contador |
