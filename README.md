> [!IMPORTANT]
> Linhas que estão com o icone :red_circle: no final, são observações para auxiliar no desenvolvimento, precisam ser deletadas antes de ser publicada.

> [!IMPORTANT]
> Linhas que estão com o icone :green_circle: no final, são decisões que depende do processo. Deverá ser escolhida entre uma alternativa OU outra e posteriormente deletadas.

> [!WARNING]
> Deletar todas as linhas com os icones vermelhos :red_circle: e verdes :green_circle: após a finalização do README.

Titulo do projeto deverá ser alterado após o “-“ abaixo, mantendo o “# README” :red_circle:

# :globe_with_meridians: README – Titulo do projeto 

## :scroll: Sobre o Projeto 
Breve explicação sobre o projeto, a ideia é tentar fazer com que alguém que não tenha conhecimento sobre o projeto consiga entender o motivo do mesmo ter sido feito e suas responsabilidades. Não precisa ser uma explicação técnica. :red_circle:

O Firma Simples é um sistema ERP (Enterprise Resource Planning) web, desenvolvido para oferecer uma solução completa e integrada para a gestão empresarial. Utiliza tecnologias modernas para proporcionar uma experiência de usuário eficiente e intuitiva.

## :computer: Tecnologias Utilizadas
Aqui iremos mencionar todas as tecnologias usadas no projeto, desde a própria linguagem que será desenvolvida, bancos de dados que estão sendo utilizados. :red_circle:

- **C# e ASP.NET**: Para o desenvolvimento do backend.
- **PostgreSQL**: Como sistema de gerenciamento de banco de dados.
- **MongoDB**: Banco não-relacional para gravação de arquivos.
- **Redis**:  Banco de dados em memória usado como armazenamento de chave-valor.

## :books: Bibliotecas Utilizadas
Aqui iremos mencionar todas as bibliotecas usadas no projeto :red_circle:

- **Entity Framework**: Para ORM (Object-Relational Mapping).
- **AutoMapper**: Para o mapeamento de objetos.
- **NLog**: Para o gerenciamento de logs.
- **FluentValidation**: Para validação de dados.

## :bricks: Arquitetura e Padrões
Breve detalhamento sobre como o projeto foi construído, se foi feito com base em alguma arquitetura especifica, se foi utilizado algum padrão de projeto ou algum design system especifico. :red_circle:

- [Arquitetura MVC (Model-View-Controller)](https://learn.microsoft.com/pt-br/aspnet/core/mvc/overview?view=aspnetcore-8.0).
- [Aurum  - Design System](https://zeroheight.com/8a3643989/p/56ee4e-aurum-web)
- [Dev Express – Blazor UI](https://demos.devexpress.com/blazor/Grid)
- Uso do Git Flow para gerenciamento de versões.

## :clipboard: Pré-requisitos
Defina os requisitos necessários para a execução do projeto. Nessa etapa ainda não iremos rodar o projeto, apenas instalar tudo que for necessário para o seu funcionamento.
Obs: Se o possível, especificar versão do que precisa ser instalado para evitar incompatibilidade. :red_circle:

- **.NET Core 3.1** (SDK)
- **PostgreSQL** (Versão 12.0)
- **MongoDB** (Versão 3.2.21)
- **Node** (Versão 18)
- **IDE** (Visual Studio, Visual Studio Code, etc.)

## :gear: Instalação e Configuração
Após tudo instalado na etapa anterior, aqui iremos detalhar o que precisa fazer para o projeto rodar na máquina. :red_circle:

- Clonar o projeto na máquina.
- Alterar informações dos appsettings para os ip’s locais.
- Abrir o git bash na raiz do projeto, em seguida, executar o comando `npm install`
- Abrir a pasta “x” e executar o comando “y”
- Abrir o projeto com a IDE de sua preferencia e definir a API como projeto inicial.
- Rodar um clean, build e executar o projeto selecionando.

## :postbox: Deploy
**Deploy manual:** :green_circle:

Deverá ter um manual de como é realizado o deploy de forma manual. Desde o publish do projeto, arquivos que deverão ser salvos ou alterados, transferência para os servidores que serão publicados, os servidores que sofreram alteração, etc. Todo o detalhamento para a realização do deploy.

**Deploy via Jenkins:** :green_circle:

 Para realização de deploy via Jenkins, especificar como deverá ser feito. Se possuir parâmetros para build, explicar quais parâmetros devem ser utilizados e uma breve explicação do mesmo. Em seguida, como saber se a esteira teve sucesso ou falha (console output) e a url da esteira.

## :label: Ambientes
1. **Ambiente de produção:**
   - [Aplicação](https://www.questor.com.br)
   - [Swagger](https://www.questor.com.br)
   - [Hangfire](https://www.questor.com.br)

2. **Ambiente de homologação:**
   - [Aplicação](https://www.questor.com.br)
   - [Swagger](https://www.questor.com.br)
   - [Hangfire](https://www.questor.com.br)

## :thumbsup: Contribuição
Defina as etapas que deverão ser seguidas para que seja feita um commit na aplicação. :red_circle:

1. Faça checkout para a branch develop.
2. Crie uma branch seguindo o Git Flow.
3. Faça commit das alterações seguindo o seguinte padrão (`Numero Da Tarefa - DescricaoDaTarefa`) quando trabalhado com tarefas, ou por tipo de commit (`TipoDoComit - Descricao do que foi feito`)
 - Exemplo 1: #123 - Adicionar campo x na tabela y.
 - Exemplo 2: feat - Adicionado campo y na tabela x.
5. Faça push para a branch criada na etapa 2.
6. Faça um pull-request de sua branch para develop.

## :bookmark_tabs: Licença
Defina a licença sob a qual o projeto será disponibilizado (MIT, Apache, GNU, BSD, MPL, AGPL) ou o time responsável juntamente com o gestor/supervisor do projeto. :red_circle:

[Licenciar um repositório - Docs do GitHub](https://docs.github.com/pt/repositories/managing-your-repositorys-settings-and-features/customizing-your-repository/licensing-a-repository)

Este projeto é distribuído sob a licença MIT. A equipe atualmente responsável pelo projeto é Questor, sendo gerenciado por Fulano de Tal.

## :trophy: Link Útil
[Documentação do Github sobre Readme](https://docs.github.com/pt/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax)

[Icones](https://github.com/ikatyang/emoji-cheat-sheet/blob/master/README.md#computer)
