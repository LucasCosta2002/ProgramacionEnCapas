using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using Entidades;

namespace Datos
{
    public class DatosTurnos : DatosConexionDB
    {
        public int abmTurnos(string accion, Turno objTurno)
        {
            int resultado = -1;
            string orden = string.Empty;

            if(accion == "Alta")
            {
                if(objTurno.pCobro == false){
                    orden = $"insert into Turno (cliente, DNI, celular, fecha, cobro) values ('{objTurno.pCliente}', {objTurno.pDNI}, '{objTurno.pCelular}', '{objTurno.pFecha}', {0});";
                }
            }

            if(accion == "Modificar")
            {
                orden = $"update Turno set cliente='{objTurno.pCliente}', celular='{objTurno.pCelular}', fecha='{objTurno.pFecha}', cobro={objTurno.pCobro} WHERE DNI Like '%{objTurno.pDNI}%';";
            }

            if(accion == "Borrar")
            {
                orden = $"DELETE from Turno WHERE DNI like '%{objTurno.pDNI}%';";
            }

            OleDbCommand command = new OleDbCommand(orden, conexion);
            
            try
            {
                AbrirConexion();
                resultado = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception($"Error de la accion {accion}", e);
            }
            finally
            {
                CerrarConexion();
                command.Dispose();
            }

            return resultado;
        }
    
        public DataSet listadoTurnos(string cual)
        {
            string orden = string.Empty;
            if (cual != "Todos")
                orden = "select * from Turno where Id = " + int.Parse(cual) + ";";
            else
                orden = "select * from Turno;";
            OleDbCommand cmd = new OleDbCommand(orden, conexion);
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                AbrirConexion();
                cmd.ExecuteNonQuery();
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (Exception e)
            {
                throw new Exception("Error al listar Turnos", e);
            }
            finally
            {
                CerrarConexion();
                cmd.Dispose();
            }
            return ds;
        }
    }
}
