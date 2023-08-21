using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace Datos
{
    public class DatosConexionDB
    {
        public OleDbConnection conexion;
        public string stringConexion = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Users\Lucas\Documents\Turnos.accdb";

        public DatosConexionDB()
        {
            conexion = new OleDbConnection(stringConexion);
        }

        public void AbrirConexion()
        {
            try
            {
                if(conexion.State == ConnectionState.Broken || conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al tratar de abrir la conexion", e);
            }
        }

        public void CerrarConexion()
        {
            try
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al tratar de cerrar la conexion", e);
            }
        }
    }
}
