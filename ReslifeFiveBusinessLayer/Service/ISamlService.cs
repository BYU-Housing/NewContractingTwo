using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBusinessLayer.Service
{
    public interface ISamlService
    {

        string GenerateSAMLLoginUrl();
    }
}
