# CORRECCIÓN AL 23-09

## Resumen de la Sesión
- **Fecha**: 23 de septiembre de 2025
- **Repositorios analizados**: SOK-Programacion-I
- **Enfoque principal**: Revisión de modelado de datos, estructura de proyectos y lógica de negocio básica

## Correcciones Realizadas

### Repositorio: SOK-Programacion-I

#### ARCHIVO NO ENCONTRADO: EstancieroData/
**Problema identificado**: No existen entidades implementadas
**Corrección aplicada**: Crear todas las entidades necesarias
**Archivos a crear**:
- Usuario.cs
- Partida.cs
- CasilleroTablero.cs
- PartidaDetalle.cs
- Movimiento.cs
- Enums.cs

**Explicación técnica**: Según el enunciado, se requieren todas las entidades para el modelado de datos del juego.

---

#### ARCHIVO NO ENCONTRADO: EstancieroRequests/
**Problema identificado**: No existen clases Request
**Corrección aplicada**: Crear proyecto de Requests
**Archivos a crear**:
- CrearPartidaRequest.cs
- LanzarDadoRequest.cs
- ActualizarEstadoPartidaRequest.cs

**Explicación técnica**: Según la arquitectura de referencia, se requieren clases Request para encapsular los datos de entrada de la API.

---

#### ARCHIVO NO ENCONTRADO: EstancieroResponses/
**Problema identificado**: No existen clases Response
**Corrección aplicada**: Crear proyecto de Responses
**Archivos a crear**:
- ApiResponse.cs
- PartidaResponse.cs
- LanzarDadoResponse.cs

**Explicación técnica**: Según la arquitectura de referencia, se requieren clases Response para estandarizar las respuestas de la API.

---

#### ARCHIVO NO ENCONTRADO: EstancieroService/
**Problema identificado**: No existe lógica de negocio
**Corrección aplicada**: Crear proyecto de servicios
**Archivos a crear**:
- PartidaService.cs
- JugadorService.cs

**Explicación técnica**: Según el enunciado, se requiere la lógica de negocio para manejar las operaciones del juego.

---

#### ARCHIVO NO ENCONTRADO: EstancieroWebAppi/
**Problema identificado**: No existe proyecto Web API
**Corrección aplicada**: Crear proyecto Web API con controladores
**Archivos a crear**:
- Program.cs
- Controllers/PartidaController.cs
- Controllers/JugadorController.cs

**Explicación técnica**: Según el enunciado, se requiere una API REST en C# (.NET Core/7/8) como backend.

---

## Recomendaciones Generales
- Implementar la arquitectura completa de 6 proyectos según la referencia
- Crear todas las entidades necesarias según el enunciado
- Implementar la lógica de negocio completa
- Crear archivos JSON de configuración del tablero
- Implementar todos los endpoints de la API según el enunciado

## Conceptos Teóricos Aplicados
- **Unidad 1**: POO - Encapsulación, herencia y polimorfismo en el modelado de entidades
- **Unidad 4**: Manejo de archivos JSON para persistencia de datos
- **Unidad 5**: Arquitectura REST con separación de responsabilidades en capas
- **Unidad 5**: Request/Response pattern para API REST

## Próximos Pasos Sugeridos
- Crear todos los proyectos faltantes
- Implementar todas las entidades según el enunciado
- Desarrollar la lógica de negocio completa
- Implementar la persistencia en archivos JSON
- Desarrollar el frontend HTML/CSS/JavaScript

## Resumen de Avance
**Lo que está bien:**
- Estructura básica de proyectos creada
- README con información del equipo

**Lo que falta/está mal:**
- No hay implementación de entidades
- No hay lógica de negocio
- No hay API REST
- No hay persistencia
- No hay frontend

**Grado de avance: 5%** - Solo se tiene la estructura básica de proyectos, pero falta toda la implementación según el enunciado.