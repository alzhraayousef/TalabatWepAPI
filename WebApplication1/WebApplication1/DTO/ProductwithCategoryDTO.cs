using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class ProductwithCategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public int ResturantID { get; set; }

        public static implicit operator List<object>(ProductwithCategoryDTO v)
        {
            throw new NotImplementedException();
        }
        //public string Resturant { get; set; }
    }

  
}
