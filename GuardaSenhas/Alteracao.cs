using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuardaSenhas
{
    public partial class frmCriptografado : Form
    {
        public frmCriptografado(string login, string senha)
        {
            InitializeComponent();

            Criptografia cript = new Criptografia();

            txtLogin.Text = cript.Encrypt(login);
            txtSenha.Text = cript.Encrypt(senha);
        }
    }
}
