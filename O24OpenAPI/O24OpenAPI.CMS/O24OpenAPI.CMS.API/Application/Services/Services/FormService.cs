using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.CMS.API.Application.Features.Requests;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;
using O24OpenAPI.Core.Extensions;
using System.Reflection;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

/// <summary>
/// Ctor
/// </summary>
/// <param name="FormRepository"></param>
public partial class FormService(IRepository<Form> FormRepository) : IFormService
{
    private readonly IRepository<Form> _formRepository = FormRepository;

    public Task Delete(string tx_code, string app)
    {
        throw new NotImplementedException();
    }

    public Task<List<FormModel>> GetByApp(string app)
    {
        throw new NotImplementedException();
    }

    public async Task<Form> GetById(int id)
    {
        return await _formRepository.Table.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Form>> GetAll()
    {
        return await _formRepository.Table.ToListAsync();
    }

    public virtual async Task<FormModel> GetByIdAndApp(string formCode, string app)
    {
        FormModel? getForm = await _formRepository
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
                    : JsonConvert.DeserializeObject<RequestModel>(s.MasterData),
            })
            .FirstOrDefaultAsync();

        return getForm;
    }

    public Task<List<RoleCacheModel>> GetRoleCacheByApp(string app)
    {
        throw new NotImplementedException();
    }

    public async Task Insert(Form form)
    {
        await _formRepository.Insert(form);
    }

    public async Task Update(Form form)
    {
        await _formRepository.Update(form);
    }

    public async Task FeedDataRequestMapping()
    {
        List<Form> forms = await GetAll();
        ITypeFinder? _typeFinder = EngineContext.Current.Resolve<ITypeFinder>();
        foreach (Form form in forms)
        {
            if (form.MasterData.NullOrEmpty())
            {
                continue;
            }

            List<FormRequestMappingModel> requestMapping = form.MasterData.JsonConvertToModel<
                List<FormRequestMappingModel>
            >();
            foreach (FormRequestMappingModel item in requestMapping)
            {
                Type entityType =
                    _typeFinder.FindEntityTypeByName(item.TableName)
                    ?? throw new Exception($"Domain table {item.TableName} not found");
                foreach (PropertyInfo info in entityType.GetProperties())
                {
                    item.Data[info.Name] = info.Name.ToLower();
                }
            }
            form.MasterData = requestMapping.ToSerialize();
            await Update(form);
        }
    }
}
