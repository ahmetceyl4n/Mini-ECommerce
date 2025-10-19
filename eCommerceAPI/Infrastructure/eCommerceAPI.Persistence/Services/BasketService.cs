using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Application.ViewModels.Basket;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepositories _orderReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;

        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepositories orderReadRepository, IBasketItemWriteRepository basketItemWriteRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
        }

        private async Task<Basket?> ContextUser()
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (userName is not null)
            {
                AppUser? user = await _userManager.Users
                    .Include(u=> u.Baskets)
                    .FirstOrDefaultAsync(u => u.UserName == userName);

                var _basket = from basket in user.Baskets
                              join order in _orderReadRepository.Table
                              on basket.ID equals order.ID into BasketOrders
                              from ba in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                Basket =basket,
                                Order = ba
                              };

                Basket? targetBasket = null;
                if(_basket.Any(bo => bo.Order == null))
                {
                    targetBasket = _basket.FirstOrDefault(bo => bo.Order == null)?.Basket;
                }
                else
                {
                    targetBasket = new();
                    user.Baskets.Add(targetBasket);
                }

                await _basketItemWriteRepository.SaveAsync();
                return targetBasket;
            }
            throw new Exception("Kullanıcı bulunamadı");
        }

        public Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            throw new NotImplementedException();
        }

        public Task<List<BasketItem>> GetBasketItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveBasketItemAsync(string basketItemId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBasketItemAsync(VM_Update_BasketItem basketItem)
        {
            throw new NotImplementedException();
        }
    }
}
