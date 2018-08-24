using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamieDB.Model
{
    public enum FoodPlanDateType { Breakfast, Lunch, Dinner, Snack };

    public partial class FoodPlan : INotifyPropertyChanging, INotifyPropertyChanged
    {

        public DateTime GetNextOpenFoodPlanItemDate()
        {
            DateTime? ReturnDateTime = null;
            DateTime NextDateTime;

            foreach (FoodPlanDateType t in Enum.GetValues(typeof(FoodPlanDateType)))
            {
                NextDateTime = GetNextOpenFoodPlanItemDate(FoodPlanDateType.Breakfast);
                if ((ReturnDateTime == null) || (ReturnDateTime > NextDateTime)) ReturnDateTime = NextDateTime;
            }
            return (DateTime) ReturnDateTime;
        }
        public DateTime GetNextOpenFoodPlanItemDate(FoodPlanDateType FPDT)
        {
            const int MaxSnackIndexCounter = 2;

            DateTime? ReturnDateTime = null;
            DateTime DateTimeLoopCounter;
            int IndexCounter = 0;
            TimeSpan[] TimeOffset = new TimeSpan[MaxSnackIndexCounter + 1];
            
            switch (FPDT)
            {
                case FoodPlanDateType.Breakfast:
                    TimeOffset[0] = new TimeSpan(8, 0, 0);
                    break;
                case FoodPlanDateType.Lunch:
                    TimeOffset[0] = new TimeSpan(11, 30, 0);
                    break;
                case FoodPlanDateType.Dinner:
                    TimeOffset[0] = new TimeSpan(19, 00, 0);
                    break;
                case FoodPlanDateType.Snack:
                    TimeOffset[0] = new TimeSpan(10, 00, 0);
                    TimeOffset[1] = new TimeSpan(13, 00, 0);
                    TimeOffset[2] = new TimeSpan(15, 30, 0);
                    break;
                default:
                    TimeOffset[0] = new TimeSpan(0, 0, 0); // Sollte nicht passieren.
                    break;
            }

            var FirstFoodPlanListItemDate = this.FoodPlanItems.OrderBy(f => f.DateTime).FirstOrDefault();

            if (FirstFoodPlanListItemDate == null)
            {
                ReturnDateTime = StartDate + TimeOffset[IndexCounter];
            }
            else
            {
                DateTimeLoopCounter = FirstFoodPlanListItemDate.DateTime.Date;
                if (StartDate < DateTimeLoopCounter) DateTimeLoopCounter = StartDate;

                do
                {
                    if (this.FoodPlanItems.Where(f=>f.DateTime == DateTimeLoopCounter+TimeOffset[IndexCounter]).Count()==0)
                    {
                        ReturnDateTime = DateTimeLoopCounter + TimeOffset[IndexCounter];
                    }
                    else
                    {
                        if (FPDT == FoodPlanDateType.Snack)
                        {
                            if (IndexCounter >= MaxSnackIndexCounter)
                            {
                                IndexCounter = 0;
                                DateTimeLoopCounter = DateTimeLoopCounter.AddDays(1);
                            }
                            else IndexCounter++;
                        }
                        else DateTimeLoopCounter = DateTimeLoopCounter.AddDays(1);
                    }

                } while (ReturnDateTime == null);
            }
            return (DateTime) ReturnDateTime;
        }


    }
}
