using System.Diagnostics.CodeAnalysis;
using src.Features.Shared.Endpoints;
using src.Features.Shared.Interfaces;

namespace src.Utilities;

/// <summary>
/// Helper class providing named Type references (for use with typeof(...)) to make code more readable.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Types
{
    internal static readonly Type Int = typeof(int);
    internal static readonly Type NullableInt = typeof(int?);
    internal static readonly Type Guid = typeof(Guid);
    internal static readonly Type NullableGuid = typeof(Guid?);
    internal static readonly Type IMapper = typeof(IMapper<,,>);
    internal static readonly Type IBaseRepository = typeof(IBaseRepository<>);
    internal static readonly Type IEndpoint = typeof(IEndpoint);
    internal static readonly Type Program = typeof(Program);
}