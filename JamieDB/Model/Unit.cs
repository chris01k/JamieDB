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
        public bool IsStandardUnit
        {
        
            get
            {
                return (this == this.UnitType.Unit);
            }
            set
            {
                this.UnitType.Unit = this;
            }
        
        }

        public string DisplayName
        {
            get
            {
                return (this.Symbol + ": " +this.Name);
            }
        
        }
     }



}

