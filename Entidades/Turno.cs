using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Turno
    {
        //atributos
        private string cliente;
        private string celular;
        private string DNI;
        private DateTime fecha;
        private bool cobro;

        public Turno(string cliente, string celular, string DNI, DateTime fecha, bool cobro)
        {
            this.cliente = cliente;
            this.celular = celular;
            this.DNI = DNI;
            this.fecha = fecha;
            this.cobro = cobro;
        }

        //propiedades
        public string pCliente { set { cliente = value; } get { return cliente; } }
        public string pCelular { set { celular = value; } get { return celular; } }
        public string pDNI { set { DNI = value; } get { return DNI; } }
        public DateTime pFecha { set { fecha = value; } get { return fecha; } }
        public bool pCobro { set { cobro = value; } get { return cobro; } }
    }
}
