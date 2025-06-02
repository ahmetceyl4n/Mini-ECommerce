using eTicaretAPI.Application.Repositories;
using eTicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepositories _productReadRepositories;
        private readonly IProductWriteRepositories _productWriteRepositories;
        private readonly IOrderWriteRepositories _orderWriteRepositories;
        private readonly IOrderReadRepositories _orderReadRepositories;
        private readonly ICustomerWriteRepositories _customerWriteRepositories;
        public ProductsController(IProductReadRepositories productReadRepositories, IProductWriteRepositories productWriteRepositories, IOrderWriteRepositories orderWriteRepositories, IOrderReadRepositories orderReadRepositories, ICustomerWriteRepositories customerWriteRepositories)
        {
            _productReadRepositories = productReadRepositories;
            _productWriteRepositories = productWriteRepositories;
            _orderWriteRepositories = orderWriteRepositories;
            _orderReadRepositories = orderReadRepositories;
            _customerWriteRepositories = customerWriteRepositories;
        }

        [HttpGet]
        public async Task Get()
        {/*
          
            await _productWriteRepositories.AddRangeAsync(new() {
                new() {ID = Guid.NewGuid(), Name = "Product1", CreatedDate = DateTime.UtcNow, Price = 100, Stock = 10},  
                new() {ID = Guid.NewGuid(), Name = "Product2", CreatedDate = DateTime.UtcNow, Price = 200, Stock = 20}, 
                new() {ID = Guid.NewGuid(), Name = "Product3", CreatedDate = DateTime.UtcNow, Price = 300, Stock = 30}, 
                new() {ID = Guid.NewGuid(), Name = "Product4", CreatedDate = DateTime.UtcNow, Price = 400, Stock = 40}  
            });
            await _productWriteRepositories.SaveAsync();   //verileri database'e kaydetmek için 
            
          */
         /*
             
            Product product = await _productReadRepositories.GetByIdAsync("1a30a7d0-125f-48d4-b142-7b55cf93bde3");
            product.Name = "Furkan";
            await _productWriteRepositories.SaveAsync();        //Burada tracking default olarak true olduğu için fiziksel databasede ürün ismi Furkan olacak

            Product product2 = await _productReadRepositories.GetByIdAsync("1a30a7d0-125f-48d4-b142-7b55cf93bde3", false);
            product.Name = "Reçber";
            await _productWriteRepositories.SaveAsync();         //Burada tracking false olduğu için fiziksel databasede ürün ismi değişmeyecek
            
          */

            /*
            var CustomerId = Guid.NewGuid();
            await _customerWriteRepositories.AddAsync(new() { ID = CustomerId, Name = "Furkan"});

            await _orderWriteRepositories.AddAsync(new()        //Order oluşturduk. CreatedTime otomatik verildi.
            {
                ID = Guid.NewGuid(),
                CustomerId = CustomerId,
                Address = "Address",
                Description = "Description"
            });

            await _orderWriteRepositories.SaveAsync();  //Order'ı kaydettik

            */
            Order order = await _orderReadRepositories.GetByIdAsync("41417bb1-062d-400a-92f5-bde29455159f"); //Order'ı id ile getirdik
            order.Address = "Yeni Adres"; //Order'ın adresini güncelledik
            await _orderWriteRepositories.SaveAsync(); //Order'ı kaydettik
        }
            
    }
}
