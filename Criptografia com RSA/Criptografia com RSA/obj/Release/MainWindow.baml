   M S B A M L   `   `   `  ���� O  KCriptografia com RSA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Criptografia_com_RSA.PropertiesCriptografia_com_RSA.Properties  %    Criptografia_com_RSA.MainWindow   ^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    B
Properties-clr-namespace:Criptografia_com_RSA.Properties    -     �Name$  formPrincipal�� � Title$Q KCriptografia assimétrica com RSA - prof. Dirceu e Angelita - Novembro/2012��$	��310��$	��783�� � WindowStartupLocation$ CenterScreen=� � Icon$ favicon.ico�� � 
ResizeMode$ NoResize=�.��� $
��Left=�$	��205��$	��Top=�$	��673��$��	10,10,0,0q� � RowDefinitions �� $	��57* ��� $	��83* ��� $	��55* �.+��� ���	#FFF4F4F5$
��Left=�$��57��$��480,10,-251,0q�$��1��
���   �$	��Top=�$	��270���� ���	#FFF4F4F5$
��Left=�$��95��$��480,71,-77,-21q�$��1��
���   �$	��Top=�$	��270��$��2���� ���	#FFF4F4F5$
��Left=�$��60��$��480,3,-251,0q�
���   �$	��Top=�$	��270��$��2���� -    ��Name$ 
Retangulo1�����	#FFF4F4F5$
��Left=�$	��223��$��	0,3,0,-21q�
���   �$	��Top=�$	��473��$��3���� -    ��Name$ btnGerarChavePrivada��$��Gerar Chave Privada��$��205,86,0,-50q�$	��129��$	��Top=�$
��Left=� K� ZIndex$ 1��$��2���� -   $ 	btnCifrar��$��	Cifrar >>��$��
193,35,0,0q�$��81��$
��Left=�$ 1��$��1��$��22��$	��Top=��� -   $ btnDecifrar��$��<< Decifrar��$��
193,72,0,0q�$��81��$	��Top=�$
��Left=�$ 1��$��1��$��2���� -   $ 	btnFechar��$��Fechar��$��356,86,0,-50q�$	��119��$	��Top=�$
��Left=�	 �� IsCancel	 . $ 1��$��2���� -   
 ��Name$ 
 lblCaptionMensagemOriginal��$��Mensagem Original��$
��Left=�$��6,8,0,0q�$	��Top=�$	��186���� -    ��Name$ textoOriginal��$
��Left=�$	��179��$��
6,31,0,-15q� �� TextWrapping$
 Wrap=�$	��Top=�$	��186��$��3���� -	   $
 lblCaptionMensagemCifrada��$��Mensagem Cifrada��$
��Left=�$��	273,8,0,0q�$	��Top=�$	��186���� -
   $ textoCifrado��$
��Left=�$	��151��$��
277,31,0,0q�$
 Wrap=�$	��Top=�$	��186��$��3���� -   $
 lblTamanhoChave��$"��Nível de segurança do RSA:��$��29��$��487,7,-251,0q�$	��Top=�$
��Bold"� J� RenderTransformOrigin$ 4.8,1&��� -   $
 lblTamanhoMaximoTexto��$��Label��$
��Left=�$��29��$��480,10,-244,0q�$	��Top=�$
��Bold"�$��1��$	��263���� -   $.
 (lblCaptionCaminhoRepositorioChavePublica��$#��Repositório chaves públicas��$
��Left=�$��26��$��480,68,-152,0q�$��1��$	��Top=�$��2��$
��Bold"�$	��171���� -   $ btnProcurarRepositorio��$	��...��$
��Left=�$��716,0,-68,0q�$��2��$��25��$ 0.741,3.679&�$��25��$	��Top=��� -    ��Name$ sliderTamanhoTexto��$
��Left=�$��490,39,-244,0q�$	��Top=�$	��253�� � Minimum$	 128Q� � Maximum$
 1000Q� � SmallChange$ 1Q�$��1���� -   $ edtCaminhoRepositorio��$
��Left=�$��22��$��488,3,-36,0q�$��2��$
 Wrap=�$	��Top=�$	��221��	� -    	�Name$ 
rdgRSA2048��$��	2048 bits��$
��Left=�$��490,36,-91,0q�$	��Top=�$	��100�� d� 	IsChecked$
 TrueY� 	� 	GroupName$	 RSA��	� -   $ 
rdgRSA4096��$��	4096 bits��$
��Left=�$��575,36,-176,0q�$	��Top=�$	��100��$	 RSA��	� -   $ 
rdgRSA6144��$��	6144 bits��$
��Left=�$��665,36,-244,0q�$	��Top=�$��78��$	 RSA���� -   $ btnTextoCopiar��$
��Left=�$��28��$��277,47,0,-20q�$��2��$	��Top=�$��43��$��Copiar���� -   $ btnColar��$��Colar��$
��Left=�$��320,47,0,-17q�$��2��$��40��$��28���� -   $ btnTextoSalvar��$��Salvar��$
��Left=�$��360,47,0,-20q�$��2��$	��Top=�$��38��$��28���� -   $ btnTextoCarregar��$��Carregar��$
��Left=�$��398,47,0,-20q�$��2��$	��Top=�$��65��$��28��$ 0.2,1&��� -   $/
 )lblCaptionCaminhoRepositorioTrocaMensagem��$+��%Repositório troca mensagens cifradas��$
��Left=�$��26��$��480,26,-210,0q�$��2��$	��Top=�$
��Bold"�$	��229���� -   $ edtCaminhoTrocaMensagem��$
��Left=�$��22��$��488,48,-36,-12q�$��2��$
 Wrap=�$	��Top=�$	��221���� -   $ btnProcurarTrocaMensagem��$	��...��$
��Left=�$��716,45,-68,-12q�$��2��$��25��$ 0.741,3.679&�