using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CetBookStore1.ViewModel
{
    public class SearchViewModel
    {
         [Display(Name ="Arama Metni")]
        public string SearchText { get; set; }

        [Display(Name ="Açıklamalarda Ara")]
        public bool SearchInDescription { get; set; }

        [Display(Name ="Kategori Seçimi")]
        public int? CategoryId { get; set; }

        public List<Models.Book> Results { get; set; }

        //Yeni Eklenen
        [Display(Name ="En Düşük Fiyat")]
        public decimal MinPrice { get; set; }

        //Yeni Eklenen
        [Display(Name = "En Yüksek Fiyat")]
        public decimal MaxPrice { get; set; }
    }
}
