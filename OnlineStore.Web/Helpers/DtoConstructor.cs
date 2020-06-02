using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Data.Models.Entities; 

namespace OnlineStore.Web.Helpers
{
    public class DtoConstructor
    {
        private UnitOfWork _uow { get; }

        public DtoConstructor(UnitOfWork uow)
        {
            _uow = uow;
        }
        public HomeDto GetHomeDto(int amount, int frontAmount)
        {
            var repo = _uow.GetGenericRepository<Product>();
            var _discount = repo.GetAll().Take(1).ToList()[0]; // instead of 5 choose a (maybe random) category with has a product with a discount
            var _products = repo.GetAll().Take(amount).ToList();
            var _frontImages = repo.GetAll().Take(frontAmount + 1).ToList(); 
            
            if (_products == null || _frontImages == null || _discount == null)
                return null;


            DiscountImage discountImage = new DiscountImage
            {
                CategoryName = _discount.Category,
                DiscountAmount = (int)Math.Floor((_discount.DiscountedPrice * 100) / _discount.Price), // calculate percentage
                ImageUrl = _uow.GetGenericRepository<ImageUri>().FirstOrDefault(y => y.ProductId == _discount.Id).Uri,
                Url = "https://localhost:5001/category/" + _discount.Category
            };
            IEnumerable<SimpleProduct> simpleProducts = _products == null ? null : _products.Select(x =>
                new SimpleProduct
                {
                    Name = x.Name,
                    ImageUrl = _uow.GetGenericRepository<ImageUri>().FirstOrDefault(y => y.ProductId == x.Id).Uri,
                    Url = "https://localhost:5001/product?id=" + x.Id,
                    OriginalPrice = x.Price,
                    DiscountedPrice = x.DiscountedPrice,
                    StatusClass = "NEW" // TODO MAKE STATUS CLASS IN PRODUCT
                }).ToList();
            
            IEnumerable<FrontImage> frontImages = _frontImages == null ? null : _frontImages.Select(x =>
                new FrontImage
                {
                    BackgroundImageUrl = _uow.GetGenericRepository<ImageUri>().FirstOrDefault(y => y.ProductId == x.Id).Uri,
                    Title = x.Name,
                    Text = x.DescriptionMain // or desription extra?
                }).ToList();

            var homeDto = new HomeDto
            {
               DiscountImage = discountImage,
               FrontImages = frontImages.Take(frontAmount),
               FrontImage = frontImages.Last(),
               Products = simpleProducts,
            };

            return homeDto;

        }


        public UserDto UserDtoByCustomerId(string id)
        {
            var userRepo =  _uow.GetGenericRepository<ApplicationUser>().Find(x => x.Id == id).FirstOrDefault();


            var ordersById = _uow.GetGenericRepository<Order>().Find(x => x.CustomerId == userRepo.CustomerId);

            var cardsId = _uow.GetGenericRepository<CreditCard>().Find(x => x.CustomerId == userRepo.CustomerId);

            IEnumerable<Order> orders = ordersById?.Select(x =>
                new Order
                {
                    //address of order
                    Address = x.Address,
                    CustomerId = x.CustomerId,
                    Id = x.Id,
                    IsDelivered = x.IsDelivered,
                    TotalPrice = x.TotalPrice

                }).ToList();
            //username , userid , customer address, order


            IEnumerable<CreditCard> cards = cardsId?.Select(x =>
                new CreditCard
                {
                    CardNumber = x.CardNumber,
                     Cvc = x.Cvc , 
                      ExpiryDate = x.ExpiryDate,
                       FullName = x.FullName,
                }).ToList();


            UserDto userDto = new UserDto {
                Orders = orders,
                UserId = id,
                Username = userRepo.UserName,
                 FirstName = userRepo.FirstName,
                  LastName =userRepo.LastName,
                   Email = userRepo.Email,
                    PhoneNumber = userRepo.PhoneNumber,
                     CreditCards = cards
            };

            return userDto; 
        }

        public UserDto AddressDtoByCustomerId(string id)
        {
            var userRepo = _uow.GetGenericRepository<ApplicationUser>().Find(x => x.Id == id).FirstOrDefault();

            //get all addresses by the customerId
            var addressesById = _uow.GetGenericRepository<Address>().Find(x => x.UserId == userRepo.CustomerId);

            IEnumerable<Address> addresses = addressesById?.Select(x =>
               new Address
               {
                   //addresses
                   UserId = x.UserId,
                   City = x.City,
                   AddressDetail = x.AddressDetail,
                   Province = x.Province,
                   ZipCode = x.ZipCode,
                   Id = x.Id

               }).ToList();

            UserDto userDto = new UserDto
            {
                UserId = id,
                Username = userRepo.UserName,
                FirstName = userRepo.FirstName,
                LastName = userRepo.LastName,
                Addresses = addresses
            };


            return userDto;
        }

        public UserDto CreditCardDtoByCustomerId(string id)
        {
            var userRepo = _uow.GetGenericRepository<ApplicationUser>().Find(x => x.Id == id).FirstOrDefault();


            var cardById = _uow.GetGenericRepository<CreditCard>().Find(x => x.CustomerId == userRepo.CustomerId);


            IEnumerable<CreditCard> cards = cardById?.Select(x =>
               new CreditCard
               {

                    CardNumber = x.CardNumber , 
                    Cvc = x.Cvc ,


                   //CardNumber = x.CardNumber ,
                   // Cvc = x.Cvc ,
                   ExpiryDate = x.ExpiryDate,
                   FullName = x.FullName,
                   Id = x.Id,
                   CustomerId = x.CustomerId



               }).ToList();




        UserDto userDto = new UserDto
            {
                UserId = id,
                Username = userRepo.UserName,
                FirstName = userRepo.FirstName,
                LastName = userRepo.LastName,
                CreditCards = cards
            };


            return userDto;
        }



        public InvoiceDto InvoiceDtoByOrderID(int orderId) { 

            var orderedProducts = _uow.GetGenericRepository<OrderedProduct>().Find(x => x.OrderId == orderId);

            var repo = _uow.GetGenericRepository<Product>();


            IEnumerable<CartItem> items = orderedProducts == null ? null : orderedProducts.Select(x =>
               new CartItem
               {
                   Id = x.ProductId,
                   ItemQuantity = x.Quantity,
                   ItemName = repo.GetById(x.ProductId).Name,
                   //should be price -discounted price
                   ItemPrice = repo.GetById(x.ProductId).Price  
               }).ToList();

       
            var priceRepo = _uow.GetGenericRepository<Order>();
            var addressRepo = _uow.GetGenericRepository<Address>();
            var usersRepo = _uow.GetGenericRepository<ApplicationUser>();

            var userId = priceRepo.FirstOrDefault(x => x.Id == orderId).CustomerId;
            InvoiceDto invoiceDto = new InvoiceDto
            {
                //Customer name or User name should be included in database !!!!!!!!

                Items = items,
                OrderId = orderId,
                totalPrice = priceRepo.FirstOrDefault(x => x.CustomerId == orderId).TotalPrice,
                CustomerAddress = addressRepo.FirstOrDefault(x => x.UserId == userId),
                CustomerName =  usersRepo.FirstOrDefault(x => x.CustomerId == userId).FirstName + " " +
                                usersRepo.FirstOrDefault(x => x.CustomerId == userId).LastName
            };

            return invoiceDto; 


        }

        public ProductDto GetProductDtoByProductId(int id)
        {
            var repo = _uow.GetGenericRepository<Product>();
            var product = repo.GetById(id);

            if (product == null)
                return null;

            var imageUris = _uow.GetGenericRepository<ImageUri>().Find(x => x.ProductId == product.Id);
            var distributor = _uow.GetGenericRepository<Distributor>().FirstOrDefault(x => x.Id == product.DistributorId);

            var _relatedProducts = repo.Find(x => x.Category == product.Category && x.Id != product.Id).Take(4);


            var header = new Header
            {
                BackgroundImageUrl = "images/categories.jpg",
                Title = product.Name,
                Text = product.DescriptionMain
            };
            IEnumerable<RelatedProduct> relatedProducts = _relatedProducts == null ? null : _relatedProducts.Select(x =>
                new RelatedProduct
                {
                    Name = x.Name,
                    ImageUrl = _uow.GetGenericRepository<ImageUri>().FirstOrDefault(y => y.ProductId == x.Id).Uri,
                    Url = "https://localhost:5001/product?id=" + x.Id,
                    Price = x.DiscountedPrice,
                    StatusClass = "NEW" // TODO MAKE STATUS CLASS IN PRODUCT
                }).ToList();

            

            var productDto = new ProductDto
            {
                ProductId = product.Id,
                Header = header,
                Name = product.Name,
                Description = product.DescriptionMain,
                DescriptionExtra = product.DescriptionExtra,
                ImageUrls = imageUris?.Select(x => x.Uri),
                OriginalPrice = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                RelatedProducts = relatedProducts
            };

            return productDto;

        }




    }
}
