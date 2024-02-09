using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace NullableExports;

struct FileWithImport
{
    public AdditionalText data;
    public AdditionalText import;
}

[Generator]
public class PreloadGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var resourceData = context.AdditionalTextsProvider.Where(file => !file.Path.EndsWith(".tres") && !file.Path.EndsWith(".tscn"));
        var resouceImports = resourceData.Collect().SelectMany((files, token) => {
            List<FileWithImport> importers = [];
            Dictionary<string, AdditionalText> pendingImports = [];
            Dictionary<string, AdditionalText> pendingData = [];
            foreach (var file in files)
            {
                if (file.Path.EndsWith(".import"))
                {
                    string dataPath = Path.GetFileNameWithoutExtension(file.Path);
                    if (!pendingData.ContainsKey(dataPath))
                    {
                        pendingImports.Add(file.Path, file);
                        continue;
                    }
                    importers.Add(new()
                    {
                        data = pendingData[dataPath],
                        import = file
                    });
                    pendingData.Remove(dataPath);
                }
                else
                {
                    string importPath = file.Path + ".import";
                    if (!pendingImports.ContainsKey(importPath))
                    {
                        pendingData.Add(file.Path, file);
                        continue;
                    }
                    importers.Add(new()
                    {
                        data = file,
                        import = pendingImports[importPath]
                    });
                    pendingImports.Remove(importPath);
                }
            }
            return importers;
        }).Select((file, token) => {
            //Import the file
            return file.data;
        }).Collect().Select((files, token) => {
            Dictionary<string, AdditionalText> lookupTable = [];
            foreach (var file in files)
            {
                lookupTable.Add(file.Path, file);
            }
            return lookupTable;
        });
        
        var resourceFiles = context.AdditionalTextsProvider.Where(file => file.Path.EndsWith(".tres") || file.Path.EndsWith(".tscn")).Collect();
        var resourceTree = resouceImports.Combine(resourceFiles).Select((files, token) => {
            Dictionary<string, AdditionalText> tree = files.Left;
            foreach (var file in files.Right)
            {
                tree.Add(file.Path, file);
            }
            return tree;
        });
       
        var syntax = context.SyntaxProvider
            .ForAttributeWithMetadataName("Sanicball.PreloadAttribute", SyntaxFilter, SyntaxTransformer)
            .Collect()
            .SelectMany((fields, token) => {
                Dictionary<string, ClassAndFields> sorted = [];
                foreach (var field in fields)
                {
                    string fullName = field.containingNamespace + field.className;
                    if (!sorted.ContainsKey(fullName))
                    {
                        sorted.Add(fullName, new()
                        {
                            containingNamespace = field.containingNamespace,
                            className = field.className,
                            fields = []
                        });
                    }

                    ClassAndFields parent = sorted[fullName];
                    parent.fields.Add(field);
                }
                return sorted.Values;
            }).Combine(resourceTree);
        context.RegisterSourceOutput(syntax, (ctx, compilationContext) => {
            var (classData, resourceTree) = compilationContext;

            int files = resourceTree.Count;
            string avalible = string.Join("\n", resourceTree.Select(res => res.Key));

            StringBuilder builder = new();
            builder.Append($$"""
            namespace Sanicball.{{classData.containingNamespace}};

            public partial class {{classData.className}}
            {
                static {{classData.className}}()
                {
            """);

            foreach (var field in classData.fields)
            {
                if (resourceTree.ContainsKey(field.path))
                {
                    builder.Append($"{field.fieldName} = Godot.GD.Load<{field.fieldType}>(\"{field.path}\");");
                    continue;
                }

                DiagnosticDescriptor description = new(
                    "GD0000",
                    "Invalid Resource File Path",
                    $"File path: {field.path} does not exist, {avalible} files exist",
                    "Compilation",
                    DiagnosticSeverity.Error,
                    true,
                    $"File path: {field.path} does not exist, {avalible} files exist"
                );
                Diagnostic invalidPath = Diagnostic.Create(description, field.location);
                ctx.ReportDiagnostic(invalidPath);
            }

            builder.Append("""
                }
            }
            """);

            ctx.AddSource($"{classData.className}.g.cs", builder.ToString());
        });

        context.RegisterPostInitializationOutput(ctx => {
            ctx.AddSource("PreloadAttribute.g.cs", @"
                namespace Sanicball;
                #pragma warning disable CS9113 // Parameter is unread.
                [System.AttributeUsage(System.AttributeTargets.Field)]
                public class PreloadAttribute(string path) : System.Attribute
                {
                }
                #pragma warning restore CS9113 // Parameter is unread.
            ");
        });
    }

    private static bool SyntaxFilter(SyntaxNode node, CancellationToken _cancellationToken)
    {
        return true;
    }

    private static FieldDescriptor SyntaxTransformer(GeneratorAttributeSyntaxContext context, CancellationToken _cancellationToken)
    {
        return new(){
            containingNamespace = context.TargetSymbol.ContainingNamespace.Name,
            className = context.TargetSymbol.ContainingType.Name,
            location = context.TargetNode.GetLocation(),
            fieldName = context.TargetSymbol.Name,
            fieldType = ((IFieldSymbol)context.TargetSymbol).Type!.Name,
            path = (string)context.Attributes[0].ConstructorArguments[0].Value!
        };
    }
}

internal struct ClassAndFields
{
    public string containingNamespace;
    public string className;
    public List<FieldDescriptor> fields;
}

internal struct FieldDescriptor
{
    public string containingNamespace;
    public string className;

    public Location location;
    public string fieldName;
    public string fieldType;
    public string path;
}