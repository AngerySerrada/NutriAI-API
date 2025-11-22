# ?? Guía Rápida de Prueba - Endpoints de PDFs

## ? Inicio Rápido (5 minutos)

### 1?? Iniciar la Aplicación
```bash
# En la raíz del proyecto NutriAI-API
dotnet run
```

### 2?? Abrir Swagger UI
```
Navegador ? https://localhost:44344/
```

### 3?? Autenticarse (Si es necesario)

**Opción A: Login con usuario existente**
```
1. Expandir POST /api/auth/login
2. Try it out
3. Body:
{
  "login": "tu_usuario",
  "password": "tu_password"
}
4. Execute
5. Copiar el "token" de la respuesta
```

**Opción B: Registrar nuevo usuario**
```
1. Expandir POST /api/auth/register
2. Try it out
3. Completar datos
4. Execute
5. Usar credenciales para login
```

**Configurar token en Swagger:**
```
1. Click botón "Authorize" (candado) arriba a la derecha
2. Value: Bearer {tu_token_aqui}
3. Click "Authorize"
4. Click "Close"
```

---

## ?? Probar POST /api/pdfs (Subir PDF)

### Paso a Paso
```
1. Expandir POST /api/pdfs
2. Click "Try it out"
3. Completar campos:
   
   IdUsuario: 1
   FileName: prueba_conversacion.pdf
   Title: Mi Primera Conversación
   Description: Esta es una conversación de prueba
   IdConversation: (dejar vacío o poner ID válido)
   PdfFile: Click "Choose File" ? Seleccionar cualquier PDF
   
4. Click "Execute"
5. Ver respuesta:
```

**Respuesta Esperada (200 OK):**
```json
{
  "success": true,
  "message": "PDF guardado exitosamente",
  "idPdfDocument": 1
}
```

**Nota**: Guarda el `idPdfDocument` para las siguientes pruebas.

---

## ?? Probar GET /api/pdfs/usuario/{userId} (Listar PDFs)

### Paso a Paso
```
1. Expandir GET /api/pdfs/usuario/{userId}
2. Click "Try it out"
3. Completar:
   
   userId: 1
   pageNumber: 1
   pageSize: 10
   
4. Click "Execute"
```

**Respuesta Esperada (200 OK):**
```json
[
  {
    "idPdfDocument": 1,
    "idUsuario": 1,
    "fileName": "prueba_conversacion.pdf",
    "title": "Mi Primera Conversación",
    "description": "Esta es una conversación de prueba",
    "fileSize": 245760,
    "fechaCreacion": "2024-11-22T14:30:25Z",
    "contentType": "application/pdf",
    "idConversation": null,
    "tamanoFormateado": "240.00 KB"
  }
]
```

---

## ?? Probar GET /api/pdfs/{pdfId} (Información del PDF)

### Paso a Paso
```
1. Expandir GET /api/pdfs/{pdfId}
2. Click "Try it out"
3. pdfId: 1 (usar el ID del PDF que guardaste)
4. Click "Execute"
```

**Respuesta Esperada (200 OK):**
```json
{
  "idPdfDocument": 1,
  "idUsuario": 1,
  "fileName": "prueba_conversacion.pdf",
  "title": "Mi Primera Conversación",
  "description": "Esta es una conversación de prueba",
  "fileSize": 245760,
  "fechaCreacion": "2024-11-22T14:30:25Z",
  "contentType": "application/pdf",
  "idConversation": null,
  "tamanoFormateado": "240.00 KB"
}
```

---

## ?? Probar GET /api/pdfs/{pdfId}/download (Descargar PDF)

### Paso a Paso
```
1. Expandir GET /api/pdfs/{pdfId}/download
2. Click "Try it out"
3. pdfId: 1
4. Click "Execute"
5. Ver botón "Download file"
6. Click para descargar el PDF
```

**Resultado Esperado:**
- Archivo PDF se descarga correctamente
- Nombre del archivo coincide con el guardado
- Contenido del PDF es el mismo que subiste

---

## ??? Probar DELETE /api/pdfs/{pdfId} (Eliminar PDF)

### Paso a Paso
```
1. Expandir DELETE /api/pdfs/{pdfId}
2. Click "Try it out"
3. pdfId: 1
4. Click "Execute"
```

**Respuesta Esperada (200 OK):**
```json
{
  "message": "PDF eliminado correctamente"
}
```

**Verificación:**
```
Intentar GET /api/pdfs/1 nuevamente
Debería retornar 404 Not Found
```

---

## ?? Probar GET /api/pdfs/conversacion/{conversacionId}

### Requisito Previo
Primero necesitas crear una conversación:

```
1. Expandir POST /api/conversaciones (o POST /api/ai/conversaciones/nueva)
2. Try it out
3. Body:
{
  "idUsuario": 1,
  "titulo": "Consulta Nutricional"
}
4. Execute
5. Copiar el idConversacion de la respuesta (ejemplo: 5)
```

### Subir PDF asociado a conversación
```
1. POST /api/pdfs
2. IdConversation: 5 (usar el ID copiado)
3. Completar otros campos y subir PDF
4. Execute
```

### Listar PDFs de la conversación
```
1. Expandir GET /api/pdfs/conversacion/{conversacionId}
2. Click "Try it out"
3. conversacionId: 5
4. Click "Execute"
```

**Respuesta Esperada:**
Lista de PDFs asociados a esa conversación.

---

## ? Checklist de Pruebas

### Casos Positivos
- [ ] ? Subir PDF válido (< 10MB)
- [ ] ? Listar PDFs con paginación
- [ ] ? Obtener información de un PDF
- [ ] ? Descargar PDF
- [ ] ? Eliminar PDF
- [ ] ? Listar PDFs por conversación

### Casos Negativos (Validaciones)
- [ ] ? Subir archivo que no es PDF
  - Esperado: Error 400 "El archivo debe ser de tipo PDF"
  
- [ ] ? Subir PDF > 10MB
  - Esperado: Error 400 "El archivo excede el tamaño máximo"
  
- [ ] ? Subir sin archivo
  - Esperado: Error 400 "El archivo PDF es requerido"
  
- [ ] ? Campos requeridos vacíos
  - Esperado: Error 400 con detalles de validación
  
- [ ] ? Usuario no existe
  - Esperado: Error 400 "El usuario no existe o está inactivo"
  
- [ ] ? Conversación no existe
  - Esperado: Error 400 "La conversación especificada no existe"
  
- [ ] ? PDF no existe
  - Esperado: Error 404 "PDF no encontrado"

---

## ?? Problemas Comunes y Soluciones

### Problema: 401 Unauthorized
```
Solución:
1. Verificar que has hecho login
2. Copiar el token completo
3. En Authorize poner: Bearer {token}
4. Token no debe estar expirado
```

### Problema: 400 Bad Request - "Usuario no existe"
```
Solución:
1. Verificar que el IdUsuario existe en la BD
2. Usar el ID de un usuario válido y activo
3. Verificar en: GET /api/usuarios
```

### Problema: Error al subir archivo
```
Solución:
1. Verificar que el archivo sea PDF real
2. Verificar tamaño < 10MB
3. Verificar todos los campos requeridos estén completos
```

### Problema: Swagger no muestra el endpoint
```
Solución:
1. Verificar compilación exitosa
2. Reiniciar la aplicación
3. Limpiar caché del navegador (Ctrl+Shift+R)
4. Verificar que FileUploadOperationFilter está registrado
```

---

## ?? Datos de Prueba Sugeridos

### Usuario de Prueba
```json
{
  "login": "test_user",
  "email": "test@nutriai.com",
  "password": "Test123!",
  "nombre": "Usuario",
  "apellido": "Prueba"
}
```

### Conversación de Prueba
```json
{
  "idUsuario": 1,
  "titulo": "Consulta sobre Dieta Mediterránea",
  "mensajeInicial": "Hola, necesito información sobre dieta mediterránea"
}
```

### PDF de Prueba
```
FileName: Consulta_Dieta_Mediterranea_20241122.pdf
Title: Consulta Dieta Mediterránea - 22/11/2024
Description: Conversación sobre beneficios y recetas de la dieta mediterránea
```

---

## ?? Escenarios de Prueba Completos

### Escenario 1: Flujo Completo (Happy Path)
```
1. Registrar usuario nuevo
2. Login con ese usuario
3. Crear conversación
4. Subir PDF asociado a la conversación
5. Listar PDFs del usuario
6. Ver información del PDF
7. Descargar el PDF
8. Eliminar el PDF
9. Verificar que ya no existe
```

### Escenario 2: Múltiples PDFs
```
1. Subir 3 PDFs diferentes
2. Listar con pageSize=2
3. Verificar paginación (2 items)
4. Cambiar pageNumber=2
5. Verificar siguiente página (1 item)
```

### Escenario 3: PDFs por Conversación
```
1. Crear Conversación A
2. Crear Conversación B
3. Subir 2 PDFs a Conversación A
4. Subir 1 PDF a Conversación B
5. Listar PDFs de Conversación A (debe retornar 2)
6. Listar PDFs de Conversación B (debe retornar 1)
```

---

## ?? Notas de Rendimiento

### Tiempos Esperados
- POST /api/pdfs (1MB): ~500ms
- GET /api/pdfs/usuario/{id}: <100ms
- GET /api/pdfs/{id}: <50ms
- GET /api/pdfs/{id}/download (1MB): ~200ms
- DELETE /api/pdfs/{id}: <100ms

### Recomendaciones
- No subir PDFs > 5MB en desarrollo
- Usar paginación siempre que sea posible
- Comprimir PDFs antes de subir (opcional)

---

## ? Pruebas Completadas

```
Fecha: _________________
Probado por: _________________

[ ] POST /api/pdfs - PASS
[ ] GET /api/pdfs/usuario/{userId} - PASS
[ ] GET /api/pdfs/{pdfId} - PASS
[ ] GET /api/pdfs/{pdfId}/download - PASS
[ ] DELETE /api/pdfs/{pdfId} - PASS
[ ] GET /api/pdfs/conversacion/{id} - PASS

[ ] Validaciones funcionan correctamente
[ ] Errores se manejan apropiadamente
[ ] Autenticación requerida en todos los endpoints
[ ] Documentación Swagger es clara y precisa

Estado: ? APROBADO / ? REQUIERE AJUSTES
```

---

## ?? Enlaces Útiles

- **Swagger UI**: https://localhost:44344/
- **Documentación**: Ver `Controllers/PDF/README_PDFs.md`
- **Solución de Errores**: Ver `SOLUCION_SWAGGER_ERROR.md`
- **Resumen Completo**: Ver `IMPLEMENTACION_COMPLETA.md`

---

**Happy Testing! ??**
