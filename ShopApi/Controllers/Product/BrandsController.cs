using Microsoft.AspNetCore.Mvc;
using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method.Token;
using ShopApi.Services.Product;

namespace ShopApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly BrandService _brandService;
        private Token _token;

        public BrandsController(DatabaseContext context)
        {
            _token = new Token(context);
            _brandService = new BrandService(context);
        }

        [HttpGet]
        public ActionResult<CrudOperationDs> GetBrands()
        {
            return Ok(_brandService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CrudOperationDs> GetBrand(int id)
        {
            var brand = _brandService.GetById(id);
            if (brand == null)
                return NotFound();
            return Ok(brand);
        }

        [HttpPut("{id}")]
        public ActionResult<CrudOperationDs> UpdateBrand(int id, BrandDs brand)
        {
            if (id != brand.Id)
                return BadRequest();
            if (!_brandService.Exist(brand.Id))
                return NotFound();
            return Ok(_brandService.Update(brand));
        }

        [HttpPost]
        public ActionResult<CrudOperationDs> AddBrand(BrandDs brand)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_brandService.Add(brand));
        }

        [HttpDelete("{id}")]
        public ActionResult<CrudOperationDs> DeleteBrand(int id)
        {
            return Ok(_brandService.Delete(id));
        }
    }
}
