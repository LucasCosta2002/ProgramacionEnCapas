using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;
using Entidades;

namespace Negocios
{
    public class NegTurnos
    {
        DatosTurnos objDatosTurnos = new DatosTurnos();

        public int abmTurnos(string accion, Turno objTurno)
        {
            return objDatosTurnos.abmTurnos(accion, objTurno);
        }

        public DataSet listadoTurnos(string cual)
        {
            return objDatosTurnos.listadoTurnos(cual);
        }

    }
}
