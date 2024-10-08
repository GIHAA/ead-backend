/*
 * File: ProductCatCreateDto.cs
 * Project: Healthy Bites.Dtos
 * Description: Data Transfer Object (DTO) for creating a new product category (ProductCat). It contains properties such as 
 *              CatName, CatDescription, and CatImageUrl that are required when submitting a new category creation request.
 */


namespace TechFixBackend.Dtos
{
    public class ProductCatCreateDto
    {
        public string CatName { get; set; }
        public string CatDescription { get; set; }
        public string CatImageUrl { get; set; }
    }
}
