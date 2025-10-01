# \# 🚀 ControlRH: Sistema de Ponto Eletrônico e Gestão Documental

# 

# O \*\*ControlRH\*\* é uma solução web moderna e robusta, desenvolvida para simplificar e automatizar a gestão de ponto eletrônico e a distribuição de documentos corporativos. Ele oferece uma interface intuitiva tanto para o colaborador quanto para o gestor, garantindo precisão nos registros e conformidade legal.

# 

# ---

# 

# \## 🎯 Funcionalidades Principais

# 

# O ControlRH foi projetado em torno de três pilares centrais:

# 

# \### 1. Sistema de Ponto Eletrônico (Marcação)

# 

# \* \*\*Marcação de Ponto Precisa:\*\* Colaboradores podem registrar seus pontos eletronicamente, garantindo a marcação exata de horários (dia e hora).

# \* \*\*Detalhes da Jornada:\*\* O sistema captura e registra:

# &nbsp;   \* \*\*Entrada\*\* e \*\*Saída\*\* (Início e Fim da Jornada).

# &nbsp;   \* \*\*Saída para Intervalo\*\* e \*\*Retorno do Intervalo\*\*.

# \* \*\*Relatórios Comprobatórios:\*\* Geração de relatórios completos e auditáveis que discriminam todas as marcações de ponto para fins de conferência e compliance.

# \* \*\*Exportação de Dados:\*\* Possibilidade de \*\*exportar relatórios\*\* de marcações para formatos comuns (e.g., PDF, Excel).

# 

# \### 2. Gestão Documental e Comunicação

# 

# O sistema serve como um portal centralizado para a distribuição de documentos importantes:

# 

# \* \*\*Holerites:\*\* Distribuição segura e privada dos demonstrativos de pagamento.

# \* \*\*Avisos de Férias:\*\* Envio e confirmação eletrônica de avisos e programação de férias.

# \* \*\*Comunicados da Empresa:\*\* Espaço dedicado para avisos gerais e comunicados internos.

# 

# \### 3. Gestão e Administração Corporativa

# 

# \* \*\*Estrutura de Clientes/Grupos:\*\* Permite à empresa criar \*\*carteiras de clientes\*\* para organizar colaboradores em grupos específicos (Exemplo: "Equipe de Call Center - Banco X").

# \* \*\*Gestão de Horários:\*\* Criação e cadastro de novos modelos de horários de trabalho, permitindo flexibilidade na gestão da jornada.

# \* \*\*Vínculo de Colaboradores:\*\* Ferramentas para cadastrar novos colaboradores e vinculá-los aos grupos e horários de trabalho definidos.

# 

# ---

# 

# \## ⚙️ Stack Tecnológico

# 

# O ControlRH é construído sobre uma arquitetura moderna e escalável, utilizando as seguintes tecnologias:

# 

# \### Backend e Lógica de Negócios

# 

# | Tecnologia | Descrição |

# | :--- | :--- |

# | \*\*C#\*\* | Linguagem de programação robusta, utilizada para desenvolver toda a lógica de negócios e APIs. |

# | \*\*ASP.NET Core MVC\*\* | Framework para o desenvolvimento da aplicação web, utilizando o padrão \*\*Model-View-Controller\*\* para uma separação clara de responsabilidades. |

# | \*\*.NET\*\* | Plataforma de desenvolvimento unificada e de alto desempenho. |

# 

# \### Banco de Dados e ORM

# 

# | Tecnologia | Descrição |

# | :--- | :--- |

# | \*\*MySQL\*\* | Sistema de Gerenciamento de Banco de Dados (SGBD) utilizado para armazenamento seguro de todos os dados de ponto e cadastros. |

# | \*\*Entity Framework Core\*\* | ORM (Object-Relational Mapper) que facilita a interação com o banco de dados MySQL, permitindo que a lógica de dados seja escrita em C# (código orientado a objetos). |

# 

# \### Containerização e Deploy

# 

# | Tecnologia | Descrição |

# | :--- | :--- |

# | \*\*Docker\*\* | Plataforma de containerização que empacota a aplicação (.NET Core) e o banco de dados (MySQL) em ambientes isolados e portáteis, garantindo que o sistema funcione de forma idêntica em qualquer ambiente. |

# | \*\*Docker Compose\*\* | Ferramenta utilizada para definir e executar aplicações multi-container Docker. Permite orquestrar o ambiente completo do \*\*ControlRH\*\* (aplicação web + banco de dados) com um único comando. |

# 

# ---

# 

# \## 💻 Como Executar o Projeto (Docker)

# 

# Para colocar o ControlRH no ar, você só precisa ter o \*\*Docker\*\* e o \*\*Docker Compose\*\* instalados.

# 

# 1\.  \*\*Clone o Repositório:\*\*

# &nbsp;   ```bash

# &nbsp;   git clone \[Link do seu Repositório]

# &nbsp;   cd controlrh

# &nbsp;   ```

# 2\.  \*\*Executar com Docker Compose:\*\*

# &nbsp;   \* Este comando construirá a imagem da aplicação e iniciará os containers (aplicação e MySQL) usando a configuração no `docker-compose.yml`.

# &nbsp;   ```bash

# &nbsp;   docker compose up -d

# &nbsp;   ```

# 3\.  \*\*Acesso:\*\*

# &nbsp;   \* A aplicação estará acessível no seu navegador em: `https://localhost:7158` (ou `http://localhost:5026`).

# 

# ---

# 

# \## 🤝 Contribuição

# 

# @WesleyFer

# 

