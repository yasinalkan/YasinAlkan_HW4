using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CetBookStore1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using CetBookStore1.Data;
using Microsoft.EntityFrameworkCore;
using CetBookStore1.ViewModel;

namespace CetBookStore1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Search()
        {            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel searchModel)
        {
            IQueryable<Book> books = _context.Books.AsQueryable();

            //Kitap ismi girildiyse.
            //Açıklamalarda arama da seçildiyse.
            if (!String.IsNullOrWhiteSpace(searchModel.SearchText))
            {
                if (searchModel.SearchInDescription)
                {
                    books = books.Where(b => b.Title.Contains(searchModel.SearchText) || b.Description.Contains(searchModel.SearchText));
                }
                else
                {
                    books = books.Where(b => b.Title.Contains(searchModel.SearchText));
                }
            }

            //Kullanıcı en düşük fiyat girdiyse fiyata göre küçükten büyüğe sıralar.
            if (!String.IsNullOrWhiteSpace(searchModel.MinPrice.ToString()) 
                && searchModel.MinPrice >=0)
            {
                books = books.Where(b => b.Price >= searchModel.MinPrice)
                              .OrderBy(b => b.Price);

            }

            //Kullanıcı en yüksek fiyat girdiyse fiyata göre küçükten büyüğe sıralar.
            if (!String.IsNullOrWhiteSpace(searchModel.MaxPrice.ToString()) 
                && searchModel.MaxPrice > 0)
            {
                books = books.Where(b => b.Price >= 0 && b.Price <= searchModel.MaxPrice)
                              .OrderBy(b => b.Price);
            }

            //Arama kategori içeriyorsa
            if (searchModel.CategoryId.HasValue)
            {
                books = books.Where(b => b.CategoryId == searchModel.CategoryId.Value);
            }

            //Post metodu çalıştıktan sonra dropdown içinde tekrar kategorilerin gözükmesi isteniyorsa.
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", searchModel.CategoryId);

            searchModel.Results = await books.ToListAsync();
            return View(searchModel);
        }
    }
}
