🚀 ControlRH: Sistema de Ponto Eletrônico e Gestão Documental

O ControlRH é uma solução web moderna e robusta, desenvolvida para simplificar e automatizar o controle de ponto eletrônico e a distribuição de documentos corporativos.
Com uma interface intuitiva tanto para colaboradores quanto para gestores, garante precisão nos registros e conformidade legal.

🎯 Funcionalidades

O ControlRH foi projetado em torno de três pilares principais:

1. Sistema de Ponto Eletrônico

Marcação de Ponto Precisa: Registro eletrônico de horários de entrada, saída e intervalos.

Detalhes da Jornada: Captura de:

Entrada e Saída (início e fim da jornada).

Saída e Retorno do Intervalo.

Relatórios Auditáveis: Emissão de relatórios completos para conferência e compliance.

Exportação de Dados: Geração de relatórios em formatos comuns (PDF, Excel).

2. Gestão Documental e Comunicação

Holerites: Distribuição segura e individualizada de demonstrativos de pagamento.

Avisos de Férias: Envio e confirmação eletrônica da programação de férias.

Comunicados: Centralização de avisos e informativos da empresa.

3. Gestão Corporativa

Estrutura de Clientes/Grupos: Organização de colaboradores em grupos específicos (ex.: Equipe Call Center – Banco X).

Gestão de Horários: Criação e cadastro de modelos de jornada de trabalho.

Vínculo de Colaboradores: Cadastro de colaboradores com associação a grupos e jornadas.

⚙️ Stack Tecnológico

O ControlRH utiliza uma arquitetura moderna e escalável, baseada em:

Backend
Tecnologia	Descrição
C#	Linguagem robusta para APIs e lógica de negócios.
ASP.NET Core MVC	Framework web baseado no padrão MVC.
.NET	Plataforma de desenvolvimento unificada e de alto desempenho.
Banco de Dados e ORM
Tecnologia	Descrição
MySQL	Banco de dados para armazenamento seguro de registros.
Entity Framework Core	ORM que simplifica a interação com o MySQL via C#.
Containerização e Deploy
Tecnologia	Descrição
Docker	Containerização da aplicação e do banco de dados.
Docker Compose	Orquestração multi-container (app + MySQL).
💻 Executando o Projeto com Docker

Pré-requisitos: Docker e Docker Compose instalados.

Clone o Repositório:

git clone [link-do-seu-repositorio]
cd controlrh


Suba os Containers:

docker compose up -d


Acesse a Aplicação:

https://localhost:7158

ou http://localhost:5026

🤝 Contribuição

Feito com dedicação por @WesleyFer.
Sinta-se à vontade para contribuir com melhorias e sugestões.