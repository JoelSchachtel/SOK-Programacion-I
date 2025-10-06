using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstancieroResponse
{

    public enum EstadoPartida { 
        EnJuego, 
        Finalizada, 
        Suspendida, 
        Pausada }
    public enum EstadoJugador { 
        EnJuego, 
        Derrotado }
    public enum TipoCasillero { 
        Inicio, 
        Provincia, 
        Multa, 
        Premio }
}
