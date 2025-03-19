﻿namespace BakingSisters.Api.Models;

public class Customer : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public virtual ICollection<Order> Orders { get; set; }
}