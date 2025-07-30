# Sistema de Modales - Voluntariado Conectado RD

## Descripción
Se han creado dos vistas parciales modales modernas para manejar mensajes de éxito y error en toda la aplicación:

- `_Exito.cshtml` - Modal para mensajes de éxito
- `_Error.cshtml` - Modal para mensajes de error

## Características

### Modal de Éxito (`_Exito.cshtml`)
- Diseño moderno con gradiente verde
- Icono de check animado
- Auto-cierre después de 3 segundos
- Botón "Entendido" para cerrar manualmente
- Animaciones suaves de entrada

### Modal de Error (`_Error.cshtml`)
- Diseño moderno con gradiente rojo
- Icono de advertencia animado
- Auto-cierre después de 5 segundos
- Botones "Cerrar" y "Reintentar"
- Animaciones de shake para el icono

## Uso en Controladores

### Para mostrar mensaje de éxito:
```csharp
TempData["MensajeExito"] = "¡Operación completada exitosamente!";
return RedirectToAction("ActionName", "ControllerName");
```

### Para mostrar mensaje de error:
```csharp
TempData["MensajeError"] = "Ha ocurrido un error. Por favor, inténtalo de nuevo.";
return View(model);
```

## Uso en Vistas

### Incluir en la vista:
```html
@section Scripts {
    @await Html.PartialAsync("_ValidationScriptsPartial")
    
    @if (TempData["MensajeExito"] != null)
    {
        @await Html.PartialAsync("_Exito", TempData["MensajeExito"].ToString())
        <script>
            document.addEventListener('DOMContentLoaded', function() {
                mostrarExito('@TempData["MensajeExito"]');
            });
        </script>
    }
    
    @if (TempData["MensajeError"] != null)
    {
        @await Html.PartialAsync("_Error", TempData["MensajeError"].ToString())
        <script>
            document.addEventListener('DOMContentLoaded', function() {
                mostrarError('@TempData["MensajeError"]');
            });
        </script>
    }
}
```

## Funciones JavaScript Disponibles

### Para mostrar modales programáticamente:
```javascript
// Mostrar modal de éxito
mostrarExito("Mensaje personalizado");

// Mostrar modal de error
mostrarError("Mensaje de error personalizado");

// Mostrar modal y redirigir después
mostrarExitoYRedirigir("Mensaje", "/url-destino", 2000);
mostrarErrorYRedirigir("Error", "/url-destino", 3000);
```

## Estilos CSS
Los modales utilizan Bootstrap CSS framework con estilos personalizados:
- Bordes redondeados (20px)
- Sombras elegantes
- Gradientes de color
- Animaciones CSS personalizadas
- Diseño responsive

## Implementación Actual
- ✅ AccountController actualizado
- ✅ Login.cshtml actualizado
- ✅ Registro.cshtml actualizado
- ✅ RegistroONG.cshtml actualizado
- ✅ RegistroExito.cshtml eliminado (ya no necesario)
- ✅ Animaciones CSS agregadas

## Beneficios
1. **Consistencia**: Mismo diseño en toda la aplicación
2. **UX Mejorada**: Feedback visual claro y atractivo
3. **Mantenibilidad**: Código centralizado y reutilizable
4. **Responsive**: Funciona en todos los dispositivos
5. **Accesibilidad**: Cumple con estándares de accesibilidad web 