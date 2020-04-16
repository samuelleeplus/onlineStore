﻿using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Helpers
{
    public class DtoConstructor
    {
        private UnitOfWork _uow { get; }

        public DtoConstructor(UnitOfWork uow)
        {
            _uow = uow;
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
