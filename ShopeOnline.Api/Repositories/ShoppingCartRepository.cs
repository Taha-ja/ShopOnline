using Microsoft.EntityFrameworkCore;
using ShopeOnline.Api.Data;
using ShopeOnline.Api.Entities;
using ShopeOnline.Api.Repositories.Contracts;
using ShopeOnline.Models.Dtos;

namespace ShopeOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext _shopOnlineDbContext;
        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await _shopOnlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
        }
        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            _shopOnlineDbContext = shopOnlineDbContext;
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAdd)
        {
            if (! await CartItemExists(cartItemToAdd.CartId,cartItemToAdd.ProductId))
            {
                var item = await _shopOnlineDbContext.Products
                                 .Where(p => p.Id == cartItemToAdd.ProductId)
                                 .Select(p => new CartItem {  
                                     ProductId = p.Id,
                                     CartId = cartItemToAdd.CartId,
                                     Qty = cartItemToAdd.Qty
                                 }).SingleOrDefaultAsync();
                if(item != null)
                {
                    var result = await _shopOnlineDbContext.CartItems.AddAsync(item);
                    await _shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;

                }
            }
            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await  _shopOnlineDbContext.CartItems.FindAsync(id);
            if(item != null)
            {
                _shopOnlineDbContext.CartItems.Remove(item);
                await _shopOnlineDbContext.SaveChangesAsync();
            }
            return item;
        }

        public async Task<CartItem> GetItem(int id)
        {
            var item = await _shopOnlineDbContext.Carts
                            .Join(_shopOnlineDbContext.CartItems,
                                  cart => cart.Id,
                                  cartItem => cartItem.CartId,
                                  (cart,cartItem) => new {cart,cartItem})
                            .Where(joinedResult => joinedResult.cartItem.Id == id)
                            .Select(joinedResult => new CartItem
                            {
                                Id = joinedResult.cartItem.Id,
                                ProductId = joinedResult.cartItem.ProductId,
                                Qty = joinedResult.cartItem.Qty,
                                CartId = joinedResult.cartItem.CartId,
                            })
                            .SingleOrDefaultAsync();
            return item;
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await _shopOnlineDbContext.CartItems
                            .Join(_shopOnlineDbContext.Carts,
                                    cartItem => cartItem.CartId,
                                    cart => cart.Id,
                                    (cartItem, cart) => new { cartItem, cart })
                            .Where(joinResult => joinResult.cart.UserId == userId)
                            .Select(joinResult => new CartItem
                            {
                                Id = joinResult.cartItem.Id,
                                ProductId = joinResult.cartItem.ProductId,
                                Qty = joinResult.cartItem.Qty,
                                CartId = joinResult.cartItem.CartId,
                            })
                            .ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await _shopOnlineDbContext.CartItems.FindAsync(id);
            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await _shopOnlineDbContext.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}
