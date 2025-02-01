using FitFoodAPI.Models;
using FitFoodAPI.Models.Nutrition;
using Newtonsoft.Json;
using OpenFoodFacts4Net.Json.Data;

namespace FitFoodAPI.Services;

public class OpenFoodFactsClient
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<ProductData?> GetProductByBarcodeAsync(string barcode)
    {
        string apiUrl = $"https://world.openfoodfacts.org/api/v0/product/{barcode}.json";

        HttpResponseMessage response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Ошибка получения данных от Open Food Facts.");
        }

        string jsonResponse = await response.Content.ReadAsStringAsync();
        dynamic? productData = JsonConvert.DeserializeObject(jsonResponse);
        
        if(productData == null) return null;

        if (productData == null || productData.status != 1)
        {
            throw new Exception("Product not found.");
        }

        var product = new ProductData
        {
            Name = productData.product.product_name ?? "Неизвестно",
            Manufacturer = productData.product.brands ?? "Неизвестно",
            Calories = (double)(productData.product.nutriments["energy-kcal_100g"] ?? 0),
            Protein = (double)(productData.product.nutriments["proteins_100g"] ?? 0),
            Fat = (double)(productData.product.nutriments["fat_100g"] ?? 0),
            Carbohydrates = (double)(productData.product.nutriments["carbohydrates_100g"] ?? 0),
            Weight = (double)(productData.product.product_quantity ?? 0),
            Ingredients = productData.product.ingredients_text ?? "Неизвестно"
        };

        return product;
    }
}