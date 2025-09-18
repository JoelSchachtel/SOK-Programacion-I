using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroEntity
{
    public enum EstadoPartida
    {
        EnJuego,
        Finalizada,
        Suspendida
    }

    public enum TipoCasillero
    {
        Inicio,
        Provincia,
        Multa,
        Premio
    }

    public enum MotivoVictoria
    {
        ProvinciasCompradas,
        UnicoSaldoPositivo,
        JugadorConMayorSaldo
    }

    public enum EstadoJugador
    {
        Activo,
        Eliminado
    }
}
