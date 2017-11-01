using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamieDB.Model
{
    partial class UnitTranslation

    {   
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += "ID->"+this.Id + ": ";
            ReturnString += this.BaseUnitID + " = ";
            ReturnString += this.Factor + " * ";
            ReturnString += this.TargetUnitID + " ";
            return ReturnString;

        }

    }
}
