### Informações técnicas do projeto

- Microsserviço desenvolvido em C# com .NET 6
- Utilizado Visual Studio 2022
- Utilizado o ORM Entity Framework Core
- Utilizado o xUnit para escrita dos testes de unidade 
- Documentação swagger 

### Arquitetura da solução
Para o desenvolvimento da aplicação foi utilizada da arquitetura de 3 camadas. Com ela já se tornou possível separar as responsábilidades e ter uma melhor organização da solução sem aumentar a complexidade do projeto desnecessáriamente.

### Como gerar o banco de dados?
Para gerar o banco de dados, basta executar os comandos abaixo no Package Manager Console do Visual Studio. Porém primeiramente é necessário verificar os dados de conexão com o banco de dados no arquivo appsettings.Development.json.
Utilizado o servidor: (localdb)\mssqllocaldb

```
add-migration EstruturaInicialDb -project src\ApiNetCoreCliente.Data
```
```
update-database
```
