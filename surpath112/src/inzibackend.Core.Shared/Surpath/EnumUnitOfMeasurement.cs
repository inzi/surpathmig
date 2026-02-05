using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace inzibackend.Surpath
{
    public enum EnumUnitOfMeasurement
    {
        [Display(Name = "ng/ml")]
        ngml,
        [Display(Name = "pg/mg")]
        pgmg,
    }
}
