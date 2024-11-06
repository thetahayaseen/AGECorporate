using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AGECorporate.Models;
using AGECorporate.Repo_Patterns;
using AGECorporate.View_Models;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly string _uploadsFolderPath;

    public ProductController(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        _productCategoryRepository = productCategoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var productCategories = await _productCategoryRepository.GetAllAsync();
        ProductFormViewModel productFormViewModel = new ProductFormViewModel();
        productFormViewModel.productCategories = productCategories.ToList();
        productFormViewModel.product = new Product();
        return View(productFormViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductFormViewModel productFormViewModel, IFormFile? imageFile)
    {
        var product = productFormViewModel.product;

        if (ModelState.IsValid)
        {
            // Insert the product into the database first
            await _productRepository.InsertAsync(product!);

            // Only process the image if the file is not null and has content
            if (imageFile != null && imageFile.Length > 0)
            {
                if (!Directory.Exists(_uploadsFolderPath))
                {
                    Directory.CreateDirectory(_uploadsFolderPath);
                }

                var fileName = $"{product.Id}.jpg"; // Use the product ID for naming the image file
                var filePath = Path.Combine(_uploadsFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Update the product with the image path
                product.ImagePath = $"/uploads/{fileName}";
                await _productRepository.UpdateAsync(product);
            }

            // Redirect to the Products page after successful creation
            return RedirectToAction("Products", "Admin");
        }

        // If we reach here, something went wrong with validation, so reload the categories
        var productCategories = await _productCategoryRepository.GetAllAsync();
        productFormViewModel.productCategories = productCategories.ToList();

        return View(productFormViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        var productCategories = await _productCategoryRepository.GetAllAsync();
        ProductFormViewModel productFormViewModel = new ProductFormViewModel();
        productFormViewModel.productCategories = productCategories.ToList();
        productFormViewModel.product = product;

        return View(productFormViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, ProductFormViewModel productFormViewModel, IFormFile? imageFile)
    {
        var product = productFormViewModel.product;

        // Check if the provided product ID matches the model's ID
        if (id != product!.Id)
        {
            return BadRequest("Product ID mismatch.");
        }

        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            var productCategories = await _productCategoryRepository.GetAllAsync();
            productFormViewModel.productCategories = productCategories.ToList();
            return View(productFormViewModel);
        }

        // Get the existing product from the database
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        // Update basic properties
        existingProduct.Title = product.Title;
        existingProduct.Description = product.Description;

        // Handle image upload if a new image is provided
        if (imageFile != null && imageFile.Length > 0)
        {
            try
            {
                if (!Directory.Exists(_uploadsFolderPath))
                {
                    Directory.CreateDirectory(_uploadsFolderPath);
                }

                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(existingProduct.ImagePath))
                {
                    var oldImagePath = Path.Combine(_uploadsFolderPath, Path.GetFileName(existingProduct.ImagePath));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Generate a new unique filename based on timestamp
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = $"{existingProduct.Id}_{timestamp}.jpg";
                var filePath = Path.Combine(_uploadsFolderPath, fileName);

                // Save the new image to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Update the image path in the product entity
                existingProduct.ImagePath = $"/uploads/{fileName}";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Image upload failed: " + ex.Message);
                // Reload categories and return the view with an error
                var productCategories = await _productCategoryRepository.GetAllAsync();
                productFormViewModel.productCategories = productCategories.ToList();
                return View(productFormViewModel);
            }
        }

        // Save the updated product to the database
        await _productRepository.UpdateAsync(existingProduct);

        // Redirect to the Products page after successful update
        return RedirectToAction("Products", "Admin");
    }

    [HttpGet, ActionName("Delete")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(product.ImagePath))
        {
            var imagePath = Path.Combine(_uploadsFolderPath, Path.GetFileName(product.ImagePath));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        await _productRepository.DeleteAsync(id);
        return RedirectToAction("Products", "Admin");
    }
}