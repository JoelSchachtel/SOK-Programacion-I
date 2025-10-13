# CORRECCIONES AL 12/10 - SOK-PROGRAMACION-I

## Resumen de la Sesión
- **Fecha**: 12 de octubre de 2025
- **Repositorio**: SOK-Programacion-I
- **Enfoque principal**: Evaluación de la lógica de negocio implementada según los hitos del 07/10
- **Integrantes**: No especificado en README

## Evaluación de Hitos del 07/10: "webapi y lógica version final"

### ❌ **Método para consultar turno actual**
**Estado**: ❌ **NO IMPLEMENTADO**
- **Problema**: No existe lógica de negocio implementada
- **Archivo**: `EstancieroService/PartidaService.cs` - Método incompleto
- **Corrección requerida**: Implementar lógica de consulta de turno actual

### ❌ **Método para mover el jugador y aplicar reglas automáticamente según el casillero**
**Estado**: ❌ **NO IMPLEMENTADO**
- **Problema**: No existe lógica de movimiento de jugadores ni lanzamiento de dado
- **Archivo**: `EstancieroService/PartidaService.cs` - Método incompleto
- **Corrección requerida**: Implementar lógica de movimiento y aplicación de reglas

### ❌ **Gestionar compra de propiedades**
**Estado**: ❌ **NO IMPLEMENTADO**
- **Problema**: No existe lógica de compra de propiedades
- **Archivo**: `EstancieroService/PartidaService.cs` - Método incompleto
- **Corrección requerida**: Implementar lógica de compra de propiedades

### ❌ **Calcular e imputar alquileres**
**Estado**: ❌ **NO IMPLEMENTADO**
- **Problema**: No existe lógica de alquileres
- **Archivo**: `EstancieroService/PartidaService.cs` - Método incompleto
- **Corrección requerida**: Implementar lógica de alquileres

### ❌ **Administrar multas y premios**
**Estado**: ❌ **NO IMPLEMENTADO**
- **Problema**: No existe lógica de multas y premios
- **Archivo**: `EstancieroService/PartidaService.cs` - Método incompleto
- **Corrección requerida**: Implementar lógica de multas y premios

## Análisis Detallado por Componente

### 🎯 **Entidades (EstancieroEntity)**
**Estado**: ⚠️ **IMPLEMENTACIÓN PARCIAL**

**Fortalezas identificadas**:
1. **Estructura básica**: Entidades creadas con propiedades básicas
2. **Constructores**: Constructores implementados en algunas entidades
3. **Propiedades correctas**: Algunas propiedades según el enunciado

**Problemas identificados**:
1. **Partida incompleta**: Falta propiedades esenciales como FechaInicio, FechaFin, Estado, etc.
2. **Falta DetallePartida**: No hay entidad para detalle de partida
3. **Falta Movimiento**: No hay entidad para movimientos
4. **Falta enums**: No hay enums para estados y tipos

### 🎯 **Servicio de Negocio (PartidaService.cs)**
**Estado**: ❌ **NO IMPLEMENTADO**

**Problemas identificados**:
1. **Método incompleto**: `IniciarPartidaNueva()` no retorna nada
2. **Lógica incorrecta**: No implementa la lógica de creación de partida
3. **Falta persistencia**: No guarda la partida creada
4. **Falta validaciones**: No valida cantidad de jugadores
5. **Falta lógica de juego**: No hay métodos para jugar, mover, etc.

### 🎯 **Persistencia de Datos (EstancieroData)**
**Estado**: ⚠️ **IMPLEMENTACIÓN PARCIAL**

**Fortalezas identificadas**:
1. **Clases de datos**: Clases para manejo de archivos JSON
2. **Métodos básicos**: GetAll() y WritePartida() implementados
3. **Uso de Newtonsoft.Json**: Serialización/deserialización correcta

**Problemas identificados**:
1. **Falta TableroData**: No hay implementación de tablero
2. **Falta JugadorData**: No hay implementación de jugadores
3. **Falta PartidaDetalleData**: No hay implementación de detalle de partida

### 🎯 **Request/Response**
**Estado**: ⚠️ **IMPLEMENTACIÓN PARCIAL**

**Fortalezas identificadas**:
1. **Clases creadas**: Request y Response básicos implementados
2. **Estructura correcta**: Propiedades según el enunciado

**Problemas identificados**:
1. **Request incompleto**: `CrearPartida` no tiene propiedades
2. **Falta validaciones**: No hay Data Annotations
3. **Falta LanzarDadoRequest**: No hay request para lanzar dado
4. **Falta MovimientoResponse**: No hay response para movimientos

### 🎯 **Controladores Web API**
**Estado**: ❌ **NO IMPLEMENTADO**

**Problemas identificados**:
1. **Solo WeatherForecast**: Solo tiene el controlador por defecto
2. **Falta PartidaController**: No hay controlador para partidas
3. **Falta JugadorController**: No hay controlador para jugadores

## Correcciones Requeridas

### 🔧 **Corrección 1: Completar Entidad Partida**
**Archivo**: `EstancieroEntity/Partida.cs`
**Problema**: Entidad incompleta según el enunciado
**Corrección requerida**:
```csharp
using System;
using System.Collections.Generic;

namespace EstancieroEntity
{
    public class Partida
    {
        public int NumeroPartida { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public TimeSpan? Duracion { get; set; }
        public int Estado { get; set; } // 0-EnJuego, 1-Pausada, 2-Suspendida, 3-Finalizada
        public int TurnoActual { get; set; }
        public List<ConfiguracionTurno> ConfiguracionTurnos { get; set; }
        public List<CasilleroTablero> Tablero { get; set; }
        public List<JugadorEnPartida> Jugadores { get; set; }
        public int? DniGanador { get; set; }
        public string? MotivoVictoria { get; set; }

        public Partida()
        {
            ConfiguracionTurnos = new List<ConfiguracionTurno>();
            Tablero = new List<CasilleroTablero>();
            Jugadores = new List<JugadorEnPartida>();
        }
    }

    public class ConfiguracionTurno
    {
        public int NumeroTurno { get; set; }
        public int DniJugador { get; set; }
    }
}
```

### 🔧 **Corrección 2: Crear Entidad JugadorEnPartida**
**Archivo**: `EstancieroEntity/JugadorEnPartida.cs` (crear archivo)
**Problema**: Falta entidad para jugadores en partida
**Corrección requerida**:
```csharp
using System;
using System.Collections.Generic;

namespace EstancieroEntity
{
    public class JugadorEnPartida
    {
        public int NumeroPartida { get; set; }
        public int DniJugador { get; set; }
        public int PosicionActual { get; set; }
        public double DineroDisponible { get; set; }
        public int Estado { get; set; } // 0-EnJuego, 1-Derrotado
        public List<Movimiento> HistorialMovimientos { get; set; }

        public JugadorEnPartida()
        {
            HistorialMovimientos = new List<Movimiento>();
        }
    }
}
```

### 🔧 **Corrección 3: Crear Entidad Movimiento**
**Archivo**: `EstancieroEntity/Movimiento.cs` (crear archivo)
**Problema**: Falta entidad para movimientos
**Corrección requerida**:
```csharp
using System;

namespace EstancieroEntity
{
    public class Movimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Tipo { get; set; } // 0-MovimientoDado, 1-CompraProvincia, 2-PagoAlquiler, 3-Multa, 4-Premio
        public string Descripcion { get; set; }
        public double Monto { get; set; }
        public int CasilleroOrigen { get; set; }
        public int CasilleroDestino { get; set; }
        public int? DniJugadorAfectado { get; set; }
    }
}
```

### 🔧 **Corrección 4: Implementar Lógica de Negocio Completa**
**Archivo**: `EstancieroService/PartidaService.cs`
**Problema**: Método incompleto y sin implementación
**Corrección requerida**:
```csharp
using EstancieroData;
using EstancieroEntity;
using EstancieroRequest;
using EstancieroResponse;
using Newtonsoft.Json;

namespace EstancieroService
{
    public class PartidaService
    {
        private readonly PartidaData _partidaData;
        private readonly PartidaDetalleData _partidaDetalleData;
        private readonly JugadorData _jugadorData;
        private readonly TableroData _tableroData;

        public PartidaService()
        {
            _partidaData = new PartidaData();
            _partidaDetalleData = new PartidaDetalleData();
            _jugadorData = new JugadorData();
            _tableroData = new TableroData();
        }

        public ApiResponse<PartidaResponse> CrearPartida(CrearPartida request)
        {
            var response = new ApiResponse<PartidaResponse>();

            // Validar cantidad de jugadores
            if (request.DNIs.Count < 2 || request.DNIs.Count > 4)
            {
                response.Success = false;
                response.Message = "La partida debe tener entre 2 y 4 jugadores";
                return response;
            }

            // Validar que todos los jugadores existan
            foreach (var dni in request.DNIs)
            {
                var jugador = _jugadorData.GetAll().FirstOrDefault(j => j.DNI == dni);
                if (jugador == null)
                {
                    response.Success = false;
                    response.Message = $"El jugador con DNI {dni} no existe";
                    return response;
                }
            }

            // Crear partida
            var partida = new Partida
            {
                NumeroPartida = GenerarNumeroPartida(),
                FechaInicio = DateTime.Now,
                Estado = 0, // EnJuego
                TurnoActual = 1,
                ConfiguracionTurnos = new List<ConfiguracionTurno>(),
                Tablero = CargarTablero(),
                Jugadores = new List<JugadorEnPartida>()
            };

            // Configurar turnos
            for (int i = 0; i < request.DNIs.Count; i++)
            {
                partida.ConfiguracionTurnos.Add(new ConfiguracionTurno
                {
                    NumeroTurno = i + 1,
                    DniJugador = int.Parse(request.DNIs[i])
                });
            }

            // Crear jugadores en partida
            foreach (var dni in request.DNIs)
            {
                partida.Jugadores.Add(new JugadorEnPartida
                {
                    NumeroPartida = partida.NumeroPartida,
                    DniJugador = int.Parse(dni),
                    PosicionActual = 0,
                    DineroDisponible = 5000000,
                    Estado = 0, // EnJuego
                    HistorialMovimientos = new List<Movimiento>()
                });
            }

            // Guardar partida
            _partidaData.WritePartida(partida);

            response.Success = true;
            response.Message = "Partida creada exitosamente";
            response.Data = MapearPartida(partida);

            return response;
        }

        public ApiResponse<LanzarDadoResponse> LanzarDado(LanzarDado request)
        {
            var response = new ApiResponse<LanzarDadoResponse>();

            var partida = _partidaData.GetAll().FirstOrDefault(p => p.NumeroPartida == request.NumeroPartida);
            if (partida == null)
            {
                response.Success = false;
                response.Message = "Partida no encontrada";
                return response;
            }

            var jugador = partida.Jugadores.FirstOrDefault(j => j.DniJugador == request.DniJugador);
            if (jugador == null)
            {
                response.Success = false;
                response.Message = "Jugador no encontrado en la partida";
                return response;
            }

            // Lanzar dado
            Random random = new Random();
            int valorDado = random.Next(1, 7);

            // Mover jugador
            int nuevaPosicion = jugador.PosicionActual + valorDado;
            if (nuevaPosicion > partida.Tablero.Count)
            {
                nuevaPosicion = nuevaPosicion - partida.Tablero.Count;
                jugador.DineroDisponible += 100000; // Bonificación por pasar inicio
            }

            jugador.PosicionActual = nuevaPosicion;

            // Aplicar reglas del casillero
            var casillero = partida.Tablero.FirstOrDefault(c => c.NroCasillero == nuevaPosicion);
            if (casillero != null)
            {
                AplicarReglasCasillero(partida, jugador, casillero);
            }

            // Guardar cambios
            _partidaData.WritePartida(partida);

            response.Success = true;
            response.Message = "Jugador movido exitosamente";
            response.Data = new LanzarDadoResponse
            {
                DniJugador = request.DniJugador,
                ValorDado = valorDado,
                PosicionNueva = nuevaPosicion,
                DineroDisponible = jugador.DineroDisponible
            };

            return response;
        }

        private void AplicarReglasCasillero(Partida partida, JugadorEnPartida jugador, CasilleroTablero casillero)
        {
            switch (casillero.TipoCasillero)
            {
                case 1: // Provincia
                    if (casillero.DniPropietario == null)
                    {
                        // Lógica para compra de provincia
                        if (jugador.DineroDisponible >= (double)casillero.PrecioCompra)
                        {
                            jugador.DineroDisponible -= (double)casillero.PrecioCompra;
                            casillero.DniPropietario = jugador.DniJugador.ToString();
                        }
                    }
                    else if (casillero.DniPropietario != jugador.DniJugador.ToString())
                    {
                        // Lógica para pago de alquiler
                        double alquiler = (double)casillero.PrecioAlquiler;
                        if (jugador.DineroDisponible >= alquiler)
                        {
                            jugador.DineroDisponible -= alquiler;
                            var propietario = partida.Jugadores.FirstOrDefault(j => j.DniJugador.ToString() == casillero.DniPropietario);
                            if (propietario != null)
                            {
                                propietario.DineroDisponible += alquiler;
                            }
                        }
                    }
                    break;
                case 2: // Multa
                    jugador.DineroDisponible -= (double)casillero.MontoSancion;
                    break;
                case 3: // Premio
                    jugador.DineroDisponible += (double)casillero.MontoSancion;
                    break;
            }
        }

        private int GenerarNumeroPartida()
        {
            var partidas = _partidaData.GetAll();
            return partidas.Count == 0 ? 1 : partidas.Max(p => p.NumeroPartida) + 1;
        }

        private List<CasilleroTablero> CargarTablero()
        {
            // Cargar tablero desde archivo JSON
            return new List<CasilleroTablero>();
        }

        private PartidaResponse MapearPartida(Partida partida)
        {
            return new PartidaResponse
            {
                NroPartida = partida.NumeroPartida,
                Estado = (EstadoPartida)partida.Estado,
                TurnoActual = partida.TurnoActual,
                DniJugadorTurno = partida.ConfiguracionTurnos.FirstOrDefault(t => t.NumeroTurno == partida.TurnoActual)?.DniJugador.ToString(),
                DniGanador = partida.DniGanador?.ToString(),
                MotivoVictoria = partida.MotivoVictoria,
                Jugadores = partida.Jugadores.Select(j => new JugadorEnPartidaResponse
                {
                    DniJugador = j.DniJugador,
                    PosicionActual = j.PosicionActual,
                    DineroDisponible = j.DineroDisponible,
                    Estado = j.Estado
                }).ToList()
            };
        }
    }
}
```

### 🔧 **Corrección 5: Implementar Controladores Web API**
**Archivo**: `EstancieroWebApi/Controllers/PartidaController.cs` (crear archivo)
**Problema**: No hay controladores implementados
**Corrección requerida**:
```csharp
using Microsoft.AspNetCore.Mvc;
using EstancieroService;
using EstancieroRequest;
using EstancieroResponse;

namespace EstancieroWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly PartidaService partidaService = new PartidaService();

        [HttpPost("crear")]
        public IActionResult CrearPartida([FromBody] CrearPartida request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = partidaService.CrearPartida(request);
            
            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }

        [HttpPost("lanzarDado")]
        public IActionResult LanzarDado([FromBody] LanzarDado request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = partidaService.LanzarDado(request);
            
            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }

        [HttpGet("turno/{numeroPartida}")]
        public IActionResult ConsultarTurnoActual(int numeroPartida)
        {
            try
            {
                var partida = partidaService.ObtenerPartida(numeroPartida);
                if (partida == null)
                {
                    return NotFound(new { message = "Partida no encontrada" });
                }

                return Ok(new 
                { 
                    TurnoActual = partida.TurnoActual,
                    EstadoPartida = partida.Estado,
                    DniJugadorTurno = partida.ConfiguracionTurnos.FirstOrDefault(t => t.NumeroTurno == partida.TurnoActual)?.DniJugador
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
```

## Evaluación General del Repositorio

### ✅ **Aspectos Positivos**
1. **Estructura de proyectos**: Arquitectura de 6 proyectos presente
2. **Entidades básicas**: Algunas entidades creadas
3. **Request/Response**: DTOs básicos implementados
4. **Persistencia básica**: Métodos para archivos JSON

### ❌ **Aspectos Críticos**
1. **Lógica de negocio ausente**: No hay implementación de funcionalidades
2. **Entidades incompletas**: Falta propiedades esenciales
3. **Controladores ausentes**: No hay endpoints de API
4. **Falta implementación**: Métodos incompletos o vacíos

### 📊 **Grado de Avance: 25%**

**Desglose por hitos del 07/10**:
- ❌ **Método para consultar turno actual**: 0% - No implementado
- ❌ **Método para mover el jugador y aplicar reglas**: 0% - No implementado
- ❌ **Gestionar compra de propiedades**: 0% - No implementado
- ❌ **Calcular e imputar alquileres**: 0% - No implementado
- ❌ **Administrar multas y premios**: 0% - No implementado

## Conceptos Teóricos Aplicados

### 🎯 **Unidad 1 - POO**
- ⚠️ **Encapsulación**: Parcialmente implementado
- ⚠️ **Herencia**: No implementado
- ❌ **Polimorfismo**: No implementado

### 🎯 **Unidad 5 - REST API**
- ❌ **HTTP Methods**: No implementados
- ⚠️ **Request/Response Pattern**: Parcialmente implementado
- ❌ **Status Codes**: No implementados
- ❌ **RESTful Design**: No implementado

### 🎯 **Unidad 4 - Persistencia**
- ⚠️ **Archivos JSON**: Parcialmente implementado
- ⚠️ **Newtonsoft.Json**: Parcialmente implementado
- ⚠️ **Gestión de archivos**: Parcialmente implementado

## Recomendaciones Finales

### 🚀 **Para Completar al 100%**
1. **Implementar toda la lógica de negocio** - Prioridad crítica
2. **Completar entidades faltantes** - Prioridad crítica
3. **Crear controladores Web API** - Prioridad crítica
4. **Implementar persistencia completa** - Prioridad alta
5. **Agregar validaciones** - Prioridad media

### 🎯 **Fortalezas del Repositorio**
- **Estructura de proyectos correcta**
- **Algunas entidades implementadas**
- **Request/Response básicos creados**

## Conclusión

El repositorio **SOK-Programacion-I** presenta una **estructura básica correcta** con algunos componentes implementados, pero **carece de la implementación de la lógica de negocio, entidades completas y controladores**. Es necesario implementar toda la funcionalidad desde cero.

**Calificación general: 25% - ESTRUCTURA BÁSICA CON IMPLEMENTACIÓN INCOMPLETA** 🔧

**Próximo paso crítico**: Implementar toda la lógica de negocio, completar entidades y crear controladores Web API.
