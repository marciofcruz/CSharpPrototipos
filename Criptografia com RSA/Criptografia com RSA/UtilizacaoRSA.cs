using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections;

//[System.Serializable()]

public class UtilizacaoRSA
{
    private RSACryptoServiceProvider provider;
    private int bitsRSA;
    private int tamanhoChave;
    private int tamanhoMaximoTexto;

    private string xmlchave;
    public string xmlChave // contém o XML da chave privada ou pública
    {
        get
        {
            return xmlchave;
        }
        set
        {
            xmlchave = value;
            provider.FromXmlString(value);
        }
    }


    public string mensagem; // contém o texto original ou o texto cifrado

    public UtilizacaoRSA(int abitsRSA) // Construtor com sobrecarga
    {
        tamanhoChave = abitsRSA / 8;
        tamanhoMaximoTexto = tamanhoChave - 42;

        bitsRSA = abitsRSA;

        provider = new RSACryptoServiceProvider(bitsRSA);
    }


    public string Cifrar()
    {
        try
        {
            byte[] bytes = Encoding.UTF32.GetBytes(mensagem); // vamos ter que criptografar cada byte da Mensagem

            int tamanhoArrayBytes = bytes.Length;
            int loops = tamanhoArrayBytes / tamanhoMaximoTexto;

            StringBuilder stringCifrada = new StringBuilder();

            for (int i = 0; i <= loops; i++)
            {
                byte[] temp = new byte[(tamanhoArrayBytes - tamanhoMaximoTexto * i > tamanhoMaximoTexto) ? tamanhoMaximoTexto : tamanhoArrayBytes - tamanhoMaximoTexto * i];

                Buffer.BlockCopy(bytes, tamanhoMaximoTexto * i, temp, 0, temp.Length);

                byte[] byteCifrado = provider.Encrypt(temp, true);

                stringCifrada.Append(Convert.ToBase64String(byteCifrado));
            }

            string retorno = stringCifrada.ToString();

            if (retorno.Trim().Length == 0)
            {
                throw new Exception("Não foi possível criptografar a mensagem!");
            }


            return retorno;
        }
        catch (Exception E)
        {
            throw new Exception("Erro ao criptografar mensagem com chave de " + bitsRSA.ToString() + " bits!\n\n" +
                                "Mensagem original do erro:\n" + E.Message);
        }
    }

    public string Decifrar()
    {
        try
        {
            int baseBloco64 = (tamanhoChave % 3 != 0) ? ((tamanhoChave / 3) * 4) + 4 : (tamanhoChave / 3) * 4;
            int loops = mensagem.Length / baseBloco64;

            ArrayList arrayDecifrado = new ArrayList();

            for (int i = 0; i < loops; i++)
            {
                byte[] encryptedBytes = Convert.FromBase64String(mensagem.Substring(baseBloco64 * i, baseBloco64));

                arrayDecifrado.AddRange(provider.Decrypt(encryptedBytes, true));
            }

            string retorno = Encoding.UTF32.GetString(arrayDecifrado.ToArray(Type.GetType("System.Byte")) as byte[]);

            if (retorno.Trim().Length == 0)
            {
                throw new Exception("Não foi possível descriptografar a mensagem!");
            }

            return retorno;
        }
        catch (Exception E)
        {
            throw new Exception("Não foi possível descriptografar mensagem.\n"+
                                "Esta mensagem não foi cifrada com a chave pública que faz par a esta chave privada!\n\n"+
                                "Mensagem original do erro:\n"+E.Message);
        }
    }

    public void gerarChavePublicaePrivada(ref string chavePrivada, ref string chavePublica)
    {
        chavePrivada = "<RSABits>" +
                        bitsRSA.ToString() +
                        "</RSABits>" +
                        provider.ToXmlString(true); // true identifica a inserção de conteúdo Privado

        chavePublica = "<RSABits>" +
                        bitsRSA.ToString() +
                        "</RSABits>" +
                        provider.ToXmlString(false);
    }

}
