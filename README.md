# Simples e prático

Totalmente ao oposto do [mem00x](https://github.com/JonasUliana/mem00x), essa API visa unicamente a simplicidade para realizar leitura e escrita em determinados endereços de memória de outro processo.

### Features

Você pode ler e escrever em processos de 32 bits e 64 bits, dependendo da *role* do usuário que iniciou a aplicação cliente da **memmin**. A notação de endereço pode ser feita com binários literais, endereçamentos binários e hexadecimais padrões.

A estrutura de controle da carga de trabalho fica delegada para instâncias de código não gerenciavel contidas na biblioteca dinâmica **kernel32.dll**, possibilitando interações com usuários de baixo privilégios de leiutra e escrita.

### Exemplos

Existe um pequeno exemplo dentro do diretório de solução `Uiana.Library.Memmim.Sample`, onde é apresentado ambas as operações descritas acima. Lembre-se de alterar o endereço definido pela variável `address` e também o nome do processo na `processName`, também é necessário a alteração da assinatura de tipo da leiutra, recomenda-se a utilização dos primitivos ou palavras-chaves destes.

> Nota: A aplicação de testes necessita de uma instância com privilégios elevados para depuração.



### Dependências

Para compilar, e gerar uma build da bilbioteca é necessário um ambiente de desenvolvimento com o .NET Framework 4.6 ou superior instalado na maquina host, juntamente com o utilitário completo do MSBUILD.

> Windows XP SP1 e Windows Server 2003 **é somente suportado na [versão 0.0.0.1](https://github.com/JonasUliana/memmin-core/releases/tag/0.0.0.1)**, devido a limitações de API fornecidas por kernel32.



### Contribuição

Toda e qualquer contribuição é bem vinda, desde a criação de uma *issue* até seu PR, que é muito bem aceito, especialmente em contribuições de programação dinâmica para notação dos tipos enumerados da leitura.

### Licença

Todo o código está sob a lincença [WTFPL](http://www.wtfpl.net/).