using Bogus;
using ApiNetCoreCliente.Domain.Entities;
using System;

namespace ApiNetCoreCliente.Tests.DataFaker;

public class ClienteFaker : Faker<Cliente>
{
    public ClienteFaker()
    {
        RuleFor(p => p.Id, f => Guid.NewGuid());
        RuleFor(p => p.Idade, f => f.Random.Int(1, 100));
        RuleFor(p => p.Nome, f => f.Name.FullName());
    }
}
