using FluentValidation;
using FluentValidation.Results;

namespace ApiNetCoreCliente.Domain.Entities;

public class Cliente
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }

    public ValidationResult Validar()
        => new ClienteValidation().Validate(this);
}

public class ClienteValidation : AbstractValidator<Cliente>
{
    public ClienteValidation()
    {
        RuleFor(c => c.Nome)
            .MaximumLength(100)
            .WithMessage("Campo Nome deve conter até 100 caracteres.");

        RuleFor(c => c.Nome)
            .MinimumLength(1)
            .WithMessage("Campo Nome é obrigatório.");

        RuleFor(c => c.Idade)
            .GreaterThan(0)
            .WithMessage("Campo Idade deve ser maior ou igual a 1.");
    }
}
