using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace GuardaSenhas
{
    public partial class FrmCadastroSenhas : Form
    {
        string caminhoTxt = ConfigurationManager.AppSettings["caminhoArquivo"];

        public FrmCadastroSenhas()
        {
            InitializeComponent();

            var dataLink = new DataGridViewLinkColumn();
            dataLink.Name = "Descricao";
            dataLink.HeaderText = "Descricao";

            dataLink.VisitedLinkColor = Color.Green;
            dataLink.LinkBehavior = LinkBehavior.NeverUnderline;

            DataGridView1.Columns.Add(dataLink);
            DataGridView1.Columns.Add("Login", "Login");
            DataGridView1.Columns.Add("Senha", "Senha");
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text != string.Empty && txtSenha.Text != string.Empty && txtDescricao.Text != string.Empty) {
                var dadosLogin = new DadosCadastro();
                Criptografia criptografia = new Criptografia();

                dadosLogin.Login = txtLogin.Text;
                dadosLogin.Senha = txtSenha.Text;
                dadosLogin.Descricao = txtDescricao.Text;

                DataGridView1.AllowUserToAddRows = false;
                DataGridView1.Rows.Insert(0, dadosLogin.Descricao, dadosLogin.Login, dadosLogin.Senha);

                LimparCampos();

                BtnAtualizar.Enabled = ValidarCampos();
                BtnExcluir.Enabled = ValidarCampos();
                txtDescricao.Select();
            }
            else {
                MessageBox.Show("Necessario preencher todos campos para guardar o login", "Atenção");
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (DataGridView1.SelectedRows.Count == 0)
                MessageBox.Show("Selecione uma linha para exclusão", "Atenção");

            foreach (DataGridViewRow linha in this.DataGridView1.SelectedRows)
                DataGridView1.Rows.RemoveAt(linha.Index);

            LimparCampos();
            BtnExcluir.Enabled = ValidarCampos();
            BtnAtualizar.Enabled = ValidarCampos();
        }

        private void DataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int i = DataGridView1.CurrentCell.RowIndex;
            txtLogin.Text = Convert.ToString(DataGridView1.Rows[i].Cells["Login"].Value);
            txtSenha.Text = Convert.ToString(DataGridView1.Rows[i].Cells["Senha"].Value);
            txtDescricao.Text = Convert.ToString(DataGridView1.Rows[i].Cells["Descricao"].Value);

            BtnAtualizar.Enabled = ValidarCampos();
            BtnExcluir.Enabled = ValidarCampos();
        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text != string.Empty && txtSenha.Text != string.Empty && txtDescricao.Text != string.Empty) {
                Criptografia criptografia = new Criptografia();

                int i = DataGridView1.CurrentCell.RowIndex;
                DataGridView1.Rows[i].Cells["Login"].Value = txtLogin.Text;
                DataGridView1.Rows[i].Cells["Senha"].Value = txtSenha.Text;
                DataGridView1.Rows[i].Cells["Descricao"].Value = txtDescricao.Text;

                LimparCampos();
                BtnAtualizar.Enabled = ValidarCampos();
                BtnExcluir.Enabled = ValidarCampos();

                txtLogin.Focus();
            }
            else {
                MessageBox.Show("Por favor selecione uma linha para alteração", "Atenção");
            }
            txtDescricao.Select();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView data = new DataGridView();

            if (e.RowIndex >= 0) {
                if (Convert.ToBoolean(DataGridView1.Columns[e.ColumnIndex].Name == "Descricao")) {
                    var dado = from coluna in DataGridView1.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>()
                               select coluna.Value;

                    frmCriptografado formAlteracao = new frmCriptografado(dado.ElementAt(1).ToString(), dado.ElementAt(2).ToString());
                    formAlteracao.ShowDialog();
                }
            }
        }

        private void BtnLimparCampos_Click(object sender, EventArgs e)
        {
            LimparCampos();
            BtnAtualizar.Enabled = ValidarCampos();
            BtnExcluir.Enabled = ValidarCampos();
        }

        public void LimparCampos()
        {
            txtLogin.Text = string.Empty;
            txtSenha.Text = string.Empty;
            txtDescricao.Text = string.Empty;
        }

        public bool ValidarCampos()
        {
            if (txtLogin.Text == string.Empty && txtDescricao.Text == string.Empty && txtSenha.Text == string.Empty)
                return false;

            return true;
        }
    }
}

public class DadosCadastro
{
    public string Descricao { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
}

