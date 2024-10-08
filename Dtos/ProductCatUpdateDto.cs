/*
 * File: ProductCatUpdateDto.cs
 * Project: TechFixBackend.Dtos
 * Description: Data Transfer Object (DTO) for updating an existing product category (ProductCat). It includes optional 
 *              properties such as catName, catDescription, catImageUrl, and catStatus (represented by an enum CategoryStatus).
 *              This DTO is used for submitting update requests for product categories.
 */


using TechFixBackend._Models;

namespace TechFixBackend.Dtos
{
    public class ProductCatUpdateDto
    {
        public string? catName { get; set; }
        public string? catDescription { get; set; }
        public string? catImageUrl { get; set; }
        public CategoryStatus? catStatus { get; set; }
    }
}
