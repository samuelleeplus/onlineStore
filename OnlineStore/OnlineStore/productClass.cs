using System;

namespace OnlineStore
{
    public class productClass
    {
        //product should have id, name, model number, description, price, quanitity in stock, 
        // warranty status, distributor information and applied discounts.
        private string productId;
        private string productName;
        private string modelNumber;
        private string description;
        private double price;
        private int quantity;

        // should we include warranty years? From starting date to ending date? 
        private bool warrantyStatus;

        //can there be many distributors? Should we keep an array of distributors? 
        private string distributorInfo;

        //should there be multiple applied discounts?
        private float appliedDiscount; //this is a percentage value. e.g. 10% is written as 0.1

        //simple constructor 
        public productClass(string id, string name, string modelNum, string description, double price, int quantity, bool warranty, string distributorInfo, string appliedDiscount) {
            productId = id;
            productName = name;
            modelNumber = modelNum;
            this.description = description;
            this.price = price;
            this.quantity = quantity;
            warrantyStatus = warranty;
            this.distributorInfo = distributorInfo;
            this.appliedDiscount = appliedDiscount; 
        }

        //getters and setters 

        public float getAppliedDiscount()
        {
            return this.appliedDiscount;
        }

        public void setAppliedDiscount(float appliedDiscount)
        {
            this.appliedDiscount = appliedDiscount;
            Console.WriteLine("The applied discount is set to : ", appliedDiscount, ".\n");
        }

        public double getPrice()
        {
            //returns the price checking applied discounts 
            if (getAppliedDiscount != 0)
            {
                Console.WriteLine("The price of product ", getProductName, " after applied discount is : ", (this.price * getAppliedDiscount), ".\n");
                return (this.price * getAppliedDiscount);
            }
            Console.WriteLine("The price of product ", getProductName, " is :", this.price, ".\n"); 
            return this.price;
        }

        public void setWarrantyStatus(bool warrantyStatus)
        {
            //checks warranty status 
            if (warrantyStatus == true)
            {
                this.warrantyStatus = warrantyStatus;
                Console.WriteLine("The product's warranty status is set to: ", warrantyStatus, ".\n");

            }
            else
            {
                this.warrantyStatus = warrantyStatus;
                Console.WriteLine("The product's warranty status is set to: ", warrantyStatus, ".\n");
            }

        }

        public void setPrice(double price)
        {
            this.price = price;
            Console.WriteLine("The price is set to: ", price, ".\n");
        }

        public string getProductId()
        {
            return this.productId;
        }

        public void setProductId(string productId)
        {
            this.productId = productId;
            Console.WriteLine("The product id is set to: ", productId, ".\n");
        }

        public string getProductName()
        {
            return this.productName;
        }

        public void setProductName(string productName)
        {
            this.productName = productName;
            Console.WriteLine("The product id is set to: ", productName, ".\n");
        }

        public string getModelNumber()
        {
            return this.modelNumber;
        }

        public void setModelNumber(string modelNumber)
        {
            this.modelNumber = modelNumber;
            Console.WriteLine("The model number id is set to: ", productId, ".\n");
        }

        public string getDescription()
        {
            return this.description;
        }

        public void setDescription(string description)
        {
            this.description = description;
            Console.WriteLine("The description id is set to: ", description, ".\n");
        }

        public int getQuantity()
        {
            return this.quantity;
        }

        public void setQuantity(int quantity)
        {
            this.quantity = quantity;
            Console.WriteLine("The quantity is set to: ", quantity, ".\n");
        }

        public bool isWarrantyStatus()
        {
            return this.warrantyStatus;
        }

        public string getDistributorInfo()
        {
            return this.distributorInfo;
        }

        public void setDistributorInfo(string distributorInfo)
        {
            this.distributorInfo = distributorInfo;
            Console.WriteLine("The distributor information is set to: ", distributorInfo, ".\n");
        }

     
    }

}

