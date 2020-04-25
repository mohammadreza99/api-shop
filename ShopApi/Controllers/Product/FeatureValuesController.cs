using Microsoft.AspNetCore.Mvc;
using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method.Token;
using ShopApi.Services.Product;

namespace ShopApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureValuesController : ControllerBase
    {
        private readonly FeatureValueService _featureValueService;
        private readonly Token _token;

        public FeatureValuesController(DatabaseContext context)
        {
            _token = new Token(context);
            _featureValueService = new FeatureValueService(context);
        }

        [HttpGet]
        public ActionResult<CrudOperationDs> GetFeatureValues()
        {
            return Ok(_featureValueService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CrudOperationDs> GetFeatureValue(int id)
        {
            var featureValue = _featureValueService.GetById(id);
            if (featureValue == null)
                return NotFound();
            return Ok(featureValue);
        }

        [HttpPut("{id}")]
        public ActionResult<CrudOperationDs> UpdateFeatureValue(int id, FeatureValueDs featureValue)
        {
            if (id != featureValue.Id)
                return BadRequest();
            if (!_featureValueService.Exist(featureValue))
                return NotFound();
            return Ok(_featureValueService.Update(featureValue));
        }

        [HttpPost]
        public ActionResult<CrudOperationDs> AddFeatureValue(FeatureValueDs featureValue)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_featureValueService.Add(featureValue));
        }

        [HttpDelete("{id}")]
        public ActionResult<CrudOperationDs> DeleteFeatureValue(int id)
        {
            return Ok(_featureValueService.Delete(id));
        }
    }
}
