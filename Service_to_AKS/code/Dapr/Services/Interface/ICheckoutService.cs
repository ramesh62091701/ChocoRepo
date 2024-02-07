using ECommerce.Services.Model;
using System.Threading.Tasks;


namespace ECommerce.CheckoutService.Model
{
   public interface ICheckoutService
   {
      Task<CheckoutSummary> CheckoutAsync(string userId);

      Task<CheckoutSummary[]> GetOrderHitoryAsync(string userId);
   }
}
