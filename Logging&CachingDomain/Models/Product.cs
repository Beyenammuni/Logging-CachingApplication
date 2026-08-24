using System;
using System.Collections.Generic;
using System.Text;

namespace Logging_CachingDomain.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? Quentity { get; set; }
}
