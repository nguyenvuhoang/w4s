using Linh.JsonKit.Json;
using LinKit.Core.Abstractions;
using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.CMS.API.Application.Features.Requests;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;

namespace O24OpenAPI.CMS.API.Application.Features.Forms;

public interface IFormService
{
    Task<FormModel> GetByIdAndApp(string formCode, string app);
}

[RegisterService(Lifetime.Scoped)]
public class FormService(IFormRepository formRepository) : IFormService
{
    public virtual async Task<FormModel> GetByIdAndApp(string formCode, string app)
    {
        FormModel getForm = await formRepository
            .Table.Where(s => s.App.Equals(app.Trim()) && s.FormId.Equals(formCode.Trim()))
            .Select(s => new FormModel
            {
                Id = s.Id,
                App = app,
                FormId = formCode,
                ListLayout = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(
                    s.ListLayout
                ),
                Info = JsonConvert.DeserializeObject<InfoForm>(s.Info),
                MasterData = string.IsNullOrEmpty(s.MasterData)
                    ? null
                    : s.MasterData.FromJson<RequestModel>(),
            })
            .FirstOrDefaultAsync();

        return getForm;
    }
}
