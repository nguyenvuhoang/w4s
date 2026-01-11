using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using O24OpenAPI.Generator.Infrastructure;
using O24OpenAPI.Generator.Models;

[Generator]
public sealed class WorkflowStepGenerator : ISourceGenerator
{
    private const string IQueryInterfaceName = "LinKit.Core.Cqrs.IQuery";

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MethodSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not MethodSyntaxReceiver receiver)
            return;

        var compilation = context.Compilation;

        // Lấy Type Symbol của các Attribute và Interface cần thiết
        var workflowStepAttr = compilation.GetTypeByMetadataName(
            "O24OpenAPI.Framework.Attributes.WorkflowStepAttribute"
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

            if (attr is null || method.Parameters.Length == 0)
                continue;

            var stepCode = attr.ConstructorArguments[0].Value?.ToString();
            if (string.IsNullOrWhiteSpace(stepCode))
                continue;

            var inputTypeSymbol = method.Parameters[0].Type;
            var inputTypeName = inputTypeSymbol.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat
            );

            // Kiểm tra xem inputType có thực thi IQuery hay không
            bool isQuery = false;
            isQuery = inputTypeSymbol.AllInterfaces.Any(i =>
                i.OriginalDefinition.ToDisplayString().Contains(IQueryInterfaceName)
            );

            steps.Add(new WorkflowStepInfo(stepCode!, inputTypeName, isQuery));
        }

        var source = WorkflowInvokerEmitter.Emit(steps);
        context.AddSource("WorkflowStepInvoker.g.cs", source);
    }
}
