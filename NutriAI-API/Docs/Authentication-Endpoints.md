# ?? Endpoints de Autenticación - Documentación

## ?? Resumen de Endpoints

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/auth/login` | Iniciar sesión |
| `POST` | `/api/auth/register` | Registrar nuevo usuario |
| `POST` | `/api/auth/refresh` | Renovar tokens |
| `POST` | `/api/auth/forgot-password` | **NUEVO** - Solicitar restablecimiento |
| `POST` | `/api/auth/reset-password` | **NUEVO** - Confirmar restablecimiento |
| `POST` | `/api/auth/change-password` | **NUEVO** - Cambiar contraseña |

---

## ?? **Nuevos Endpoints Implementados**

### 1. **Forgot Password**
```http
POST /api/auth/forgot-password
Content-Type: application/json

{
  "emailOrLogin": "usuario@email.com"
}
```

**Respuesta exitosa:**
```json
{
  "message": "Si el usuario existe, se ha enviado un enlace de restablecimiento.",
  "resetToken": "eyJhbGciOiJIUzI1NiIs..." // Solo en desarrollo
}
```

### 2. **Reset Password**
```http
POST /api/auth/reset-password
Content-Type: application/json

{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "newPassword": "nuevaContraseña123"
}
```

**Respuesta exitosa:**
```json
{
  "message": "Contraseña restablecida exitosamente."
}
```

### 3. **Change Password**
```http
POST /api/auth/change-password
Content-Type: application/json

{
  "login": "usuario123",
  "currentPassword": "contraseñaActual",
  "newPassword": "nuevaContraseña123"
}
```

**Respuesta exitosa:**
```json
{
  "message": "Contraseña cambiada exitosamente."
}
```

---

## ?? **Flujo de Restablecimiento de Contraseña**

### **Paso 1: Solicitar Reset**
1. Usuario ingresa email/username en formulario
2. Frontend llama a `POST /api/auth/forgot-password`
3. Backend genera token JWT válido por **1 hora**
4. Backend retorna mensaje (en prod enviaría email)

### **Paso 2: Confirmar Reset**
1. Usuario recibe token (actualmente en respuesta, en prod por email)
2. Usuario ingresa nueva contraseña
3. Frontend llama a `POST /api/auth/reset-password` con token
4. Backend valida token y actualiza contraseña

---

## ?? **Validaciones Implementadas**

### **Forgot Password:**
- ? EmailOrLogin requerido
- ? Manejo seguro (no revela si usuario existe)

### **Reset Password:**
- ? Token requerido y válido
- ? Nueva contraseña requerida (mín. 6 caracteres)
- ? Token debe ser tipo "reset" y no expirado

### **Change Password:**
- ? Todos los campos requeridos
- ? Nueva contraseña mín. 6 caracteres
- ? Nueva contraseña diferente a actual
- ? Contraseña actual debe coincidir

---

## ??? **Características de Seguridad**

1. **Token JWT Seguro**: Firmado con HMAC-SHA256
2. **Expiración Corta**: Tokens de reset válidos solo 1 hora
3. **No Revelación**: No se confirma si un usuario existe
4. **Hash Seguro**: PBKDF2 con 100,000 iteraciones + salt
5. **Validación Robusta**: Múltiples capas de validación

---

## ?? **Para el Frontend**

### **Flujo Completo:**
```typescript
// 1. Solicitar reset
const response = await fetch('/api/auth/forgot-password', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ emailOrLogin: 'usuario@email.com' })
});

// 2. Confirmar reset (con token recibido)
const resetResponse = await fetch('/api/auth/reset-password', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ 
    token: 'eyJhbGciOiJIUzI1NiIs...', 
    newPassword: 'nuevaPassword123' 
  })
});
```

### **Estados del Formulario:**
1. **Solicitar Reset**: Input email + botón "Enviar"
2. **Esperando Token**: Mensaje "Revisa tu email"
3. **Nueva Contraseña**: Input password + botón "Restablecer"
4. **Éxito**: Mensaje de confirmación + redirect a login

---

## ?? **Notas de Desarrollo**

- ?? **Desarrollo**: Token se retorna en respuesta para testing
- ?? **Producción**: Quitar `resetToken` de respuesta e implementar envío de email
- ?? **Números**: Todos los tokens son JWT válidos y verificables
- ? **Tiempo**: Reset tokens expiran en exactamente 1 hora