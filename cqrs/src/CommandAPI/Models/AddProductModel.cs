namespace CommandAPI.Models
{
    public record AddProductModel
    {
        //product management
        public string? Name { get; set; }
        public string? SKU { get; set; }
        public string[]? Gtin { get; set; }

        // Market place properties
        public bool Searchable { get; set; } = true;
        public decimal Price { get; set; }
        //SEO
        public string? SeoDescription { get; set; }
        public string? SeoTags { get; set; }
        public string? SeoName { get; set; }
        //Inventory
        public bool ManageInventory { get; set; }
        public uint InventoryQuantity { get; set; }
        //alerts
        public bool SendInventoryAlert { get; set; }
        public uint InventoryAlertOnQuantity { get; set; }
        public string? InventoryAlertWebHook { get; set; }
        public bool SendOrderAlert { get; set; }
        public uint OrderAlertBatchSize { get; set; } = 1;
        public string? OrderAlertWebHook { get; set; }
    }
}