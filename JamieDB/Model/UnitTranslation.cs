using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamieDB.Model
{
    partial class UnitTranslation

    {   

        public UnitTranslation(Unit u, Unit v)
        {
            this.Unit = u;
            this.Unit1 = v;
            this.Factor = 0;
            this.IsTypeChange = (u.UnitType != v.UnitType);
            this.IsIngredientDependent = false;
            this.IsOK = false;
            this.IsAutomaticCreated = true;
        }
            
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += "ID->"+this.Id + ": ";
            ReturnString += this.BaseUnitID + " = ";
            ReturnString += this.Factor + " * ";
            ReturnString += this.TargetUnitID + " ";
            if (!this.IsTypeChange) ReturnString += "NO ";
            ReturnString += "TypeChange";
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
            ReturnObject.Factor = 1.0 / Factor;
            ReturnObject.AffectedIngredientID = AffectedIngredientID;
            ReturnObject.Ingredient = Ingredient;
            ReturnObject.IsAutomaticCreated = IsAutomaticCreated;
            ReturnObject.IsIngredientDependent = IsIngredientDependent;
            ReturnObject.IsOK = IsOK;
            ReturnObject.IsTypeChange = IsTypeChange;


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
        public static void AddBasicUnitTranslations()
        {
             
            JamieDBLinqDataContext dc = new JamieDBLinqDataContext();

            var UnitTypes = dc.UnitTypes;
            List<UnitTranslation> ExistingUnitTranslations = new List<UnitTranslation>();
            ExistingUnitTranslations = dc.UnitTranslations.ToList();

            List<UnitTranslation> GeneratedUnitTranslations = new List<UnitTranslation>();

            foreach (UnitType u in UnitTypes)
            {
                foreach(UnitType v in UnitTypes)
                {
                    if (u.Unit != v.Unit)
                    {
                        if (GeneratedUnitTranslations.Where(ut=>((ut.Unit==u.Unit)&&(ut.Unit1==v.Unit))||((ut.Unit == v.Unit) && (ut.Unit1 == u.Unit))).Count()==0)
                        {
                            GeneratedUnitTranslations.Add(new UnitTranslation(u.Unit, v.Unit));
                        }
                    }
                }

                foreach(Unit w in u.Units)
                {
                    if (w != u.Unit)
                    { 
                        if (GeneratedUnitTranslations.Where(ut => ((ut.Unit == u.Unit) && (ut.Unit1 == w)) || ((ut.Unit == w) && (ut.Unit1 == u.Unit))).Count() == 0)
                        {
                            GeneratedUnitTranslations.Add(new UnitTranslation(w, u.Unit));
                        }
                    }

                }
            }


            var NewUnitTranslations = GeneratedUnitTranslations.Where(GeneratedUT => !ExistingUnitTranslations.Any(ExistingUT => ((GeneratedUT.Unit == ExistingUT.Unit)&& (GeneratedUT.Unit1 == ExistingUT.Unit1)) ||
                                                                                                                                 ((GeneratedUT.Unit == ExistingUT.Unit1) && (GeneratedUT.Unit1 == ExistingUT.Unit))

                                                                      ));


            if (NewUnitTranslations.Count()>0)
            {
                dc.UnitTranslations.InsertAllOnSubmit(NewUnitTranslations);
                dc.SubmitChanges();
            }

        }
        #endregion


    }
}
