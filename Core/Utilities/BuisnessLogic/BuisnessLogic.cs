using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.BuisnessLogic
{
    public class BuisnessLogic
    {
        public static IBuisnessLogicResult Run(params IBuisnessLogicResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic;
                }
            }
            return null;
        }
    }
}
