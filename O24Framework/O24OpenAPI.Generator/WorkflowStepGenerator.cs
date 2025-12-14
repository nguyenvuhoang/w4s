using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using O24OpenAPI.Generator.Infrastructure;
using O24OpenAPI.Generator.Models;

[Generator]
public sealed class WorkflowStepGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MethodSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not MethodSyntaxReceiver receiver)
            return;

        var compilation = context.Compilation;

        var workflowStepAttr = compilation.GetTypeByMetadataName(
            "O24OpenAPI.Web.Framework.Attributes.WorkflowStepAttribute"
        );

        if (workflowStepAttr is null)
            return;

        var steps = new List<WorkflowStepInfo>();

        foreach (var methodSyntax in receiver.CandidateMethods)
        {
            var semanticModel = compilation.GetSemanticModel(methodSyntax.SyntaxTree);

            if (semanticModel.GetDeclaredSymbol(methodSyntax) is not IMethodSymbol method)
                continue;

            var attr = method
                .GetAttributes()
                .FirstOrDefault(a =>
                    SymbolEqualityComparer.Default.Equals(a.AttributeClass, workflowStepAttr)
                );

            if (attr is null)
                continue;

            if (method.Parameters.Length == 0)
                continue;

            var stepCode = attr.ConstructorArguments[0].Value?.ToString();
            if (string.IsNullOrWhiteSpace(stepCode))
                continue;

            var inputType = method
                .Parameters[0]
                .Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            steps.Add(new WorkflowStepInfo(stepCode!, inputType));
        }

        // 🔥 LUÔN generate – kể cả steps rỗng
        var source = WorkflowInvokerEmitter.Emit(steps);
        context.AddSource("WorkflowStepInvoker.g.cs", source);
    }
}
