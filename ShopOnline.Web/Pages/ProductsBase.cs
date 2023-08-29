using Microsoft.AspNetCore.Components;
using ShopeOnline.Models.Dtos;
using ShopOnline.Web.Services;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetItems();
            var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            var totalQty = shoppingCartItems.Sum(item => item.Qty);
            ShoppingCartService.RaizeEventOnShppingCartChanged(totalQty);
        }
        protected IOrderedEnumerable<IGrouping<int,ProductDto>> GetGroupedProductsByCategory()
        {
            return Products.GroupBy(p => p.CategoryId).OrderBy(p => p.Key);
        }
        protected string GetCategoryName(IGrouping<int,ProductDto> groupedProductDtos) {
            return groupedProductDtos.FirstOrDefault(p => p.CategoryId == groupedProductDtos.Key).CategoryName;
        }
    }
}
