using System.Net;
using System.Security.Claims;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Requests;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/sport")]
public class SportController : ControllerBase
{
    private readonly SportService sportService = new();

    //Программы тренировок
    [HttpPost("plans/create")]
    [Authorize]
    public async Task<JsonResult> CreatePlan(
        [FromBody]CreatePlanRequest request)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var plan = await sportService.CreatePlan(userId, request.name, request.description);
        plan.User = null;
        
        Console.WriteLine($"Training plan {plan.Id} has been built for user {userId}");
        return new JsonResult(plan){ StatusCode = StatusCodes.Status201Created };
    }
    [HttpPut("plans/{planId:guid}/exercise")]
    [Authorize]
    public async Task<JsonResult> AddExercise(Guid planId, [FromBody]AddExerciseRequest request)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await sportService.AddExercise(userId, planId, request);

        return result != null
            ? new JsonResult(result) { StatusCode = StatusCodes.Status200OK }
            : new JsonResult(new { message = "Ошибка при добавлении упражнения" })
                { StatusCode = StatusCodes.Status400BadRequest };
    }
    [HttpDelete("plans/{planId:guid}/delete")]
    [Authorize]
    public async Task<JsonResult> DeletePlan(Guid planId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await sportService.DeletePlan(planId, userId);

        return result
            ? new JsonResult(new { message = "Plan was deleted!" }) { StatusCode = StatusCodes.Status200OK }
            : new JsonResult(new { message = "Something is wrong! Plan was not deleted" })
                { StatusCode = StatusCodes.Status400BadRequest };
    }
    [HttpDelete("plans/exercise/{exerId:guid}/delete")]
    [Authorize]
    public async Task<JsonResult> DeleteExercise(Guid exerId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await sportService.DeleteExercise(exerId, userId);

        return result
            ? new JsonResult(new { message = "Exercise was deleted!" }) { StatusCode = StatusCodes.Status200OK }
            : new JsonResult(new { message = "Something is wrong! Exercise was not deleted" })
                { StatusCode = StatusCodes.Status400BadRequest };
    }
    [HttpGet("plans/{planId:guid}")]
    [Authorize]
    public async Task<JsonResult> GetById(Guid planId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var plan = await sportService.GetPlan(planId, userId);
        return plan == null ? 
            new JsonResult(new { message = "Программа тренировки не найдена!" }) { StatusCode = 404 } : 
            new JsonResult(plan) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet("plans/kcal:{kcal:int}")]
    [Authorize]
    public async Task<JsonResult> GetByKcal(int kcal)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var plan = await sportService.FindPlanByCalories(kcal);
        return plan == null ? 
            new JsonResult(new { message = "Программа тренировки не найдена!" }) { StatusCode = 404 } : 
            new JsonResult(plan) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet("plans")]
    [Authorize]
    public async Task<JsonResult> GetAll()
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var plans = await sportService.GetPlans(userId);
        return plans.Count < 1 ? 
            new JsonResult(new { message = "Программы тренировок не найдены!" }) { StatusCode = 404 } : 
            new JsonResult(plans) { StatusCode = StatusCodes.Status200OK };
    }
    
    //Тренировки
    [HttpPost("trainings/create:{planId:guid}")]
    [Authorize]
    public async Task<JsonResult> CreateTraining(Guid planId)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var training = await sportService.CreateTraining(userId, planId);
        if(training == null) return new JsonResult(new { message = "Ошибка при создании тренировки!" }){ StatusCode = StatusCodes.Status400BadRequest };
        
        var trainingId = await sportService.AddExercisesToTraining(training.Id);
        if(trainingId == null) return new JsonResult(new { message = "Ошибка при загрузке списка упражнений!" }){ StatusCode = StatusCodes.Status400BadRequest };
        
        trainingId = await sportService.AddSetsToTraining(training.Id);
        if(trainingId == null) return new JsonResult(new { message = "Ошибка при добавлении подходов!" }){ StatusCode = StatusCodes.Status400BadRequest };
        
        training.User = null;
        
        Console.WriteLine($"Тренировка {training.Id} создана для пользователя {userId}");
        return new JsonResult(training){ StatusCode = StatusCodes.Status201Created };
    }
    [HttpDelete("trainings/{trainingId:guid}/delete")]
    [Authorize]
    public async Task<JsonResult> DeleteTraining(Guid trainingId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await sportService.DeleteTraining(trainingId, userId);

        return result
            ? new JsonResult(new { message = "Training was deleted!" }) { StatusCode = StatusCodes.Status200OK }
            : new JsonResult(new { message = "Something is wrong! Training was not deleted" })
                { StatusCode = StatusCodes.Status400BadRequest };
    }
    [HttpGet("trainings/{trainingId:guid}")]
    [Authorize]
    public async Task<JsonResult> GetTrainingById(Guid trainingId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var training = await sportService.GetTraining(trainingId, userId);
        return training == null ? 
            new JsonResult(new { message = "Тренировка не найдена!" }) { StatusCode = 404 } : 
            new JsonResult(training) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet("trainings")]
    [Authorize]
    public async Task<JsonResult> GetAllTrainings()
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var trainings = await sportService.GetTrainings(userId);
        return trainings.Count < 1 ? 
            new JsonResult(new { message = "Тренировки не найдены!" }) { StatusCode = 404 } : 
            new JsonResult(trainings) { StatusCode = StatusCodes.Status200OK };
    }
    
    //Подходы
    [HttpPut("set/{setId:guid}/{reps:int}:{weight:double}")]
    [Authorize]
    public async Task<JsonResult> CompleteSet(Guid setId, int reps, double weight)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await sportService.CompleteSet(userId, setId, reps, weight);
        
        return result == null ?
            new JsonResult(new { message = "Ошибка при завершении подхода!" }) { StatusCode = StatusCodes.Status400BadRequest } :
            new JsonResult(new { message = "Подход завершен. Так держать!", value = result }) { StatusCode = StatusCodes.Status200OK };
    }
}