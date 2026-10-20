using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AksesKita.Desktop.Data;
using AksesKita.Desktop.Models;

namespace AksesKita.Desktop.Services;

/// <summary>
/// CRUD over the SQLite database. A fresh DbContext is created per operation so the
/// short-lived units of work never clash — the recommended pattern for EF Core.
/// </summary>
public class ReportService : IReportService
{
    public async Task<List<Report>> GetAllReportsAsync()
    {
        using var db = new AksesKitaDbContext();
        return await db.Reports.Include(r => r.Category)
                               .OrderByDescending(r => r.CreatedAt)
                               .ToListAsync();
    }

    public async Task<Report?> GetReportByIdAsync(int id)
    {
        using var db = new AksesKitaDbContext();
        return await db.Reports.Include(r => r.Category)
                               .Include(r => r.StatusLogs)
                               .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Report> AddReportAsync(Report report)
    {
        using var db = new AksesKitaDbContext();
        report.CreatedAt = DateTime.Now;
        report.Status = ReportStatus.Baru;
        db.Reports.Add(report);
        await db.SaveChangesAsync();

        // Ticket code depends on the generated id, so set it after the first save.
        report.TicketCode = $"#AKS-{report.CreatedAt:yyyy}-{report.Id:D4}";
        await db.SaveChangesAsync();
        return report;
    }

    public async Task DeleteReportAsync(int id)
    {
        using var db = new AksesKitaDbContext();
        var report = await db.Reports.FirstOrDefaultAsync(r => r.Id == id);
        if (report != null)
        {
            db.Reports.Remove(report);
            await db.SaveChangesAsync();
        }
    }

    public async Task UpdateReportStatusAsync(int reportId, ReportStatus newStatus, string note, string updatedBy)
    {
        using var db = new AksesKitaDbContext();
        var report = await db.Reports.FirstOrDefaultAsync(r => r.Id == reportId);
        if (report == null) return;

        var oldStatus = report.Status;
        report.Status = newStatus;
        db.ReportStatusLogs.Add(new ReportStatusLog
        {
            ReportId = reportId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            Note = note,
            UpdatedBy = updatedBy,
            CreatedAt = DateTime.Now
        });
        await db.SaveChangesAsync();
    }

    public async Task AddSupportAsync(int reportId)
    {
        using var db = new AksesKitaDbContext();
        var report = await db.Reports.FirstOrDefaultAsync(r => r.Id == reportId);
        if (report != null)
        {
            report.SupportCount++;
            await db.SaveChangesAsync();
        }
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        using var db = new AksesKitaDbContext();
        return await db.Categories.ToListAsync();
    }

    public async Task<List<ReportStatusLog>> GetRecentLogsAsync(int take)
    {
        using var db = new AksesKitaDbContext();
        return await db.ReportStatusLogs.OrderByDescending(l => l.CreatedAt)
                                        .Take(take)
                                        .ToListAsync();
    }
}
