using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.CTH.Domain.AggregatesModel.TestAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Tests;

public class TestRevertCommand : BaseTransactionModel, ICommand<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
}

[CqrsHandler]
public class TestRevertCommandHandler(IRepository<Test> repository)
    : ICommandHandler<TestRevertCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_TEST_REVERT)]
    public async Task<bool> HandleAsync(
        TestRevertCommand request,
        CancellationToken cancellationToken = default
    )
    {
        //Test test = new() { Name = request.Name, Amount = request.Amount };

        //await repository.InsertAsync(test);
        Test test = await repository.Table.Where(s => s.Name == request.Name).FirstOrDefaultAsync(token: cancellationToken);
        await repository.Delete(test);

        return true;
    }
}
