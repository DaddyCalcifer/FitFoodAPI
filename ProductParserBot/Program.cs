using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using Microsoft.EntityFrameworkCore;
using ProductParserBot;

class Program
{
    static void Main()
    {
        string pdfPath = "C:\\Users\\Nautilus\\Pictures\\Saved Pictures\\tabliza_kaloriinosti.pdf";
        List<ProductData> products = ExtractProductsFromPdf(pdfPath);

        using (var db = new ApplicationDbContext())
        {
            db.Products.AddRange(products);
            db.SaveChanges();
        }

        Console.WriteLine($"Данные успешно сохранены в БД! Позиций: {products.Count}");
    }

    static List<ProductData> ExtractProductsFromPdf(string pdfPath)
    {
        var productList = new List<ProductData>();
        string lastMainProduct = ""; // Хранит последнее основное название

        // Улучшенное регулярное выражение: поддерживает основные и вложенные продукты
        var regex = new Regex(@"(>>\s*)?([\w\s\""\-()]+)\s+([\d,.-]+)\s+([\d,.-]+)\s+([\d,.-]+)\s+([\d,.-]+)");

        using (PdfDocument pdf = PdfDocument.Open(pdfPath))
        {
            List<string> lines = new List<string>();

            // Собираем текст из всех страниц в одну строку
            foreach (var page in pdf.GetPages())
            {
                string text = page.Text;

                // Удаляем разрывы строк и лишние пробелы
                text = Regex.Replace(text, @"\n+", " ");
                text = Regex.Replace(text, @"\s{2,}", " ").Trim();

                lines.Add(text);
            }

            string fullText = string.Join(" ", lines);

            var matches = regex.Matches(fullText);

            foreach (Match match in matches)
            {
                if (match.Groups.Count < 6) continue; // Пропускаем некорректные строки

                try
                {
                    bool isSubProduct = !string.IsNullOrEmpty(match.Groups[1].Value); // Проверяем `>>`
                    string productName = CleanProductName(match.Groups[2].Value);

                    // Если это подкатегория (`>> говяжьи`), добавляем к последнему основному продукту
                    if (isSubProduct && !string.IsNullOrEmpty(lastMainProduct))
                    {
                        productName = $"{lastMainProduct} {productName}";
                    }
                    else
                    {
                        lastMainProduct = productName; // Обновляем основное название
                    }

                    // Фильтруем подозрительные названия (где подряд идут 4 числа или тире)
                    if (IsSuspiciousName(productName)) continue;

                    var product = new ProductData
                    {
                        Id = Guid.NewGuid(),
                        Name = productName.Replace("ккал", "").Trim(),
                        Calories = ParseValue(match.Groups[6].Value),
                        Protein = ParseValue(match.Groups[3].Value),
                        Fat = ParseValue(match.Groups[4].Value),
                        Carbohydrates = ParseValue(match.Groups[5].Value),
                        Weight = 100
                    };
                    Console.WriteLine(product.ToString());
                    productList.Add(product);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка парсинга строки: {match.Value}. Ошибка: {ex.Message}");
                }
            }
        }
        return productList;
    }

    /// <summary>
    /// Убирает лишние символы из названия продукта
    /// </summary>
    static string CleanProductName(string name)
    {
        name = name.Trim();
        name = Regex.Replace(name, @"[""()]", ""); // Убираем кавычки и скобки
        name = Regex.Replace(name, @"\s{2,}", " "); // Убираем лишние пробелы
        return name;
    }

    /// <summary>
    /// Проверяет, является ли название подозрительным (если в нем подряд идут 4 числа или тире)
    /// </summary>
    static bool IsSuspiciousName(string name)
    {
        var pattern = @"(\d+|-)\s+(\d+|-)\s+(\d+|-)\s+(\d+|-)";
        return Regex.IsMatch(name, pattern);
    }

    /// <summary>
    /// Преобразует строку в число, обрабатывает прочерки и диапазоны
    /// </summary>
    static double ParseValue(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input == "-")
            return 0;

        // Если это диапазон (например, "3,8-4,5"), берем среднее значение
        if (input.Contains("-"))
        {
            var parts = input.Split('-');
            if (parts.Length == 2 && 
                double.TryParse(parts[0].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double val1) &&
                double.TryParse(parts[1].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double val2))
            {
                return (val1 + val2) / 2;
            }
        }

        // Парсим одиночное число (целое или с запятой)
        if (double.TryParse(input.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            return result;

        return 0; // В случае ошибки возвращаем 0
    }
}