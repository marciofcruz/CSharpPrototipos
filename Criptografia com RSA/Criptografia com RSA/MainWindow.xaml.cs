using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Criptografia_com_RSA
{

    public partial class MainWindow : Window
    {
        private int tamanhotexto; // tamanho máximo do texto permitido a criptografar
        public int tamanhoTexto
        {
            get
            { return tamanhotexto; }
            set
            { 
               tamanhotexto = value;
               lblTamanhoMaximoTexto.Content = "Tamanho máximo texto: " + value.ToString()+" caracteres";
               setMudancaTextoOriginal();
            }
        }

        private int rsabits = 2048; // quanto maior o número , mais difícil quebrar
        public int rsaBits
        {
            get
            { return rsabits; }
            set
            { 
                rsabits = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void setMudancaTextoOriginal()
        {
            this.lblCaptionMensagemOriginal.Content =
                "Mensagem Original - " +
                this.textoOriginal.Text.Length.ToString() + "/" +
                this.tamanhoTexto.ToString();
        }

        private Parametros GetParametros()
        {
            Parametros parametros = null;
            if (File.Exists("Parametros.bin"))
            {
                StreamReader streamReader = new StreamReader("Parametros.bin");
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                parametros = (Parametros)binaryFormatter.Deserialize(streamReader.BaseStream);
                streamReader.Close();
            }
            return parametros;
        }

        private void copiarTextoCifradoAreaTransferencia()
        {
            if (textoCifrado.Text.Length == 0)
            {
                System.Windows.MessageBox.Show("Não há texto criptografado para ser copiado!",
                                               "Erro");

            }
            else
            {
                textoCifrado.SelectAll();
                System.Windows.Forms.Clipboard.SetText(textoCifrado.Text);

                System.Windows.MessageBox.Show("Texto copiado com sucesso a área de transferência!",
                                               "Área de transferência");
            }
        }

        private void setParametros(string settingChanged)
        {
            Parametros parametros = new Parametros();

            if (File.Exists("Parametros.bin"))
            {
                StreamReader streamReader = new StreamReader("Parametros.bin");
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                parametros = (Parametros)binaryFormatter.Deserialize(streamReader.BaseStream);
                streamReader.Close();

                switch (settingChanged)
                {
                    case "REPOSITORIO":
                        {
                            parametros.diretorioRepositorio = edtCaminhoRepositorio.Text;
                            break;
                        }
                    case "TAMANHOTEXTO": 
                        {
                            parametros.tamanhoTexto = this.tamanhoTexto;
                            break;
                        }
                    case "RSABITS":
                        {
                            parametros.rsaBits = this.rsaBits;
                            break;
                        }
                    case "TROCAMENSAGEM":
                        {
                            parametros.diretorioTrocaMensagem = edtCaminhoTrocaMensagem.Text;
                            break;
                        }
                }

                StreamWriter streamWriter = new StreamWriter("Parametros.bin", false);
                binaryFormatter.Serialize(streamWriter.BaseStream, parametros);
                streamWriter.Close();
            }
            else
            {
                parametros.diretorioRepositorio = edtCaminhoRepositorio.Text;
                parametros.diretorioTrocaMensagem = edtCaminhoTrocaMensagem.Text;
                parametros.tamanhoTexto = this.tamanhoTexto;
                parametros.rsaBits = this.rsaBits;

                StreamWriter streamWriter = new StreamWriter("Parametros.bin", false);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(streamWriter.BaseStream, parametros);
                streamWriter.Close();
            }

        }

        private void btnGerarChavePrivada_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ManipulacaoArquivo manipulacaoarquivo = new ManipulacaoArquivo(rsaBits);

                if (!Directory.Exists(edtCaminhoRepositorio.Text)) 
                { 
                    throw new Exception("Diretório do caminho do repositório não foi encontrado!");
                }

                string mensagem = "";

                bool continuarProcesso = true;

                if (rsaBits == 4096)
                {
                    mensagem = "A chave será gerada com tamanho de " + rsaBits.ToString() + " bits!\n" +
                               "Este processo poderá levar alguns segundos.\n" +
                               "Deseja criar nova chave privada?";
                }
                else if (rsaBits == 6144)
                {
                    mensagem = "A chave será gerada com tamanho de " + rsaBits.ToString() + " bits!\n" +
                               "Este processo poderá levar alguns minutos.\n" +
                               "Deseja criar nova chave privada?";
                }

                // se a chave for mais exigir mais tempo pra ser gerada, fazer uma pergunta anterior
                if (mensagem != "")
                {
                    continuarProcesso =
                    System.Windows.MessageBox.Show(mensagem,
                                                   "Confirmação",
                                                   System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes;
                }

                if (continuarProcesso)
                {
                    // nome e path do arquivo onde será gravado cada chave
                    string arquivoChavePrivada = "";
                    string arquivoChavePublica = "";

                    // contém o XML das chaves
                    string chavePrivada = "";
                    string chavePublica = "";

                    UtilizacaoRSA rsa = new UtilizacaoRSA(rsaBits);

                    manipulacaoarquivo.getNomeArquivoChavePrivadaNovo(ref arquivoChavePrivada); // Selecionar novo Arquivo de chave Privada

                    if (arquivoChavePrivada != "")
                    {
                        // Alimentar a variável para apontar onde deve ser gravado a chave Pública
                        arquivoChavePublica = System.IO.Path.ChangeExtension(arquivoChavePrivada, "pub"); // trocando a extensão
                        arquivoChavePublica = System.IO.Path.GetFileName(arquivoChavePublica); // nome do arquivo sem o path

                        arquivoChavePublica = edtCaminhoRepositorio.Text + System.IO.Path.DirectorySeparatorChar + arquivoChavePublica; // Novo Caminho

                        Cursor = System.Windows.Input.Cursors.Wait;

                        btnGerarChavePrivada.Content = "Aguarde...";
                        btnGerarChavePrivada.IsEnabled = false;
                        System.Windows.Forms.Application.DoEvents(); // Libera Mensagens do processamento do Windows

                        try
                        {
                            rsa.gerarChavePublicaePrivada(ref chavePrivada, ref chavePublica);

                            manipulacaoarquivo.salvarConteudoEmArquivo(arquivoChavePrivada, chavePrivada); // salvando chave privada no disco removível
                            manipulacaoarquivo.salvarConteudoEmArquivo(System.IO.Path.ChangeExtension(arquivoChavePrivada, "pub"), chavePublica); // salvando chave pública no disco removível (backup)

                            manipulacaoarquivo.salvarConteudoEmArquivo(arquivoChavePublica, chavePublica); // salvando chave pública no repositório

                            System.Windows.MessageBox.Show( "Chaves pública e privada foram geradas e gravadas com sucesso!\n"+
                                                            "Atenção: A chave privada deve ser mantida em segurança. Esta em hipótese alguma deve ser divulgada.", 
                                                            "Gravação Ok");
                        }
                        catch (Exception E)
                        {
                            System.Windows.MessageBox.Show(E.Message, "Erro");
                        }
                        finally
                        {
                            this.Cursor = System.Windows.Input.Cursors.Arrow;
                        }

                        btnGerarChavePrivada.IsEnabled = true;
                        btnGerarChavePrivada.Content = "Chave Privada";
                    }
                }
            }
            catch (Exception E) 
            {
                System.Windows.MessageBox.Show(E.Message, "Erro");
            }
        }

        private void btnCifrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ManipulacaoArquivo manipulacaoarquivo = new ManipulacaoArquivo(rsaBits);

                if (textoOriginal.Text.Length == 0 ||
                                this.textoOriginal.Text.Length > this.tamanhoTexto)
                {
                    throw new ArgumentException("Digite uma mensagem de até " + tamanhoTexto.ToString() + " caracteres!");
                }

                if (!Directory.Exists(edtCaminhoRepositorio.Text))
                {
                    throw new ArgumentException("Não foi encontrado o caminho do repositório de chaves públicas: " +
                                                edtCaminhoRepositorio.Text);
                }

                string arquivoChavePublica = "";

                manipulacaoarquivo.getNomeArquivoChavePublica(edtCaminhoRepositorio.Text, ref arquivoChavePublica);

                if (arquivoChavePublica != "")
                {
                    Cursor = System.Windows.Input.Cursors.Wait;

                    btnCifrar.Content = "Aguarde...";
                    btnCifrar.IsEnabled = false;

                    System.Windows.Forms.Application.DoEvents(); // Libera Mensagens do processamento do Windows

                    string xmlChavePublica = manipulacaoarquivo.carregarConteudo(arquivoChavePublica);

                    if (xmlChavePublica != "")
                    {
                        string bitsString = xmlChavePublica.Substring(0, xmlChavePublica.IndexOf("</RSABits>") + 10);
                        xmlChavePublica = xmlChavePublica.Replace(bitsString, ""); // Limpando parte da sring
                        int bits = Convert.ToInt32(bitsString.Replace("<RSABits>", "").Replace("</RSABits>", ""));

                        UtilizacaoRSA rsa = new UtilizacaoRSA(bits);
                        rsa.mensagem = textoOriginal.Text;
                        rsa.xmlChave = xmlChavePublica;

                        textoCifrado.Text = rsa.Cifrar();
                        textoOriginal.Text = "";
                    }

                }
            }
            catch (Exception E)
            {
                System.Windows.MessageBox.Show(E.Message, "Erro");
            }
            finally
            {
                Cursor = System.Windows.Input.Cursors.Arrow;
                btnCifrar.Content = "Cifrar >>";
                btnCifrar.IsEnabled = true;

            }
        }

        private void btnDecifrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(edtCaminhoRepositorio.Text))
                {
                    throw new Exception("Diretório do caminho do repositório não foi encontrado!");
                }

                ManipulacaoArquivo manipulacaoarquivo = new ManipulacaoArquivo(rsaBits);

                if (textoCifrado.Text.Length == 0)
                {
                    System.Windows.MessageBox.Show("Cole ou carrege algum texto cifrado!", "Erro");
                }
                else
                {
                    string arquivo = "";

                    manipulacaoarquivo.getNomeArquivoChavePrivadaExistente(ref arquivo);

                    if (arquivo != "")
                    {
                        Cursor = System.Windows.Input.Cursors.Wait;

                        btnDecifrar.Content = "Aguarde...";
                        btnDecifrar.IsEnabled = false;

                        System.Windows.Forms.Application.DoEvents(); // Libera Mensagens do processamento do Windows

                        string xmlChavePrivada = manipulacaoarquivo.carregarConteudo(arquivo);

                        if (xmlChavePrivada != "")
                        {
                            string bitsString = xmlChavePrivada.Substring(0, xmlChavePrivada.IndexOf("</RSABits>") + 10);
                            xmlChavePrivada = xmlChavePrivada.Replace(bitsString, ""); // Limpando parte da sring

                            int bits = Convert.ToInt32(bitsString.Replace("<RSABits>", "").Replace("</RSABits>", ""));

                            UtilizacaoRSA rsa = new UtilizacaoRSA(bits);
                            rsa.mensagem = textoCifrado.Text;
                            rsa.xmlChave = xmlChavePrivada;

                            textoOriginal.Text = rsa.Decifrar();
                            textoCifrado.Text = "";
                        }
                    }
                }
            }
            catch (Exception E)
            {
                this.textoOriginal.Text = "";
                System.Windows.MessageBox.Show(E.Message, "Erro");
            }
            finally
            {
                Cursor = System.Windows.Input.Cursors.Arrow;
                btnDecifrar.Content = "<< Decifrar";
                btnDecifrar.IsEnabled = true;

            }
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textoOriginal_TextChanged(object sender, TextChangedEventArgs e)
        {
            setMudancaTextoOriginal();
        }

        private void formPrincipal_Initialized(object sender, EventArgs e)
        {
            edtCaminhoRepositorio.Text = "";

            rsaBits = 128;
            tamanhoTexto = 128;

            sliderTamanhoTexto.Value = tamanhoTexto;
        }

        private void btnProcurarRepositorio_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();

            if (openFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                edtCaminhoRepositorio.Text = openFolder.SelectedPath;
            }
        }

        private void formPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                textoCifrado.IsReadOnly = true;
                edtCaminhoRepositorio.IsReadOnly = true;

                if (File.Exists("Parametros.bin"))
                {
                    Parametros parametros = GetParametros();

                    edtCaminhoRepositorio.Text = parametros.diretorioRepositorio;

                    edtCaminhoTrocaMensagem.Text = parametros.diretorioTrocaMensagem;
                   
                    tamanhoTexto = parametros.tamanhoTexto;
                    sliderTamanhoTexto.Value = this.tamanhoTexto;

                    rsaBits = parametros.rsaBits;

                    switch (this.rsaBits)
                    {
                        case 2048:
                                rdgRSA2048.IsChecked = true;
                                break;
                        case 4096:
                                rdgRSA4096.IsChecked = true;
                                break;
                        case 6144:
                                rdgRSA6144.IsChecked = true;
                                break;
                        default:
                            {
                                rdgRSA2048.IsChecked = true;
                                break;
                            }

                    }

                }
            }
            catch (Exception E)
            {
                System.Windows.MessageBox.Show("Deve-se Excluir o arquivo Parametros.bin manualmente!\n\n"+
                                               E.Message, "Erro");
            }
        }

        private void sliderTamanhoTexto_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tamanhoTexto = Convert.ToInt16(Math.Truncate(sliderTamanhoTexto.Value));
        }

        private void formPrincipal_Closed(object sender, EventArgs e)
        {
            setParametros("TAMANHOTEXTO");
            setParametros("RSABITS");
            setParametros("REPOSITORIO");
            setParametros("TROCAMENSAGEM");
        }

        private void rdgRSA2048_Click(object sender, RoutedEventArgs e)
        {
            rsaBits = 2048; 
        }

        private void rdgRSA4096bits_Click(object sender, RoutedEventArgs e)
        {
            rsaBits = 4096; 
        }

        private void rdgRSA6144bits_Click(object sender, RoutedEventArgs e)
        {
            rsaBits = 6144; 
        }

        private void btnTextoCopiar_Click(object sender, RoutedEventArgs e)
        {
            copiarTextoCifradoAreaTransferencia();
        }

        private void btnColar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string areaTransferencia = System.Windows.Forms.Clipboard.GetText();

                if (areaTransferencia.Trim().Length == 0)
                {
                    throw new Exception("Não há texto na área de transferência!");
                }

                textoCifrado.Text = areaTransferencia;
                textoOriginal.Text = "";
            }
            catch (Exception E)
            {
                System.Windows.MessageBox.Show(E.Message,
                                               "Erro");
            }
        }

        private void btnTextoSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (textoCifrado.Text.Length == 0)
            {
                System.Windows.MessageBox.Show("Não há texto criptografado para ser salvo!",
                                               "Erro");

            }
            else if (!Directory.Exists(edtCaminhoTrocaMensagem.Text))
            {
                System.Windows.MessageBox.Show("Diretório de troca de mensagens não encontrado!",
                                               "Erro");
            }
            else
            {
                string arquivo = "";

                ManipulacaoArquivo manipulacaoarquivo = new ManipulacaoArquivo(rsaBits);
                manipulacaoarquivo.getNomeArquivoMensagemCriptografadaNovo(ref arquivo, edtCaminhoTrocaMensagem.Text);

                if (arquivo != "")
                {
                    manipulacaoarquivo.salvarConteudoEmArquivo(arquivo, textoCifrado.Text);

                    System.Windows.MessageBox.Show("Texto copiado com sucesso em " + arquivo,
                                                   "Gravação em disco");

                }
            }
        }

        private void btnTextoCarregar_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(edtCaminhoTrocaMensagem.Text))
            {
                System.Windows.MessageBox.Show("Diretório de troca de mensagens não encontrado!",
                                               "Erro");
            }
            else
            {
                string arquivo = "";

                ManipulacaoArquivo manipulacaoarquivo = new ManipulacaoArquivo(rsaBits);

                manipulacaoarquivo.getNomeArquivoMensagemCriptografadaExistente(ref arquivo, edtCaminhoTrocaMensagem.Text);

                if (arquivo != "")
                {
                    textoCifrado.Text = manipulacaoarquivo.carregarConteudo(arquivo);
                    textoOriginal.Text = "";
                }
            }
        }

        private void btnProcurarTrocaMensagem_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();

            if (openFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                edtCaminhoTrocaMensagem.Text = openFolder.SelectedPath;
            }

        }

    }
}
