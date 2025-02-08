using System.Net;
using FitFoodAPI.Database;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Fit;
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
        if(plan!.UserId != userId || plan.isDeleted) return null;
        return plan;
    }
    public async Task<List<TrainingPlan>> GetPlans(Guid userId)
    {
        await using var context = new FitEntitiesContext();
    
        var plans = await context.TrainingPlans
            .Where(e => e.UserId == userId && !e.isDeleted)
            .Include(e => e.Exercises)
            .AsNoTracking()
            .ToListAsync();
    
        return plans;
    }
    
    //Тренировки
    public async Task<Training?> CreateTraining(Guid userId, Guid trainingPlanId)
    {
        var training = new Training()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TrainingPlanId = trainingPlanId
        };

        await using var context = new FitEntitiesContext();
        
        var plan = await context.TrainingPlans
            .Include(e => e.Exercises)
            .FirstOrDefaultAsync(e => e.Id == trainingPlanId);
        foreach (var exerc in plan!.Exercises)
        {
            var exercise = new ExerciseProgress()
            {
                Id = Guid.NewGuid(),
                ExerciseId = exerc.Id,
                TrainingId = training.Id
            };
            for (int i = 0; i < exerc.Sets; i++)
            {
                exercise.Sets.Add(new Set()
                {
                    SetNumber = (byte)(i+1),
                    ExerciseProgressId = exercise.Id,
                    Reps = exerc.Reps,
                    Weight = exerc.Weight,
                    isCompleted = false
                });
            }
            training.Exercises.Add(exercise);
        }
        
        var user = await context.Users
            .Include(u => u.Trainings)
            .FirstOrDefaultAsync(c => c.Id == userId);
            
        if (user == null) return null;
            
        user.Trainings.Add(training);
        context.Users.Update(user);
            
        await context.SaveChangesAsync();

        return training;
    }
    
    public async Task<bool> DeleteTraining(Guid trainingId, Guid userId)
    {
        await using var context = new FitEntitiesContext();
        var training = await context.Trainings.Include(training => training.Exercises).FirstOrDefaultAsync(c => c.Id == trainingId);
            
        if (training == null) return false;
        if (training.UserId != userId) return false;

        foreach (var exercise in training.Exercises)
        {
            training.Exercises!.Remove(exercise);
        }
        
        context.Trainings.Update(training);
        await context.SaveChangesAsync();
        
        context.Trainings.Remove(training);
        await context.SaveChangesAsync();
        
        return true;
    }
    public async Task<Training?> GetTraining(Guid trainingId, Guid userId)
    {
        await using var context = new FitEntitiesContext();
        var training = await context.Trainings
            .Include(t => t.Exercises)
            .FirstOrDefaultAsync(e => e.Id == trainingId);
        return training!.UserId != userId ? null : training;
    }
    public async Task<List<Training>> GetTrainings(Guid userId)
    {
        await using var context = new FitEntitiesContext();
    
        var trainings = await context.Trainings
            .Where(e => e.UserId == userId)
            .Include(t => t.Exercises)
            .ToListAsync();
    
        return trainings;
    }
    
    //Подходы
    public async Task<Set?> CompleteSet(Guid userId, Guid setId, int reps, double weight)
    {
        await using var context = new FitEntitiesContext();
    
        var set = await context.Sets
            .Where(e => e.Id == setId)
            .Include(s => s.ExerciseProgress)
            .FirstOrDefaultAsync();
        
        set!.Reps = reps;
        set!.Weight = weight;
        set!.isCompleted = true;
        return set;
    }
}