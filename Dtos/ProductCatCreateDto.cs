/*
 * File: ProductCatCreateDto.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductCatCreateDto data transfer object for the Healthy Bites system. It represents the data required to create 
 *              a new product category, including category name, description, and image URL.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductCatCreateDto: Data transfer object used for creating a new product category in the Healthy Bites system.
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
