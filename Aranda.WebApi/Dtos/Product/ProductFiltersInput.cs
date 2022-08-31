namespace Aranda.WebApi.Dtos
{
    public class ProductFiltersInput
    {
        public string SortBy { get; set; }
        public string SortMethod { get; set; }
        public string SearchValue { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; } = 5;
    }
}
