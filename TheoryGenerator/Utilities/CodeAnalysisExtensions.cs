using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Reductech.Utilities.TheoryGenerator.Utilities
{

public static class CodeAnalysisExtensions
{
    public static IEnumerable<INamespaceSymbol> GetAllNamespaces(
        this INamespaceSymbol namespaceSymbol)
    {
        return namespaceSymbol.DescendantsAndSelf(x => x.GetNamespaceMembers());
    }

    public static IEnumerable<ITypeSymbol> GetAllTypes(this INamespaceSymbol namespaceSymbol)
    {
        return namespaceSymbol
            .GetAllNamespaces()
            .SelectMany(x => x.GetTypeMembers())
            .SelectMany(x => x.DescendantsAndSelf(y => y.GetTypeMembers()));
    }

    public static bool SelfOrDescendantHasAttribute(
        this ITypeSymbol typeSymbol,
        INamedTypeSymbol attributeClass)
    {
        return typeSymbol.DescendantsAndSelf(x => x.BaseType)
            .Any(
                x => x.GetAttributes()
                    .Any(
                        a =>
                            a.AttributeClass != null &&
                            a.AttributeClass.Equals(attributeClass, SymbolEqualityComparer.Default)
                    )
            );
    }

    public static bool HasName(this AttributeData attributeData, string name)
    {
        return attributeData.AttributeClass != null
            && attributeData.AttributeClass.Name.Equals(name);
    }

public static bool SelfOrDescendantHasAttributeWithName(
        this ITypeSymbol typeSymbol,
        string name)
    {
        return typeSymbol.DescendantsAndSelf(x => x.BaseType)
            .Any(
                x => x.GetAttributes()
                    .Any(
                        a => a.HasName(name)
                    )
            );
    }
}

}
