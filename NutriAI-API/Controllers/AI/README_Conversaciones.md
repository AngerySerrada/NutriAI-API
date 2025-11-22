# Controladores de Conversaciones para NutriAI

Este documento describe los controladores creados para manejar las conversaciones con IA en la aplicación NutriAI.

## Controladores Creados

### 1. ConversacionesController (`/api/conversaciones`)
Controlador dedicado exclusivamente a la gestión de conversaciones.

### 2. MensajesController (`/api/mensajes`)
Controlador dedicado exclusivamente a la gestión de mensajes dentro de las conversaciones.

### 3. AIController (`/api/ai`) - **RECOMENDADO**
Controlador unificado que combina la funcionalidad de conversaciones y mensajes, diseñado para uso práctico en aplicaciones de IA.

## Endpoints del AIController (Recomendado)

### Gestión de Conversaciones

#### POST `/api/ai/conversaciones/nueva`
Crear una nueva conversación con mensaje inicial opcional.

**Request Body:**
```json
{
  "idUsuario": 1,
  "titulo": "Consulta nutricional",
  "mensajeInicial": "¿Puedes ayudarme con una dieta?"
}
```

#### GET `/api/ai/conversaciones/{id}`
Obtener una conversación completa con todos sus mensajes.

#### GET `/api/ai/usuarios/{idUsuario}/conversaciones`
Obtener el historial completo de conversaciones de un usuario.

### Gestión de Mensajes

#### POST `/api/ai/conversaciones/{idConversacion}/mensajes`
Agregar un nuevo mensaje a una conversación existente.

**Request Body:**
```json
{
  "rol": "user",
  "contenido": "¿Cuántas calorías debería consumir?",
  "tokensUtilizados": 15
}
```

#### GET `/api/ai/conversaciones/{idConversacion}/mensajes`
Obtener todos los mensajes de una conversación.

#### POST `/api/ai/conversaciones/{idConversacion}/intercambio` - **MUY ÚTIL**
Guardar tanto el mensaje del usuario como la respuesta de la IA en una sola operación.

**Request Body:**
```json
{
  "mensajeUsuario": "¿Cuántas calorías necesito?",
  "respuestaIA": "Basándome en tu perfil, necesitas aproximadamente 2000 calorías diarias...",
  "tokensUtilizados": 45,
  "contextoIncluido": "perfil_usuario, objetivos_nutricionales"
}
```

### Administración

#### PUT `/api/ai/conversaciones/{id}/titulo`
Actualizar el título de una conversación.

**Request Body:**
```json
{
  "nuevoTitulo": "Plan de dieta personalizado"
}
```

#### DELETE `/api/ai/conversaciones/{id}`
Archivar una conversación (la marca como inactiva).

#### PUT `/api/ai/conversaciones/{id}/restaurar`
Restaurar una conversación archivada.

## Estructura de Respuestas

### ConversacionDto
```json
{
  "idConversacion": 1,
  "idUsuario": 1,
  "titulo": "Consulta nutricional",
  "fechaCreacion": "2024-01-15T10:30:00Z",
  "fechaActualizacion": "2024-01-15T11:00:00Z",
  "activa": true,
  "totalMensajes": 4,
  "primerMensaje": "¿Puedes ayudarme con una dieta?"
}
```

### MensajeConversacionDto
```json
{
  "idMensaje": 1,
  "idConversacion": 1,
  "rol": "user",
  "contenido": "¿Cuántas calorías debería consumir?",
  "fechaEnvio": "2024-01-15T10:30:00Z",
  "tokensUtilizados": 15,
  "contextoIncluido": null
}
```

### HistorialConversacionesViewModel
```json
{
  "conversaciones": [...],
  "totalConversaciones": 5,
  "totalMensajes": 23
}
```

## Roles de Mensajes

- **`user`**: Mensajes enviados por el usuario
- **`assistant`**: Respuestas generadas por la IA
- **`system`**: Mensajes del sistema (instrucciones, contexto, etc.)

## Autorización

Todos los endpoints requieren autenticación (atributo `[Authorize]`). Asegúrate de incluir el token JWT en el header:

```
Authorization: Bearer <token>
```

## Manejo de Errores

Todos los controladores manejan errores de forma consistente:

- **400 Bad Request**: Datos inválidos o falta información requerida
- **401 Unauthorized**: Token de autenticación faltante o inválido
- **404 Not Found**: Conversación o recurso no encontrado
- **500 Internal Server Error**: Error interno del servidor

## Ejemplos de Uso

### Flujo típico para chat con IA:

1. **Crear conversación nueva:**
   ```
   POST /api/ai/conversaciones/nueva
   {
     "idUsuario": 1,
     "titulo": "Consulta nutricional",
     "mensajeInicial": "Hola, necesito ayuda con mi dieta"
   }
   ```

2. **Intercambio de mensajes:**
   ```
   POST /api/ai/conversaciones/1/intercambio
   {
     "mensajeUsuario": "¿Cuántas calorías debo consumir?",
     "respuestaIA": "Según tu perfil, recomiendo 2000 calorías diarias..."
   }
   ```

3. **Obtener historial:**
   ```
   GET /api/ai/usuarios/1/conversaciones
   ```

## Notas Importantes

- El servicio `IConversacionService` debe estar registrado en el contenedor de dependencias
- Las validaciones se realizan tanto en el nivel de atributos como en el controlador
- Se utiliza `CancellationToken.None` por simplicidad, pero se puede mejorar para manejar cancelaciones
- Los timestamps se manejan automáticamente en UTC por el servicio