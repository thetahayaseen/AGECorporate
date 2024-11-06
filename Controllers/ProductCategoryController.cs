using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AGECorporate.Models;
using AGECorporate.Repo_Patterns;

public class ProductCategoryController : Controller
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly string _uploadsFolderPath;

    public ProductCategoryController(IProductCategoryRepository productCategoryRepository)
    {
        _productCategoryRepository = productCategoryRepository;
        _uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCategory productCategory, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            await _productCategoryRepository.InsertAsync(productCategory);

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!Directory.Exists(_uploadsFolderPath))
                {
                    Directory.CreateDirectory(_uploadsFolderPath);
                }

                var fileName = $"{productCategory.Id}.jpg";
                var filePath = Path.Combine(_uploadsFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                productCategory.ImagePath = $"/uploads/{fileName}";
                await _productCategoryRepository.UpdateAsync(productCategory);
                return RedirectToAction("ProductCategories", "Admin");
            }

            ModelState.AddModelError("ImageFile", "Please upload a valid image.");
        }

        return View(productCategory);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var productCategory = await _productCategoryRepository.GetByIdAsync(id);
        if (productCategory == null)
        {
            return NotFound();
        }
        return View(productCategory);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, ProductCategory productCategory, IFormFile? imageFile)
    {
        if (id != productCategory.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(productCategory);
        }

        var existingCategory = await _productCategoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        existingCategory.Title = productCategory.Title;
        existingCategory.Description = productCategory.Description;

        if (imageFile != null && imageFile.Length > 0)
        {
            if (!Directory.Exists(_uploadsFolderPath))
            {
                Directory.CreateDirectory(_uploadsFolderPath);
            }

            if (!string.IsNullOrEmpty(existingCategory.ImagePath))
            {
                var oldImagePath = Path.Combine(_uploadsFolderPath, Path.GetFileName(existingCategory.ImagePath));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"{existingCategory.Id}_{timestamp}.jpg";
            var filePath = Path.Combine(_uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            existingCategory.ImagePath = $"/uploads/{fileName}";
        }

        await _productCategoryRepository.UpdateAsync(existingCategory);
        return RedirectToAction("ProductCategories", "Admin");
    }

    [HttpGet, ActionName("Delete")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        var productCategory = await _productCategoryRepository.GetByIdAsync(id);
        return View(productCategory);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var productCategory = await _productCategoryRepository.GetByIdAsync(id);

        if (productCategory == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(productCategory.ImagePath))
        {
            var imagePath = Path.Combine(_uploadsFolderPath, Path.GetFileName(productCategory.ImagePath));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        await _productCategoryRepository.DeleteAsync(id);
        return RedirectToAction("ProductCategories", "Admin");
    }
}