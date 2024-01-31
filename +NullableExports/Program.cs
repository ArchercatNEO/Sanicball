using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace SourceGenerator
{
    [Generator]
    public class HelloSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var nullableContainingTypes = context.Compilation.Assembly.Modules
                .Select(module => module.GlobalNamespace)
                .SelectMany(rootNamespace => rootNamespace.GetTypeMembers())
                .Where(type => type.GetMembers().Where(member => 
                    member.GetAttributes().Any(attribute => attribute.AttributeClass!.Name == "ExportAttribute")
                ).Any());

            foreach (var nullableType in nullableContainingTypes)
            {
                StringBuilder configurationWarnings = new();

                configurationWarnings.Append($@"
                    public partial class {nullableType.Name}
                    {{
                ");
                
                configurationWarnings.Append("""
                public override string[] _GetConfigurationWarnings()
                {
                    List<string> warnings = ["Bad"];
                """);
                   

                configurationWarnings.Append(" return warnings.ToArray();} } ");
                    
                


                context.AddSource(nullableType.Name + ".g.cs", configurationWarnings.ToString());
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}