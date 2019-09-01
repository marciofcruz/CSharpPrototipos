using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable()]

public class Parametros
{
    private string diretoriotrocamensagem;
    public string diretorioTrocaMensagem
    {
        get
        { return diretoriotrocamensagem; }
        set
        { diretoriotrocamensagem = value; }

    }

    private string diretoriorepositorio;
    public string diretorioRepositorio
    {
        get
        { return diretoriorepositorio; }
        set
        { diretoriorepositorio = value;}
        
    }

    private int tamanhotexto;
    public int tamanhoTexto
    {
        get
        { return tamanhotexto; }
        set
        { tamanhotexto = value;}
    }

    private int rsabits;
    public int rsaBits
    {
        get
        { return rsabits; }
        set
        { rsabits = value;}
    }

       
}
