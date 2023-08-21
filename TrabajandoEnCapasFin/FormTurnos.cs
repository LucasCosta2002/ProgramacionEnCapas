using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;
using Negocios;

namespace TrabajandoEnCapasFin
{
    public partial class FormTurnos : Form
    {
        public FormTurnos()
        {
            InitializeComponent();
            dgvTurnos.ColumnCount = 6;
            dgvTurnos.Columns[0].HeaderText = "Identificacion";
            dgvTurnos.Columns[1].HeaderText = "Cliente";
            dgvTurnos.Columns[2].HeaderText = "DNI";
            dgvTurnos.Columns[3].HeaderText = "Celular";
            dgvTurnos.Columns[4].HeaderText = "Fecha";
            dgvTurnos.Columns[5].HeaderText = "Cobro";
            LlenarDGV();

            button1.Visible = false;
            //SetearFecha();
        }

        public Turno objEntTurno = new Turno("cliente", "celular", "DNI", DateTime.Today, false);
        public NegTurnos objNegTurno = new NegTurnos();

        private void LlenarDGV()
        {
            dgvTurnos.Rows.Clear();

            DataSet ds = new DataSet();

            ds = objNegTurno.listadoTurnos("Todos");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dgvTurnos.Rows.Add(dr[0], dr[1], dr[2], dr[3], dr[4], dr[5]);
                }
            }
            else
            {
                lblMensaje.Text = "No hay Turnos registrados";
            }
        }
        private void Limpiar()
        {
            txtCliente.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtDNI.Text = string.Empty;
        }
        private void TxtBox_a_Obj()
        {
            objEntTurno.pCliente = txtCliente.Text;
            objEntTurno.pCelular = txtCelular.Text;
            objEntTurno.pDNI = txtDNI.Text;
            objEntTurno.pFecha = dtpFecha.Value;
        }
        private void Ds_a_TxtBox(DataSet ds)
        {
            txtCliente.Text = ds.Tables[0].Rows[0]["Cliente"].ToString();
            txtCelular.Text = ds.Tables[0].Rows[0]["Celular"].ToString();
            txtDNI.Text = ds.Tables[0].Rows[0]["DNI"].ToString();
            dtpFecha.Value = (DateTime)ds.Tables[0].Rows[0]["Fecha"];
        }
        private void txtCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            //evitar que escriba numeros
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            //evitar que escriba muchos letras
            if (txtCelular.Text.Length >= 20)
            {
                e.Handled = true;
            }
        }
        private void txtCelular_KeyPress(object sender, KeyPressEventArgs e)
        {
            //comprobar que solo escriba numeros
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;   return;
            }

            //evitar que escriba mas de 10 numeros
            if (txtCelular.Text.Length >= 10)
            {
                e.Handled = true;
            }
        }
        private void txtDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            //comprobar que solo escriba numeros
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; return;
            }

            //evitar que escriba mas de 10 numeros
            if (txtDNI.Text.Length >= 9)
            {
                e.Handled = true;
            }
        }
        private bool TurnoUnico(string DNI)
        {

            foreach (DataGridViewRow row in dgvTurnos.Rows)
            {
                bool existeDNI = row.Cells[2].Value != null && row.Cells[2].Value.ToString() == DNI;
                //bool existeFecha = row.Cells[4].Value != null && row.Cells[4].Value.ToString() == Fecha;

                //comprobar que el valor de la celda DNI no se igual al dni del textBox
                if (existeDNI)
                {
                    return false; // existe ese dni
                }
            }
            return true;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            int nGrabados = -1;

            TxtBox_a_Obj();//llamo al método que carga los datos del objeto

            //comprobar que los campos no esten vacios
            if (txtCliente.Text == "" || txtCelular.Text == "" || txtDNI.Text == "")
            {
                MessageBox.Show("Todos los campos son obligatorios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }

            #region validaciones celular
            string expresionCel = "^\\d{10}$\r\n";
            //comprobar que el numero de telefono sea valido
            if (Regex.IsMatch(txtCelular.Text, expresionCel))
            {
                MessageBox.Show("Formato Invalido. Inserte por ejemplo 3562430891", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            if (txtCelular.Text.Length < 10)
            {
                MessageBox.Show("El celular debe tener 10 caracteres, sin el 0 ni el 15 delante", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            #endregion

            #region validaciones DNI
            string expresionDNI = "^[0-9]{7,8}$\r\n";

            if (Regex.IsMatch(txtDNI.Text, expresionDNI))
            {
                MessageBox.Show("DNI no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            if (!TurnoUnico(txtDNI.Text))
            {
                MessageBox.Show("El turno ingresado ya existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.
                    Error);
                return;
            }

            nGrabados = objNegTurno.abmTurnos("Alta", objEntTurno); //invoco a la capa de negocio
            if (nGrabados == -1)
            {
                MessageBox.Show("No se pudo Cargar Turno en el sistema");
                return;
            }
            else
            {
                MessageBox.Show("Exito al cargar Turno en el sistema", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LlenarDGV();
                Limpiar();
            }
        }
        private void dgvTurnos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataSet ds = new DataSet();
            object celda = dgvTurnos.CurrentRow.Cells[0].Value;

            //comprobar que exista algo en las celdas para modificar o eliminar
            if (celda != null && !string.IsNullOrEmpty(celda.ToString()))
            {
                objEntTurno.pDNI = Convert.ToString(dgvTurnos.CurrentRow.Cells[0].Value);
                ds = objNegTurno.listadoTurnos(objEntTurno.pDNI.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Ds_a_TxtBox(ds);
                    btnGuardar.Visible = false;
                    lblMensaje.Text = String.Empty;
                }
            }
            else
            {
                return;
            }

        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            int nResultado = -1;
            TxtBox_a_Obj();
            nResultado = objNegTurno.abmTurnos("Modificar", objEntTurno);


            //comprobar que los campos no esten vacios
            if (txtCliente.Text == "" || txtCelular.Text == "" || txtDNI.Text == "")
            {
                MessageBox.Show("Debes seleccionar un Turno a modificar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }



            if (nResultado != -1)
            {
                MessageBox.Show("Modificado correctamente","Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LlenarDGV();
                Limpiar();
                txtDNI.Enabled = true;
            }
            else
            {
                MessageBox.Show("Hubo un error");

            }
            btnGuardar.Visible = true;
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int nResultado = -1;
            TxtBox_a_Obj();
            nResultado = objNegTurno.abmTurnos("Borrar", objEntTurno);

            //comprobar que los campos no esten vacios
            if (txtCliente.Text == "" || txtCelular.Text == "" || txtDNI.Text == "")
            {
                MessageBox.Show("Debes seleccionar un Turno a eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }

            if (nResultado != -1)
            {
                MessageBox.Show("Eliminado con exito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LlenarDGV();
                Limpiar();
                txtDNI.Enabled = true;
            }
            else
            {
                MessageBox.Show("Hubo un error");

            }
            btnGuardar.Visible = true;

        }

    }   
}
