/*
 * File: ProductCatDto.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductCatDto data transfer object for the Healthy Bites system. It represents the data related to a product 
 *              category, including category ID, name, description, image URL, and status.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductCatDto: Data transfer object used for representing a product category in the Healthy Bites system.
 */

using HealthyBites._Models;

namespace HealthyBites.Dtos
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
