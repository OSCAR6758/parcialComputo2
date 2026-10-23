using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace parcialComputo2
{
    public partial class frm_usuario : Form
    {
        private DataSet ds;
        private int posicion = 0;

        public frm_usuario()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
          
            if (btnAgregar.Text == "Nuevo")
            {
                txtUsuario.Clear();
                txtClave.Clear();
                txtConfirmarClave.Clear();
                txtNombre.Clear();
                txtDireccion.Clear();
                txtTelefono.Clear();


                txtUsuario.Enabled = true;
                txtClave.Enabled = true;
                txtConfirmarClave.Enabled = true;
                txtNombre.Enabled = true;
                txtDireccion.Enabled = true;
                txtTelefono.Enabled = true;

                btnAgregar.Text = "Guardar";


                btnModificar.Enabled = false;
                btnEliminar.Enabled = false;

                txtUsuario.Focus();
                return;
            }

           
            if (btnAgregar.Text == "Guardar")
            {
              
                if (txtUsuario.Text.Length < 6 || txtUsuario.Text.Length > 16)
                {
                    MessageBox.Show("El usuario debe tener un mínimo de 6 caracteres y un máximo de 16.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsuario.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El campo Nombre es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }

                if (txtClave.Text != txtConfirmarClave.Text)
                {
                    MessageBox.Show("Las contraseñas no coinciden. Por favor, verifíquese.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmarClave.Clear();
                    txtConfirmarClave.Focus();
                    return;
                }

               
                bool tieneLetras = System.Text.RegularExpressions.Regex.IsMatch(txtClave.Text, "[a-zA-Z]");
                bool tieneNumeros = System.Text.RegularExpressions.Regex.IsMatch(txtClave.Text, "[0-9]");

                if (!tieneLetras || !tieneNumeros)
                {
                    MessageBox.Show("La clave debe llevar obligatoriamente letras y números.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClave.Focus();
                    return;
                }

                try
                {
                    Conexion_base_datos.mantenimiento_usuarios_usss077126(
                        txtUsuario.Text,
                        txtClave.Text,
                        txtNombre.Text,
                        txtDireccion.Text,
                        txtTelefono.Text
                    );

                    MessageBox.Show("¡Usuario registrado con éxito en la base de datos!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnAgregar.Text = "Nuevo";

                    btnModificar.Enabled = true;
                    btnEliminar.Enabled = true;

                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar en la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frm_usuario_Load(object sender, EventArgs e)
        {
            if (btnAgregar.Text == "" || btnAgregar.Text == "Agregar")
            {
                btnAgregar.Text = "Nuevo";
            }

            txtClave.UseSystemPasswordChar = true;
            txtConfirmarClave.UseSystemPasswordChar = true;

            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                ds = Conexion_base_datos.crearDs();
                if (ds != null && ds.Tables["usuarios"].Rows.Count > 0)
                {
                    dgvRegistros.DataSource = ds.Tables["usuarios"];
                    if (posicion >= ds.Tables["usuarios"].Rows.Count)
                    {
                        posicion = ds.Tables["usuarios"].Rows.Count - 1;
                    }
                    MostrarRegistro(posicion);
                }
                else
                {

                    txtUsuario.Clear();
                    txtClave.Clear();
                    txtConfirmarClave.Clear();
                    txtNombre.Clear();
                    txtDireccion.Clear();
                    txtTelefono.Clear();
                    lblIndicador.Text = "0 de 0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los registros: " + ex.Message);
            }
        }

        private void MostrarRegistro(int index)
        {
            if (ds != null && ds.Tables["usuarios"].Rows.Count > 0 && index >= 0 && index < ds.Tables["usuarios"].Rows.Count)
            {
                DataRow fila = ds.Tables["usuarios"].Rows[index];
                txtUsuario.Text = fila["usuario"].ToString();
                txtClave.Text = fila["clave"].ToString();
                txtConfirmarClave.Text = fila["clave"].ToString();
                txtNombre.Text = fila["nombre"].ToString();
                txtDireccion.Text = fila["dirección"].ToString();
                txtTelefono.Text = fila["teléfono"].ToString();

                
                lblIndicador.Text = (index + 1) + " de " + ds.Tables["usuarios"].Rows.Count;

         
                BloquearCampos();

             
                btnAgregar.Text = "Nuevo";
                btnModificar.Text = "Modificar";
                btnAgregar.Enabled = true;
                btnModificar.Enabled = true;
                btnEliminar.Enabled = true;
            }
        }

        private void BloquearCampos()
        {
            txtUsuario.Enabled = false;
            txtClave.Enabled = false;
            txtConfirmarClave.Enabled = false;
            txtNombre.Enabled = false;
            txtDireccion.Enabled = false;
            txtTelefono.Enabled = false;
        }

        private void txtDireccion_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblClave_Click(object sender, EventArgs e)
        {

        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            if (ds != null && ds.Tables["usuarios"].Rows.Count > 0)
            {
                posicion = 0;
                MostrarRegistro(posicion);
            }
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            if (ds != null && ds.Tables["usuarios"].Rows.Count > 0)
            {
                if (posicion > 0)
                {
                    posicion--;
                    MostrarRegistro(posicion);
                }
            }
        }

        private void btnAdelante_Click(object sender, EventArgs e)
        {
            if (ds != null && ds.Tables["usuarios"].Rows.Count > 0)
            {
                if (posicion < ds.Tables["usuarios"].Rows.Count - 1)
                {
                    posicion++;
                    MostrarRegistro(posicion);
                }
            }
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            if (ds != null && ds.Tables["usuarios"].Rows.Count > 0)
            {
                posicion = ds.Tables["usuarios"].Rows.Count - 1;
                MostrarRegistro(posicion);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (btnModificar.Text == "Modificar")
            {
                txtUsuario.Enabled = true;
                txtClave.Enabled = true;
                txtConfirmarClave.Enabled = true;
                txtNombre.Enabled = true;
                txtDireccion.Enabled = true;
                txtTelefono.Enabled = true;

                btnModificar.Text = "Actualizar";

                btnAgregar.Enabled = false;
                btnEliminar.Enabled = false;

                MessageBox.Show("Modifique los campos necesarios y vuelva a hacer clic en 'Actualizar' para guardar los cambios.", "Modo Edición", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            if (btnModificar.Text == "Actualizar")
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtNombre.Text))
                    {
                        MessageBox.Show("El campo Nombre es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtNombre.Focus();
                        return;
                    }

                    if (ds != null && ds.Tables["usuarios"].Rows.Count > 0)
                    {
                        DataRow fila = ds.Tables["usuarios"].Rows[posicion];
                        int idUsuario = Convert.ToInt32(fila["idUsuario"]);

                        Conexion_base_datos.modificar_usuario(
                            idUsuario,
                            txtUsuario.Text,
                            txtClave.Text,
                            txtNombre.Text,
                            txtDireccion.Text,
                            txtTelefono.Text
                        );

                        MessageBox.Show("¡Registro modificado con éxito!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnModificar.Text = "Modificar";

                      
                        btnAgregar.Enabled = true;
                        btnEliminar.Enabled = true;

                        CargarDatos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            btnEliminar_Click_1(sender, e);
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ds != null && ds.Tables["usuarios"].Rows.Count > 0)
                {
                    DataRow fila = ds.Tables["usuarios"].Rows[posicion];
                    string nombreUsuario = fila["usuario"].ToString();

                    DialogResult confirmacion = MessageBox.Show("¿Estás seguro de eliminar al usuario " + nombreUsuario + "?", "Eliminando usuarios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmacion == DialogResult.Yes)
                    {
                        int idUsuario = Convert.ToInt32(fila["idUsuario"]);

                        Conexion_base_datos.eliminar_usuario(idUsuario);

                        MessageBox.Show("¡Registro eliminado con éxito!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        posicion = 0;
                        CargarDatos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRegistros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < ds.Tables["usuarios"].Rows.Count)
            {
                posicion = e.RowIndex;
                MostrarRegistro(posicion);
            }
        }

        private void lblIndicador_Click(object sender, EventArgs e)
        {

        }

        private void dgvRegistros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnVerClave_Click(object sender, EventArgs e)
        {
            
            if (txtClave.UseSystemPasswordChar)
            {
                txtClave.UseSystemPasswordChar = false;
                txtConfirmarClave.UseSystemPasswordChar = false;
            }
            else
            {
                txtClave.UseSystemPasswordChar = true;
                txtConfirmarClave.UseSystemPasswordChar = true;
            }
        }

        private void grpRegistro_Enter(object sender, EventArgs e)
        {

        }
    } 
} 