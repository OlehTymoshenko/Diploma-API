using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configurations.Sections
{
    public class MorpherSection
    {
        public Guid? Token { get; set; }

        public string MorpherWebApiUrl { get; set; }
    }
}
