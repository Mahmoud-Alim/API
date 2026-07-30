﻿using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    void Update(User user);
    void Remove(User user);

    Task<UserSalary?> GetSingleUserSalaryAsync(int id, CancellationToken cancellationToken = default);

    Task<UserJobInfo?> GetSingleUserJobInfoAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<User>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<User> Items, int TotalCount)> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool descending,
        UserFilter? filter = null,
        CancellationToken cancellationToken = default);
}
