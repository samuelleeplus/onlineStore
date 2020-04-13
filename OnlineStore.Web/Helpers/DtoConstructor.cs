using OnlineStore.Data.Models.Entities;
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

        public ProductDTO GetProductDtoByProductId(int id)
        {
            var repo = _uow.GetGenericRepository<Product>();
            var product = repo.GetById(id);

            if (product == null)
                return null;

            var imageUris = _uow.GetGenericRepository<ImageUri>().Find(x => x.ProductId == product.Id);
            var distributor = _uow.GetGenericRepository<Distributor>().FirstOrDefault(x => x.Id == product.DistributorId);

            var _relatedProducts = repo.Find(x => x.Category == product.Category && x.Id != product.Id).Take(5);


            var header = new Header
            {
                BackgroundImageUrl = "https://openimagedenoise.github.io/images/moana_16spp_oidn.jpg",
                Title = product.Name,
                Text = product.DescriptionMain
            };

            IEnumerable<RelatedProduct> relatedProducts = null; /* _relatedProducts == null ? null : _relatedProducts.Select(x =>
                new RelatedProduct
                {
                    ImageUrl = "https://openimagedenoise.github.io/images/moana_16spp_oidn.jpg", // x.ImageUris.Take(1).Select(y => y.Uri).FirstOrDefault(),
                    Url = "https://localhost:44396/",
                    Price = x.DiscountedPrice,
                    StatusClass = "New"
                }).ToList();
            */

            var productDTO = new ProductDTO
            {
                ProductId = product.Id,
                Header = header,
                Name = product.Name,
                Description = product.DescriptionMain,
                DescriptionExtra = product.DescriptionExtra,
                ImageUrls = imageUris == null ? null : imageUris.Select(x => x.Uri),
                OriginalPrice = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                RelatedProducts = relatedProducts
            };

            return productDTO;

        }




    }
}
