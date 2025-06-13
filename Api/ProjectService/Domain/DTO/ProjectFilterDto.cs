namespace Domain.DTO
{
    public class ProjectFilterDto
    {
        public SortOrder SortOrder { get; set; } = SortOrder.Default;
        public string? Name { get; set; } = null;
    }

    public enum SortOrder
    {
        Default = 0, // Стандартная сортировка (например, по дате создания)
        ByRating = 1, // По рейтингу (от высокого к низкому)
        ByViews = 2, // По количеству просмотров (от большего к меньшему)
        ByNameAsc = 3, // По имени по возрастанию (A-Z)
        ByNameDesc = 4 // По имени по убыванию (Z-A)
    }
}