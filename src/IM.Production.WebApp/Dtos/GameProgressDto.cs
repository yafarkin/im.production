using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.Production.WebApp.Dtos
{
    public class GameProgressDto
    {
        //Sum
        public decimal MoneyBalance { get; set; }
        //the same
        public decimal RDProgress { get; set; }
        public int FactoryGenerationLevel { get; set; }
    }
}
