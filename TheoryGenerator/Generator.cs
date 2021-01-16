using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Reductech.Utilities.TheoryGenerator.Utilities;
using static Reductech.Utilities.TheoryGenerator.Constants;

namespace Reductech.Utilities.TheoryGenerator
{

[Generator]
public class Generator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) { }

    public void Execute(GeneratorExecutionContext context) //TODO use syntax receiver
    {
        //#if DEBUG
        //            if (!Debugger.IsAttached)
        //            {
        //                Debugger.Launch();
        //            }
        //#endif

        if (!(context.Compilation is CSharpCompilation csc &&
              csc.SyntaxTrees[0].Options is CSharpParseOptions options))
            return;

        var syntaxTrees = new List<SyntaxTree>();

        var hasIgnoreAttribute = context.Compilation.Assembly.GetAttributes()
            .Any(x => x.AttributeClass.Name == DontAddAutoTheoryNamespaceAttribute);

        foreach (var (fileName, text) in DefaultFiles.StaticFiles)
        {
            var sourceBase = SourceText.From(text, Encoding.UTF8);

            syntaxTrees.Add(CSharpSyntaxTree.ParseText(text, options));

            if(!hasIgnoreAttribute)
                context.AddSource(fileName, sourceBase);
        }

        var compilation = context.Compilation.AddSyntaxTrees(syntaxTrees.ToArray());

        var allTypes = compilation.Assembly.GlobalNamespace.GetAllTypes().ToList();

        var allPropertiesInConcreteClasses = allTypes
                .OfType<INamedTypeSymbol>()
                .Where(x => !x.IsAbstract)
                .SelectMany(
                    concreteType =>
                        concreteType.DescendantsAndSelf(t => t.BaseType)
                            .SelectMany(
                                t => t.GetMembers()
                                    .OfType<IPropertySymbol>()
                                    .Where(x => !x.IsAbstract)
                            )
                            .Select(property => (concreteType, property))
                )
            ;

        var generateTheoryProperties = new List<GenerateTheoryProperty>();

        foreach (var (concreteType, propertySymbol) in allPropertiesInConcreteClasses)
        {
            var newProperties = FindGenerateTheoryProperties(
                context,
                propertySymbol,
                concreteType
            );

            generateTheoryProperties.AddRange(newProperties);
        }

        var classes = generateTheoryProperties.Distinct().GroupBy(x => x.TestClass)
            .Select(x => (Class: x.Key, Properties: x.ToList()))
            .ToList();

        var extraClasses = allTypes
            .OfType<INamedTypeSymbol>()
            .Where(x => !x.IsAbstract)
            .Where(x => x.SelfOrDescendantHasAttributeWithName(UseTestOutputHelperAttribute))
            .Except(classes.Select(x => x.Class))
            .ToList();

        classes.AddRange(extraClasses.Select(x => (x, new List<GenerateTheoryProperty>())));

        foreach (var (testClass, properties) in classes)
        {
            var testCases      = properties.Select(GetCode).Select(x=>x.Trim()).Distinct().ToList();
            var testCaseString = string.Join("\r\n\r\n", testCases);

            var code = $@"
//<autogenerated />

namespace {testClass.ContainingNamespace.ToDisplayString()}
{{
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using {AutoTheory};


public partial class {testClass.Name}
    {{
        public ITestOutputHelper TestOutputHelper {{ get; }}

        public {testClass.Name}(ITestOutputHelper testOutputHelper)
        {{
            TestOutputHelper = testOutputHelper;
            OnInitialized();
        }}

        partial void OnInitialized();

        {testCaseString}
    }}
}}";

            var testClassSource = SourceText.From(code, Encoding.UTF8);
            context.AddSource($"{testClass.Name}.cs", testClassSource);
        }
    }

    private static string GetCode(GenerateTheoryProperty property)
    {
        var attribute    = property.Attribute;
        var isAsync      = property.IsAsync;
        var propertyName = property.Property.Name;

        var testName = attribute.ConstructorArguments.First().Value.ToString();

        var category = attribute.NamedArguments
            .Where(x => x.Key == "Category")
            .Select(x => x.Value.ToCSharpString())
            .FirstOrDefault();

        var interfaceName = isAsync ? IAsyncTestInstance : ITestInstance;

        var categoryString = category == null ? "" : $"\r\n[Trait(\"Category\", \"{category}\")]";

        var staticMethodName = testName + "Data";

        var outputType = isAsync ? "async Task" : "void";
        var runText    = isAsync ? "await instance.RunAsync" : "instance.Run";

        var s =
            $@"#region {testName}
        [Theory]
        [MemberData(nameof({staticMethodName}))]{categoryString}
        public {outputType} {testName}({interfaceName} instance)
        {{
            {runText}(TestOutputHelper);
        }}

        public static TheoryData<{interfaceName}> {staticMethodName}()
        {{
            var data = new TheoryData<{interfaceName}>();
			var instance = new {property.TestClass}(null);
			var cases = instance.{propertyName};
			if(cases == null)
			{{
                data.Add(new NullTestInstance());
			}}
			else
			{{
				foreach (var test in cases)
					data.Add(test);

				if (!data.Any())
					data.Add(new SkipTestInstance());
            }}
			return data;
        }}
#endregion {testName}";

        return s;
    }

    private static IEnumerable<GenerateTheoryProperty> FindGenerateTheoryProperties(
        GeneratorExecutionContext context,
        IPropertySymbol propertySymbol,
        INamedTypeSymbol containingType)
    {
        var descendants = propertySymbol.DescendantsAndSelf(x => x.OverriddenProperty).ToList();

        var attributes = descendants.SelectMany(
            d =>
                d.GetAttributes()
                    .Where(
                        a =>
                        a.HasName(GenerateTheoryAttribute) || a.HasName(GenerateAsyncTheoryAttribute)
                    )
        );

        foreach (var attributeData in attributes)
        {
            Debug.Assert(
                attributeData.AttributeClass != null,
                "attributeData.AttributeClass != null"
            );

            var isAsync =
                attributeData.HasName(GenerateAsyncTheoryAttribute);

            var expectedInstanceSymbol = isAsync ? IAsyncTestInstance : ITestInstance;

            var allInterfaces =
                propertySymbol.Type.DescendantsAndSelf(x => x.AllInterfaces)
                    .Distinct(SymbolEqualityComparer.Default)
                    .OfType<INamedTypeSymbol>()
                    .ToList();

            var testInstanceName =
                allInterfaces
                    .Where(
                        i =>
                            i.IsGenericType &&
                            i.ConstructUnboundGenericType().Name.Equals("IEnumerable") &&
                            i.TypeArguments.Length == 1 &&
                            i.TypeArguments.Single()
                                .DescendantsAndSelf(x => x.AllInterfaces)
                                .Any(x=>x.Name.Equals(expectedInstanceSymbol))
                    )
                    .Select(x => x.TypeArguments.Single().Name)
                    .FirstOrDefault();

            if (testInstanceName == null)
            {
                const string messageCode = "TG001";

                var message =
                    $"Properties implementing {attributeData.AttributeClass.Name} should return IEnumerable<{expectedInstanceSymbol}>";

                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            messageCode,
                            message,
                            message,
                            "Testing",
                            DiagnosticSeverity.Error,
                            true
                        ),
                        propertySymbol.Locations.FirstOrDefault()
                    )
                );
            }
            else
            {
                yield return new GenerateTheoryProperty(
                    propertySymbol,
                    attributeData,
                    isAsync,
                    testInstanceName,
                    containingType
                );
            }
        }
    }
}

internal readonly struct GenerateTheoryProperty
{
    public GenerateTheoryProperty(
        IPropertySymbol property,
        AttributeData attribute,
        bool isAsync,
        string testInstanceName,
        INamedTypeSymbol testClass)
    {
        Property         = property;
        Attribute        = attribute;
        IsAsync          = isAsync;
        TestInstanceName = testInstanceName;
        TestClass        = testClass;
    }

    public IPropertySymbol Property { get; }
    public AttributeData Attribute { get; }
    public bool IsAsync { get; }
    public string TestInstanceName { get; }
    public INamedTypeSymbol TestClass { get; }
}

}
