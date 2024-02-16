namespace DemoAPI.Model
{
    [GenerateSerializer]
    public class UIModel
    {
        [Id(0)] public string FormName { get; set; }
        [Id(1)] public string FormId { get; set; }
        [Id(2)] public PurchaseOrder PurchaseOrder { get; set; }
    }

    [GenerateSerializer]
    public class PurchaseOrder
    {
        [Id(0)] public string PurchaseOrderText { get; set; }
        [Id(1)] public bool PurchaseOrderVisible { get; set; }
        [Id(2)] public string PurchaseOrderToolTip { get; set; }
    }
}
