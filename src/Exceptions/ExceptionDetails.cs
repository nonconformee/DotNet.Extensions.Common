
namespace nonconformee.DotNet.Extensions.Exceptions;

/// <summary>
/// Specifies the details to include when representing an exception as a string (e.g. for error messages or logging).
/// </summary>
[Flags]
public enum ExceptionDetails
{
    // --------------
    // Common details
    // --------------

    /// <summary>
    /// No exception details.
    /// </summary>
    None = 0x00000000,

    /// <summary>
    /// All type information.
    /// </summary>
    Type = TypeName | TypeNamespace | TypeAssembly,

    /// <summary>
    /// All exception details.
    /// </summary>
    All = 0x0FFFFFFF,

    // -------------------------
    // Exception type properties
    // -------------------------

    /// <summary>
    /// The <see cref="Exception.StackTrace"/> property."/>
    /// </summary>
    StackTrace = 0x00000001,

    /// <summary>
    /// The <see cref="Exception.Source"/> property."/>
    /// </summary>
    Source = 0x00000002,

    /// <summary>
    /// The <see cref="Exception.Message"/> property."/>
    /// </summary>
    Message = 0x00000004,

    /// <summary>
    /// The <see cref="Exception.HResult"/> property."/>
    /// </summary>
    HResult = 0x00000008,

    /// <summary>
    /// The <see cref="Exception.Data"/> property."/>
    /// </summary>
    Data = 0x00000010,

    /// <summary>
    /// The <see cref="Exception.TargetSite"/> property."/>
    /// </summary>
    TargetSite = 0x00000020,

    /// <summary>
    /// The <see cref="Exception.HelpLink"/> property."/>
    /// </summary>
    HelpLink = 0x00000040,

    // -------------------------
    // Exception type reflection
    // -------------------------

    /// <summary>
    /// The type name of the exception (without namespace, without Assembly).
    /// </summary>
    TypeName = 0x00000100,

    /// <summary>
    /// The namespace in which the exception type is defined.
    /// </summary>
    TypeNamespace = 0x00000200,

    /// <summary>
    /// The assembly in which the exception type is defined.
    /// </summary>
    TypeAssembly = 0x00000400,

    /// <summary>
    /// All other properties of the exception type (excluding those already listed in this enumeration), especially those specified by the actual/concrete exception type.
    /// </summary>
    Properties = 0x00000800,
}
