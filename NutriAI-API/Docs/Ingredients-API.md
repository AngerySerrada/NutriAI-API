# ?? API de Ingredientes - Documentación

## ?? Resumen de Endpoints

### **Ingredientes Generales**
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/ingredientes` | Obtener todos los ingredientes |
| `GET` | `/api/ingredientes/{id}` | Obtener ingrediente por ID |
| `GET` | `/api/ingredientes/buscar?nombre={nombre}` | Buscar ingredientes por nombre |
| `GET` | `/api/ingredientes/categoria/{categoria}` | Obtener ingredientes por categoría |
| `GET` | `/api/ingredientes/categorias` | Obtener todas las categorías |

### **?? Asociaciones Usuario-Ingrediente**
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/ingredientes/asociar` | Asociar ingrediente a usuario |
| `DELETE` | `/api/ingredientes/desasociar` | Desasociar ingrediente de usuario |
| `GET` | `/api/ingredientes/usuario/{idUsuario}` | Obtener ingredientes de un usuario |
| `GET` | `/api/ingredientes/verificar-asociacion` | Verificar si ingrediente está asociado |
| `PUT` | `/api/ingredientes/actualizar-cantidad` | Actualizar cantidad de ingrediente |
| `GET` | `/api/ingredientes/asociacion` | Obtener asociación específica |
| `DELETE` | `/api/ingredientes/eliminar-asociacion/{id}` | Eliminar asociación por ID |

---

## ?? **Endpoints de Ingredientes Generales**

### 1. **Obtener Todos los Ingredientes**
```http
GET /api/ingredientes
```

**Respuesta exitosa:**
```json
[
  {
    "idIngrediente": 1,
    "nombre": "Manzana",
    "categoria": "Frutas",
    "calorias": 52.00,
    "proteinas": 0.30,
    "carbohidratos": 14.00,
    "grasas": 0.20
  }
]
```

### 2. **Obtener Ingrediente por ID**
```http
GET /api/ingredientes/1
```

### 3. **Buscar Ingredientes por Nombre**
```http
GET /api/ingredientes/buscar?nombre=man
```

### 4. **Obtener Ingredientes por Categoría**
```http
GET /api/ingredientes/categoria/Frutas
```

### 5. **Obtener Todas las Categorías**
```http
GET /api/ingredientes/categorias
```

---

## ?? **Endpoints de Asociaciones Usuario-Ingrediente**

### 1. **Asociar Ingrediente a Usuario**
```http
POST /api/ingredientes/asociar
Content-Type: application/json

{
  "idUsuario": 1,
  "idIngrediente": 5,
  "cantidadGramos": 100.50
}
```

**Respuesta exitosa:**
```json
{
  "message": "Ingrediente asociado exitosamente.",
  "data": {
    "idUsuarioIngrediente": 15,
    "idUsuario": 1,
    "idIngrediente": 5,
    "cantidadGramos": 100.50,
    "fechaRegistro": "2024-01-15T10:30:00Z",
    "ingrediente": {
      "idIngrediente": 5,
      "nombre": "Arroz",
      "categoria": "Cereales",
      "calorias": 130.00,
      "proteinas": 2.70,
      "carbohidratos": 28.00,
      "grasas": 0.30
    }
  }
}
```

### 2. **Desasociar Ingrediente de Usuario**
```http
DELETE /api/ingredientes/desasociar?idUsuario=1&idIngrediente=5
```

**Respuesta exitosa:**
```json
{
  "message": "Ingrediente desasociado exitosamente."
}
```

### 3. **Obtener Ingredientes de un Usuario**
```http
GET /api/ingredientes/usuario/1
```

**Respuesta exitosa:**
```json
[
  {
    "idUsuarioIngrediente": 15,
    "idUsuario": 1,
    "idIngrediente": 5,
    "cantidadGramos": 100.50,
    "fechaRegistro": "2024-01-15T10:30:00Z",
    "ingrediente": {
      "idIngrediente": 5,
      "nombre": "Arroz",
      "categoria": "Cereales",
      "calorias": 130.00,
      "proteinas": 2.70,
      "carbohidratos": 28.00,
      "grasas": 0.30
    }
  }
]
```

### 4. **Verificar Asociación Usuario-Ingrediente**
```http
GET /api/ingredientes/verificar-asociacion?idUsuario=1&idIngrediente=5
```

**Respuesta exitosa:**
```json
{
  "estaAsociado": true
}
```

### 5. **Actualizar Cantidad de Ingrediente**
```http
PUT /api/ingredientes/actualizar-cantidad
Content-Type: application/json

{
  "idUsuarioIngrediente": 15,
  "cantidadGramos": 150.75
}
```

**Respuesta exitosa:**
```json
{
  "message": "Cantidad actualizada exitosamente."
}
```

### 6. **Obtener Asociación Específica**
```http
GET /api/ingredientes/asociacion?idUsuario=1&idIngrediente=5
```

### 7. **Eliminar Asociación por ID**
```http
DELETE /api/ingredientes/eliminar-asociacion/15
```

---

## ?? **Validaciones Implementadas**

### **Asociaciones Usuario-Ingrediente:**
- ? IDs de usuario e ingrediente deben ser > 0
- ? Usuario debe existir y estar activo
- ? Ingrediente debe existir
- ? Cantidad debe estar entre 0.01 y 9999.99 gramos
- ? Prevención de asociaciones duplicadas (actualiza existente)
- ? Validación de modelos con Data Annotations

---

## ??? **Características Técnicas**

### **Gestión de Asociaciones:**
1. **Prevención de Duplicados**: Si ya existe asociación, se actualiza
2. **Información Completa**: Incluye datos del ingrediente en respuestas
3. **Timestamps Automáticos**: Fecha de registro se actualiza automáticamente
4. **Validaciones Robustas**: Verificación de existencia de entidades
5. **Transacciones Seguras**: Manejo consistente de la base de datos

### **Performance Optimizada:**
- AsNoTracking() para consultas de solo lectura
- Proyecciones directas a DTOs
- Includes eficientes para datos relacionados
- Ordenamiento en base de datos

---

## ?? **Para el Frontend**

### **Casos de Uso Comunes:**

```javascript
// 1. Asociar ingrediente favorito del usuario
const asociar = await fetch('/api/ingredientes/asociar', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    idUsuario: 1,
    idIngrediente: 5,
    cantidadGramos: 100
  })
});

// 2. Obtener ingredientes del usuario para su despensa
const misDespensa = await fetch('/api/ingredientes/usuario/1');

// 3. Verificar si ingrediente ya está en despensa
const verificar = await fetch('/api/ingredientes/verificar-asociacion?idUsuario=1&idIngrediente=5');

// 4. Actualizar cantidad en despensa
const actualizar = await fetch('/api/ingredientes/actualizar-cantidad', {
  method: 'PUT',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    idUsuarioIngrediente: 15,
    cantidadGramos: 200
  })
});

// 5. Remover ingrediente de despensa
const remover = await fetch('/api/ingredientes/desasociar?idUsuario=1&idIngrediente=5', {
  method: 'DELETE'
});
```

### **Componentes Sugeridos:**
1. **Despensa Virtual**: Lista de ingredientes del usuario
2. **Botón Agregar/Quitar**: Toggle para associar/desasociar
3. **Input Cantidad**: Para especificar gramos disponibles
4. **Tarjeta de Ingrediente Favorito**: Con info nutricional
5. **Lista de Ingredientes Disponibles**: Con indicador de asociación

---

## ?? **Flujos de Trabajo**

### **1. Gestión de Despensa Virtual**
```
Usuario ? Buscar Ingrediente ? Agregar a Despensa ? Especificar Cantidad
```

### **2. Actualización de Inventario**
```
Despensa ? Seleccionar Ingrediente ? Actualizar Cantidad ? Guardar
```

### **3. Planificación de Comidas**
```
Receta ? Verificar Ingredientes Disponibles ? Mostrar Faltantes
```

---

## ?? **Estados de Asociación**

1. **No Asociado**: Ingrediente no está en despensa del usuario
2. **Asociado Sin Cantidad**: Ingrediente marcado como disponible
3. **Asociado Con Cantidad**: Ingrediente con cantidad específica
4. **Actualizado**: Cantidad modificada recientemente

---

## ?? **Métricas y Análisis**

La API permite recopilar datos valiosos:
- Ingredientes más populares entre usuarios
- Patrones de consumo por categorías
- Tendencias estacionales de ingredientes
- Análisis nutricional de despensas

---

## ?? **Notas de Implementación**

- ?? **Auto-actualización**: Fecha de registro se actualiza automáticamente
- ??? **Prevención de Duplicados**: Manejo inteligente de asociaciones existentes
- ? **Performance**: Consultas optimizadas con includes selectivos
- ?? **Flexibilidad**: Múltiples formas de gestionar asociaciones
- ?? **UX Friendly**: Respuestas completas con información contextual