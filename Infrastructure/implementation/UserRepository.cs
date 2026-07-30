using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

namespace Infrastructure.implementation;

public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<User> Items, int TotalCount)> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool descending,
        UserFilter? filter = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = _context.Users.AsNoTracking();

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.ToLower().Trim();
                query = query.Where(x =>
                    x.FirstName.ToLower().Contains(term) ||
                    x.LastName.ToLower().Contains(term) ||
                    x.Email.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(filter.Gender))
            {
                var gender = filter.Gender.Trim();
                query = query.Where(x => x.Gender.ToLower() == gender.ToLower());
            }

            if (filter.Active.HasValue)
            {
                query = query.Where(x => x.Active == filter.Active.Value);
            }
        }

        query = sortBy?.ToLower() switch
        {
            "userid" => descending ? query.OrderByDescending(x => x.UserId) : query.OrderBy(x => x.UserId),
            "firstname" => descending ? query.OrderByDescending(x => x.FirstName) : query.OrderBy(x => x.FirstName),
            "lastname" => descending ? query.OrderByDescending(x => x.LastName) : query.OrderBy(x => x.LastName),
            "email" => descending ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email),
            "gender" => descending ? query.OrderByDescending(x => x.Gender) : query.OrderBy(x => x.Gender),
            "active" => descending ? query.OrderByDescending(x => x.Active) : query.OrderBy(x => x.Active),
            _ => descending ? query.OrderByDescending(x => x.UserId) : query.OrderBy(x => x.UserId)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items.AsReadOnly(), totalCount);
    }
    public async Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
    }

    public async Task<UserSalary?> GetSingleUserSalaryAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserSalaries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
    }

    public async Task<UserJobInfo?> GetSingleUserJobInfoAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserJobInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(x => x.Active)
            .OrderBy(x => x.FirstName)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(x => x.UserId == id, cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Remove(User user)
    {
        _context.Users.Remove(user);
    }
}
