using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserCleanupService : IUserCleanupService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public UserCleanupService(
        UserManager<AppUser> userManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task DeleteExpiredUnconfirmedUsersAsync()
    {
        var users = await _userManager.Users
            .Include(u => u.Patient)
            .ThenInclude(p => p.Person)
            .Where(u =>
                !u.EmailConfirmed &&
                u.CreatedAt <= DateTime.Now.AddDays(-14))
            .ToListAsync();

        foreach (var user in users)
        {
            var patient = user.Patient;
            var person = patient?.Person;

            // Delete Identity User
            await _userManager.DeleteAsync(user);

            // Delete Patient
            if (patient != null)
                _context.Patients.Remove(patient);

            // Delete Person
            if (person != null)
                _context.Persons.Remove(person);

            Console.WriteLine(
        $"Deleted unconfirmed user: {user.Email}");
        }

        await _context.SaveChangesAsync();
    }
}