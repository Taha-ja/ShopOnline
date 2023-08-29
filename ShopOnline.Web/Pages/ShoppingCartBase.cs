using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopeOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Globalization;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }
        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CartChanged();

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);
            RemoveCartItem(id);

        }
        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }
        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);
            CartChanged();
        }
        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateUtemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };
                    var returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateUtemDto);
                    UpdateItemTotalPrice(returnedUpdateItemDto);
                    CartChanged();
                    await MakeUpdateQtyButtonVisible(id, false);
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id);
                    if(item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        } 
        protected async Task UpdateQty_Input(int id)
        {
            await MakeUpdateQtyButtonVisible( id, true);
        }
        protected async Task MakeUpdateQtyButtonVisible(int id, bool visible)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
        }
        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if(item != null)
            {
                item.TotalPrice = item.Price * cartItemDto.Qty; 
            }
        }
        private void CalculateCartSummaryTotals()
        {
            setTotalPrice();
            SetTotalQuantity();
        }
        private void setTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C", CultureInfo.GetCultureInfo("en-US"));
        }
        private void SetTotalQuantity() {
            TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);
                }
        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaizeEventOnShppingCartChanged(TotalQuantity);
        }
    }
}
