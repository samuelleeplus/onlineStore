using System;

/// <summary>
/// Delivery list class provides delivery ID, customer ID, product UD, quantity, total price, delivery address, and is
///delivered properties. These can be viewed by ProductManager.

namespace OnlineStore.Data.Models.Entities
{
    public class deliveryList
    {
        public int deliveryId { get; set; }
        //public int customerId { get; set; } already created in the Order class
        public int productUd { get; set; }
        public int quantity { get; set; }
        public int totalPrice { get; set; }
        public string deliveryAddress { get; set; }
        public bool isDelivered { get; set; }
    }
}