﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Composite;

public class EmployeeInfoResult
{
    [Column("db_user_id")] public int DbUserId { get; set; }

    [Column("first_name")] public string FirstName { get; set; } = null!;

    [Column("second_name")] public string SecondName { get; set; } = null!;

    [Column("telephone_number")] public string TelephoneNumber { get; set; } = null!;

    [Column("login")] public string Login { get; set; } = null!;

    [Column("password_hash")] public string PasswordHash { get; set; } = null!;

    [Column("role")] public string Role { get; set; } = null!;

    [Column("employee_id")] public int EmployeeId { get; set; }
}