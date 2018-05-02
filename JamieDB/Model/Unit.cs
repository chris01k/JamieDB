using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamieDB.Model
{

    public partial class Unit : INotifyPropertyChanging, INotifyPropertyChanged
    {

        public Unit Clone()
        {
            Unit ClonedUnit = new Unit();

            ClonedUnit.Id = this.Id;
            ClonedUnit.Symbol = this.Symbol;
            ClonedUnit.Name = this.Name;
            ClonedUnit.TypeID = this.TypeID;
            ClonedUnit.TypeStandard = this.TypeStandard;
            ClonedUnit.TypeFactor = this.TypeFactor;
            ClonedUnit.TypeUniversal = this.TypeUniversal;
            //ClonedUnit.UnitType = this.UnitType;

            return ClonedUnit;
        }

        partial void OnTypeStandardChanging(bool value)
        {
//            if (this.TypeStandard != true) TypeFactor = 1;
//            else this.TypeFactor = 10;
        }

        public override string ToString()
        {
            return this.Name + ": " + this.Symbol;
        }

        

    }
}


