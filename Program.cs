using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

interface IEmployee
{
    void DoWork();
}

class Programmer : IEmployee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Activity { get; set; }
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }

    public void DoWork()
    {
        Console.WriteLine($"{FirstName} {LastName} is working on {Activity}.");
    }
}

class ProjectTeam
{
    public string TeamName { get; set; }
    public List<Programmer> Programmers { get; set; }
    public bool FullSalary { get; set; }

    public ProjectTeam(string teamName, bool fullSalary)
    {
        TeamName = teamName;
        FullSalary = fullSalary;
        Programmers = new List<Programmer>();
    }

    public void AddProgrammer(Programmer programmer)
    {
        Programmers.Add(programmer);
    }

    public void PrintReport()
    {
        Console.WriteLine($"{TeamName}:");
        foreach (var programmer in Programmers)
        {
            Console.WriteLine($"- {programmer.LastName}, {programmer.FirstName}, in charge of {programmer.Activity} from {programmer.StartDate:d} to {programmer.StartDate.AddDays(programmer.Duration):d} (duration: {programmer.Duration}), this month: {programmer.Duration} days (total cost = {GetTotalCost(programmer)}$).");
        }
    }

    private decimal GetTotalCost(Programmer programmer)
    {
        decimal dailyRate = FullSalary ? 10.0m : 5.0m;
        return dailyRate * programmer.Duration;
    }
}

class ITCompany
{
    public List<ProjectTeam> ProjectTeams { get; set; }

    public ITCompany()
    {
        ProjectTeams = new List<ProjectTeam>();
    }

    public void LoadData(string fileName)
    {
        try
        {
            string jsonData = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<List<ProjectTeam>>(jsonData);
            ProjectTeams = data ?? new List<ProjectTeam>();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading data: " + e.Message);
        }
    }

    public void UpdateData()
    {
        foreach (var team in ProjectTeams)
        {
            foreach (var programmer in team.Programmers)
            {
                programmer.Duration++;
            }
        }
    }

    public void SaveData(string fileName)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(ProjectTeams, Formatting.Indented);
            File.WriteAllText(fileName, jsonData);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error saving data: " + e.Message);
        }
    }

    public void GenerateReport()
    {
        Console.WriteLine("IT COMPANY - Report:");
        Console.WriteLine($"IT Company is actually composed of {ProjectTeams.Count} Project Teams and {GetTotalProgrammers()} Programmers");

        int totalDaysConsumed = GetTotalDaysConsumed();
        int totalDaysInCharge = GetTotalDaysInCharge();
        Console.WriteLine($"This month {totalDaysConsumed} days have been consumed by {GetTotalProgrammers()} programmers and {totalDaysInCharge} days still in charge.");

        Console.WriteLine("PROJECT TEAMS DETAILS:");
        foreach (var team in ProjectTeams)
        {
            team.PrintReport();
        }
    }

    private int GetTotalProgrammers()
    {
        int total = 0;
        foreach (var team in ProjectTeams)
        {
            total += team.Programmers.Count;
        }
        return total;
    }
    private int GetTotalDaysConsumed()
    {
        int total = 0;
        foreach (var team in ProjectTeams)
        {
            foreach (var programmer in team.Programmers)
            {
                total += programmer.Duration;
            }
        }
        return total;
    }

    private int GetTotalDaysInCharge()
    {
        int total = 0;
        foreach (var team in ProjectTeams)
        {
            foreach (var programmer in team.Programmers)
            {
                total += programmer.Duration;
            }
        }
        return total;
    }
}

class Program
{
    static void Main(string[] args)
    {
        ITCompany company = new ITCompany();
        
        company.LoadData("data.json");

        
        company.UpdateData();

        
        company.SaveData("data.json");

        
        company.GenerateReport();
    }
}