using DemoAPI.Model;
using Orleans;
using Required.Interface;

namespace DemoAPI.GrainUser
{
    public class GrainUser : Grain , IGrainUser
    {
        private UIModel uiModel;

        public GrainUser()
        {
            uiModel = new UIModel
            {
                FormName = "Purchase Order Form",
                FormId = "P123",
                PurchaseOrder = new PurchaseOrder
                {
                    PurchaseOrderText = "9088",
                    PurchaseOrderVisible = true,
                    PurchaseOrderToolTip = "Some ToolTip"
                }
            };
        }

        public Task<UIModel> GetFormsAsync()
        {
            return Task.FromResult(uiModel);
        }

    }
}