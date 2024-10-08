/*
 * File: ProductCatUpdateDto.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductCatUpdateDto data transfer object for the Healthy Bites system. It represents the data required to update 
 *              an existing product category, including category name, description, image URL, and status.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductCatUpdateDto: Data transfer object used for updating an existing product category in the Healthy Bites system.
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
