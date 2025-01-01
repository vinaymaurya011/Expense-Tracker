using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Charts;

namespace Expense_Tracker.Controllers
{
    public class DashboardController(ApplicationDbContext _context) : Controller
    {
        public async Task<ActionResult> Index()
        {
            //Last 7 days
            DateTime startDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;

            List<Transaction> selectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= startDate && y.Date <= endDate)
                .ToListAsync();

            int totalIncome = selectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = totalIncome.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            
            int totalExpense = selectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = totalExpense.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            int balance = totalIncome - totalExpense;
            ViewBag.Balance = balance.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("en-US"));

            //Doughnut chart - Expense by category

            ViewBag.DoughnutChartData = selectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("en-US")),

                })
                .OrderByDescending(l => l.amount)
                .ToList();

            //Spline chart - Income vs Expense

            List<SplineChartData> incomeSummery = selectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData
                {
                    Day = k.First().Date.ToString("dd-MMM"),
                    Income = k.Sum(l => l.Amount)
                }).ToList();
            
            List<SplineChartData> ExpenseSummery = selectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData
                {
                    Day = k.First().Date.ToString("dd-MMM"),
                    Expense = k.Sum(l => l.Amount)
                }).ToList();

            //Combine income and expense

            string[] last7Days = Enumerable.Range(0, 7)
                .Select(i => startDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in last7Days
                                      join income in incomeSummery on day equals income.Day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummery on day equals expense.Day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.Income,
                                          expense = expense == null ? 0 : expense.Expense,
                                      };

            //Recent Transactions

            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }

    public class SplineChartData
    {
        public string Day;
        public int Income;
        public int Expense;
    }
}
