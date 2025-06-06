﻿using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = context.Orders.Add(entity).Entity; 
                await context.SaveChangesAsync();
                return order.Id > 0 ? new Response(true, "Order placed successfully") : new Response(false, "Error occured while placing orders");
            }
            catch (Exception ex)
            {

                //Log original exception
                LogException.LogExceptions(ex);

                //Display scary free message to user

                return new Response(false, "An error occurred while placeing order. Please try again later.");
            }
        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order == null)
                {
                    return new Response(false, "Order not found");
                }

                context.Orders.Remove(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Order deleted successfully");
            }
            catch (Exception ex)
            {

                //Log original exception
                LogException.LogExceptions(ex);

                //Display scary free message to user

                return new Response(false, "An error occurred while deleting order. Please try again later.");
            }
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders!.FindAsync(id);
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {

                //Log original exception
                LogException.LogExceptions(ex);

                //Display scary free message to user

                throw new Exception("An error occurred while retrieving order. Please try again later.");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {

                //Log original exception
                LogException.LogExceptions(ex);

                //Display scary free message to user

                throw new Exception("An error occurred while placeing order. Please try again later.");
            }
        }

        public async Task<Order> GetBySync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {

                //Log original exception
                LogException.LogExceptions(ex);

                //Display scary free message to user

                throw new Exception("An error occurred while retrieving order. Please try again later.");
            }
        }

        public async Task<IEnumerable<Order>> GetOrderAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await context.Orders.Where(predicate).ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {

                //Log original exception
                LogException.LogExceptions(ex);

                //Display scary free message to user

                throw new Exception( "An error occurred while placeing order. Please try again later.");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id); 
                if(order is null)
                {
                    return new Response(false, "Order not found");
                }

                context.Entry(order).State = EntityState.Detached;
                context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Order updated successfully");
            }
            catch (Exception ex)
            {

                //Log original exception
                LogException.LogExceptions(ex);

                //Display scary free message to user

                return new Response(false, "An error occurred while update order. Please try again later.");
            }
        }
    }
}
