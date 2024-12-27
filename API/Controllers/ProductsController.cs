using Code.Interfaces;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
   
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        var lstProducts = await _productRepository.GetProductsAsync(brand, type, sort);
        return Ok(lstProducts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>>GetProduct(int id)
    {
        var product =   await _productRepository.GetProductByIdAsync(id);
        if(product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>>CreateProduct(Product product)
    {
        _productRepository.AddProduct(product);
         if(await _productRepository.SaveChangeAsync())
         {
            return CreatedAtAction("GetProduct",new{id = product.Id}, product);
         }
         return BadRequest("Problem in Add product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult>UpdateProduct(int id, Product product)
    {
        if(product.Id == null || !_productRepository.ProductExists(product.Id))
        {
            return BadRequest("Can not update this product!");
        }
        _productRepository.UpdateProduct(product);
        if(await _productRepository.SaveChangeAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem updating this product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult>DeleteProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if(product == null) return NotFound();

        _productRepository.DeleteProduct(product);
        if(await _productRepository.SaveChangeAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem in Delete this product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<string>>GetBrands()
    {
        return Ok(await _productRepository.GetBrands());
    }

    [HttpGet("types")]
    public async Task<ActionResult<string>>GetTypes()
    {
        return Ok(await _productRepository.GetTypes());
    }


}