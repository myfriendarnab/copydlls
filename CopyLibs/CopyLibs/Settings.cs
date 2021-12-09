using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLibs
{
    public class Settings
    {
        public string basePath { get; set; }
        public string debugPath { get; set; }
        public string frameworkPath { get; set; }
        public string svcType { get; set; }
        public List<string> exclude { get; set; }
        public List<string> fwprojs { get; set; }
    }
}
