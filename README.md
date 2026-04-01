# E-commerce Project

Este projeto de e-commerce tem como objetivo consolidar meus conhecimentos em arquitetura de microserviços utilizando .NET 9 para o back-end e Angular para o front-end. A aplicação é composta por uma API de usuários, uma API de produtos e um front-end dinâmico em Angular, todos integrados com autenticação baseada em JWT (JSON Web Token).

## Funcionalidades

- **API de Usuários**: Gerenciamento de usuários, incluindo criação, autenticação e controle de permissões com JWT.
- **API de Produtos**: Gerenciamento de produtos, com funcionalidades de listagem, adição, edição e exclusão de itens.
- **Front-end em Angular**: Interface interativa que consome as APIs e oferece uma experiência de usuário fluida, com gerenciamento de estado e autenticação.

## Arquitetura

A arquitetura adotada é baseada em microserviços, com cada funcionalidade principal sendo tratada por uma API separada. Além disso, a aplicação segue uma arquitetura em camadas, com separação clara entre a camada de apresentação, aplicação e dados.

### Camadas da Arquitetura:

- **Camada de Apresentação (API)**: Responsável pela comunicação com o cliente (front-end), incluindo a autenticação via JWT.
- **Camada de Aplicação**: Contém a lógica de negócios e a comunicação entre os diferentes microserviços.
- **Camada de Dados**: Responsável pela persistência de dados, utilizando o Entity Framework e banco de dados relacional.

## Próximos Passos

- Menu sanduiche mobile não funciona
- Adicionar imagem na criação do produto
- Adicionar troca de senha por usuário logado
- Adicionar possibilidade de o admin resetar a senha de alguem e obrigalo a mudar a senha no próximo login
