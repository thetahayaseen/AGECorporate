using AGECorporate.Repo_Patterns;
using AGECorporate.View_Models;
using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly IQueryRepository _queryRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductRepository _productRepository;

    public AdminController(ILogger<AdminController> logger, IQueryRepository queryRepository, IProductCategoryRepository productCategoryRepository, IProductRepository productRepository)
    {
        _logger = logger;
        _queryRepository = queryRepository;
        _productCategoryRepository = productCategoryRepository;
        _productRepository = productRepository;
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index(AdminDashboardViewModel adminDashboardViewModel)
    {
        int countOfQueries = await _queryRepository.CountAllAsync();
        int countOfProductCategories = await _productCategoryRepository.CountAllAsync();
        int countOfProducts = await _productRepository.CountAllAsync();
        adminDashboardViewModel.countOfQueries = countOfQueries;
        adminDashboardViewModel.countOfProductCategories = countOfProductCategories;
        adminDashboardViewModel.countOfProducts = countOfProducts;
        return View(adminDashboardViewModel);
    }

    public async Task<IActionResult> Queries()
    {
        var queries = await _queryRepository.GetAllAsync();
        return View(queries.ToList()); 
    }

    public async Task<IActionResult> ProductCategories()
    {
        var productCategories = await _productCategoryRepository.GetAllAsync();
        return View(productCategories.ToList());
    }

    public async Task<IActionResult> Products()
    {
        var products = await _productRepository.GetAllAsync();
        return View(products.ToList());
    }
}
