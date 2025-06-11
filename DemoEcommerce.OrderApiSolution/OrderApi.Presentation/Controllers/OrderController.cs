using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using OrderApi.Domain.Entities;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrder()
        {
            
            var order = await orderInterface.GetAllAsync();
            if (!order.Any())
            {
                return NotFound("No orders detected in the database.");
            }

            var (_, list) = OrderConversion.FromEntity(null, order);
            return !list!.Any() ? NotFound("No orders detected in the database.") : Ok(list!);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.FindByIdAsync(id);
            if (order is null)
                return NotFound(null);

            var (_order, _) = OrderConversion.FromEntity(order, null);
            return Ok(_order);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetClientOrders(int clientId)
        {
            if (clientId <= 0) return BadRequest("Invalid data provided.");
            
            var orders = await orderService.GetOrderByClientId(clientId);
            return !orders.Any() ? NotFound("No orders found for the specified client.") : Ok(orders);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0) return BadRequest("Invalid data provided.");
            var orderDetails = await orderService.GetOrderDetails(orderId);
            return orderDetails.OrderId > 0 ? Ok(orderDetails)  : NotFound("No details found for the specified order.");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            //check model state if all data annotation are passed
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submited");
            }

            //convert to entity
            var getEntity = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            //check model state if all data annotation are passed
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submited");
            }
            //convert to entity
            var getEntity = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response.Message);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            //check model state if all data annotation are passed
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submited");
            }
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.DeleteAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response.Message);
        }
    }
}
