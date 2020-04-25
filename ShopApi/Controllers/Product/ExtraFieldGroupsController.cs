using Microsoft.AspNetCore.Mvc;
using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method.Token;
using ShopApi.Services.Product;

namespace ShopApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtraFieldGroupsController : ControllerBase
    {
        private readonly ExtraFieldGroupService _extraFieldGroupService;
        private readonly Token _token;

        public ExtraFieldGroupsController(DatabaseContext context)
        {
            _token = new Token(context);
            _extraFieldGroupService = new ExtraFieldGroupService(context);
        }

        [HttpGet]
        public ActionResult<CrudOperationDs> GetExtraFieldGroups()
        {
            return Ok(_extraFieldGroupService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CrudOperationDs> GetExtraFieldGroup(int id)
        {
            var extraFieldGroup = _extraFieldGroupService.GetById(id);
            if (extraFieldGroup == null)
                return NotFound();
            return Ok(extraFieldGroup);
        }

        [HttpPut("{id}")]
        public ActionResult<CrudOperationDs> UpdateExtraFieldGroup(int id, ExtraFieldGroupDs extraFieldGroup)
        {
            if (id != extraFieldGroup.Id)
                return BadRequest();
            if (!_extraFieldGroupService.Exist(extraFieldGroup.Id))
                return NotFound();
            return Ok(_extraFieldGroupService.Update(extraFieldGroup));
        }

        [HttpPost]
        public ActionResult<CrudOperationDs> AddExtraFieldGroup(ExtraFieldGroupDs extraFieldGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_extraFieldGroupService.Add(extraFieldGroup));
        }

        [HttpDelete("{id}")]
        public ActionResult<CrudOperationDs> DeleteExtraFieldGroup(int id)
        {
            return Ok(_extraFieldGroupService.Delete(id));
        }
    }
}
