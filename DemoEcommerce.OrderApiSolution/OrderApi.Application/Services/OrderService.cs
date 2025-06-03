using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly.Registry;
using System.Net.Http.Json;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface,HttpClient httpClient,
        ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        //Get Product
        public async Task<ProductDTO> GetProduct(int productId)
        {
            //Call product api using httpclient
            //Redirect this call to the api gateway since product api is not response to outsiders
            var getProduct = await httpClient.GetAsync($"/api/products/{productId}");

            if (!getProduct.IsSuccessStatusCode)
            {
                return null!;
            }
            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();

            return product!;
        }

        //GET USER
        public async Task<AppUserDTO> GetUser(int userId)
        {
            //Call product api using httpclient
            //Redirect this call to the api gateway since product api is not response to outsiders
            var getUser = await httpClient.GetAsync($"http://localhost:5000/api/Authentication/{userId}");

            if (!getUser.IsSuccessStatusCode)
            {
                return null!;
            }
            var product = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();

            return product!;
        }

        //get orders by client id
        public async Task<IEnumerable<OrderDTO>> GetOrderByClientId(int clientId)
        {
            //get all client's orders
            var orders = await orderInterface.GetOrderAsync(o => o.ClientId == clientId);
            if (!orders.Any())
            {
                return null!;
            }

            var (_, _orders) = OrderConversion.FromEntity(null, orders);

            return _orders!;
        }

        //Get Orders details by id
        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            //Prepare order
            var order = await orderInterface.FindByIdAsync(orderId);
            if (order is null || order!.Id <= 0)
            {
                return null!;
            }

            //Get retry pipeline
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            //prepare product
            var productDTO =await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            //prepare client
            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));


            //populate order details
            return new OrderDetailsDTO(
                order.Id,
                productDTO.Id,
                appUserDTO?.Id ?? 0,
                appUserDTO?.Name ?? "Unknown",
                appUserDTO?.Email ?? "N/A",
                appUserDTO?.Adress ?? "N/A",
                appUserDTO?.TelephoneNumber ?? "N/A",
                appUserDTO?.Name ?? "Unknown",
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Quantity * order.PurchaseQuantity,
                order.OrderedDate

                );
        }
    }
}
