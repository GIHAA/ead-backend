/*
 * File: ProductCatDto.cs
 * Project: Healthy Bites.Dtos
 * Description: Data Transfer Object (DTO) for representing a product category (ProductCat). It includes properties 
 *              such as Id, Category, CatDescription, ImageUrl, and CatStatus (represented by an enum CategoryStatus).
 *              This DTO is used for retrieving product category data.
 */



using TechFixBackend._Models;

namespace TechFixBackend.Dtos
{
    public class ProductCatDto
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string CatDescription { get; set; }
        public string ImageUrl { get; set; }
        public CategoryStatus CatStatus { get; set; }
    }
}
