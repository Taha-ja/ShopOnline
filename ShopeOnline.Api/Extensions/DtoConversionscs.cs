using ShopeOnline.Api.Entities;
using ShopeOnline.Models.Dtos;

namespace ShopeOnline.Api.Extensions
{
    public static class DtoConversionscs
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products,
                                                                IEnumerable<ProductCategory> productCategories)
        {
            return products.Join(productCategories,
                prod=>prod.CategoryId,
                prodCat=>prodCat.Id,
                (prod,prodCat)=>new ProductDto { 
                    Id = prod.Id,
                    Name = prod.Name,
                    Description = prod.Description,
                    ImageURL = prod.ImageURL,
                    Price = prod.Price,
                    Qty = prod.Qty,
                    CategoryId = prod.CategoryId,
                    CategoryName = prodCat.Name,
                    
                }).ToList();
        }

        public static ProductDto ConvertToDto(this Product product,
                                                        ProductCategory productCategory)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.CategoryId,
                CategoryName = productCategory.Name,

            };
        }
        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems,
                                                          IEnumerable<Product> products)
        {
            return cartItems.Join(products,
                cartItm => cartItm.ProductId,
                prod => prod.Id,
                (cartItm, prod) => new CartItemDto
                {
                    Id = cartItm.Id,
                    ProductId = cartItm.ProductId,
                    ProductName = prod.Name,
                    ProductDescription = prod.Description,
                    ProductImageURL = prod.ImageURL,
                    Price = prod.Price,
                    CartId = cartItm.CartId,
                    Qty = cartItm.Qty,
                    TotalPrice = prod.Price * cartItm.Qty,
                }).ToList();
        }
        public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                CartId = cartItem.CartId,
                Qty = cartItem.Qty,
                TotalPrice = product.Price * cartItem.Qty,
            };
        }
    }
}
