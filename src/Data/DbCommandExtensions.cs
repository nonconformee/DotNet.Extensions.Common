
using System.Data;

namespace nonconformee.DotNet.Extensions.Data;

/// <summary>
/// Provides extension methods for <see cref="IQueryable{T}"/>.
/// </summary>
public static class DbCommandExtensions
{
    /// <summary>
    /// Adds a command parameter to the command.
    /// </summary>
    /// <param name="command">The command. Cannot be <see langword="null"/>.</param>
    /// <param name="name">The name of the parameter. Cannot be <see langword="null"/> or an empty string.</param>
    /// <param name="value">The value of the parameter. Can be <see langword="null"/>.</param>
    /// <returns>The created and added parameter.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="command"/>is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or consists only of whitespace.</exception>
    /// <remarks>If <paramref name="value"/> is <see langword="null"/>, <see cref="DBNull"/> is used for the value added to the parameter.
    /// The parameter is not only created but also added to the command.</remarks>
    public static IDbDataParameter AddParameter(this IDbCommand command, string name, object? value = null)
    {
        if(command is null) throw new ArgumentNullException(nameof(command));
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("The parameter name cannot be null or consist only of whitespaces.", nameof(name));

        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value is null ? DBNull.Value : value;
        command.Parameters.Add(parameter);
        return parameter;
    }

    /// <summary>
    /// Adds a command parameter to the command.
    /// </summary>
    /// <param name="command">The command. Cannot be <see langword="null"/>.</param>
    /// <param name="name">The name of the parameter. Cannot be <see langword="null"/> or an empty string.</param>
    /// <param name="dbType">The database-side data type.</param>
    /// <param name="value">The value of the parameter. Can be <see langword="null"/>.</param>
    /// <returns>The created and added parameter.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="command"/>is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or consists only of whitespace.</exception>
    /// <remarks>If <paramref name="value"/> is <see langword="null"/>, <see cref="DBNull"/> is used for the value added to the parameter.
    /// The parameter is not only created but also added to the command.</remarks>
    public static IDbDataParameter AddParameter(this IDbCommand command, string name, DbType dbType, object? value = null)
    {
        if(command is null) throw new ArgumentNullException(nameof(command));
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("The parameter name cannot be null or consist only of whitespaces.", nameof(name));

        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.DbType = dbType;
        parameter.Value = value is null ? DBNull.Value : value;
        command.Parameters.Add(parameter);
        return parameter;
    }
}
