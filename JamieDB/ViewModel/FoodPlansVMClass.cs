using JamieDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JamieDB.ViewModel
{
    class FoodPlansVMClass : JamieDBViewModelBase
    {

        #region Attributes
        private DateTime _CalculatedFoodPlanEndDate;

        private FoodPlan _SelectedFoodPlan;
        private FoodPlanItem _SelectedFoodPlanItem;
        private ObservableCollection<FoodPlanItem> _SelectedFoodPlanItemsList;

        
        private double? _DefaultPortions;
        private ObservableCollection<FoodPlan> _FoodPlanList;
        private ObservableCollection<Recipe> _RecipeList;
        private string _RecipeListFilter;
        private ObservableCollection<Recipe> _RecipeListFiltered;
        private DateTime _SelectedDateTime;
        private FoodPlan _SelectedFoodPlanFromList;
        private Recipe _SelectedRecipeFromList;
        private User _SelectedUser;

        private ObservableCollection<User> _UserList;



        #region Attributes: Commands
        private JamieDBViewModelCommand _DeleteFoodPlanCommand;
        private JamieDBViewModelCommand _InsertFoodPlanCommand;
        private JamieDBViewModelCommand _LoadFoodPlanCommand;
        private JamieDBViewModelCommand _NewFoodPlanCommand;
        private JamieDBViewModelCommand _SaveFoodPlanCommand;

        private JamieDBViewModelCommand _AddBreakfastCommand;
        private JamieDBViewModelCommand _AddDinnerCommand;
        private JamieDBViewModelCommand _AddLunchCommand;
        private JamieDBViewModelCommand _AddSelected;
        private JamieDBViewModelCommand _AddSnackCommand;

        private JamieDBViewModelCommand _DeleteSelectedCommand;
        #endregion
        #endregion

        #region Constructors
        public FoodPlansVMClass()
        {
            DeleteFoodPlanCommand = new JamieDBViewModelCommand(CanExecuteDeleteFoodPlan, ExecuteDeleteFoodPlan);
            InsertFoodPlanCommand = new JamieDBViewModelCommand(CanExecuteInsertFoodPlan, ExecuteInsertFoodPlan);
            LoadFoodPlanCommand = new JamieDBViewModelCommand(CanExecuteLoadFoodPlan, ExecuteLoadFoodPlan);
            NewFoodPlanCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewFoodPlan);
            SaveFoodPlanCommand = new JamieDBViewModelCommand(CanExecuteSaveFoodPlan, ExecuteSaveFoodPlan);

            AddBreakfastCommand = new JamieDBViewModelCommand(CanExecuteAddBreakfast, ExecuteAddBreakfast);
            AddDinnerCommand = new JamieDBViewModelCommand(CanExecuteAddDinner, ExecuteAddDinner);
            AddLunchCommand = new JamieDBViewModelCommand(CanExecuteAddLunch, ExecuteAddLunch);
            AddSnackCommand = new JamieDBViewModelCommand(CanExecuteAddSnack, ExecuteAddSnack);

            DeleteSelectedCommand = new JamieDBViewModelCommand(CanExecuteDeleteSelected, ExecuteDeleteSelected);

            DefaultPortions = 2;
            RefreshRecipeList();
            RefreshRecipeListFiltered();
            RefreshFoodPlanList();
            RefreshUserList();
        }

        #endregion

        #region Events
        #region Events:EventHandler
        #endregion
        #endregion

        #region Properties
        public DateTime CalculatedFoodPlanEndDate
        {
            get
            {
                return _CalculatedFoodPlanEndDate;
            }
            set
            {
                if (_CalculatedFoodPlanEndDate != value)
                {
                    _CalculatedFoodPlanEndDate = value;
                    OnPropertyChanged("CalculatedFoodPlanEndDate");
                }
            }

        }
        public double? DefaultPortions
        {
            get
            {
                return _DefaultPortions;
            }
            set
            {
                if (_DefaultPortions != value)
                {
                    _DefaultPortions = value;
                    OnPropertyChanged("DefaultPortions");
                }
            }
        }
        public ObservableCollection<FoodPlan> FoodPlanList
        {
            get
            {
                return _FoodPlanList;
            }
            set
            {
                if (_FoodPlanList != value)
                {
                    _FoodPlanList = value;
                    OnPropertyChanged("FoodPlanList");
                }
            }
        }
        public ObservableCollection<Recipe> RecipeList
        {
            get
            {
                return _RecipeList;
            }
            set
            {
                if (_RecipeList != value)
                {
                    _RecipeList = value;
                    OnPropertyChanged("RecipeList");
                }
            }
        }
        public string RecipeListFilter
        {
            get
            {
                return _RecipeListFilter;
            }
            set
            {
                if (_RecipeListFilter != value)
                {
                    _RecipeListFilter = value;
                    RefreshRecipeListFiltered();
                    OnPropertyChanged("RecipeListFilter");
                }
            }
        }
        public ObservableCollection<Recipe> RecipeListFiltered
        {
            get
            {
                return _RecipeListFiltered;
            }
            set
            {
                if (_RecipeListFiltered != value)
                {
                    _RecipeListFiltered = value;
                    OnPropertyChanged("RecipeListFiltered");
                }
            }
        }
        public DateTime SelectedDateTime
        {
            get
            {
                return _SelectedDateTime;
            }
            set
            {
                if (_SelectedDateTime != value)
                {
                    _SelectedDateTime = value;
                    OnPropertyChanged("SelectedDateTime");
//                    OnPropertyChanged("SelectedDate");
//                    OnPropertyChanged("SelectedTime");
                }
            }
        }
/*        public DateTime SelectedDate
        {
            get
            {
                return _SelectedDateTime.Date;
            }
            set
            {
                if (_SelectedDateTime.Date != value)
                {
                    SelectedDateTime = value + SelectedDateTime.TimeOfDay;
                    OnPropertyChanged("SelectedDate");
                }
            }
        }
        public TimeSpan SelectedTime
        {
            get
            {
                return _SelectedDateTime.TimeOfDay;
            }
            set
            {
                if (_SelectedDateTime.TimeOfDay != value)
                {
                    SelectedDateTime = SelectedDateTime.Date + value;
                    OnPropertyChanged("SelectedTime");
                }
            }
        }
 */
        public FoodPlan SelectedFoodPlan
        {
            get
            {
                return _SelectedFoodPlan;
            }
            set
            {
                if (_SelectedFoodPlan != value)
                {
                    _SelectedFoodPlan = value;
                    OnPropertyChanged("SelectedFoodPlan");
                    DeleteFoodPlanCommand.OnCanExecuteChanged();
                    InsertFoodPlanCommand.OnCanExecuteChanged();
                    SaveFoodPlanCommand.OnCanExecuteChanged();

                    AddBreakfastCommand.OnCanExecuteChanged();
                    AddDinnerCommand.OnCanExecuteChanged();
                    AddLunchCommand.OnCanExecuteChanged();
                    AddSnackCommand.OnCanExecuteChanged();
                }
            }
        }
        public FoodPlanItem SelectedFoodPlanItem
        {
            get
            {
                return _SelectedFoodPlanItem;
            }
            set
            {
                if (SelectedFoodPlanItem != value)
                {
                    _SelectedFoodPlanItem = value;
                    SelectedDateTime = SelectedFoodPlanItem.DateTime;
                    OnPropertyChanged("SelectedFoodPlanItem");
                    OnPropertyChanged("SelectedFoodPlan");
                    DeleteSelectedCommand.OnCanExecuteChanged();
                }
            }
        }
        public ObservableCollection<FoodPlanItem> SelectedFoodPlanItemsList
        {
            get
            {
                return _SelectedFoodPlanItemsList;
            }
            set
            {
                if (_SelectedFoodPlanItemsList != value)
                {
                    _SelectedFoodPlanItemsList = value;
                    OnPropertyChanged("SelectedFoodPlanItemsList");
                }
            }
        }
        public FoodPlan SelectedFoodPlanFromList
        {
            get
            {
                return _SelectedFoodPlanFromList;
            }
            set
            {
                if (_SelectedFoodPlanFromList != value)
                {
                    _SelectedFoodPlanFromList = value;
                    OnPropertyChanged("SelectedFoodPlanFromList");
                    InsertFoodPlanCommand.OnCanExecuteChanged();
                }
            }
        }
        public Recipe SelectedRecipeFromList
        {
            get
            {
                return _SelectedRecipeFromList;
            }
            set
            {
                if (_SelectedRecipeFromList != value)
                {
                    _SelectedRecipeFromList = value;
                    OnPropertyChanged("SelectedRecipeFromList");

                    AddBreakfastCommand.OnCanExecuteChanged();
                    AddDinnerCommand.OnCanExecuteChanged();
                    AddLunchCommand.OnCanExecuteChanged();
                    AddSnackCommand.OnCanExecuteChanged();
                }
            }
        }
        public User SelectedUser
        {
            get
            {
                return _SelectedUser;
            }
            set
            {
                if (_SelectedUser != value)
                {
                    _SelectedUser = value;
                    OnPropertyChanged("SelectedUser");
                }
            }
        }
        public ObservableCollection<User> UserList
        {
            get
            {
                return _UserList;
            }
            set
            {
                if (_UserList != value)
                {
                    _UserList = value;
                    OnPropertyChanged("UserList");
                }
            }
        }
        #endregion

        #region Properties: Commands
        public JamieDBViewModelCommand DeleteFoodPlanCommand
        {
            get
            {
                return _DeleteFoodPlanCommand;
            }
            set
            {
                _DeleteFoodPlanCommand = value;
                OnPropertyChanged("DeleteFoodPlanCommand");
            }

        }
        public JamieDBViewModelCommand InsertFoodPlanCommand
        {
            get
            {
                return _InsertFoodPlanCommand;
            }
            set
            {
                _InsertFoodPlanCommand = value;
            }

        }
        public JamieDBViewModelCommand LoadFoodPlanCommand
        {
            get
            {
                return _LoadFoodPlanCommand;
            }
            set
            {
                _LoadFoodPlanCommand = value;
            }

        }
        public JamieDBViewModelCommand NewFoodPlanCommand
        {
            get
            {
                return _NewFoodPlanCommand;
            }
            set
            {
                _NewFoodPlanCommand = value;
            }

        }
        public JamieDBViewModelCommand SaveFoodPlanCommand
        {
            get
            {
                return _SaveFoodPlanCommand;
            }
            set
            {
                _SaveFoodPlanCommand = value;
            }

        }

        public JamieDBViewModelCommand AddBreakfastCommand
        {
            get
            {
                return _AddBreakfastCommand;
            }
            set
            {
                _AddBreakfastCommand = value;
            }

        }
        public JamieDBViewModelCommand AddDinnerCommand
        {
            get
            {
                return _AddDinnerCommand;
            }
            set
            {
                _AddDinnerCommand = value;
            }

        }
        public JamieDBViewModelCommand AddLunchCommand
        {
            get
            {
                return _AddLunchCommand;
            }
            set
            {
                _AddLunchCommand = value;
            }

        }
        public JamieDBViewModelCommand AddSnackCommand
        {
            get
            {
                return _AddSnackCommand;
            }
            set
            {
                _AddSnackCommand = value;
            }

        }
        public JamieDBViewModelCommand DeleteSelectedCommand
        {
            get
            {
                return _DeleteSelectedCommand;
            }
            set
            {
                _DeleteSelectedCommand = value;
            }

        }


        #endregion

        #region Methods
        public double? GetDefaultPortions()
        {
            return (DefaultPortions == null?SelectedFoodPlan.DefaultPortions:DefaultPortions);
        }
        public void RefreshFoodPlanList()
        {
            var result = context.FoodPlans.OrderBy(u => u.Name);

            FoodPlanList = new ObservableCollection<FoodPlan>(result);
            //FoodPlanList.CollectionChanged += new NotifyCollectionChangedEventHandler(FoodPlanListChanged);
            SelectedFoodPlanFromList = FoodPlanList.FirstOrDefault();
        }
        public void RefreshSelectedFoodPlan()
        {
            var result = context.FoodPlans.Where(r => r.Id == SelectedFoodPlan.Id).FirstOrDefault();

            if (result != null) SelectedFoodPlan = result;
        }
        public void RefreshSelectedFoodPlanItems()
    {

        SelectedFoodPlanItemsList = null;

        if (SelectedFoodPlan != null)
        {
            var FPIWithID = context.FoodPlanItems.Where(FPItems => FPItems.FoodPlanID == SelectedFoodPlan.Id).OrderBy(o => o.DateTime);

            if (FPIWithID != null && FPIWithID.Count() > 0)
            {
                SelectedFoodPlanItemsList = new ObservableCollection<FoodPlanItem>(FPIWithID);

                SelectedFoodPlan.StartDate = SelectedFoodPlanItemsList.Min(SelectedFPI => SelectedFPI.DateTime).Date;
                CalculatedFoodPlanEndDate = SelectedFoodPlanItemsList.Max(SelectedFPI => SelectedFPI.DateTime);
                
            }

              //var NewSelectedFoodplan = context.FoodPlans.Where(fp => fp.Id == SelectedFoodPlan.Id).FirstOrDefault();

              //SelectedFoodPlan.FoodPlanItems = FPIWithID;

            
        }
        
    }
        public void RefreshRecipeList()
        {
            var result = context.Recipes;

            RecipeList = new ObservableCollection<Recipe>(result);
        }
        public void RefreshUserList()
        {
            var result = context.Users;

            UserList= new ObservableCollection<User>(result);
        }


        public void RefreshRecipeListFiltered()
        {
            if (RecipeListFilter == null || RecipeListFilter =="")
            {
                RecipeListFiltered = RecipeList;
            }
            else
            {
                RecipeListFiltered = new ObservableCollection<Recipe>(RecipeList.Where(r => r.Name.ToUpper().Contains(RecipeListFilter.ToUpper())));
            }
        }

        #region Methods: Command Methods
        public void ExecuteDeleteFoodPlan(object o)
        {
            ObservableCollection<FoodPlanItem> FoodPlanItemsToBeDeleted;
            string NameOfFoodPlanToBeDeleted = SelectedFoodPlan.Name;

            FoodPlanItemsToBeDeleted = new ObservableCollection<FoodPlanItem>(context.FoodPlanItems.Where(fpi => fpi.FoodPlan == SelectedFoodPlan));

            if (SelectedFoodPlan != null)
            {
                if (FoodPlanItemsToBeDeleted.Count > 0)
                    {
                        foreach (FoodPlanItem fpi in FoodPlanItemsToBeDeleted)
                        {
                        context.FoodPlanItems.DeleteOnSubmit(fpi);
                    }
                }
                context.FoodPlans.DeleteOnSubmit(SelectedFoodPlan);
                try
                {
                    context.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Exception Handling");
                    // Make some adjustments.
                    // ...
                    // Try again.
                    //context.SubmitChanges();
                }

                RefreshFoodPlanList();
                StatusBarMessage = string.Format("FoodPlan {0} deleted", NameOfFoodPlanToBeDeleted);

                //            SelectedFoodPlan = SelectedFoodPlanFromList;
                //            RefreshSelectedFoodPlanItems();
            }
        }
        public void ExecuteDeleteSelected(object o)
        {
            ObservableCollection<FoodPlanItem> FoodPlanItemsToBeDeleted;
            string NameOfFoodPlanToBeDeleted = SelectedFoodPlan.Name;

            FoodPlanItemsToBeDeleted = new ObservableCollection<FoodPlanItem>(context.FoodPlanItems.Where(fpi => fpi.FoodPlan == SelectedFoodPlan));

            if (SelectedFoodPlanItem != null)
            {
                context.FoodPlanItems.DeleteOnSubmit(SelectedFoodPlanItem); 

                try
                {
                    context.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Exception Handling");
                    // Make some adjustments.
                    // ...
                    // Try again.
                    //context.SubmitChanges();
                }

                //RefreshFoodPlanList(); //ggf. nur RefreshFoodPlanItemList....
                RefreshSelectedFoodPlanItems();
//                StatusBarMessage = string.Format("FoodPlanItem {0} deleted");

                //            SelectedFoodPlan = SelectedFoodPlanFromList;
                //            RefreshSelectedFoodPlanItems();
            }

        }

        public void ExecuteInsertFoodPlan(object o)
        {           
        }
        public void ExecuteLoadFoodPlan(object o)
        {
            SelectedFoodPlan = SelectedFoodPlanFromList;
            RefreshSelectedFoodPlanItems();
            StatusBarMessage = string.Format("FoodPlan {0} loaded", SelectedFoodPlan.Name);
        }
        public void ExecuteNewFoodPlan(object o)
        {
            FoodPlan NewFoodPlan = new FoodPlan();
            SelectedFoodPlan = NewFoodPlan;
            NewFoodPlan.Name = "<FoodPlan>: New " + DateTime.Now.ToString();
            NewFoodPlan.StartDate = DateTime.Today;
            context.FoodPlans.InsertOnSubmit(NewFoodPlan);

            try
            {
                context.SubmitChanges();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //context.SubmitChanges();
            }
            RefreshFoodPlanList();
            SelectedFoodPlanFromList = NewFoodPlan;
            StatusBarMessage = "FoodPlan Added";
        }
        public void ExecuteSaveFoodPlan(object o)
        {
            try
            {
                context.SubmitChanges();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //context.SubmitChanges();
            }
            StatusBarMessage = string.Format("FoodPlan {0} saved", SelectedFoodPlan.Name);
        }

        public void ExecuteAddFoodPlanItem (FoodPlanDateType FPDT)
        {
            FoodPlanItem NewFoodPlanItem = new FoodPlanItem();
            NewFoodPlanItem.DateTime = SelectedFoodPlan.GetNextOpenFoodPlanItemDate(FPDT);
            //SelectedFoodPlan.GetNextOpenFoodPlanItemDate(); .. to be implemented
            NewFoodPlanItem.FoodPlan = SelectedFoodPlan;
            NewFoodPlanItem.FoodPlanID = SelectedFoodPlan.Id;
            NewFoodPlanItem.PortionCount = DefaultPortions;
            NewFoodPlanItem.Recipe = SelectedRecipeFromList;
            NewFoodPlanItem.RecipeID = SelectedRecipeFromList.Id;
            context.FoodPlanItems.InsertOnSubmit(NewFoodPlanItem);

            try
            {
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //context.SubmitChanges();
            }
            //            context.Refresh(System.Data.Linq.RefreshMode.OverwriteSelectedValues, context.SelectedFoo);
            RefreshSelectedFoodPlan();
            RefreshSelectedFoodPlanItems();
            StatusBarMessage = string.Format("{0} Added", FPDT.ToString("g"));

        }
        public void ExecuteAddBreakfast(object o)
        {
            ExecuteAddFoodPlanItem(FoodPlanDateType.Breakfast);
        }
        public void ExecuteAddDinner(object o)
        {
            ExecuteAddFoodPlanItem(FoodPlanDateType.Dinner);
        }
        public void ExecuteAddLunch(object o)
        {
            ExecuteAddFoodPlanItem(FoodPlanDateType.Lunch);
        }
        public void ExecuteAddSnack(object o)
        {
            ExecuteAddFoodPlanItem(FoodPlanDateType.Snack);
        }



        public bool CanExecuteInsertFoodPlan(object o)
        {
            return (SelectedFoodPlan != null) && (SelectedFoodPlan != SelectedFoodPlanFromList);
        }
        public bool CanExecuteDeleteFoodPlan(object o)
        {
            return (SelectedFoodPlan != null);
        }
        public bool CanExecuteDeleteSelected(object o)
        {
            return (SelectedFoodPlanItem != null);
        }
        
        public bool CanExecuteLoadFoodPlan(object o)
        {
            return (SelectedFoodPlanFromList != null);
        }
        public bool CanExecuteSaveFoodPlan(object o)
        {
            return (SelectedFoodPlan != null);
        }

        public bool CanExecuteAddBreakfast(object o)
        {
            return (SelectedFoodPlan != null) && (SelectedRecipeFromList != null);
        }
        public bool CanExecuteAddDinner(object o)
        {
            return (SelectedFoodPlan != null) && (SelectedRecipeFromList != null);
        }
        public bool CanExecuteAddLunch(object o)
        {
            return (SelectedFoodPlan != null) && (SelectedRecipeFromList != null);
        }
        public bool CanExecuteAddSnack(object o)
        {
            return (SelectedFoodPlan != null) && (SelectedRecipeFromList != null);
        }

        #region Methods: Generic Command Methods

        /*      
        public bool CanExecute<NewCommand>(object o)
        {

        }

        public void Execute<NewCommand>(object o)
        {

        }
    */
        #endregion
        #endregion
        #endregion

    }
}
