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

        public UnitTranslation Inverse()
        {
            UnitTranslation ReturnObject = new UnitTranslation();

            ReturnObject.Id = -Id;
            ReturnObject.BaseUnitID = TargetUnitID;
            ReturnObject.Unit = Unit1;
            ReturnObject.TargetUnitID = BaseUnitID;
            ReturnObject.Unit1 = Unit;
            ReturnObject.Factor = ( Factor !=0 ? 1.0 / Factor : 0);
            ReturnObject.AffectedIngredientID = AffectedIngredientID;
            ReturnObject.Ingredient = Ingredient;
            return ReturnObject;
        }

        public bool UnitTranslationExistsInDB(JamieDBLinqDataContext dcontext)
        {
            bool ReturnValue;

            

            List<UnitTranslation>UTList = new List<UnitTranslation>();

            UTList = dcontext.UnitTranslations.Where(ut => ((this.Unit == ut.Unit) && (this.Unit1 == ut.Unit1)) || ((this.Unit == ut.Unit1) && (this.Unit1 == ut.Unit))).ToList();

            ReturnValue = (UTList.Count() > 0);

            var x = dcontext.UnitTranslations.Count();

            return ReturnValue;
        }


        #region Queries
        #endregion


    }
}
