using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public class ManipulacaoArquivo
{
    private int bitsRSA;

    List<string> listaRemovivel; // lista de drivers removíveis

    public ManipulacaoArquivo(int abitsRSA)
    {
        bitsRSA = abitsRSA;

        listaRemovivel = new List<string>(); // criando instância para lista de drivers removíveis
    }

    private long getTamanhoArquivo(string arquivo)
    {
        FileInfo info = new FileInfo(@arquivo);

        return info.Length;
    }

    public void getListaDriversRemoviveis()
    {
        listaRemovivel.Clear();

        // Gerar lista de drivers removíveis
        foreach (var drivers in System.IO.DriveInfo.GetDrives())
        {
            if (drivers.DriveType == DriveType.Removable)
            {
                listaRemovivel.Add(drivers.RootDirectory.ToString());
            }
        }

        if (listaRemovivel.Count == 0)
        {
            throw new Exception("Não foi encontrado um driver removível para guardar a chave privada!");
        }
    }

    public void getNomeArquivoMensagemCriptografadaExistente(ref string arquivo, string diretorioTrocaMensagem)
    {
        try
        {
            arquivo = "";

            System.Windows.Forms.DialogResult resultado;

            OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Title = "Carregar mensagem criptografada em arquivo texto";
            dialog.Filter = "Arquivo Texto (*.txt)|*.txt";
            dialog.DefaultExt = "txt";
            dialog.AutoUpgradeEnabled = true;
            dialog.InitialDirectory = diretorioTrocaMensagem;

            bool continuarLoop = true;

            do
            {
                resultado = dialog.ShowDialog();

                if (resultado == System.Windows.Forms.DialogResult.OK)
                {
                    if (System.IO.Path.GetExtension(dialog.FileName).ToUpper() != ".TXT")
                    {
                        throw new Exception("A mensagem criptografada deve estar em arquivo do tipo TXT!");
                    }
                    else
                    {
                        continuarLoop = false;

                        arquivo = dialog.FileName;
                    }
                }
                else
                {
                    continuarLoop = false;
                }

            }
            while (continuarLoop);

        }
        catch (Exception E)
        {
            System.Windows.MessageBox.Show(E.Message, "Erro ao salvar mensagem");
        }
    }

    public void getNomeArquivoMensagemCriptografadaNovo(ref string arquivo, string diretorioTrocaMensagem)
    {
        try
        {
            arquivo = "";

            System.Windows.Forms.DialogResult resultado;

            SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Title = "Gerar mensagem criptografada em arquivo texto";
            dialog.Filter = "Arquivo Texto (*.txt)|*.txt";
            dialog.InitialDirectory = diretorioTrocaMensagem;

            dialog.OverwritePrompt = false;
            dialog.DefaultExt = "txt";
            dialog.AutoUpgradeEnabled = true;

            bool continuarLoop = true;

            do
            {
                resultado = dialog.ShowDialog();

                if (resultado == System.Windows.Forms.DialogResult.OK)
                {
                    if (System.IO.Path.GetExtension(dialog.FileName).ToUpper() != ".TXT")
                    {
                        throw new Exception("A mensagem criptografada deve estar em um arquivo TXT!");
                    }
                    else if (File.Exists(dialog.FileName))
                    {
                        continuarLoop = true;
                        System.Windows.MessageBox.Show("Já existe um arquivo gravado neste diretório com o mesmo nome!", "Erro");
                    }
                    else
                    {
                        continuarLoop = false;

                        arquivo = dialog.FileName;
                    }
                }
                else
                {
                    continuarLoop = false;
                }

            }
            while (continuarLoop);

        }
        catch (Exception E)
        {
            System.Windows.MessageBox.Show(E.Message, "Erro ao salvar mensagem");
        }
    }

    public void getNomeArquivoChavePrivadaNovo(ref string arquivo)
    {
        try
        {
            getListaDriversRemoviveis(); 

            arquivo = "";

            System.Windows.Forms.DialogResult resultado;

            SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Title = "Gerar chave primária ("+bitsRSA.ToString()+" bits) em disco removível";
            dialog.Filter = "Chave privada (*.prv)|*.prv";
            dialog.OverwritePrompt = false;
            dialog.DefaultExt = "prv";
            dialog.AutoUpgradeEnabled = true;

            dialog.CustomPlaces.Clear();

            foreach (var drivers in listaRemovivel)
            {
                dialog.CustomPlaces.Add(drivers);
            }

            bool continuarLoop = true;

            do
            {
                dialog.InitialDirectory = listaRemovivel[0].ToString();

                resultado = dialog.ShowDialog();

                if (resultado == System.Windows.Forms.DialogResult.OK)
                {
                    if (listaRemovivel.IndexOf(System.IO.Path.GetPathRoot(dialog.FileName)) < 0)
                    {
                        continuarLoop = true;
                        System.Windows.MessageBox.Show("Chave primária, por segurança, deve ser gravada em disco removível!", "Erro");
                    }
                    else if (System.IO.Path.GetExtension(dialog.FileName).ToUpper() != ".PRV")
                    {
                        throw new Exception("A chave privada deve ser um arquivo do tipo PRV!");
                    }
                    else if (File.Exists(dialog.FileName))
                    {
                        continuarLoop = true;
                        System.Windows.MessageBox.Show("Já existe uma chave privada com este nome neste diretório com o mesmo nome!", "Erro");
                    }
                    else
                    {
                        continuarLoop = false;

                        arquivo = dialog.FileName;
                    }
                }
                else
                {
                    continuarLoop = false;
                }

            }
            while (continuarLoop);
        }
        catch (Exception E)
        {
            System.Windows.MessageBox.Show(E.Message, "Erro ao salvar chave privada");
        }
    }

    public void getNomeArquivoChavePrivadaExistente(ref string arquivo)
    {
        try
        {
            getListaDriversRemoviveis(); 

            arquivo = "";

            System.Windows.Forms.DialogResult resultado;

            OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Title = "Abrir chave privada em disco removível";
            dialog.DefaultExt = "prv";
            dialog.AutoUpgradeEnabled = true;
            dialog.Filter = "Chave privada (*.prv)|*.prv";

            dialog.CustomPlaces.Clear();

            foreach (var drivers in listaRemovivel)
            {
                dialog.CustomPlaces.Add(drivers);
            }

            bool continuarLoop = true;

            do
            {
                dialog.InitialDirectory = listaRemovivel[0].ToString();

                resultado = dialog.ShowDialog();

                if (resultado == System.Windows.Forms.DialogResult.OK)
                {
                    if (listaRemovivel.IndexOf(System.IO.Path.GetPathRoot(dialog.FileName)) < 0)
                    {
                        continuarLoop = true;
                        System.Windows.MessageBox.Show("Chave primária, por segurança, deve estar localizada em um disco removível!", "Erro");
                    }
                    else if (System.IO.Path.GetExtension(dialog.FileName).ToUpper() != ".PRV")
                    {
                        throw new Exception("A chave privada deve ser um arquivo do tipo PRV!");
                    }
                    else
                    {
                        continuarLoop = false;

                        arquivo = dialog.FileName;
                    }
                }
                else
                {
                    continuarLoop = false;
                }

            }
            while (continuarLoop);
        }
        catch (Exception E)
        {
            System.Windows.MessageBox.Show(E.Message, "Erro ao localizar chave privada");
        }
    }

    public string carregarConteudo(string arquivo)
    {
        StreamReader streamReader = new StreamReader(arquivo, true);
        string conteudo = streamReader.ReadToEnd();
        streamReader.Close();

        return conteudo;
    }

    public void salvarConteudoEmArquivo(string arquivo, string conteudo)
    {
        try
        {
            StreamWriter streamWriter = new StreamWriter(arquivo, false);
            streamWriter.Write(conteudo);
            streamWriter.Close();
        }
        catch (Exception E)
        {
            System.Windows.MessageBox.Show(E.Message, "Erro ao salvar conteúdo");
        }
    }

    public void getNomeArquivoChavePublica(string caminhoRepositorio, ref string arquivo)
    {
        try
        {
            System.Windows.Forms.DialogResult resultado;

            OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Title = "Localizar chave pública para criptografar a mensagem";
            dialog.DefaultExt = "pub";
            dialog.InitialDirectory = caminhoRepositorio;
            dialog.AutoUpgradeEnabled = true;
            dialog.Filter = "Chave pública (*.pub)|*.pub";

            bool continuarLoop = true;

            do
            {
                dialog.InitialDirectory = caminhoRepositorio;

                resultado = dialog.ShowDialog();

                if (resultado == System.Windows.Forms.DialogResult.OK)
                {
                    if (System.IO.Path.GetExtension(dialog.FileName).ToUpper() != ".PUB")
                    {
                        throw new Exception("A chave pública deve ser um arquivo do tipo PUB!");
                    }
                    else if (getTamanhoArquivo(dialog.FileName) == 0)
                    {
                        throw new Exception("Arquivo de chave pública inválido!");
                    }
                    else
                    {
                        continuarLoop = false;

                        arquivo = dialog.FileName;
                    }
                }
                else
                {
                    continuarLoop = false;
                }
            }
            while (continuarLoop);
        }
        catch (Exception E)
        {
            System.Windows.MessageBox.Show(E.Message, "Erro ao abrir chave pública");
        }

    }
}
