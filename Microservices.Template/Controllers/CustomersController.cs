//
//
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using ShoppingApp.Core.Common.Models;
// using Microservices.Template.Services;
//
// namespace Microservices.Template.Controllers;
//
// [Route("api/[controller]")]
// [ApiController]
//
// public class TemplatesController : ControllerBase
// {
//     private readonly ITemplatesRepository _repository;
//
//     public TemplatesController(ITemplatesRepository repository)
//     {
//         _repository = repository;
//     }
//     
//     [HttpPost("Templates-list")]
//     public async Task<ActionResult<ApiResponse<List<ListItem>>>> List(string? searchQuery = null, Dictionary<string, object>? args = null)
//     {
//         return await _repository.List(searchQuery, args);
//     }
//     
//     
//     [HttpPost("create-Template")]
//     public async Task<ActionResult<ApiResponse<ModifyTemplateDto>>> Create(ModifyTemplateDto request)
//     {
//         return await _repository.Create(request);
//     }
//
//     [HttpGet("get-Template/{id}")]
//     public async Task<ActionResult<ApiResponse<TemplateDto>>> Get(string id)
//     {
//         return await _repository.Get(id);
//     }
//
//     [HttpGet("get-Template-to-modify/{id}")]
//     public async Task<ActionResult<ApiResponse<ModifyTemplateDto>>> GetTModify(string id)
//     {
//         return await _repository.GetModifyRecord(id);
//     }
//
//     [HttpPut("update-Template")]
//     public async Task<ActionResult<ApiResponse<ModifyTemplateDto>>> Update(ModifyTemplateDto request)
//     {
//         return await _repository.Update(request);
//     }
//
//     [HttpPut("update-Template-activation/{id}")]
//     public async Task<ActionResult<ApiResponse<TemplateDto>>> UpdateActivation(string id)
//     {
//         return await _repository.UpdateActivation(id);
//     }
//
//     [HttpDelete("delete-Template/{id}")]
//     public async Task<ActionResult<ApiResponse<bool>>> Delete(string id)
//     {
//         return await _repository.Delete(id);
//     }
// }