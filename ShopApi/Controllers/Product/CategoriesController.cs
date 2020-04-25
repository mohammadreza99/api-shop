using Microsoft.AspNetCore.Mvc;
using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method.Token;
using ShopApi.Services.Product;

namespace ShopApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly Token _token;

        public CategoriesController(DatabaseContext context)
        {
            _token = new Token(context);
            _categoryService = new CategoryService(context);
        }

        [HttpGet]
        public ActionResult<CrudOperationDs> GetCategories()
        {
            return Ok(_categoryService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CrudOperationDs> GetCategory(int id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPut("{id}")]
        public ActionResult<CrudOperationDs> UpdateCategory([FromRoute] int id, [FromBody] CategoryDs category)
        {
            if (id != category.Id)
                return BadRequest();
            if (!_categoryService.Exist(category.Id))
                return NotFound();
            return Ok(_categoryService.Update(category));
        }


        [HttpPost]
        public ActionResult<CrudOperationDs> AddCategory(CategoryDs category)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_categoryService.Add(category));
        }

        [HttpDelete("{id}")]
        public ActionResult<CrudOperationDs> DeleteCategory(int id)
        {
            return Ok(_categoryService.Delete(id));
        }
    }
}
