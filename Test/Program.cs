using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var vacationDictionary = new Dictionary<string, List<DateTime>>()
            {
                ["Evans Eva Evanshevna"] = new List<DateTime>(),
                ["Petrov Petr Petrovich"] = new List<DateTime>(),
                ["Ivanova Julia Ivanovna"] = new List<DateTime>(),
                ["Sidorov Sidor Sidorovich"] = new List<DateTime>(),
                ["Ivanov Ivan Ivanovich"] = new List<DateTime>(),
                ["Gagarin Yuri Gagarinovich"] = new List<DateTime>()
            };

            var availableWorkingDaysOfWeekWithoutWeekends = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            List<DateTime> vacations = new List<DateTime>();
            var allVacationCount = 0;

            foreach (var vacationList in vacationDictionary)
            {
                var dateList = vacationList.Value;
                Random gen = new Random();
                Random step = new Random();
                DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime end = new DateTime(DateTime.Today.Year, 12, 31);

                int vacationCount = 28;
                while (vacationCount > 0)
                {
                    int range = (end - start).Days;
                    var startDate = start.AddDays(gen.Next(range));

                    if (availableWorkingDaysOfWeekWithoutWeekends.Contains(startDate.DayOfWeek.ToString()))
                    {
                        string[] vacationSteps = { "7", "14" };
                        int vacIndex = gen.Next(vacationSteps.Length);
                        var endDate = new DateTime(DateTime.Now.Year, 12, 31);
                        float difference = 0;
                        if (vacationSteps[vacIndex] == "7")
                        {
                            endDate = startDate.AddDays(7);
                            difference = 7;
                        }
                        if (vacationSteps[vacIndex] == "14")
                        {
                            endDate = startDate.AddDays(14);
                            difference = 14;
                        }

                        if (vacationCount <= 7)
                        {
                            endDate = startDate.AddDays(7);
                            difference = 7;
                        }

                        bool canCreateVacation = false;
                        bool existStart = dateList.Any(element => element.AddMonths(1) >= startDate && element.AddMonths(1) <= endDate);
                        bool existEnd = dateList.Any(element => element.AddMonths(-1) <= startDate && element.AddMonths(-1) <= endDate);
                        if (!vacations.Any(element => element >= startDate && element <= endDate) &&
                            !vacations.Any(element => element.AddDays(3) >= startDate && element.AddDays(3) <= endDate) &&
                            (!existStart || !existEnd))
                        {
                            canCreateVacation = true;
                        }

                        if (canCreateVacation)
                        {
                            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                            {
                                vacations.Add(dt);
                                dateList.Add(dt);
                            }
                            allVacationCount++;
                            vacationCount -= (int)difference;
                        }
                    }
                }
            }

            foreach (var vacationList in vacationDictionary)
            {
                var setDateList = vacationList.Value;
                Console.WriteLine("Отпуск для " + vacationList.Key + " : ");
                foreach (var date in setDateList)
                {
                    Console.WriteLine(date);
                }
            }
        }
    }
}