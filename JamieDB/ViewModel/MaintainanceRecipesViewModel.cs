using JamieDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace JamieDB.ViewModel
{

    class JamieDBViewModelBase : INotifyPropertyChanged
    {
        #region Attributes
        #endregion

        #region static Attributes
        private static string _StatusBarMessage;
        #endregion

        #region Attributes:Context
        protected static JamieDBLinqDataContext _context;
        #endregion

        #region Attributes: Commands
//        private JamieDBViewModelCommand _SaveCommand;
        #endregion

        #region Constructors
        public JamieDBViewModelBase()
        {
            if (context == null) context = new JamieDBLinqDataContext();
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Events:EventHandler
        #endregion

        #region Properties
        public string StatusBarMessage
        {
            get
            {
                return _StatusBarMessage;
            }
            set
            {
                if (_StatusBarMessage != value)
                {
                    _StatusBarMessage = value;
                    OnPropertyChanged("StatusBarMessage");
                }
            }
        }

        protected JamieDBLinqDataContext context
        {
            get
            {
                return _context;
            }
            set
            {
                if (_context!=value)
                {
                    _context = value;
                    OnPropertyChanged("context");
                }
            }
        }
        #endregion

        #region Methods
        public void OnPropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion
        #region Methods: Generic Command Methods
        public bool CanAlwaysExecute(object o)
        {
            return true;
        }
        /*      public bool CanExecute<NewCommand>(object o)
                {

                }

                public void Execute<NewCommand>(object o)
                {

                }
        */
        #endregion

        

    }


    class MaintainanceRecipesViewModel : JamieDBViewModelBase
    {
        #region Attributes
        private ObservableCollection<FoodPlanItem> _FoodPlanItems;
        private ObservableCollection<FoodPlan> _FoodPlans;
           
        private ObservableCollection<Recipe> _Recipes;
        private ObservableCollection<RecipeIngredient> _RecipeIngredients;
        private DateTime _SelectedFoodPlanDate;
        private FoodPlanItem _SelectedFoodPlanItem;
        private FoodPlan _SelectedFoodPlan;
        private DateTime _SelectedFoodPlanEndDate;
        private Recipe _SelectedRecipe;
        private RecipeIngredient _SelectedRecipeIngredient;
        //        private ObservableCollection<ShoppingListItem> _ShoppingListItems;

        private FoodPlansVMClass _FoodPlanVM;
        private IngredientsVMClass _IngredientVM;
        private ShoppingListsVMClass _ShoppingListVM;
        private UnitsVMClass _UnitVM;


        #endregion
        #region Attributes: Commands
        private JamieDBViewModelCommand _DeleteFoodPlanItemCommand;
        private JamieDBViewModelCommand _DeleteFoodPlanCommand;
        private JamieDBViewModelCommand _DeleteRecipeCommand;
        private JamieDBViewModelCommand _DeleteRecipeIngredientCommand;
        private JamieDBViewModelCommand _DeleteUnitCommand;
        private JamieDBViewModelCommand _DeleteUnitTranslationCommand;
        private JamieDBViewModelCommand _LoadFoodPlanCommand;
        private JamieDBViewModelCommand _NewFoodPlanItemCommand;
        private JamieDBViewModelCommand _NewFoodPlanCommand;
        private JamieDBViewModelCommand _NewRecipeCommand;
        private JamieDBViewModelCommand _NewRecipeIngredientCommand;
        private JamieDBViewModelCommand _NewUnitCommand;
        private JamieDBViewModelCommand _NewUnitTranslationCommand;
        private JamieDBViewModelCommand _SaveCommand;
        #endregion

        #region Constructors
        public MaintainanceRecipesViewModel()
        {
            FoodPlanVM = new FoodPlansVMClass();
            IngredientVM = new IngredientsVMClass();
            //ShoppingListVM = new ShoppingListsVMClass();
            UnitVM = new UnitsVMClass();
            


            DeleteFoodPlanItemCommand = new JamieDBViewModelCommand(CanExecuteDeleteFoodPlanItem, ExecuteDeleteFoodPlanItem);
            DeleteFoodPlanCommand = new JamieDBViewModelCommand(CanExecuteDeleteFoodPlan, ExecuteDeleteFoodPlan);
            DeleteRecipeCommand = new JamieDBViewModelCommand(CanExecuteDeleteRecipe, ExecuteDeleteRecipe);
            DeleteRecipeIngredientCommand = new JamieDBViewModelCommand(CanExecuteDeleteRecipeIngredient, ExecuteDeleteRecipeIngredient);

            LoadFoodPlanCommand = new JamieDBViewModelCommand(CanExecuteLoadFoodPlan, ExecuteLoadFoodPlan);

            NewFoodPlanItemCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewFoodPlanItem);
            NewFoodPlanCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewFoodPlan);
            NewRecipeCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipe);
//            NewRecipeIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipeIngredient);
            SaveCommand = new JamieDBViewModelCommand(CanExecuteSaveRecipe, ExecuteSaveRecipe);

            RefreshRecipes();
            SelectedFoodPlanDate = DateTime.Now.Date;
            RefreshFoodPlans();
            RefreshFoodPlanItems();
            //RefreshShoppingListItems();

        }
        #endregion

        #region Events
        #endregion
        #region Events:EventHandler
        public void FoodPlanItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    FoodPlanItem NewFoodPlanItem = (FoodPlanItem)e.NewItems[0];

                    NewFoodPlanItem.DateTime = SelectedFoodPlanDate;
                    NewFoodPlanItem.Recipe = SelectedRecipe;

                    SelectedFoodPlanItem = NewFoodPlanItem;

                    StatusBarMessage = "FoodPlanItem Added: "
                    + ((SelectedFoodPlanItem == null) ? "null" : SelectedFoodPlanItem.Recipe.Name);

                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (FoodPlanItem FPI in e.OldItems)
                    {
                        context.FoodPlanItems.DeleteOnSubmit(FPI);
                        StatusBarMessage = "FoodPlanItem Deleted";
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                StatusBarMessage = "FoodPlanItem Replaced";
            }
        }
        public void FoodPlanChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    FoodPlan NewFoodPlan = (FoodPlan)e.NewItems[0];

                    //                    NewFoodPlan.DateTime = SelectedFoodPlanDate;

                    SelectedFoodPlan = NewFoodPlan;

                    StatusBarMessage = "FoodPlan Added: " + SelectedFoodPlan.Name;

                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (FoodPlan FPT in e.OldItems)
                    {
                        context.FoodPlans.DeleteOnSubmit(FPT);
                        StatusBarMessage = "FoodPlan Deleted";
                    }
                }
            }
        }
        public void RecipeIngredientChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    var NewIngredientChange = (RecipeIngredient)e.NewItems[0];

                    //Fill Foreign Keys

                    NewIngredientChange.RecipeID = SelectedRecipe.Id;
                    NewIngredientChange.Recipe = SelectedRecipe;

                    NewIngredientChange.IngredientID = IngredientVM.SelectedIngredient.Id;
                    NewIngredientChange.Ingredient = IngredientVM.SelectedIngredient;

                    NewIngredientChange.UnitID = UnitVM.SelectedUnit.Id;
                    NewIngredientChange.Unit = UnitVM.SelectedUnit;
                    StatusBarMessage = "RecipeIngredient Added";

                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (RecipeIngredient RI in e.OldItems)
                    {
                        context.RecipeIngredients.DeleteOnSubmit(RI);
                        StatusBarMessage = "RecipeIngredient Deleted";
                    }
                }
            }
        }
        #endregion

        #region Properties
        public ObservableCollection<FoodPlanItem> FoodPlanItems
        {
            get
            {
                return _FoodPlanItems;
            }
            set
            {
                if (_FoodPlanItems != value)
                {
                    _FoodPlanItems = value;
                    OnPropertyChanged("FoodPlanItems");
                }
            }
        }
        public ObservableCollection<FoodPlan> FoodPlans
        {
            get
            {
                return _FoodPlans;
            }
            set
            {
                if (_FoodPlans != value)
                {
                    _FoodPlans = value;
                    OnPropertyChanged("FoodPlans");
                }
            }
        }
        public long SelectedFoodPlanItemCount
        {
            get
            {
                
                if (SelectedFoodPlan == null) return 0;
                else return GetFoodPlanItemsCount(SelectedFoodPlan);
            }
        }
        
        public ObservableCollection<Recipe> Recipes
        {
            get
            {
                return _Recipes;
            }
            set
            {
                if (_Recipes != value)
                {
                    _Recipes = value;
                    OnPropertyChanged("Recipes");
                }
            }
        }
        public ObservableCollection<RecipeIngredient> RecipeIngredients
        {
            get
            {
                return _RecipeIngredients;
            }

            set
            {
                if (_RecipeIngredients != value)
                {
                    _RecipeIngredients = value;
                    OnPropertyChanged("RecipeIngredients");
                }
            }
        }
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
                    OnPropertyChanged("SelectedFoodPlanItemCount");
                    DeleteFoodPlanCommand.OnCanExecuteChanged();
                    LoadFoodPlanCommand.OnCanExecuteChanged();
                    StatusBarMessage = "SelectedFoodPlan changed: ";
                    if ((value != null) && (value.Name != null)) StatusBarMessage += value.Name;
                    else StatusBarMessage += "null";
                }
            }
        }
        public DateTime SelectedFoodPlanEndDate
        {
            get
            {
                return _SelectedFoodPlanEndDate;
            }

            set
            {
               if (_SelectedFoodPlanDate <= value)
               {
                    _SelectedFoodPlanEndDate = value;
                    OnPropertyChanged("SelectedFoodPlanEndDate");
                }
            }
        }
        public DateTime SelectedFoodPlanDate
        {
            get
            {
                return _SelectedFoodPlanDate;
            }

            set
            {
                long FoodPlanRangeTicks;
                
                if (SelectedFoodPlanEndDate == null) FoodPlanRangeTicks = 0;
                else 
                {
                    FoodPlanRangeTicks = SelectedFoodPlanEndDate.Ticks - _SelectedFoodPlanDate.Ticks;
                    if (FoodPlanRangeTicks < 0) FoodPlanRangeTicks = 0;
                }

                _SelectedFoodPlanDate = value;
                OnPropertyChanged("SelectedFoodPlanDate");
                SelectedFoodPlanEndDate = value.AddTicks(FoodPlanRangeTicks);
                DeleteRecipeCommand.OnCanExecuteChanged();  //?? Warum ??
                RefreshFoodPlanItems();
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
                if (_SelectedFoodPlanItem != value)
                {
                    _SelectedFoodPlanItem = value;
                    OnPropertyChanged("SelectedFoodPlanItem");
                    DeleteFoodPlanItemCommand.OnCanExecuteChanged();
                    StatusBarMessage = "SelectedFoodPlanItem changed: ";
                    if ((value != null) && (value.Recipe != null)) StatusBarMessage += value.Recipe.Name;
                    else StatusBarMessage += "null";
                }
            }
        }
        public Recipe SelectedRecipe
        {
            get
            {
                return _SelectedRecipe;
            }

            set
            {
                if (_SelectedRecipe != value)
                {
                    _SelectedRecipe = value;
                    OnPropertyChanged("SelectedRecipe");
                    RefreshRecipeIngredients();
                    DeleteRecipeCommand.OnCanExecuteChanged();
                }
            }
        }
        public RecipeIngredient SelectedRecipeIngredient
        {
            get
            {
                return _SelectedRecipeIngredient;
            }

            set
            {
                if (_SelectedRecipeIngredient != value)
                {
                    _SelectedRecipeIngredient = value;
                    OnPropertyChanged("SelectedRecipeIngredient");
                    DeleteRecipeIngredientCommand.OnCanExecuteChanged();
                    StatusBarMessage = "SelectedRecipeIngredient changed: ";
                    if ((value != null) && (value.Ingredient != null)) StatusBarMessage += value.Ingredient.Name;
                    else StatusBarMessage += "null";
                }
            }
        }


        public FoodPlansVMClass FoodPlanVM
        {
            get
            {
                return _FoodPlanVM;
            }

            set
            {
                if (_FoodPlanVM != value)
                {
                    _FoodPlanVM = value;
                    OnPropertyChanged("FoodPlanVM");
                }

            }
        }
        public IngredientsVMClass IngredientVM
        {
            get
            {
                return _IngredientVM;
            }

            set
            {
                if (_IngredientVM != value)
                {
                    _IngredientVM = value;
                    OnPropertyChanged("IngredientVM");
                }

            }
        }
        public ShoppingListsVMClass ShoppingListVM
        {
            get
            {
                return _ShoppingListVM;
            }

            set
            {
                if (_ShoppingListVM != value)
                {
                    _ShoppingListVM = value;
                    OnPropertyChanged("ShoppingListVM");
                }

            }
        }
        public UnitsVMClass UnitVM
        {
            get
            {
                return _UnitVM;
            }

            set
            {
                if (_UnitVM != value)
                {
                    _UnitVM = value;
                    OnPropertyChanged("UnitVM");
                }

            }
        }


        #endregion
        #region Properties: Commands
        public JamieDBViewModelCommand DeleteFoodPlanItemCommand
        {
            get
            {
                return _DeleteFoodPlanItemCommand;
            }

            set
            {
                _DeleteFoodPlanItemCommand = value;
            }
        }
        public JamieDBViewModelCommand DeleteFoodPlanCommand
        {
            get
            {
                return _DeleteFoodPlanCommand;
            }

            set
            {
                _DeleteFoodPlanCommand = value;
            }
        }
        public JamieDBViewModelCommand DeleteRecipeCommand
        {
            get
            {
                return _DeleteRecipeCommand;
            }

            set
            {
                _DeleteRecipeCommand = value;
            }
        }
        public JamieDBViewModelCommand DeleteRecipeIngredientCommand
        {
            get
            {
                return _DeleteRecipeIngredientCommand;
            }

            set
            {
                _DeleteRecipeIngredientCommand = value;
            }
        }
        public JamieDBViewModelCommand DeleteUnitCommand
        {
            get
            {
                return _DeleteUnitCommand;
            }

            set
            {
                _DeleteUnitCommand = value;
            }
        }
        public JamieDBViewModelCommand DeleteUnitTranslationCommand
        {
            get
            {
                return _DeleteUnitTranslationCommand;
            }

            set
            {
                _DeleteUnitTranslationCommand = value;
            }
        }
        // ---------------------
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
        // ---------------------
        public JamieDBViewModelCommand NewFoodPlanItemCommand
        {
            get
            {
                return _NewFoodPlanItemCommand;
            }

            set
            {
                _NewFoodPlanItemCommand = value;
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
        public JamieDBViewModelCommand NewRecipeCommand
        {
            get
            {
                return _NewRecipeCommand;
            }

            set
            {
                _NewRecipeCommand = value;
            }
        }
        public JamieDBViewModelCommand NewRecipeIngredientCommand
        {
            get
            {
                return _NewRecipeIngredientCommand;
            }

            set
            {
                _NewRecipeIngredientCommand = value;
            }
        }
        public JamieDBViewModelCommand NewUnitCommand
        {
            get
            {
                return _NewUnitCommand;
            }

            set
            {
                _NewUnitCommand = value;
            }
        }
        public JamieDBViewModelCommand NewUnitTranslationCommand
        {
            get
            {
                return _NewUnitTranslationCommand;
            }

            set
            {
                _NewUnitTranslationCommand = value;
            }
        }
        // ---------------------
        public JamieDBViewModelCommand SaveCommand
        {
            get
            {
                return _SaveCommand;
            }

            set
            {
                _SaveCommand = value;
            }
        }
        #endregion

        #region Methods
        
        private ObservableCollection<FoodPlanItem> GetFoodPlanItems(FoodPlan ConsideredFPT)
        {
            return new ObservableCollection<FoodPlanItem>(
                            context.FoodPlanItems
                                    .Where(fpti => (fpti.FoodPlan == ConsideredFPT))
                                    .OrderBy(fpti => fpti.DateTime));
        }
        private long GetFoodPlanItemsCount(FoodPlan ConsideredFPT)
        {
            return GetFoodPlanItems(ConsideredFPT).Count();
        }

        private void RefreshFoodPlanItems()
        {
            if (SelectedFoodPlanDate != null)
            {
                FoodPlanItems = new ObservableCollection<FoodPlanItem>(context.FoodPlanItems
                                                                       .Where(fpi => fpi.DateTime.Date == SelectedFoodPlanDate.Date)
                                                                       .OrderBy(fpi => fpi.DateTime));
                FoodPlanItems.CollectionChanged += new NotifyCollectionChangedEventHandler(FoodPlanItemChanged);

                if (SelectedFoodPlanDate != null)
                {
                    if (FoodPlanItems != null)
                    {
                        SelectedFoodPlanItem = FoodPlanItems.FirstOrDefault();
                    }
                }
                else
                {
                    if (!FoodPlanItems.Contains(SelectedFoodPlanItem)) SelectedFoodPlanItem = FoodPlanItems.FirstOrDefault();
                }

              //  StatusBarMessage = "FoodPlanItems refreshed: Selected FoodPlanItem = "
              //                  + ((SelectedFoodPlanItem == null) ? "null" : SelectedRecipeIngredient.Ingredient.Name);
            }
        }
        private void RefreshFoodPlans()
        {

            FoodPlans = new ObservableCollection<FoodPlan>(context.FoodPlans);
            FoodPlans.CollectionChanged += new NotifyCollectionChangedEventHandler(FoodPlanChanged);


                StatusBarMessage = "FoodPlans refreshed: Selected FoodPlan = "
                                + ((SelectedFoodPlan == null) ? "null" : SelectedFoodPlan.Name);
        }
        
        private void RefreshRecipeIngredients()
        {
            if (SelectedRecipe != null)
            {
                RecipeIngredients = new ObservableCollection<RecipeIngredient>(context.RecipeIngredients.Where(ri => ri.RecipeID == SelectedRecipe.Id));
                RecipeIngredients.CollectionChanged += new NotifyCollectionChangedEventHandler(RecipeIngredientChanged);
                

                if (SelectedRecipeIngredient == null)
                {
                    if (RecipeIngredients != null)
                    {
                        SelectedRecipeIngredient = RecipeIngredients.FirstOrDefault();
                    }
                }
                else
                {
                    if (!RecipeIngredients.Contains(SelectedRecipeIngredient)) SelectedRecipeIngredient = RecipeIngredients.FirstOrDefault();
                }
                StatusBarMessage = "RecipeIngredients refreshed: Selected RecipeIngredient = "
                                + ((SelectedRecipeIngredient == null)?"null":SelectedRecipeIngredient.Ingredient.Name);
            }
        }
        private void RefreshRecipes()
        {
            var result = context.Recipes.OrderBy(r => r.Name);

            Recipes = new ObservableCollection<Recipe>(result);

            if (SelectedRecipe == null) SelectedRecipe = result.FirstOrDefault();

        }
      
        /*  private void RefreshShoppingListItems()
        {

            ShoppingListItems = new ObservableCollection<ShoppingListItem>();
            var SelectedFoodPlanItems = context.FoodPlanItems.Where(fpi => fpi.DateTime > DateTime.Now);
            

            foreach (var fpi in SelectedFoodPlanItems)
            {
                //var RelatedRecipe = fpi.Recipe;
                var RelatedRecipeIngredients = context.RecipeIngredients.Where(ri => ri.RecipeID == fpi.RecipeID);
                
                foreach (var rri in RelatedRecipeIngredients)
                {
                    ShoppingListItems.Add(new ShoppingListItem
                    {
                        FoodPlanDate = fpi.DateTime,
                        Ingredient = rri.Ingredient.Name,
                        Quantity = (fpi.PortionCount/fpi.Recipe.PortionQuantity) * rri.Quantity,
                        Unit = rri.Unit.Symbol,
                        Recipe = rri.Recipe.Name
                    });
                }
            }

            ShoppingListItems.OrderBy(sli => sli.Ingredient);


            var result = context.ShoppingListItems
                                 .OrderBy(sli => sli.Ingredient)
                                 .ThenBy(sli=>sli.FoodPlanDate.Date)
                                 .ThenBy(sli=>sli.FoodPlanDate.TimeOfDay);


            
//            ShoppingListItems = new ObservableCollection<ShoppingListItem>(result);

        }*/
        #endregion
        #region Methods:Command Methods
        public bool CanExecuteDeleteFoodPlanItem(object o)
        {
            return (SelectedFoodPlanItem != null);
        }
        public bool CanExecuteDeleteFoodPlan(object o)
        {
            return (SelectedFoodPlan != null);
        }
        public bool CanExecuteDeleteRecipe(object o)
        {
            return (SelectedRecipe != null);
        }
        public bool CanExecuteDeleteRecipeIngredient(object o)
        {
            return (SelectedRecipeIngredient != null);
        }
        public bool CanExecuteDeleteUnit(object o)
        {
            return (UnitVM.SelectedUnit != null);
        }
        public bool CanExecuteDeleteUnitTranslation(object o)
        {
            return (UnitVM.SelectedUnitTranslation != null);
        }
        public bool CanExecuteLoadFoodPlan(object o)
        {
            return (SelectedFoodPlan != null);
        }
        public bool CanExecuteSaveRecipe(object o)
        {
            return true;
        }
        public void ExecuteDeleteFoodPlanItem(object o)
        {
            string MessageText;

            if (SelectedFoodPlanItem != null)
            {
                var rIndex = FoodPlanItems.IndexOf(SelectedFoodPlanItem);
                if (rIndex == FoodPlanItems.Count() - 1) rIndex -= 1;
                MessageText = "FoodPlanItem " + SelectedFoodPlanItem.Recipe.Name + " deleted";


                context.FoodPlanItems.DeleteOnSubmit(SelectedFoodPlanItem);

                try
                {
                    context.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Exception Handling");
                    MessageText = "FoodPlanItem " + SelectedFoodPlanItem.Recipe.Name + " NOT deleted";

                    // Make some adjustments.
                    // ...
                    // Try again.
                    //context.SubmitChanges();
                }
                RefreshFoodPlanItems();
                if (rIndex >= 0) SelectedFoodPlanItem = FoodPlanItems[rIndex];
                else SelectedFoodPlanItem = null;
                StatusBarMessage = MessageText;

            }

        }
        public void ExecuteDeleteFoodPlan(object o)
        {
            string MessageText;

            if (SelectedFoodPlan != null)
            {
                var rIndex = FoodPlans.IndexOf(SelectedFoodPlan);
                if (rIndex == FoodPlans.Count() - 1) rIndex -= 1;
                MessageText = "FoodPlan " + SelectedFoodPlan.Name + " deleted";


                context.FoodPlans.DeleteOnSubmit(SelectedFoodPlan);

                // foreach () in Detailtable --> DeleteOnSubmit DetailEntry
                if (FoodPlanItems != null)
                {
                    foreach (FoodPlanItem ToBeDeletedFoodPlanItem in FoodPlanItems)
                    {
                        context.FoodPlanItems.DeleteOnSubmit(ToBeDeletedFoodPlanItem);
                    }
                }

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

                RefreshFoodPlans();
                if (rIndex >= 0) SelectedFoodPlan = FoodPlans[rIndex];
                else SelectedFoodPlan = null;
                StatusBarMessage = MessageText;
            }

        }
        public void ExecuteDeleteRecipe(object o)
        {
            string MessageText;

            if (SelectedRecipe != null)
            {
                var rIndex = Recipes.IndexOf(SelectedRecipe);
                if (rIndex == Recipes.Count() - 1) rIndex -= 1;
                MessageText = "Recipe " + SelectedRecipe.Name + " deleted";

                context.Recipes.DeleteOnSubmit(SelectedRecipe);

                // foreach () in Detailtable --> DeleteOnSubmit DetailEntry
                if (RecipeIngredients != null)
                {
                    foreach (RecipeIngredient ToBeDeletedRecipeIngredient in RecipeIngredients)
                    {
                        context.RecipeIngredients.DeleteOnSubmit(ToBeDeletedRecipeIngredient);
                    }

                }
                try
                {
                    context.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Exception Handling");
                    MessageText = "Recipe " + SelectedRecipe.Name + " deleted";

                    // Make some adjustments.
                    // ...
                    // Try again.
                    //context.SubmitChanges();
                }

                RefreshRecipes();
                if (rIndex >= 0) SelectedRecipe = Recipes[rIndex];
                else SelectedRecipe = null;
                StatusBarMessage = MessageText;
            }

        }
        public void ExecuteDeleteRecipeIngredient(object o)
        {
            string MessageText;

            if (SelectedRecipeIngredient != null)
            {
                var rIndex = RecipeIngredients.IndexOf(SelectedRecipeIngredient);
                if (rIndex == RecipeIngredients.Count() - 1) rIndex -= 1;
                MessageText = "RecipeIngredient " + SelectedRecipeIngredient.Ingredient.Name + " deleted";


                context.RecipeIngredients.DeleteOnSubmit(SelectedRecipeIngredient);

            try
            {
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                MessageText = "RecipeIngredient " + SelectedRecipeIngredient.Ingredient.Name + " NOT deleted";

                    // Make some adjustments.
                    // ...
                    // Try again.
                    //context.SubmitChanges();
                }
                RefreshRecipeIngredients();
                if (rIndex >= 0) SelectedRecipeIngredient = RecipeIngredients[rIndex];
                else SelectedRecipeIngredient = null;
                StatusBarMessage = MessageText;

            }

        }
        public void ExecuteDeleteUnitTranslation(object o)
        {
        }
        public void ExecuteSaveRecipe(object o)
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
            RefreshRecipes();
            StatusBarMessage = "All Saved";
        }  
        public void ExecuteLoadFoodPlan(object o)
        {
            ObservableCollection<FoodPlanItem> FoodPlanItemsToBeLoaded;

            if (SelectedFoodPlan != null)
            {
                FoodPlanItemsToBeLoaded = GetFoodPlanItems(SelectedFoodPlan);

                if (FoodPlanItemsToBeLoaded!=null && FoodPlanItemsToBeLoaded.Count()>0)
                {
                    foreach (FoodPlanItem FPTItem in FoodPlanItemsToBeLoaded)
                    {
                        FoodPlanItem NewFoodPlanItem = new FoodPlanItem();
                        NewFoodPlanItem.DateTime = SelectedFoodPlanDate + (FPTItem.DateTime - SelectedFoodPlan.StartDate);
                        NewFoodPlanItem.PortionCount = FPTItem.PortionCount;
                        NewFoodPlanItem.Recipe = FPTItem.Recipe;
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
                        RefreshFoodPlanItems();
                        StatusBarMessage = "FoodPlan Loaded: ";
                    }
                }
            }
            else StatusBarMessage = "Select Food Plan  first";
        }
        public void ExecuteNewFoodPlanItem(object o)
        {

            FoodPlanItem NewFoodPlanItem = new FoodPlanItem();

            NewFoodPlanItem.Recipe = SelectedRecipe;

            FoodPlanItem NextToSelectedFoodPlanItem;
            int FoodPlanItemCount = FoodPlanItems.Count();

            NewFoodPlanItem.DateTime = SelectedFoodPlanDate;

            if ((FoodPlanItems == null) || (FoodPlanItemCount == 0)) // FoodPlanItems leer
            {
                NewFoodPlanItem.DateTime = NewFoodPlanItem.DateTime.AddHours(7); //FirstMealTime


            }
            else if (FoodPlanItemCount == 1)
            {
                NewFoodPlanItem.DateTime = NewFoodPlanItem.DateTime.AddHours(19); //LastMealTime
            }
            else
            {
                if (SelectedFoodPlanItem == FoodPlanItems.First()) NextToSelectedFoodPlanItem = FoodPlanItems[1];
                else NextToSelectedFoodPlanItem = FoodPlanItems[FoodPlanItems.IndexOf(SelectedFoodPlanItem) - 1];

                NewFoodPlanItem.DateTime = NewFoodPlanItem.DateTime.Add(TimeSpan.FromTicks(
                    (SelectedFoodPlanItem.DateTime.TimeOfDay.Ticks + NextToSelectedFoodPlanItem.DateTime.TimeOfDay.Ticks) / 2));
            }

            SelectedFoodPlanItem = NewFoodPlanItem;

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
            RefreshFoodPlanItems();
            StatusBarMessage = "FoodPlanItem Added: " + SelectedFoodPlanItem.Recipe.Name;
        }
        public void ExecuteNewFoodPlan(object o)
        {
            if (FoodPlanItems != null)
            {
                ObservableCollection<FoodPlanItem> FoodPlanItemsInRange = new ObservableCollection<FoodPlanItem>();

                /// hier kommt die Exception
                FoodPlanItemsInRange = new ObservableCollection<FoodPlanItem>(context.FoodPlanItems
                                          .Where(FPI => (FPI.DateTime.Date >= SelectedFoodPlanDate.Date)
                                                     && (FPI.DateTime.Date <= SelectedFoodPlanEndDate)));
                if (FoodPlanItemsInRange.Count() > 0)
                {

                    FoodPlan NewFoodPlan = new FoodPlan();

                    NewFoodPlan.Name = "<New>" + DateTime.Now;
                    NewFoodPlan.StartDate = SelectedFoodPlanDate.Date;

                    SelectedFoodPlan = NewFoodPlan;

                    context.FoodPlans.InsertOnSubmit(NewFoodPlan);

                    try
                    {
                        context.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Exception Handling");
                    }

                    foreach (FoodPlanItem FPIinRange in FoodPlanItemsInRange)
                    {
                        FoodPlanItem NewFoodPlanItem = new FoodPlanItem();
                        NewFoodPlanItem.FoodPlan = SelectedFoodPlan;
                        NewFoodPlanItem.DateTime = FPIinRange.DateTime;
                        NewFoodPlanItem.PortionCount = FPIinRange.PortionCount;
                        NewFoodPlanItem.Recipe = FPIinRange.Recipe;
                    }
                    try
                    {
                        context.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Exception Handling");
                    }

                    RefreshFoodPlans();

                }
                else StatusBarMessage = "No FoodPlanItems in Range";
            }
            else StatusBarMessage = "FoodPlanItems is null"; 




            /*FoodPlanItem NewFoodPlanItem = new FoodPlanItem();

            NewFoodPlanItem.Recipe = SelectedRecipe;

            FoodPlanItem NextToSelectedFoodPlanItem;
            int FoodPlanItemCount = FoodPlanItems.Count();

            NewFoodPlanItem.DateTime = SelectedFoodPlanDate;

            if ((FoodPlanItems == null) || (FoodPlanItemCount == 0)) // FoodPlanItems leer
            {
                NewFoodPlanItem.DateTime = NewFoodPlanItem.DateTime.AddHours(7); //FirstMealTime


            }
            else if (FoodPlanItemCount == 1)
            {
                NewFoodPlanItem.DateTime = NewFoodPlanItem.DateTime.AddHours(19); //LastMealTime
            }
            else
            {
                if (SelectedFoodPlanItem == FoodPlanItems.First()) NextToSelectedFoodPlanItem = FoodPlanItems[1];
                else NextToSelectedFoodPlanItem = FoodPlanItems[FoodPlanItems.IndexOf(SelectedFoodPlanItem) - 1];

                NewFoodPlanItem.DateTime = NewFoodPlanItem.DateTime.Add(TimeSpan.FromTicks(
                    (SelectedFoodPlanItem.DateTime.TimeOfDay.Ticks + NextToSelectedFoodPlanItem.DateTime.TimeOfDay.Ticks) / 2));
            }

            SelectedFoodPlanItem = NewFoodPlanItem;

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
            RefreshFoodPlanItems();
            */
            StatusBarMessage = "FoodPlan Added: ";// + SelectedFoodPlanItem.Recipe.Name;
        }
        public void ExecuteNewRecipe(object o)
        {
            Recipe NewRecipe = new Recipe();
            NewRecipe.Name = "<Recipe>: New " + DateTime.Now.ToString();
            context.Recipes.InsertOnSubmit(NewRecipe);
            
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
            RefreshRecipes();
            SelectedRecipe = NewRecipe;
            StatusBarMessage = "Recipe Added";
        }
        
        #endregion
        #region Methods: Generic Command Methods

        /*      public bool CanExecute<NewCommand>(object o)
                {

                }

                public void Execute<NewCommand>(object o)
                {

                }
        */
        #endregion
    }



    /*
        [Flags] public enum TranslationType
                            { IsTypeChange = 0x1, IsIngredientDependent = 0x2 }

        public class UnitTranslation : IEquatable<UnitTranslation>
        {
            //Variables
            private long? _ID;
            private Ingredient _AffectedIngredient;
            private Unit _BaseUnit;
            private IngredientType _IngredientType;
            private Unit _TargetUnit;
            private double _TranslationFactor;
            private TranslationType _TranslationFlag;
            private ListEntryStatus _TranslationStatus;

            //Constructors
            public UnitTranslation()
            {
                TranslationFlag = (TranslationType)0;
                TranslationStatus = ListEntryStatus.IsOK;
            }
            public UnitTranslation(Unit Base, Unit Target, double TranslationFactor, UnitTranslation Template)
            {
                // auf der Basis einer Vorlage (Ingredient und TranslationFlag werden übernommen),

                _BaseUnit = Base;
                _TargetUnit = Target;
                _TranslationFactor = TranslationFactor;
                _AffectedIngredient = Template.AffectedIngredient;
                _TranslationFlag = Template.TranslationFlag;
                _TranslationStatus = ListEntryStatus.IsOK;

            }
            public UnitTranslation(Unit Base, Unit Target, double TranslationFactor, Ingredient AffectedIngredient,
                                   ListEntryStatus Status)
            {
                _BaseUnit = Base;
                _TargetUnit = Target;
                _TranslationFactor = TranslationFactor;
                _AffectedIngredient = AffectedIngredient;
                _TranslationFlag = (TranslationType)0;
                if (AffectedIngredient != null) _TranslationFlag = TranslationType.IsIngredientDependent;
                if (Base.Type != Target.Type) _TranslationFlag |= TranslationType.IsTypeChange;
                _TranslationStatus = Status;
            }
            public UnitTranslation(string BaseUnitSymbol, string TargetUnitSymbol, double TranslationFactor, TranslationType TType, IngredientType IType, UnitSet UnitSetData)
            {
                _BaseUnit = UnitSetData.SelectItem(BaseUnitSymbol);
                _IngredientType = IType;
                _TargetUnit = UnitSetData.SelectItem(TargetUnitSymbol);
                _TranslationFactor = TranslationFactor;
                _TranslationFlag = TType;
                _TranslationStatus = ListEntryStatus.IsOK;
                _UnitSetData = UnitSetData;
            }


            //Methods
            public bool Equals(UnitTranslation ItemToCompare)
            {
                if (ItemToCompare == null) return false;
                return _ID.Equals(ItemToCompare._ID) || EqualKey(ItemToCompare);
            }
            public bool EqualKey(UnitTranslation ItemToCompare)
            {
                bool ReturnValue;


                ReturnValue = TranslationFlag.Equals(ItemToCompare.TranslationFlag);
                if (TranslationFlag == (TranslationType)0)
                {
                    ReturnValue &= (BaseUnit.Equals(ItemToCompare.BaseUnit) && TargetUnit.Equals(ItemToCompare.TargetUnit)) ||
                                   (BaseUnit.Equals(ItemToCompare.TargetUnit) && TargetUnit.Equals(ItemToCompare.BaseUnit));
                }

                if ((AffectedIngredient != null) && TranslationFlag.HasFlag(TranslationType.IsIngredientDependent))
                {
                    ReturnValue &= AffectedIngredient.Equals(ItemToCompare.AffectedIngredient);
                }

                if (TranslationFlag.HasFlag(TranslationType.IsTypeChange) && (!TranslationFlag.HasFlag(TranslationType.IsIngredientDependent)))
                {
                    ReturnValue &= (IngredientType == ItemToCompare.IngredientType);
                }

                if ((TranslationFlag.HasFlag(TranslationType.IsIngredientDependent)) && (AffectedIngredient != null) &&
                    (ItemToCompare.AffectedIngredient != null)) ReturnValue &= AffectedIngredient.Equals(ItemToCompare.AffectedIngredient);


                return ReturnValue;
            }
            public UnitTranslation Inverse()
            {
                UnitTranslation ReturnItem;

                ReturnItem = new Model.UnitTranslation();

                ReturnItem.ID = 0;
                ReturnItem.BaseUnit = this.TargetUnit;
                ReturnItem.TargetUnit = this.BaseUnit;
                ReturnItem.TranslationFactor = (1 / this.TranslationFactor);

                return ReturnItem;
            }
            public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData)
            {
                _IngredientSetData = IngredientSetData;
                _UnitSetData = UnitSetData;
            }
            public override string ToString()
            {
                string ReturnString = (TranslationStatus == ListEntryStatus.IsNotConfirmed ? "invalid: " : "  valid: ");
                ReturnString += string.Format("{0,6}-UnitTranslation: {1,5} =  {2,15:F6} {3,-5} {4}\n   - IngredientType:{5} \n    {6}", ID, BaseUnit, TranslationFactor, TargetUnit,
                                      (TranslationFlag == 0 ? "NoTypeChange, IngredientIndepedant" : string.Format("{0}", TranslationFlag)), IngredientType, AffectedIngredient);
                return ReturnString;
            }

        }

        public class UnitTranslationSet : ObservableCollection<UnitTranslation>
        {

            //Methods
            /* AddInactiveTranslation(BaseUnit, TargetUnit, Ingredient)
            * - Creates UnitTranslation Base->Target for Ingredient to be verified.
            * 
            * AddInactiveTranslation (BaseUnit, TargetUnit, Ingredient.Type)
            * - Creates UnitTranslation Base->Target for Ingredient.Type to be verified.
            * 
            * AddInactiveranslation (BaseUnit, TargetUnit, Ingredient.Type)
            * - Creates UnitTranslation Base->Target for Ingredient.Type to be verified.


            public void AddInactiveItem(Unit Base, Unit Target)
            {
                if (Base.Type == Target.Type)
                {
                    UnitTranslation NewUnitTranslation = new UnitTranslation();
                    NewUnitTranslation.BaseUnit = Base;
                    NewUnitTranslation.TargetUnit = Target;
                    NewUnitTranslation.AffectedIngredient = null;
                    NewUnitTranslation.TranslationFactor = 0;
                    NewUnitTranslation.TranslationFlag = (TranslationType)0;
                    NewUnitTranslation.TranslationStatus = ListEntryStatus.IsNotConfirmed;
                    AddItem(NewUnitTranslation);
                }
            }

            public void AddInactiveItem(Unit Base, Unit Target, Ingredient AffectedIngredient)
            {
                UnitTranslation NewUnitTranslation = new UnitTranslation();
                NewUnitTranslation.BaseUnit = Base;
                NewUnitTranslation.TargetUnit = Target;
                NewUnitTranslation.AffectedIngredient = AffectedIngredient;
                NewUnitTranslation.IngredientType = AffectedIngredient.Type;
                NewUnitTranslation.TranslationFactor = 0;
                NewUnitTranslation.TranslationFlag = (TranslationType.IsIngredientDependent | TranslationType.IsTypeChange);
                NewUnitTranslation.TranslationStatus = ListEntryStatus.IsNotConfirmed;
                AddItem(NewUnitTranslation);
            }
            public void AddInactiveItem(Unit Base, Unit Target, IngredientType IType)
            {
                UnitTranslation NewUnitTranslation = new UnitTranslation();
                NewUnitTranslation.BaseUnit = Base;
                NewUnitTranslation.TargetUnit = Target;
                NewUnitTranslation.AffectedIngredient = null;
                NewUnitTranslation.IngredientType = IType;
                NewUnitTranslation.TranslationFactor = 0;
                NewUnitTranslation.TranslationFlag = TranslationType.IsTypeChange;
                NewUnitTranslation.TranslationStatus = ListEntryStatus.IsNotConfirmed;
                AddItem(NewUnitTranslation);
            }

            public bool AddItem(UnitTranslation ItemToBeAdded)
            {
                bool ReturnValue = true;

                if (Count == 0) ItemToBeAdded.SetDataReference(IngredientSetData, UnitSetData);

                //Fälle können reduziert werden

                //ItemToBeAdded und der Kehrwert nicht vorhanden
                if (!Contains(ItemToBeAdded))
                {
                    if ((ItemToBeAdded.TranslationFlag & TranslationType.IsIngredientDependent) == TranslationType.IsIngredientDependent)
                    // von Zutat ABHÄNGIG   

                    {
                        if ((ItemToBeAdded.TranslationFlag & TranslationType.IsTypeChange) == TranslationType.IsTypeChange)
                        // Fall 3: von Zutat ABHÄNGIG - MIT Wechsel des UnitTyps
                        {
                        }
                        else
                        // Fall 2: von Zutat ABHÄNGIG - OHNE Wechsel des UnitTyps
                        {
                            //trow Exception --> Fall 2 kommt nicht vor
                        }

                    }

                    else
                    // von Zutat UNABHÄNGIG
                    {
                        if ((ItemToBeAdded.TranslationFlag & TranslationType.IsTypeChange) == TranslationType.IsTypeChange)
                        // Fall 1: von Zutat UNABHÄNGIG - MIT Wechsel des UnitTyps
                        {
                        }
                        else
                        // Fall 0: von Zutat UNABHÄNGIG - OHNE Wechsel des UnitTyps
                        {
                            AddAllItemsWithSameType(ItemToBeAdded);
                            _SelectedItem = ItemToBeAdded;
                        }

                    }
                    ItemToBeAdded.ID = ++_MaxID;
                    Add(ItemToBeAdded);
                }

                else ReturnValue = false;

                return ReturnValue;
            }
            public void AddAllItemsWithSameType(UnitTranslation ItemToBeAdded)
            {
                UnitTranslation GeneratedUnitTranslation;
                UnitTranslation HelpingUnitTranslation;

                if (ItemToBeAdded.BaseUnit.Type == ItemToBeAdded.TargetUnit.Type)
                {
                    // TranslationTobeAdded.BaseUnit -> Different Unit with same Type
                    var ImplicitTranslations = (from u in UnitSetData
                                                where (u.Type == ItemToBeAdded.BaseUnit.Type) && (u != ItemToBeAdded.BaseUnit)
                                                select new { Target = (Unit)u }).ToList();

                    foreach (var i in ImplicitTranslations)
                    {
                        // Factor(ItemToBeAdded.Base, i.Target) 
                        //       = ItemToBeAdded.Factor * Factor(ItemToBeAdded.Target, i.Target)
                        //       = ItemToBeAdded.Factor * HelpingUnitTranslation.Factor
                        //       --> HelpingUnitTranslation = ItemToBeAdded.Target --> i.Target

                        HelpingUnitTranslation = GetTranslation(ItemToBeAdded.TargetUnit, i.Target);
                        if (HelpingUnitTranslation != null)
                        {
                            GeneratedUnitTranslation = new UnitTranslation(ItemToBeAdded.BaseUnit, i.Target,
                                                              ItemToBeAdded.TranslationFactor * HelpingUnitTranslation.TranslationFactor,
                                                              ItemToBeAdded);
                            GeneratedUnitTranslation.ID = ++_MaxID;
                            Add(GeneratedUnitTranslation);
                        }


                    }
                    // and TranslationTobeAdded.TargetUnit -> Different Unit with same Type
                    ImplicitTranslations = (from u in UnitSetData
                                            where (u.Type == ItemToBeAdded.TargetUnit.Type) && (u != ItemToBeAdded.TargetUnit)
                                            select new { Target = (Unit)u }).ToList();

                    foreach (var i in ImplicitTranslations)
                    {
                        // Factor(ItemToBeAdded.Target, i.Target) 
                        //       =   Factor(ItemToBeAdded.Target, ItemToBeAdded.Base) * Factor(ItemToBeAdded.Base, i.Target) 
                        //       = 1/Factor(ItemToBeAdded.Base, ItemToBeAdded.Target) * Factor(ItemToBeAdded.Base, i.Target) 
                        //       =   Factor(ItemToBeAdded.Base, i.Target) / Factor(ItemToBeAdded.Base, ItemToBeAdded.Target)
                        //       =   HelpingUnitTranslation.Factor / ItemToBeAdded.Factor
                        //       --> HelpingUnitTranslation = ItemToBeAdded.Base --> i.Target

                        HelpingUnitTranslation = GetTranslation(ItemToBeAdded.BaseUnit, i.Target);
                        if (HelpingUnitTranslation != null)
                        {
                            GeneratedUnitTranslation = new UnitTranslation(ItemToBeAdded.TargetUnit, i.Target,
                                                               HelpingUnitTranslation.TranslationFactor / ItemToBeAdded.TranslationFactor,
                                                               ItemToBeAdded);
                            GeneratedUnitTranslation.ID = ++_MaxID;
                            Add(GeneratedUnitTranslation);
                        }

                    }


                }
            }
            public void DeleteSelectedItem()
            {
                int NewSelectedIndex;

                if ((Count == 0) || (SelectedItem == null)) return;

                if (Count > 1)
                {
                    NewSelectedIndex = IndexOf(SelectedItem) - 1;
                    if (NewSelectedIndex < 0) NewSelectedIndex = 0;
                }
                else NewSelectedIndex = 1;

                Remove(SelectedItem);

                if (Count > 0) _SelectedItem = this[NewSelectedIndex];
                else _SelectedItem = null;
            }


            public UnitTranslation GetTranslation(Unit Base, Unit Target)
            {
                UnitTranslation ReturnObject = null;

                if (Base.Type == Target.Type)
                {
                    var TList = this
                                .Where(s => (s.AffectedIngredient == null) &&
                                (s.TranslationStatus != ListEntryStatus.IsNotConfirmed) &&
                                ((s.BaseUnit == Base) && (s.TargetUnit == Target)) ||
                                ((s.BaseUnit == Target) && (s.TargetUnit == Base)));

                    if (TList.Count() >= 1) ReturnObject = TList.ElementAt(0);
                    if ((ReturnObject != null) && (ReturnObject.BaseUnit != Base)) ReturnObject.Inverse();
                }
                return ReturnObject;
            }
            public UnitTranslation GetTranslation(Unit Base, Unit Target, Ingredient Ingred)
            {
                UnitTranslation BaseTranslation;
                UnitTranslation InterTypeTranslation;
                UnitTranslation ReturnObject = null;
                UnitTranslation TargetTranslation;

                if (Base.Type == Target.Type)
                {
                    ReturnObject = GetTranslation(Base, Target);
                }
                else
                {
                    InterTypeTranslation = GetTranslation(Base.Type, Target.Type, Ingred);

                    if (InterTypeTranslation != null)
                    {
                        BaseTranslation = GetTranslation(Base, InterTypeTranslation.BaseUnit);
                        TargetTranslation = GetTranslation(InterTypeTranslation.TargetUnit, Target);

                        if ((BaseTranslation != null) && (TargetTranslation != null))
                        {
                            ReturnObject = new UnitTranslation();
                            ReturnObject.BaseUnit = Base;
                            ReturnObject.TargetUnit = Target;
                            ReturnObject.AffectedIngredient = Ingred;
                            ReturnObject.IngredientType = Ingred.Type;
                            ReturnObject.TranslationFlag = TranslationType.IsTypeChange | TranslationType.IsIngredientDependent;
                            ReturnObject.TranslationFactor = BaseTranslation.TranslationFactor *
                                                             InterTypeTranslation.TranslationFactor *
                                                             TargetTranslation.TranslationFactor;
                        }
                    }
                    else
                    {
                        AddInactiveItem(Base, Target, Ingred);
                        InterTypeTranslation = GetTranslation(Base, Target, Ingred.Type);
                        if (InterTypeTranslation != null)
                        {
                            BaseTranslation = GetTranslation(Base, InterTypeTranslation.BaseUnit);
                            TargetTranslation = GetTranslation(InterTypeTranslation.TargetUnit, Target);
                            if ((BaseTranslation != null) && (TargetTranslation != null))
                            {
                                ReturnObject = new UnitTranslation();
                                ReturnObject.BaseUnit = Base;
                                ReturnObject.TargetUnit = Target;
                                ReturnObject.AffectedIngredient = null;
                                ReturnObject.IngredientType = Ingred.Type;
                                ReturnObject.TranslationFlag = TranslationType.IsTypeChange;
                                ReturnObject.TranslationFactor = BaseTranslation.TranslationFactor *
                                                                 InterTypeTranslation.TranslationFactor *
                                                                 TargetTranslation.TranslationFactor;
                            }
                        }
                        else AddInactiveItem(Base, Target, Ingred.Type);
                    }

                }
                return ReturnObject;
            }
            public UnitTranslation GetTranslation(Unit Base, Unit Target, IngredientType IType)
            {
                UnitTranslation ReturnObject = null;

                // Ermittle alle UnitTranslations mit zugeordnetem Ingredient, welches den IngredientType IType hat
                // mit den beiden Units Base und Target
                var TList = this
                            .Where(s => (s.AffectedIngredient != null) && (s.IngredientType == IType) &&
                            (s.TranslationStatus != ListEntryStatus.IsNotConfirmed) &&
                            ((s.BaseUnit == Base) && (s.TargetUnit == Target)) ||
                            ((s.BaseUnit == Target) && (s.TargetUnit == Base)));

                if (TList.Count() >= 1) ReturnObject = TList.ElementAt(0);
                if ((ReturnObject != null) && (ReturnObject.BaseUnit != Base)) ReturnObject.Inverse();

                return ReturnObject;
            }
            public UnitTranslation GetTranslation(UnitType BaseType, UnitType TargetType, Ingredient Ingred)
            {
                UnitTranslation ReturnObject = null;

                var TList = this
                            .Where(s => (s.AffectedIngredient != null) && (s.AffectedIngredient == Ingred) &&
                                        (s.TranslationStatus != ListEntryStatus.IsNotConfirmed) &&
                                        (((s.BaseUnit.Type == BaseType) && (s.TargetUnit.Type == TargetType)) ||
                                        ((s.BaseUnit.Type == TargetType) && (s.TargetUnit.Type == BaseType))));

                if (TList.Count() == 1)
                {
                    ReturnObject = TList.ElementAt(0);
                    if (ReturnObject.BaseUnit.Type != BaseType) ReturnObject.Inverse();
                }

                return ReturnObject;
            }
            public UnitTranslationSet OpenSet(string FileName)
            {
                UnitTranslationSet ReturnSet = this;
                ReturnSet.Clear();
                FileName += FileExtension;
                using (Stream fs = new FileStream(FileName, FileMode.Open))
                {
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(ReturnSet.GetType());
                    ReturnSet = (UnitTranslationSet)x.Deserialize(fs);
                }
                return ReturnSet;

            }// --> Data

            public void PopulateSetWithDefaults()
            {

                TranslationType UTType = TranslationType.IsTypeChange;

                Clear();
                //IsWeight
                AddItem(new UnitTranslation("kg", "g", 1000.0, 0, 0, UnitSetData));
                AddItem(new UnitTranslation("g", "dg", 10.0, 0, 0, UnitSetData));   //dg: Dezi-Gramm
                AddItem(new UnitTranslation("g", "cg", 100.0, 0, 0, UnitSetData));  //cg: Zenti-Gramm
                AddItem(new UnitTranslation("g", "mg", 1000.0, 0, 0, UnitSetData)); //mg: Milli-Gramm
                AddItem(new UnitTranslation("g", "dag", 0.1, 0, 0, UnitSetData)); //mg: Milli-Gramm
                AddItem(new UnitTranslation("pf", "g", 500.0, 0, 0, UnitSetData));  //pf: Pfund


                AddItem(new UnitTranslation("oz", "g", 28.3495, 0, 0, UnitSetData));   //oz: Unzen
                AddItem(new UnitTranslation("lb", "oz", 16.0, 0, 0, UnitSetData));  //lb: Pound
                AddItem(new UnitTranslation("oz", "dr", 16.0, 0, 0, UnitSetData));  //dr: dram
                AddItem(new UnitTranslation("lb", "gr", 7000.0, 0, 0, UnitSetData));  //gr: grain

                //IsWeight: zu ermitteln
                //  Msp (Messerspitze) -> g (Gramm)
                //  Pr (Prise) -> g (Gramm)


                //IsVolume
                AddItem(new UnitTranslation("l", "ml", 1000.0, 0, 0, UnitSetData));
                AddItem(new UnitTranslation("l", "cl", 100.0, 0, 0, UnitSetData));
                AddItem(new UnitTranslation("l", "dl", 10.0, 0, 0, UnitSetData));
                AddItem(new UnitTranslation("Ta", "ml", 237.0, 0, 0, UnitSetData));              //Ta = Tasse
                AddItem(new UnitTranslation("TL", "ml", 5.0, 0, 0, UnitSetData));                //TL = Teelöffel
                AddItem(new UnitTranslation("BL", "ml", 5.0, 0, 0, UnitSetData));                //BL = Barlöffel
                AddItem(new UnitTranslation("EL", "ml", 15.0, 0, 0, UnitSetData));               //TL = Teelöffel
                AddItem(new UnitTranslation("ml", "Tr", 30.0, 0, 0, UnitSetData));               //Tr = Tropfen
                AddItem(new UnitTranslation("ds", "ml", 0.6, 0, 0, UnitSetData));                //Dash = Spritzer
                AddItem(new UnitTranslation("Spr", "ml", 25.0, 0, 0, UnitSetData));              //sht = Schuss
                AddItem(new UnitTranslation("ga (US)", "l", 3.785, 0, 0, UnitSetData));          //ga(US) = US Galone
                AddItem(new UnitTranslation("ga (US)", "fl.oz (US)", 128, 0, 0, UnitSetData));   //fl.oz (US) = fluid ounce US
                AddItem(new UnitTranslation("qt (US)", "fl.oz (US)", 32, 0, 0, UnitSetData));    //qt (US) = fluid ounce US
                AddItem(new UnitTranslation("pt (US)", "fl.oz (US)", 16, 0, 0, UnitSetData));    //pt (US) = fluid ounce US


                AddItem(new UnitTranslation("ga (UK)", "l", 4.546, 0, 0, UnitSetData));          //ga(UK): ga(UK) = Imperial Galone
                AddItem(new UnitTranslation("ga (UK)", "fl.oz (UK)", 160, 0, 0, UnitSetData));   //fl.oz (US) = fluid ounce US
                AddItem(new UnitTranslation("qt (UK)", "fl.oz (UK)", 40, 0, 0, UnitSetData));    //qt (US) = fluid ounce US
                AddItem(new UnitTranslation("pt (UK)", "fl.oz (UK)", 20, 0, 0, UnitSetData));    //pt (US) = fluid ounce US


                // IsTypeChange = 0x1 --> max ein Eintrag je IngredientType
                UTType = TranslationType.IsTypeChange;

                //Volume -> Weight
                AddItem(new UnitTranslation("l", "kg", 1.0, UTType, IngredientType.IsFluid, UnitSetData)); //Volume -> Weight
                AddItem(new UnitTranslation("l", "kg", 1.0, UTType, IngredientType.IsSolid, UnitSetData)); //Volume -> Weight
                AddItem(new UnitTranslation("l", "kg", 1.0, UTType, IngredientType.IsCrystal, UnitSetData)); //Volume -> Weight
                AddItem(new UnitTranslation("l", "g", 1000.0 / 145 * 70, UTType, IngredientType.IsPowder, UnitSetData)); //Volume -> Weight
                AddItem(new UnitTranslation("l", "g", 1000.0 / 115 * 85, UTType, IngredientType.IsGranular, UnitSetData)); //Volume -> Weight
                AddItem(new UnitTranslation("l", "g", 1000.0 / 115 * 85, UTType, IngredientType.IsHerb, UnitSetData)); //Volume -> Weight

                //Count --> Weight
                AddItem(new UnitTranslation("st", "g", 100.0, UTType, IngredientType.IsSolid, UnitSetData)); //Count --> Weight


            }
     */

}


    
    