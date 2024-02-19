﻿namespace Mensageria.Entities;

public sealed class Usuario : BaseEntity
{
    public Usuario(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string email,
        string senha,
        string nome,
        string? telefone,
        string? cnpj)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Email = email;
        Senha = senha;
        Nome = nome;
        Telefone = telefone;
        Cnpj = cnpj;
    }

    public string Email { get; private set; }
    public string Senha { get; private set; }
    public string Nome { get; private set; }
    public string? Telefone { get; private set; }
    public string? Cnpj { get; set; }
}
