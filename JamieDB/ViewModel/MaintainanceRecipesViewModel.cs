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
            if (_context == null) _context = new JamieDBLinqDataContext();
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
        private ObservableCollection<FoodPlanTemplate> _FoodPlanTemplates;
        private ObservableCollection<FoodPlanTemplateItem> _FoodPlanTemplateItems;
           
        private ObservableCollection<Recipe> _Recipes;
        private ObservableCollection<RecipeIngredient> _RecipeIngredients;
        private DateTime _SelectedFoodPlanDate;
        private FoodPlanItem _SelectedFoodPlanItem;
        private FoodPlanTemplate _SelectedFoodPlanTemplate;
        private DateTime _SelectedFoodPlanTemplateEndDate;
        private Recipe _SelectedRecipe;
        private RecipeIngredient _SelectedRecipeIngredient;
//        private ObservableCollection<ShoppingListItem> _ShoppingListItems;

        private UnitsVMClass _UnitVM;
        private IngredientsVMClass _IngredientVM;

        #endregion
        #region Attributes: Commands
        private JamieDBViewModelCommand _DeleteFoodPlanItemCommand;
        private JamieDBViewModelCommand _DeleteFoodPlanTemplateCommand;
        private JamieDBViewModelCommand _DeleteRecipeCommand;
        private JamieDBViewModelCommand _DeleteRecipeIngredientCommand;
        private JamieDBViewModelCommand _DeleteUnitCommand;
        private JamieDBViewModelCommand _DeleteUnitTranslationCommand;
        private JamieDBViewModelCommand _LoadFoodPlanTemplateCommand;
        private JamieDBViewModelCommand _NewFoodPlanItemCommand;
        private JamieDBViewModelCommand _NewFoodPlanTemplateCommand;
        private JamieDBViewModelCommand _NewRecipeCommand;
        private JamieDBViewModelCommand _NewRecipeIngredientCommand;
        private JamieDBViewModelCommand _NewUnitCommand;
        private JamieDBViewModelCommand _NewUnitTranslationCommand;
        private JamieDBViewModelCommand _SaveCommand;
        #endregion

        #region Constructors
        public MaintainanceRecipesViewModel()
        {
            UnitVM = new UnitsVMClass();
            IngredientVM = new IngredientsVMClass();


            DeleteFoodPlanItemCommand = new JamieDBViewModelCommand(CanExecuteDeleteFoodPlanItem, ExecuteDeleteFoodPlanItem);
            DeleteFoodPlanTemplateCommand = new JamieDBViewModelCommand(CanExecuteDeleteFoodPlanTemplate, ExecuteDeleteFoodPlanTemplate);
            DeleteRecipeCommand = new JamieDBViewModelCommand(CanExecuteDeleteRecipe, ExecuteDeleteRecipe);
            DeleteRecipeIngredientCommand = new JamieDBViewModelCommand(CanExecuteDeleteRecipeIngredient, ExecuteDeleteRecipeIngredient);

            LoadFoodPlanTemplateCommand = new JamieDBViewModelCommand(CanExecuteLoadFoodPlanTemplate, ExecuteLoadFoodPlanTemplate);

            NewFoodPlanItemCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewFoodPlanItem);
            NewFoodPlanTemplateCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewFoodPlanTemplate);
            NewRecipeCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipe);
//            NewRecipeIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipeIngredient);
            SaveCommand = new JamieDBViewModelCommand(CanExecuteSaveRecipe, ExecuteSaveRecipe);

            RefreshRecipes();
            SelectedFoodPlanDate = DateTime.Now.Date;
            RefreshFoodPlanTemplates();
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
                        _context.FoodPlanItems.DeleteOnSubmit(FPI);
                        StatusBarMessage = "FoodPlanItem Deleted";
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                StatusBarMessage = "FoodPlanItem Replaced";
            }
        }
        public void FoodPlanTemplateChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    FoodPlanTemplate NewFoodPlanTemplate = (FoodPlanTemplate)e.NewItems[0];

                    //                    NewFoodPlanTemplate.DateTime = SelectedFoodPlanDate;

                    SelectedFoodPlanTemplate = NewFoodPlanTemplate;

                    StatusBarMessage = "FoodPlanTemplate Added: " + SelectedFoodPlanTemplate.Name;

                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (FoodPlanTemplate FPT in e.OldItems)
                    {
                        _context.FoodPlanTemplates.DeleteOnSubmit(FPT);
                        StatusBarMessage = "FoodPlanTemplate Deleted";
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
                        _context.RecipeIngredients.DeleteOnSubmit(RI);
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
        public ObservableCollection<FoodPlanTemplate> FoodPlanTemplates
        {
            get
            {
                return _FoodPlanTemplates;
            }
            set
            {
                if (_FoodPlanTemplates != value)
                {
                    _FoodPlanTemplates = value;
                    OnPropertyChanged("FoodPlanTemplates");
                }
            }
        }
        public ObservableCollection<FoodPlanTemplateItem> FoodPlanTemplateItems
        {
            get
            {
                return _FoodPlanTemplateItems;
            }
            set
            {
                if (_FoodPlanTemplateItems != value)
                {
                    _FoodPlanTemplateItems = value;
                    OnPropertyChanged("FoodPlanTemplateItems");
                }
            }
        }
        public long SelectedFoodPlanTemplateItemCount
        {
            get
            {
                
                if (SelectedFoodPlanTemplate == null) return 0;
                else return GetFoodPlanTemplateItemsCount(SelectedFoodPlanTemplate);
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
        public FoodPlanTemplate SelectedFoodPlanTemplate
        {
            get
            {
                return _SelectedFoodPlanTemplate;
            }

            set
            {
                if (_SelectedFoodPlanTemplate != value)
                {
                    _SelectedFoodPlanTemplate = value;
                    OnPropertyChanged("SelectedFoodPlanTemplate");
                    OnPropertyChanged("SelectedFoodPlanTemplateItemCount");
                    DeleteFoodPlanTemplateCommand.OnCanExecuteChanged();
                    LoadFoodPlanTemplateCommand.OnCanExecuteChanged();
                    StatusBarMessage = "SelectedFoodPlanTemplate changed: ";
                    if ((value != null) && (value.Name != null)) StatusBarMessage += value.Name;
                    else StatusBarMessage += "null";
                }
            }
        }
        public DateTime SelectedFoodPlanTemplateEndDate
        {
            get
            {
                return _SelectedFoodPlanTemplateEndDate;
            }

            set
            {
               if (_SelectedFoodPlanDate <= value)
               {
                    _SelectedFoodPlanTemplateEndDate = value;
                    OnPropertyChanged("SelectedFoodPlanTemplateEndDate");
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
                long FoodPlanTemplateRangeTicks;
                
                if (SelectedFoodPlanTemplateEndDate == null) FoodPlanTemplateRangeTicks = 0;
                else 
                {
                    FoodPlanTemplateRangeTicks = SelectedFoodPlanTemplateEndDate.Ticks - _SelectedFoodPlanDate.Ticks;
                    if (FoodPlanTemplateRangeTicks < 0) FoodPlanTemplateRangeTicks = 0;
                }

                _SelectedFoodPlanDate = value;
                OnPropertyChanged("SelectedFoodPlanDate");
                SelectedFoodPlanTemplateEndDate = value.AddTicks(FoodPlanTemplateRangeTicks);
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
/*        public ObservableCollection<ShoppingListItem> ShoppingListItems
        {
            get
            {
                return _ShoppingListItems;
            }
            set
            {
                if (_ShoppingListItems != value)
                {
                    _ShoppingListItems = value;
                    OnPropertyChanged("ShoppingListItems");
                }
            }
        } */


            
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
        public JamieDBViewModelCommand DeleteFoodPlanTemplateCommand
        {
            get
            {
                return _DeleteFoodPlanTemplateCommand;
            }

            set
            {
                _DeleteFoodPlanTemplateCommand = value;
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
        public JamieDBViewModelCommand LoadFoodPlanTemplateCommand
        {
            get
            {
                return _LoadFoodPlanTemplateCommand;
            }

            set
            {
                _LoadFoodPlanTemplateCommand = value;
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
        public JamieDBViewModelCommand NewFoodPlanTemplateCommand
        {
            get
            {
                return _NewFoodPlanTemplateCommand;
            }

            set
            {
                _NewFoodPlanTemplateCommand = value;
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
        
        private ObservableCollection<FoodPlanTemplateItem> GetFoodPlanTemplateItems(FoodPlanTemplate ConsideredFPT)
        {
            return new ObservableCollection<FoodPlanTemplateItem>(
                            _context.FoodPlanTemplateItems
                                    .Where(fpti => (fpti.FoodPlanTemplate == ConsideredFPT))
                                    .OrderBy(fpti => fpti.DateTime));
        }
        private long GetFoodPlanTemplateItemsCount(FoodPlanTemplate ConsideredFPT)
        {
            return GetFoodPlanTemplateItems(ConsideredFPT).Count();
        }

        private void RefreshFoodPlanItems()
        {
            if (SelectedFoodPlanDate != null)
            {
                FoodPlanItems = new ObservableCollection<FoodPlanItem>(_context.FoodPlanItems
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
        private void RefreshFoodPlanTemplates()
        {

            FoodPlanTemplates = new ObservableCollection<FoodPlanTemplate>(_context.FoodPlanTemplates);
            FoodPlanTemplates.CollectionChanged += new NotifyCollectionChangedEventHandler(FoodPlanTemplateChanged);


                StatusBarMessage = "FoodPlanTemplates refreshed: Selected FoodPlanTemplate = "
                                + ((SelectedFoodPlanTemplate == null) ? "null" : SelectedFoodPlanTemplate.Name);
        }
        
        private void RefreshRecipeIngredients()
        {
            if (SelectedRecipe != null)
            {
                RecipeIngredients = new ObservableCollection<RecipeIngredient>(_context.RecipeIngredients.Where(ri => ri.RecipeID == SelectedRecipe.Id));
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
            var result = _context.Recipes.OrderBy(r => r.Name);

            Recipes = new ObservableCollection<Recipe>(result);

            if (SelectedRecipe == null) SelectedRecipe = result.FirstOrDefault();

        }
      
        /*  private void RefreshShoppingListItems()
        {

            ShoppingListItems = new ObservableCollection<ShoppingListItem>();
            var SelectedFoodPlanItems = _context.FoodPlanItems.Where(fpi => fpi.DateTime > DateTime.Now);
            

            foreach (var fpi in SelectedFoodPlanItems)
            {
                //var RelatedRecipe = fpi.Recipe;
                var RelatedRecipeIngredients = _context.RecipeIngredients.Where(ri => ri.RecipeID == fpi.RecipeID);
                
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


            var result = _context.ShoppingListItems
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
        public bool CanExecuteDeleteFoodPlanTemplate(object o)
        {
            return (SelectedFoodPlanTemplate != null);
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
        public bool CanExecuteLoadFoodPlanTemplate(object o)
        {
            return (SelectedFoodPlanTemplate != null);
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


                _context.FoodPlanItems.DeleteOnSubmit(SelectedFoodPlanItem);

                try
                {
                    _context.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Exception Handling");
                    MessageText = "FoodPlanItem " + SelectedFoodPlanItem.Recipe.Name + " NOT deleted";

                    // Make some adjustments.
                    // ...
                    // Try again.
                    //_context.SubmitChanges();
                }
                RefreshFoodPlanItems();
                if (rIndex >= 0) SelectedFoodPlanItem = FoodPlanItems[rIndex];
                else SelectedFoodPlanItem = null;
                StatusBarMessage = MessageText;

            }

        }
        public void ExecuteDeleteFoodPlanTemplate(object o)
        {
            string MessageText;

            if (SelectedFoodPlanTemplate != null)
            {
                var rIndex = FoodPlanTemplates.IndexOf(SelectedFoodPlanTemplate);
                if (rIndex == FoodPlanTemplates.Count() - 1) rIndex -= 1;
                MessageText = "FoodPlanTemplate " + SelectedFoodPlanTemplate.Name + " deleted";


                _context.FoodPlanTemplates.DeleteOnSubmit(SelectedFoodPlanTemplate);

                // foreach () in Detailtable --> DeleteOnSubmit DetailEntry
                if (FoodPlanTemplateItems != null)
                {
                    foreach (FoodPlanTemplateItem ToBeDeletedFoodPlanTemplateItem in FoodPlanTemplateItems)
                    {
                        _context.FoodPlanTemplateItems.DeleteOnSubmit(ToBeDeletedFoodPlanTemplateItem);
                    }
                }

                try
                {
                    _context.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Exception Handling");

                    // Make some adjustments.
                    // ...
                    // Try again.
                    //_context.SubmitChanges();
                }

                RefreshFoodPlanTemplates();
                if (rIndex >= 0) SelectedFoodPlanTemplate = FoodPlanTemplates[rIndex];
                else SelectedFoodPlanTemplate = null;
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

                _context.Recipes.DeleteOnSubmit(SelectedRecipe);

                // foreach () in Detailtable --> DeleteOnSubmit DetailEntry
                if (RecipeIngredients != null)
                {
                    foreach (RecipeIngredient ToBeDeletedRecipeIngredient in RecipeIngredients)
                    {
                        _context.RecipeIngredients.DeleteOnSubmit(ToBeDeletedRecipeIngredient);
                    }

                }
                try
                {
                    _context.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Exception Handling");
                    MessageText = "Recipe " + SelectedRecipe.Name + " deleted";

                    // Make some adjustments.
                    // ...
                    // Try again.
                    //_context.SubmitChanges();
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


                _context.RecipeIngredients.DeleteOnSubmit(SelectedRecipeIngredient);

            try
            {
                _context.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                MessageText = "RecipeIngredient " + SelectedRecipeIngredient.Ingredient.Name + " NOT deleted";

                    // Make some adjustments.
                    // ...
                    // Try again.
                    //_context.SubmitChanges();
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
                _context.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
            RefreshRecipes();
            StatusBarMessage = "All Saved";
        }  
        public void ExecuteLoadFoodPlanTemplate(object o)
        {
            ObservableCollection<FoodPlanTemplateItem> FoodPlanItemsToBeLoaded;

            if (SelectedFoodPlanTemplate != null)
            {
                FoodPlanItemsToBeLoaded = GetFoodPlanTemplateItems(SelectedFoodPlanTemplate);

                if (FoodPlanItemsToBeLoaded!=null && FoodPlanItemsToBeLoaded.Count()>0)
                {
                    foreach (FoodPlanTemplateItem FPTItem in FoodPlanItemsToBeLoaded)
                    {
                        FoodPlanItem NewFoodPlanItem = new FoodPlanItem();
                        NewFoodPlanItem.DateTime = SelectedFoodPlanDate + (FPTItem.DateTime - SelectedFoodPlanTemplate.StartDate);
                        NewFoodPlanItem.PortionCount = FPTItem.PortionCount;
                        NewFoodPlanItem.Recipe = FPTItem.Recipe;
                        _context.FoodPlanItems.InsertOnSubmit(NewFoodPlanItem);
                        try
                        {
                            _context.SubmitChanges();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString(), "Exception Handling");
                            // Make some adjustments.
                            // ...
                            // Try again.
                            //_context.SubmitChanges();
                        }
                        RefreshFoodPlanItems();
                        StatusBarMessage = "FoodPlanTemplate Loaded: ";
                    }
                }
            }
            else StatusBarMessage = "Select Food Plan Template first";
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

            _context.FoodPlanItems.InsertOnSubmit(NewFoodPlanItem);

            try
            {
                _context.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
            RefreshFoodPlanItems();
            StatusBarMessage = "FoodPlanItem Added: " + SelectedFoodPlanItem.Recipe.Name;
        }
        public void ExecuteNewFoodPlanTemplate(object o)
        {
            if (FoodPlanItems != null)
            {
                ObservableCollection<FoodPlanItem> FoodPlanItemsInRange = new ObservableCollection<FoodPlanItem>();

                /// hier kommt die Exception
                FoodPlanItemsInRange = new ObservableCollection<FoodPlanItem>(_context.FoodPlanItems
                                          .Where(FPI => (FPI.DateTime.Date >= SelectedFoodPlanDate.Date)
                                                     && (FPI.DateTime.Date <= SelectedFoodPlanTemplateEndDate)));
                if (FoodPlanItemsInRange.Count() > 0)
                {

                    FoodPlanTemplate NewFoodPlanTemplate = new FoodPlanTemplate();

                    NewFoodPlanTemplate.Name = "<New>" + DateTime.Now;
                    NewFoodPlanTemplate.StartDate = SelectedFoodPlanDate.Date;

                    SelectedFoodPlanTemplate = NewFoodPlanTemplate;

                    _context.FoodPlanTemplates.InsertOnSubmit(NewFoodPlanTemplate);

                    try
                    {
                        _context.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Exception Handling");
                    }

                    foreach (FoodPlanItem FPIinRange in FoodPlanItemsInRange)
                    {
                        FoodPlanTemplateItem NewFoodPlanTemplateItem = new FoodPlanTemplateItem();
                        NewFoodPlanTemplateItem.FoodPlanTemplate = SelectedFoodPlanTemplate;
                        NewFoodPlanTemplateItem.DateTime = FPIinRange.DateTime;
                        NewFoodPlanTemplateItem.PortionCount = FPIinRange.PortionCount;
                        NewFoodPlanTemplateItem.Recipe = FPIinRange.Recipe;
                    }
                    try
                    {
                        _context.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString(), "Exception Handling");
                    }

                    RefreshFoodPlanTemplates();

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

            _context.FoodPlanItems.InsertOnSubmit(NewFoodPlanItem);

            try
            {
                _context.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
            RefreshFoodPlanItems();
            */
            StatusBarMessage = "FoodPlanTemplate Added: ";// + SelectedFoodPlanItem.Recipe.Name;
        }
        public void ExecuteNewRecipe(object o)
        {
            Recipe NewRecipe = new Recipe();
            NewRecipe.Name = "<Recipe>: New " + DateTime.Now.ToString();
            _context.Recipes.InsertOnSubmit(NewRecipe);
            
            try
            {
                _context.SubmitChanges();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
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


    
    