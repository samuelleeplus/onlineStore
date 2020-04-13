using OnlineStore.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Repositories
{
    public class ProductRepository
    {
        private UnitOfWork _uow { get; }
        private IRepository<Product> _repo { get; }

        public ProductRepository(UnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetGenericRepository<Product>();
        }
        /*
        public Product GetProductById(int id)
        {

            
            product.ImageUris = imageUris;
            product.Distributor = distributor;

            return product;
        }
        */
    }
}
