using AnyService.Events;
using CommandAPI.Domain;
using CommandAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CommandAPI.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : ControllerBase
    {
            [HttpGet]
            public IActionResult Get() => Ok("from command");

        private static readonly string INSERT_SQL = $"INSERT INTO {nameof(ManagedProduct)}s Values " +
            $"(@{nameof(ManagedProduct.Gtin)}," +
            $"@{nameof(ManagedProduct.InventoryQuantity)}," +
            $"@{nameof(ManagedProduct.ManageInventory)}," +
            $"@{nameof(ManagedProduct.Name)}," +
            $"@{nameof(ManagedProduct.Price)}," +
            $"@{nameof(ManagedProduct.Searchable)}," +
            $"@{nameof(ManagedProduct.SeoDescription)}," +
            $"@{nameof(ManagedProduct.SeoName)}," +
            $"@{nameof(ManagedProduct.SeoTags)}," +
            $"@{nameof(ManagedProduct.SKU)}" +
            $"@{nameof(ManagedProduct.SendInventoryAlert)}" +
            $"@{nameof(ManagedProduct.InventoryAlertOnQuantity)}" +
            $"@{nameof(ManagedProduct.InventoryAlertWebHook)}" +
            $"@{nameof(ManagedProduct.SendOrderAlert)}" +
            $"@{nameof(ManagedProduct.OrderAlertBatchSize)}" +
            $"@{nameof(ManagedProduct.OrderAlertWebHook)}" +
            $" );";

        static string? ConnectionString;
        private readonly ICrossDomainEventPublishManager _publishManager;
        #region ctor
        public ProductController(IConfiguration configuration, ICrossDomainEventPublishManager publishManager)
        {
            ConnectionString ??= configuration.GetConnectionString("ProductDatabase");
            _publishManager = publishManager;
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddProductModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var managedProduct = new ManagedProduct
            {
                Gtin = model.Gtin,
                InventoryQuantity = model.InventoryQuantity,
                InventoryAlertOnQuantity = model.InventoryAlertOnQuantity,
                InventoryAlertWebHook = model.InventoryAlertWebHook,
                ManageInventory = model.ManageInventory,
                Name = model.Name,
                OrderAlertBatchSize = model.OrderAlertBatchSize,
                OrderAlertWebHook = model.OrderAlertWebHook,
                Price = model.Price,
                Searchable = model.Searchable,
                SeoDescription = model.SeoDescription,
                SeoTags = model.SeoTags,
                SeoName = model.SeoName,
                SKU = model.SKU,
                SendInventoryAlert = model.SendInventoryAlert,
                SendOrderAlert = model.SendOrderAlert,
            };

            using var connection = new SqlConnection(ConnectionString);
            var affectedRows = await connection.ExecuteAsync(INSERT_SQL, managedProduct);
            if (affectedRows == default)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var ie = new IntegrationEvent("products-ex", "")
            {
                Data = managedProduct,
            };
            await _publishManager.PublishToAll(ie);

            return Ok();
        }
    }
}