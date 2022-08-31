using Aranda.Core.Entities;
using Aranda.Core.Interfaces;
using Aranda.WebApi.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Aranda.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #region Public Methods
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        [HttpPost("GetAll")]
        public async Task<IEnumerable<ProductOutput>> GetAllAsync(ProductFiltersInput input)
        {
            IEnumerable<Product> products;
            if (!string.IsNullOrEmpty(input.SearchValue)) {
                products = _unitOfWork.Products.Find(u => input.SearchValue.Contains(u.Name) ||
                input.SearchValue.Contains(u.Description) ||
                input.SearchValue.Contains(u.CategoryName));
            }
            else
            {
                products = await _unitOfWork.Products.GetAllAsync();
            }

            products
                .Skip(input.Skip)
                .Take(input.Take);

            if (input.SortBy.Equals("name"))
            {
                products = input.SortMethod.Equals("asc") ? products.OrderBy(o => o.Name) : products.OrderByDescending(o => o.Name);
            }
            else
            {
                products = input.SortMethod.Equals("asc") ? products.OrderBy(o => o.CategoryName) : products.OrderByDescending(o => o.CategoryName);
            }

            return _mapper.Map<IEnumerable<ProductOutput>>(products);
        }


        [HttpGet("GetById")]
        public async Task<ProductOutput> GetByIdAsync(int id)
        {
            var product = await FindByIdAsync(id);
            return _mapper.Map<ProductOutput>(product);
        }


        [HttpPost("Create")]
        public async Task<Product> CreateAsync(ProductCreateInput input)
        {
            if (input == null)
            {
                throw new Exception("No ha ingresado ningún producto");
            }

            var product = _mapper.Map<Product>(input);
            await _unitOfWork.Products.CreateAsync(product);
            _unitOfWork.Complete();
            return product;
        }


        [HttpPut("Update")]
        public async Task<Product> UpdateAsync(ProductUpdateInput input)
        {
            if (input == null)
            {
                throw new Exception("No ha ingresado ningún producto");
            }

            var product = await FindByIdAsync(input.Id);

            product.Name = input.Name;
            product.Description = input.Description;
            product.CategoryName = input.CategoryName;
            product.Image = input.Image;

            _unitOfWork.Products.Update(product);
            _unitOfWork.Complete();
            return product;
        }

        [HttpDelete("Delete")]
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await FindByIdAsync(id);
            _unitOfWork.Products.Remove(product);
            _unitOfWork.Complete();
            return true;
        }

        #endregion

        #region Private Methods

        private async Task<Product> FindByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("El producto no ha sido encontrado");
            }
            return product;
        }

        #endregion
    }
}
