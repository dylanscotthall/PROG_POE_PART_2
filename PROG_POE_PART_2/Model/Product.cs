using System;
using System.Collections.Generic;

namespace PROG_POE_PART_2.Model;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public double ProductPrice { get; set; }

    public int ProductStock { get; set; }

    public int? FarmerId { get; set; }

    public string ProductType { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public virtual Farmer? Farmer { get; set; }
}
