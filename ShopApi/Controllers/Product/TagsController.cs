using Microsoft.AspNetCore.Mvc;
using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method.Token;
using ShopApi.Services.Product;

namespace ShopApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly TagService _tagService;
        private Token _token;

        public TagsController(DatabaseContext context)
        {
            _token = new Token(context);
            _tagService = new TagService(context);
        }

        [HttpGet]
        public ActionResult<CrudOperationDs> GetTags()
        {
            return Ok(_tagService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CrudOperationDs> GetTag(int id)
        {
            var tag = _tagService.GetById(id);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }

        [HttpPut("{id}")]
        public ActionResult<CrudOperationDs> UpdateTag(int id, TagDs tag)
        {
            if (id != tag.Id)
                return BadRequest();
            if (!_tagService.Exist(tag.Id))
                return NotFound();
            return Ok(_tagService.Update(tag));
        }

        [HttpPost]
        public ActionResult<CrudOperationDs> AddTag(TagDs tag)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_tagService.Add(tag));
        }

        [HttpDelete("{id}")]
        public ActionResult<CrudOperationDs> DeleteTag(int id)
        {
            return Ok(_tagService.Delete(id));
        }
    }
}
