namespace WebApplication1.DTO
{
    public class CategoryWithResturantDTO
    {
        public int Id { get; set; }
        public string ResturantName { get; set; }
        public List<CategoriesDTO> CategoryList { get; set; } = new List<CategoriesDTO>();
    }
    public class CategoriesDTO
    {
        public int Id { get; set; }
        public string CatergoryName { get; set; }

    }
}
