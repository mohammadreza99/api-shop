using Microsoft.AspNetCore.Mvc;
using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method.Token;
using ShopApi.Services.Product;

namespace ShopApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private Token _token;

        public ProductsController(DatabaseContext context)
        {
            _token = new Token(context);
            _productService = new ProductService(context);
        }

        [HttpGet]
        public ActionResult<CrudOperationDs> GetProducts()
        {
            return Ok(_productService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CrudOperationDs> GetProduct(long id)
        {
            return Ok(_productService.GetById(id));
        }

        [HttpPut("{id}")]
        public ActionResult<CrudOperationDs> UpdateProduct(long id, ProductDs product)
        {
            if (id != product.Id)
                return BadRequest();
            return Ok(_productService.Update(product));
        }

        [HttpPost]
        public ActionResult<CrudOperationDs> AddProduct(ProductDs product)
        {
            return Ok(_productService.Add(product));
        }

        [HttpDelete("{id}")]
        public ActionResult<CrudOperationDs> DeleteProduct(int id)
        {
            return Ok(_productService.Delete(id));
        }

        [HttpGet("getStockTypes")]
        public ActionResult<CrudOperationDs> GetStockTypes()
        {
            return Ok(_productService.GetStockTypes());
        }

        [HttpGet("getDiscountTypes")]
        public ActionResult<CrudOperationDs> GetDiscountTypes()
        {
            return Ok(_productService.GetDiscountTypes());
        }
    }
}
