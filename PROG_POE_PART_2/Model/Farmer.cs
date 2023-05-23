using System;
using System.Collections.Generic;

namespace PROG_POE_PART_2.Model;

public partial class Farmer
{
    public int FarmerId { get; set; }

    public string FarmerName { get; set; } = null!;

    public string FarmerSurname { get; set; } = null!;

    public string FarmName { get; set; } = null!;

    public string FarmerUsername { get; set; } = null!;

    public string FarmerPassword { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
