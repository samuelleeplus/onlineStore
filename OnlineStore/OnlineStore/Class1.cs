using System;

/// <summary>
/// Delivery list class provides delivery ID, customer ID, product UD, quantity, total price, delivery address, and is
///delivered properties. These can be viewed by ProductManager.

/// </summary>
public class deliveryList
{
	public deliveryList()
	{
		deliveryList();//default contructor
		~deliveryList();//destructor
		int delivery_id;
		int customer_id;
		string product_UD; // couldn't verified what UD means.
		int quantity;
		int total_price;
		string deliveryAddress;
		bool isDelivered;
	}
}
