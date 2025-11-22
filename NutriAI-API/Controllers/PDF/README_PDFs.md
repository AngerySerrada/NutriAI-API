# ?? Endpoints de PDFs de Conversaciones - Documentación

## ?? Resumen de Endpoints

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/pdfs` | Guardar nuevo PDF |
| `GET` | `/api/pdfs/usuario/{userId}` | Obtener PDFs de un usuario |
| `GET` | `/api/pdfs/{pdfId}` | Obtener información de un PDF |
| `GET` | `/api/pdfs/{pdfId}/download` | Descargar un PDF |
| `DELETE` | `/api/pdfs/{pdfId}` | Eliminar un PDF |
| `GET` | `/api/pdfs/conversacion/{conversacionId}` | Obtener PDFs de una conversación |

---

## ?? **Endpoints Implementados**

### 1. **POST /api/pdfs - Guardar PDF**

Guarda un nuevo documento PDF en el sistema.

**Request:**
```http
POST /api/pdfs
Content-Type: multipart/form-data
Authorization: Bearer {jwt_token}

FormData:
- IdUsuario: int
- FileName: string (max 255 chars)
- Title: string (max 200 chars)
- Description: string (max 500 chars, opcional)
- IdConversation: int (opcional)
- PdfFile: file (application/pdf)
```

**Respuesta exitosa (200 OK):**
```json
{
  "success": true,
  "message": "PDF guardado exitosamente",
  "idPdfDocument": 123
}
```

**Respuesta error (400 Bad Request):**
```json
{
  "success": false,
  "message": "Error de validación: [detalles]"
}
```

**Validaciones:**
- ? Usuario debe existir y estar activo
- ? Conversación debe existir (si se proporciona)
- ? Archivo debe ser PDF (application/pdf)
- ? Tamaño máximo: 10MB
- ? Todos los campos requeridos deben estar presentes

---

### 2. **GET /api/pdfs/usuario/{userId} - Obtener PDFs por Usuario**

Obtiene todos los PDFs de un usuario con paginación opcional.

**Request:**
```http
GET /api/pdfs/usuario/{userId}?pageNumber=1&pageSize=10
Authorization: Bearer {jwt_token}
```

**Parámetros Query:**
- `pageNumber` (opcional, default: 1): Número de página
- `pageSize` (opcional, default: 10, max: 100): Tamaño de página

**Respuesta exitosa (200 OK):**
```json
[
  {
    "idPdfDocument": 123,
    "idUsuario": 456,
    "fileName": "Conversacion_NutriAI_20241122_143025.pdf",
    "title": "Conversación NutriAI - 22/11/2024 14:30",
    "description": "Conversación con 5 mensajes generada el 22/11/2024 a las 14:30",
    "fileSize": 245760,
    "fechaCreacion": "2024-11-22T14:30:25Z",
    "contentType": "application/pdf",
    "idConversation": 789,
    "tamanoFormateado": "240.00 KB"
  }
]
```

---

### 3. **GET /api/pdfs/{pdfId} - Obtener Información de PDF**

Obtiene la información de un PDF específico (sin el contenido binario).

**Request:**
```http
GET /api/pdfs/{pdfId}
Authorization: Bearer {jwt_token}
```

**Respuesta exitosa (200 OK):**
```json
{
  "idPdfDocument": 123,
  "idUsuario": 456,
  "fileName": "Conversacion_NutriAI_20241122_143025.pdf",
  "title": "Conversación NutriAI - 22/11/2024 14:30",
  "description": "Conversación con 5 mensajes generada el 22/11/2024 a las 14:30",
  "fileSize": 245760,
  "fechaCreacion": "2024-11-22T14:30:25Z",
  "contentType": "application/pdf",
  "idConversation": 789,
  "tamanoFormateado": "240.00 KB"
}
```

**Respuesta error (404 Not Found):**
```json
{
  "message": "PDF no encontrado"
}
```

---

### 4. **GET /api/pdfs/{pdfId}/download - Descargar PDF**

Descarga el contenido binario del PDF.

**Request:**
```http
GET /api/pdfs/{pdfId}/download
Authorization: Bearer {jwt_token}
```

**Respuesta exitosa (200 OK):**
```
Content-Type: application/pdf
Content-Disposition: attachment; filename="archivo.pdf"

[binary PDF content]
```

**Respuesta error (404 Not Found):**
```json
{
  "message": "PDF no encontrado"
}
```

---

### 5. **DELETE /api/pdfs/{pdfId} - Eliminar PDF**

Elimina un PDF del sistema permanentemente.

**Request:**
```http
DELETE /api/pdfs/{pdfId}
Authorization: Bearer {jwt_token}
```

**Respuesta exitosa (200 OK):**
```json
{
  "message": "PDF eliminado correctamente"
}
```

**Respuesta error (404 Not Found):**
```json
{
  "message": "PDF no encontrado"
}
```

---

### 6. **GET /api/pdfs/conversacion/{conversacionId} - Obtener PDFs por Conversación**

Obtiene todos los PDFs asociados a una conversación específica.

**Request:**
```http
GET /api/pdfs/conversacion/{conversacionId}
Authorization: Bearer {jwt_token}
```

**Respuesta exitosa (200 OK):**
```json
[
  {
    "idPdfDocument": 123,
    "idUsuario": 456,
    "fileName": "Conversacion_NutriAI_20241122_143025.pdf",
    "title": "Conversación NutriAI - 22/11/2024 14:30",
    "description": "Conversación con 5 mensajes generada el 22/11/2024 a las 14:30",
    "fileSize": 245760,
    "fechaCreacion": "2024-11-22T14:30:25Z",
    "contentType": "application/pdf",
    "idConversation": 789,
    "tamanoFormateado": "240.00 KB"
  }
]
```

---

## ?? **Autenticación**

Todos los endpoints requieren autenticación mediante JWT. Incluye el token en el header:

```
Authorization: Bearer {tu_token_jwt}
```

---

## ?? **Códigos de Estado HTTP**

| Código | Descripción |
|--------|-------------|
| `200 OK` | Operación exitosa |
| `400 Bad Request` | Datos inválidos o error de validación |
| `401 Unauthorized` | Token no proporcionado o inválido |
| `404 Not Found` | Recurso no encontrado |
| `500 Internal Server Error` | Error interno del servidor |

---

## ?? **Ejemplos de Uso**

### **Ejemplo 1: Subir un PDF desde JavaScript**

```javascript
const formData = new FormData();
formData.append('IdUsuario', '123');
formData.append('FileName', 'conversacion_nutri.pdf');
formData.append('Title', 'Mi conversación con NutriAI');
formData.append('Description', 'Conversación sobre dieta mediterránea');
formData.append('IdConversation', '456');
formData.append('PdfFile', pdfFileInput.files[0]);

const response = await fetch('/api/pdfs', {
  method: 'POST',
  headers: {
    'Authorization': `Bearer ${token}`
  },
  body: formData
});

const result = await response.json();
console.log(result);
```

### **Ejemplo 2: Obtener PDFs de un usuario**

```javascript
const response = await fetch('/api/pdfs/usuario/123?pageNumber=1&pageSize=10', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});

const pdfs = await response.json();
console.log(pdfs);
```

### **Ejemplo 3: Descargar un PDF**

```javascript
const response = await fetch('/api/pdfs/123/download', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});

const blob = await response.blob();
const url = window.URL.createObjectURL(blob);
const a = document.createElement('a');
a.href = url;
a.download = 'conversacion.pdf';
a.click();
```

---

## ?? **Notas Técnicas**

### **Límites:**
- Tamaño máximo de archivo: **10 MB**
- Página máxima: **100 items por página**
- Solo archivos **application/pdf** son aceptados

### **Base de Datos:**
- Tabla: `PdfDocuments`
- Los PDFs se almacenan como **VARBINARY(MAX)** en SQL Server
- Relación con `Usuarios` (FK: IdUsuario)
- Relación opcional con `Conversaciones` (FK: IdConversation)

### **Formateo Automático:**
- La propiedad `tamanoFormateado` convierte bytes a formato legible (B, KB, MB, GB)

---

## ?? **Flujo Típico de Uso**

1. **Usuario genera conversación** en el sistema
2. **Frontend genera PDF** del contenido de la conversación
3. **Llamar a POST /api/pdfs** para guardar el PDF
4. **Sistema retorna idPdfDocument** para futuras referencias
5. **Usuario puede descargar** usando GET /api/pdfs/{id}/download
6. **Listar PDFs** del usuario con GET /api/pdfs/usuario/{id}

---

## ?? **Modelo de Datos**

### **PdfDocument Entity**
```csharp
public partial class PdfDocument
{
    public int IdPdfDocument { get; set; }
    public int IdUsuario { get; set; }
    public string FileName { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; }
    public byte[] PdfContent { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int? IdConversation { get; set; }
}
```

---

## ? **Testing**

### **Prueba con Postman:**

1. **Crear nuevo request POST**
2. **Seleccionar Body > form-data**
3. **Agregar campos:**
   - IdUsuario (Text): 1
   - FileName (Text): test.pdf
   - Title (Text): Test PDF
   - PdfFile (File): [seleccionar archivo]
4. **Agregar Authorization: Bearer {token}**
5. **Enviar request**

---

## ?? **Integración con Conversaciones**

Los PDFs pueden asociarse a conversaciones específicas mediante `IdConversation`:

```javascript
// Al guardar PDF de una conversación
formData.append('IdConversation', conversacionId);

// Luego recuperar todos los PDFs de esa conversación
const pdfs = await fetch(`/api/pdfs/conversacion/${conversacionId}`);
```

---

## ?? **Recomendaciones**

1. ? **Siempre validar** el tipo de archivo en el cliente antes de subir
2. ? **Comprimir PDFs** cuando sea posible para ahorrar espacio
3. ? **Implementar paginación** al listar muchos PDFs
4. ? **Usar nombres descriptivos** en FileName y Title
5. ? **Considerar implementar** límites de almacenamiento por usuario
6. ? **Agregar logging** para auditar descargas de PDFs

---

¿Necesitas ayuda con la implementación? Consulta la documentación de conversaciones en `Controllers/AI/README_Conversaciones.md`
