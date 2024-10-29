# Feedback do Instrutor

#### 28/10/24 - Revisão Inicial - Eduardo Pires

## Pontos Positivos:

- Boa separação de responsabilidades.
- Arquitetura enxuta
- Demonstrou conhecimento em Identity e JWT
- Usou conceito de models e handlers de forma inteligente
- Soube aplicar a validação de claims ou proprietario do post
- Mostrou entendimento do ecossistema de desenvolvimento em .NET

## Pontos Negativos:

- Não vi necessidade de uma camada Core e Data, poderiam ser uma única.
- Faltou representar a entidade Autor diretamente ligado ao user (identity)
- A API não lida com comentários

## Sugestões:

- Evoluir o projeto para as necessidades solicitadas no escopo.

## Problemas:

- Não consegui executar a aplicação de imediato na máquina. É necessário que o Seed esteja configurado corretamente, com uma connection string apontando para o SQLite.

  **P.S.** As migrations precisam ser geradas com uma conexão apontando para o SQLite; caso contrário, a aplicação não roda.
