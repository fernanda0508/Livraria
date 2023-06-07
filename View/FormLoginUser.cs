﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LivrariaFive.Controller;
using LivrariaFive.Model;

namespace LivrariaFive.View
{
    public partial class FormLoginUser : Form
    {
        public Cliente ClienteAtual { get; private set; }

        public FormLoginUser()
        {
            InitializeComponent();
        }

        private void btnCadastrarUser_Click(object sender, EventArgs e)
        {
            // Obter os dados do formulário
            string nome = txtNomeCadastro.Text;
            string email = txtEmailCadastro.Text;
            string senha = txtSenhaCadastro.Text;
            string endereco = txtEnderecoCadastro.Text;
            string cpf = maskTxtCpfCadastro.Text;
            string telefone = maskTxtTelefoneCadastro.Text;
            DateTime dataNascimento = dtpDataNascimentoCadastro.Value;

            // Criar uma instância do ClienteController
            ClienteController clienteController = new ClienteController();

            // Criar uma instância do Cliente com os dados informados
            Cliente cliente = new Cliente
            {
                Nome = nome,
                Email = email,
                Senha = senha,
                Endereco = endereco,
                CPF = cpf,
                Telefone = telefone,
                DataNascimento = dataNascimento
            };

            if (ValidaCadastroUser.ValidarCliente(cliente, out string mensagemErro))
            {
                // Todos os campos estão preenchidos corretamente
                // Prossiga com o cadastro do cliente

                try
                {
                    // Inserir o cliente no banco de dados
                    Cliente clienteInserido = clienteController.InserirCliente(cliente);

                    // Verifique se o cliente foi inserido com sucesso no banco de dados
                    if (clienteInserido != null)
                    {
                        // Cliente cadastrado com sucesso
                        MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Ocorreu um erro ao cadastrar o cliente no banco de dados
                        MessageBox.Show("Erro ao cadastrar o cliente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Exibir mensagem de erro ao usuário
                    MessageBox.Show("Erro ao cadastrar o cliente: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Exiba a mensagem de erro com os campos inválidos
                MessageBox.Show($"Os seguintes campos estão inválidos ou não foram preenchidos corretamente:\n\n{mensagemErro}", "Erro de validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLogarUser_Click(object sender, EventArgs e)
        {
            // Obter os dados do formulário
            string email = txtEmail.Text;
            string senha = txtSenhaLogin.Text;

            ClienteController clienteController = new ClienteController();
            Cliente cliente = clienteController.VerificarCredenciais(email, senha);

            if (cliente != null)
            {
                // Definir o cliente atual
                ClienteAtual = cliente;

                FormCarrinho formCarrinho = new FormCarrinho();

                // Exibir o formulário LivroForm e passar o cliente atual como argumento
                LivroForm livroForm = new LivroForm(ClienteAtual, formCarrinho);
                livroForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Credenciais inválidas. Por favor, tente novamente.");
            }
        }

        private void btnLoginAdminRestrito_Click(object sender, EventArgs e)
        {
            FrmTelaLoginAdmin login = new FrmTelaLoginAdmin();
            this.Hide();
            login.Show();
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

