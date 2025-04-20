using System.Net;
using FitFoodAPI.Database;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Fit;
using FitFoodAPI.Models.Nutrition;
using FitFoodAPI.Models.Requests;
using FitFoodAPI.Models.Sport;
using FitFoodAPI.Services.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitFoodAPI.Services;

public class SportService
{
    //Программы тренировок
    public async Task<TrainingPlan?> CreatePlan(Guid userId, string name, string description="")
    {
        var plan = new TrainingPlan()
        {
            UserId = userId,
            Name = name,
            Description = description
        };

        await using var context = new FitEntitiesContext();
        var user = await context.Users
            .Include(e => e.TrainingPlans)
            .FirstOrDefaultAsync(c => c.Id == userId);
            
        if (user == null) return null;
            
        user.TrainingPlans.Add(plan);
        context.Users.Update(user);
            
        await context.SaveChangesAsync();

        return plan;
    }
    public async Task<Exercise?> AddExercise(Guid userId, Guid planId, AddExerciseRequest exerciseRequest)
    {
        var exercise = new Exercise()
        {
            TrainingPlanId = planId,
            Name = exerciseRequest.name,
            Description = exerciseRequest.description,
            Reps = exerciseRequest.reps,
            Sets = exerciseRequest.sets,
            Weight = exerciseRequest.weight,
            RepCaloriesLoss = exerciseRequest.repCaloriesLoss,
            RepsIsSeconds = exerciseRequest.repsIsSeconds
        };

        await using var context = new FitEntitiesContext();
        var plan = await context.TrainingPlans
            .Include(e => e.Exercises)
            .FirstOrDefaultAsync(c => c.Id == planId);
            
        if (plan!.UserId != userId) return null;
            
        plan.Exercises.Add(exercise);
        context.TrainingPlans.Update(plan);
            
        await context.SaveChangesAsync();

        return exercise;
    }
    
    public async Task<bool> DeletePlan(Guid planId, Guid userId)
    {
        await using var context = new FitEntitiesContext();
        var plan = await context.TrainingPlans.FirstOrDefaultAsync(c => c.Id == planId);
            
        if (plan == null) return false;
        if (plan.UserId != userId) return false;
            
        plan.isDeleted = true;
        context.TrainingPlans.Update(plan);
            
        await context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteExercise(Guid exerciseId, Guid userId)
    {
        await using var context = new FitEntitiesContext();
        var exer = await context.Exercises
            .Include(e => e.TrainingPlan)
            .FirstOrDefaultAsync(c => c.Id == exerciseId);
            
        if (exer == null) return false;
        if (exer.TrainingPlan!.UserId != userId) return false;
        
        context.Exercises.Remove(exer);
            
        await context.SaveChangesAsync();
        return true;
    }
    public async Task<TrainingPlan?> GetPlan(Guid planId, Guid userId)
    {
        await using var context = new FitEntitiesContext();
        var plan = await context.TrainingPlans
            .Include(e => e.Exercises)
            .FirstOrDefaultAsync(e => e.Id == planId);
        return plan!.isDeleted ? null : plan;
    }
    public async Task<Guid?> FindPlanByCalories(int targetCalories)
    {
        await using var context = new FitEntitiesContext();

        var plans = await context.TrainingPlans
            .Where(p => !p.isDeleted)
            .Select(p => new
            {
                p.Id,
                p.CaloriesLoss
            })
            .ToListAsync();

        if (plans.Count == 0)
            return null;

        // Находим план с минимальной разницей между CaloriesLoss и целевым значением
        var closestPlan = plans
            .OrderBy(p => Math.Abs(p.CaloriesLoss - targetCalories))
            .First();

        return closestPlan.Id;
    }
    public async Task<List<TrainingPlan>> GetPlans(Guid userId)
    {
        await using var context = new FitEntitiesContext();
    
        var plans = await context.TrainingPlans
            .Where(e => !e.isDeleted)
            .Include(e => e.Exercises)
            .AsNoTracking()
            .ToListAsync();
    
        return plans;
    }
    
    //Тренировки
    //Создание тренировки
    public async Task<Training?> CreateTraining(Guid userId, Guid trainingPlanId)
    {
        await using var context = new FitEntitiesContext();
        
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return null;
        
        var training = new Training()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TrainingPlanId = trainingPlanId
        };

        context.Trainings.Add(training);

        await context.SaveChangesAsync();

        return training;
    }
    //Добавление упражнений
    public async Task<Guid?> AddExercisesToTraining(Guid trainingId)
    {
        await using var context = new FitEntitiesContext();

        var training = await context.Trainings
            .Include(e => e.Exercises)
            .Include(e => e.TrainingPlan).ThenInclude(trainingPlan => trainingPlan!.Exercises)
            .FirstOrDefaultAsync(e => e.Id == trainingId);

        if (training?.TrainingPlan == null)
        {
            return null;
        }

        foreach (var exercise in training.TrainingPlan.Exercises)
        {
            context.ExerciseProgress.Add(new ExerciseProgress()
            {
                ExerciseId = exercise.Id,
                TrainingId = trainingId,
            });
        }
        await context.SaveChangesAsync();

        return training.Id;
    }
    public async Task<Guid?> AddSetsToTraining(Guid trainingId)
    {
        await using var context = new FitEntitiesContext();

        var training = await context.Trainings
            .Include(e => e.Exercises)
            .Include(e => e.TrainingPlan).ThenInclude(trainingPlan => trainingPlan!.Exercises)
            .FirstOrDefaultAsync(e => e.Id == trainingId);

        if (training?.TrainingPlan == null)
        {
            return null;
        }

        foreach (var exercise in training.Exercises)
        {
            var exerc = training.TrainingPlan.Exercises
                .FirstOrDefault(e => e.Id == exercise.ExerciseId);

            if (exerc == null)
            {
                continue; // Пропускаем, если упражнение не найдено
            }

            for (var i = 0; i < exerc.Sets; i++)
            {
                context.Sets.Add(new Set()
                {
                    Id = Guid.NewGuid(),
                    SetNumber = (byte)(i + 1),
                    ExerciseProgressId = exercise.Id,
                    Reps = exerc.Reps,
                    Weight = exerc.Weight,
                    isCompleted = false
                });
            }
        }
        await context.SaveChangesAsync();

        return training.Id;
    }

    public async Task<bool> DeleteTraining(Guid trainingId, Guid userId)
    {
        await using var context = new FitEntitiesContext();
        var training = await context.Trainings.FirstOrDefaultAsync(c => c.Id == trainingId);
            
        if (training == null) return false;
        if (training.UserId != userId) return false;
        
        context.Trainings.Remove(training);
        await context.SaveChangesAsync();
        
        return true;
    }
    public async Task<Training?> GetTraining(Guid trainingId, Guid userId)
    {
        await using var context = new FitEntitiesContext();
        
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
        if(user == null) return null;
        
        var training = await context.Trainings
            .Include(t => t.TrainingPlan)
            .Include(t => t.Exercises)
                .ThenInclude(e => e.Sets.OrderBy(s => s.SetNumber))
            .Include(t => t.Exercises)
                .ThenInclude(t => t.Exercise)
            .FirstOrDefaultAsync(e => e.Id == trainingId);
        if(training == null) return null;
        
        training.User = null;
        if (training.TrainingPlan != null)
        {
            training.TrainingPlan.User = null;
            training.TrainingPlan.Exercises = [];
        }

        if (training.Exercises.Count > 0)
        {
            foreach (var exe in training.Exercises)
            {
                exe.Exercise!.TrainingPlan = null;
            }
        }

        return training.UserId != userId ? null : training;
    }
    public async Task<List<Training>> GetTrainings(Guid userId)
    {
        await using var context = new FitEntitiesContext();
    
        var trainings = await context.Trainings
            .Include(t => t.TrainingPlan)
            .Where(e => e.UserId == userId)
            .Include(t => t.Exercises)
                .ThenInclude(t => t.Sets.OrderBy(s=>s.SetNumber))
            .Include(t => t.Exercises)
                .ThenInclude(t => t.Exercise)
            .ToListAsync();

        foreach (var training in trainings)
        {
            training.User = null;
            if (training.TrainingPlan != null)
            {
                training.TrainingPlan.User = null;
                training.TrainingPlan.Exercises = [];
            }
            if (training.Exercises.Count > 0)
            {
                foreach (var exe in training.Exercises)
                {
                    exe.Exercise!.TrainingPlan = null;
                }
            }
        }
        return trainings;
    }
    
    //Подходы
    public async Task<Set?> CompleteSet(Guid userId, Guid setId, int reps, double weight)
    {
        await using var context = new FitEntitiesContext();
    
        var set = await context.Sets
            .Where(e => e.Id == setId)
            .Include(s => s.ExerciseProgress)
            .ThenInclude(ep => ep!.Exercise)
            .FirstOrDefaultAsync();
        
        set!.Reps = reps;
        set!.Weight = weight;
        set!.isCompleted = true;
    
        context.Sets.Update(set);

        var setAct = new FeedAct();
        if (set.ExerciseProgress is { Exercise: not null })
        {
            setAct.UserId = userId;
            setAct.Carb100 = setAct.Fat100 = setAct.Protein100 = 0.1;
            setAct.Mass = 100;
            setAct.Kcal100 = (set.ExerciseProgress.Exercise.TotalCaloriesLoss / set.ExerciseProgress.Exercise.Sets) * -1.0;
            setAct.FeedType = FeedType.Training;
            setAct.Date = DateTime.UtcNow.ToString("dd.MM.yyyy");
            setAct.Name = $"[Тренировка] {set.ExerciseProgress.Exercise.Name}";
        }
        context.FeedActs.Add(setAct);
        
        await context.SaveChangesAsync();
        
        return set;
    }
}