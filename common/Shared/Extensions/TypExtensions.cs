using System;
using System.Linq;
using System.Reflection;

namespace ItemsAdministration.Common.Shared.Extensions;

public static class TypExtensions
{
    public static Type[] GetTypesBasedOnAttribute<TAttribute>(this Assembly assembly)
    where TAttribute : Attribute =>
    assembly
        .GetTypes()
        .Where(t => t.IsDefined(typeof(TAttribute), inherit: false))
        .ToArray();

    public static (Type DerivedType, Type InterfaceType)[] GetDerivedTypesWithInterfaces<TAttribute>(this Assembly assembly)
        where TAttribute : Attribute =>
        assembly
            .GetTypesBasedOnAttribute<TAttribute>()
            .Select(t => (
                        DerivedType: t,
                        InterfaceType: t.GetImplementedInterface()
                                       ?? throw new ArgumentException($"Not Found {t.CreateInterfaceName()} interface which should suit {t.Name}")))
            .ToArray();

    public static Type? GetImplementedInterface(this Type type)
        => type.GetInterfaces().SingleOrDefault(t => t.Name == t.CreateInterfaceName());

    private static string CreateInterfaceName(this Type type)
        => $"I{type.Name}";
}
