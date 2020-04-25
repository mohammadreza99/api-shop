using Microsoft.AspNetCore.Mvc;
using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method.Token;
using ShopApi.Services.Product;

namespace ShopApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtraFieldsController : ControllerBase
    {
        private readonly ExtraFieldService _extraFieldService;
        private Token _token;

        public ExtraFieldsController(DatabaseContext context)
        {
            _token = new Token(context);
            _extraFieldService = new ExtraFieldService(context);
        }

        [HttpGet]
        public ActionResult<CrudOperationDs> GetExtraFields()
        {
            return Ok(_extraFieldService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CrudOperationDs> GetExtraField(int id)
        {
            var extraField = _extraFieldService.GetById(id);
            if (extraField == null)
                return NotFound();
            return Ok(extraField);
        }

        [HttpPut("{id}")]
        public ActionResult<CrudOperationDs> UpdateExtraField(int id, ExtraFieldDs extraField)
        {
            if (id != extraField.Id)
                return BadRequest();
            if (!_extraFieldService.Exist(extraField.Id))
                return NotFound();
            return Ok(_extraFieldService.Update(extraField));
        }

        [HttpPost]
        public ActionResult<CrudOperationDs> AddExtraField(ExtraFieldDs extraField)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_extraFieldService.Add(extraField));
        }

        [HttpDelete("{id}")]
        public ActionResult<CrudOperationDs> DeleteExtraField(int id)
        {
            return Ok(_extraFieldService.Delete(id));
        }

        [HttpGet("getExtraFieldsByGroupId/{id}")]
        public ActionResult<CrudOperationDs> GetExtraFieldByGroupId([FromRoute] int id)
        {
            return Ok(_extraFieldService.GetExtraFieldByGroupId(id));
        }

        [HttpGet("getExtraFieldsTypes")]
        public ActionResult<CrudOperationDs> GetExtraFieldTypes()
        {
            return Ok(_extraFieldService.GetExtraFieldTypes());
        }

        [HttpGet("getExtraFieldsByCategoryId/{id}")]
        public ActionResult<CrudOperationDs> GetExtraFieldsByCategoryId(int id)
        {
            return Ok(_extraFieldService.GetExtraFieldsByCategoryId(id));
        }

    }
}
