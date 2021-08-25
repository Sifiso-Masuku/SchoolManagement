using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace SchoolManagement.Model.Enum
{
    public enum Province
    {
        [Description("Kwazulu Natal")] KwazuluNatal = 1,
        Gauteng = 2,
        [Description("Western Cape")] WesternCape = 3,
        [Description("Eastern Cape")] EasternCape = 4,
        [Description("Northen Cape")] NorthenCape = 5,
        [Description("North West")] NorthWest = 6,
        [Description("Free State")] FreeState = 7,
        [Description("Limpopo")] Limpopo = 8,
        [Description("Mpumalanga")] Mpumalanga = 9

    }
}