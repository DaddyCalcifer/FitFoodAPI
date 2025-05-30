﻿using System.Globalization;
using System.Security.Claims;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Nutrition;
using FitFoodAPI.Models.Requests;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/food")]
public class FoodController : ControllerBase
{
    private readonly FeedService feedService = new();
    
    [HttpPost("add/{type}")]
    [Authorize]
    public async Task<JsonResult> AddFeed([FromBody] FoodRequest food, string type)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }

        var typeFormatted = type.ToLower().Trim() switch
        {
            "dinner" => FeedType.Dinner,
            "other" => FeedType.Other,
            "breakfast" => FeedType.Breakfast,
            "lunch" => FeedType.Lunch,
            _ => FeedType.Other
        };
        
        var data = new FeedAct()
        {
            UserId = userId,
            Name = food.Name,
            Carb100 = food.Carb,
            Fat100 = food.Fat,
            Protein100 = food.Protein,
            Kcal100 = food.Kcal,
            Mass = food.Mass,
            FeedType = typeFormatted
        };
        
        if(data.Mass <= 0 || data.Carb100 <= 0 && data.Fat100 <= 0 && data.Protein100 <= 0)
            return new JsonResult(new {message = "Error - bad data!"}) { StatusCode = StatusCodes.Status400BadRequest };
        
        await feedService.AddFeed(data);
        return new JsonResult(new {message = "Data addiction success!"}) { StatusCode = StatusCodes.Status201Created };
    }
    
    [HttpDelete("{feedId:guid}/delete")]
    [Authorize]
    public async Task<JsonResult> DeleteFeed(Guid feedId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await feedService.DeleteFeed(feedId, userId);
        
        return result ?
            new JsonResult(new { message = "Something is wrong! Feed data was not deleted" }) { StatusCode = StatusCodes.Status400BadRequest } :
            new JsonResult(new { message = "Feed data was deleted!" }) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet("id:{feedId:guid}")]
    [Authorize]
    public async Task<JsonResult> GetById(Guid feedId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var feed = await feedService.GetById(feedId, userId);
        return feed == null ? 
            new JsonResult(new { message = "Feed data not found!" }) { StatusCode = 404 } : 
            new JsonResult(feed) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet("stats/{day}/{type}")]
    [Authorize]
    public async Task<JsonResult> GetStats(string day, string type)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }

        if (!IsValidDate(day))
            day = DateTime.UtcNow.ToString("dd.MM.yyyy");

        var typeFormatted = type.ToLower().Trim() switch
        {
            "dinner" => FeedType.Dinner,
            "other" => FeedType.Other,
            "breakfast" => FeedType.Breakfast,
            "lunch" => FeedType.Lunch,
            "burnt" => FeedType.Training,
            _ => FeedType.Other
        };

        FeedStats feed;
        if (type == "total")
        {
            feed = await feedService.GetAllFeedStats(userId, day);
        }
        else
        {
            feed = await feedService.GetFeedStatsByType(userId, day, typeFormatted);
        }
        
        return new JsonResult(feed) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet]
    [Authorize]
    public async Task<JsonResult> GetFeeds()
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var feed = await feedService.GetAllByDate(userId, DateTime.UtcNow.ToString("dd.MM.yyyy"));
        return feed.Count == 0 ? 
            new JsonResult(new { message = "Feed data not found!" }) { StatusCode = 404 } : 
            new JsonResult(feed) { StatusCode = StatusCodes.Status200OK };
    }

    [HttpGet("{type}")]

    [Authorize]
    public async Task<JsonResult> GetFeeds(string type)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var typeFormatted = type.ToLower().Trim() switch
        {
            "dinner" => FeedType.Dinner,
            "other" => FeedType.Other,
            "breakfast" => FeedType.Breakfast,
            "lunch" => FeedType.Lunch,
            _ => FeedType.Other
        };
        var feed = await feedService.GetAllByDateAndType(userId, DateTime.UtcNow.ToString("dd.MM.yyyy"), typeFormatted);
        return feed.Count == 0 ? 
            new JsonResult(new { message = "Feed data not found!" }) { StatusCode = 404 } : 
            new JsonResult(feed) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string name)
    {
        var result = await feedService.SearchProducts(name);
        return result == null ? new JsonResult(new { message = "Ничего не найдено!" }) : new JsonResult(result);
    }

    bool IsValidDate(string dateString)
    {
        try
        {
            DateTime.ParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}