using Microsoft.AspNetCore.Mvc;
using ShopApi.DataLayer.Data;
using ShopApi.DataLayer.DataStructure;
using ShopApi.DataLayer.Method.Token;
using ShopApi.Services.Product;

namespace ShopApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly FeatureService _featureService;
        private readonly Token _token;

        public FeaturesController(DatabaseContext context)
        {
            _token = new Token(context);
            _featureService = new FeatureService(context);
        }

        [HttpGet]
        public ActionResult<CrudOperationDs> GetFeatures()
        {
            return Ok(_featureService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CrudOperationDs> GetFeature(int id)
        {
            var feature = _featureService.GetById(id);
            if (feature == null)
                return NotFound();
            return Ok(feature);
        }

        [HttpPut("{id}")]
        public ActionResult<CrudOperationDs> UpdateFeature(int id, FeatureDs feature)
        {
            if (id != feature.Id)
                return BadRequest();
            if (!_featureService.Exist(feature.Id))
                return NotFound();
            return Ok(_featureService.Update(feature));
        }

        [HttpPost]
        public ActionResult<CrudOperationDs> AddFeature(FeatureDs feature)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(_featureService.Add(feature));
        }

        [HttpDelete("{id}")]
        public ActionResult<CrudOperationDs> DeleteFeature(int id)
        {
            return Ok(_featureService.Delete(id));
        }

        [HttpGet("getFeatureTypes")]
        public ActionResult<CrudOperationDs> GetFeatureTypes()
        {
            return Ok(_featureService.GetFeatureTypes());
        }
    }
}
