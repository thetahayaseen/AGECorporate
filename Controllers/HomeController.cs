using AGECorporate.Models;
using AGECorporate.Repo_Patterns;
using AGECorporate.View_Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AGECorporate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQueryRepository _queryRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;

        public HomeController(ILogger<HomeController> logger, IQueryRepository queryRepository, IProductCategoryRepository productCategoryRepository, IProductRepository productRepository)
        {
            _logger = logger;
            _queryRepository = queryRepository;
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("~/About")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        [Route("~/Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Route("~/Contact")]
        public async Task<IActionResult> Contact(Query query)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _queryRepository.InsertAsync(query);
                    return RedirectToAction("Index");   
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return View(query);
        }

        [Route("~/Products")]
        public async Task<IActionResult> Products()
        {
            var productCategories = await _productCategoryRepository.GetAllAsync();
            return View(productCategories.ToList());
        }

        [Route("~/Products/Category/{id}")]
        public async Task<IActionResult> ProductCategory(int id)
        {
            var productCategory = await _productCategoryRepository.GetByIdAsync(id);
            var products = await _productRepository.GetByCategoryAsync(id);

            ProductCategoryDisplayViewModel productCategoryDisplayViewModel = new ProductCategoryDisplayViewModel();
            productCategoryDisplayViewModel.productCategory = productCategory;
            productCategoryDisplayViewModel.products = products.ToList();

            return View(productCategoryDisplayViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
