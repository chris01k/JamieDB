using JamieDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.Specialized;

namespace JamieDB.ViewModel
{
    class MaintainanceRecipesViewModel : INotifyPropertyChanged
    {
        #region Attributes
        private ObservableCollection<FoodPlanItem> _FoodPlanItems;
        private ObservableCollection<FoodPlanTemplate> _FoodPlanTemplates;
        private ObservableCollection<FoodPlanTemplateItem> _FoodPlanTemplateItems;
        private ObservableCollection<Ingredient> _Ingredients;
        private ObservableCollection<IngredientType> _IngredientTypes;
        private ObservableCollection<Recipe> _Recipes;
        private ObservableCollection<RecipeIngredient> _RecipeIngredients;
        private DateTime _SelectedFoodPlanDate;
        private FoodPlanItem _SelectedFoodPlanItem;
        private FoodPlanTemplate _SelectedFoodPlanTemplate;
        private DateTime _SelectedFoodPlanTemplateEndDate;
        private Ingredient _SelectedIngredient;
        private Recipe _SelectedRecipe;
        private RecipeIngredient _SelectedRecipeIngredient;
        private Unit _SelectedUnit;
        private ObservableCollection<ShoppingListItem> _ShoppingListItems;
        private string _StatusBarText;
        private ObservableCollection<Unit> _Units;
        private ObservableCollection<UnitType> _UnitTypes;

        #endregion

        #region Context
        private JamieDBLinqDataContext _context;
        #endregion

        #region Attributes: Commands
        private JamieDBViewModelCommand _DeleteFoodPlanItemCommand;
        private JamieDBViewModelCommand _DeleteFoodPlanTemplateCommand;
        private JamieDBViewModelCommand _DeleteIngredientCommand;
        private JamieDBViewModelCommand _DeleteRecipeCommand;
        private JamieDBViewModelCommand _DeleteRecipeIngredientCommand;
        private JamieDBViewModelCommand _DeleteUnitCommand;
        private JamieDBViewModelCommand _LoadFoodPlanTemplateCommand;
        private JamieDBViewModelCommand _NewFoodPlanItemCommand;
        private JamieDBViewModelCommand _NewFoodPlanTemplateCommand;
        private JamieDBViewModelCommand _NewIngredientCommand;
        private JamieDBViewModelCommand _NewRecipeCommand;
        private JamieDBViewModelCommand _NewRecipeIngredientCommand;
        private JamieDBViewModelCommand _NewUnitCommand;
        private JamieDBViewModelCommand _SaveCommand;
        #endregion

        #region Constructors
        public MaintainanceRecipesViewModel()
        {
            _context = new JamieDBLinqDataContext();

            DeleteFoodPlanItemCommand = new JamieDBViewModelCommand(CanExecuteDeleteFoodPlanItem, ExecuteDeleteFoodPlanItem);
            DeleteFoodPlanTemplateCommand = new JamieDBViewModelCommand(CanExecuteDeleteFoodPlanTemplate, ExecuteDeleteFoodPlanTemplate);
            DeleteIngredientCommand = new JamieDBViewModelCommand(CanExecuteDeleteIngredient, ExecuteDeleteIngredient);
            DeleteRecipeCommand = new JamieDBViewModelCommand(CanExecuteDeleteRecipe, ExecuteDeleteRecipe);
            DeleteRecipeIngredientCommand = new JamieDBViewModelCommand(CanExecuteDeleteRecipeIngredient, ExecuteDeleteRecipeIngredient);
            DeleteUnitCommand = new JamieDBViewModelCommand(CanExecuteDeleteUnit, ExecuteDeleteUnit);

            LoadFoodPlanTemplateCommand = new JamieDBViewModelCommand(CanExecuteLoadFoodPlanTemplate, ExecuteLoadFoodPlanTemplate);

            NewFoodPlanItemCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewFoodPlanItem);
            NewFoodPlanTemplateCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewFoodPlanTemplate);
            NewIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewIngredient);
            NewRecipeCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipe);
            NewRecipeIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipeIngredient);
            NewUnitCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewUnit);
            SaveCommand = new JamieDBViewModelCommand(CanExecuteSaveRecipe, ExecuteSaveRecipe);

            RefreshRecipes();
            SelectedFoodPlanDate = DateTime.Now.Date;
            RefreshFoodPlanTemplates();
            RefreshFoodPlanItems();
            RefreshShoppingListItems();

            Ingredients = GetIngredients();
            IngredientTypes = GetIngredientTypes();
            Units = GetUnits();
            UnitTypes = GetUnitTypes();
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
                _FoodPlanItems = value;
                OnPropertyChanged("FoodPlanItems");
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
                _FoodPlanTemplates = value;
                OnPropertyChanged("FoodPlanTemplates");
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
                _FoodPlanTemplateItems = value;
                OnPropertyChanged("FoodPlanTemplateItems");
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
        public ObservableCollection<Ingredient> Ingredients
        {
            get
            {
                return _Ingredients;
            }

            set
            {
                _Ingredients = value;
                OnPropertyChanged("Ingredients");
            }
        }
        public ObservableCollection<IngredientType> IngredientTypes
        {
            get
            {
                return _IngredientTypes;
            }

            set
            {
                _IngredientTypes = value;
                OnPropertyChanged("IngredientTypes");

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
                _Recipes = value;
                OnPropertyChanged("Recipes");
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
                _RecipeIngredients = value;
                OnPropertyChanged("RecipeIngredients");
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
                _SelectedFoodPlanTemplate = value;
                OnPropertyChanged("SelectedFoodPlanTemplate");
                OnPropertyChanged("SelectedFoodPlanTemplateItemCount");
                DeleteFoodPlanTemplateCommand.OnCanExecuteChanged();
                LoadFoodPlanTemplateCommand.OnCanExecuteChanged();
                StatusBarText = "SelectedFoodPlanTemplate changed: ";
                if ((value != null) && (value.Name != null)) StatusBarText += value.Name;
                else StatusBarText += "null";
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
                _SelectedFoodPlanItem = value;
                OnPropertyChanged("SelectedFoodPlanItem");
                DeleteFoodPlanItemCommand.OnCanExecuteChanged();
                StatusBarText = "SelectedFoodPlanItem changed: ";
                if ((value != null) && (value.Recipe != null)) StatusBarText += value.Recipe.Name;
                else StatusBarText += "null";
            }
        }
        public Ingredient SelectedIngredient
        {
            get
            {
                return _SelectedIngredient;
            }

            set
            {
                _SelectedIngredient = value;
                OnPropertyChanged("SelectedIngredient");
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
                _SelectedRecipe = value;
                OnPropertyChanged("SelectedRecipe");
                DeleteRecipeCommand.OnCanExecuteChanged();
                RefreshRecipeIngredients();
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
                _SelectedRecipeIngredient = value;
                OnPropertyChanged("SelectedRecipeIngredient");
                DeleteRecipeIngredientCommand.OnCanExecuteChanged();
                StatusBarText = "SelectedRecipeIngredient changed: ";
                if ((value != null) && (value.Ingredient!=null)) StatusBarText += value.Ingredient.Name;
                else StatusBarText += "null";
            }
        }
        public Unit SelectedUnit
        {
            get
            {
                return _SelectedUnit;
            }

            set
            {
                _SelectedUnit = value;
                OnPropertyChanged("SelectedUnit");
            }
        }
        public ObservableCollection<ShoppingListItem> ShoppingListItems
        {
            get
            {
                return _ShoppingListItems;
            }
            set
            {
                _ShoppingListItems = value;
                OnPropertyChanged("ShoppingListItems");
            }
        }
        public string StatusBarText
        {
            get
            {
                return _StatusBarText;
            }

            set
            {
                _StatusBarText = value;
                OnPropertyChanged("StatusBarText");

            }
        }
        public ObservableCollection<Unit> Units
        {
            get
            {
                return _Units;
            }

            set
            {
                _Units = value;
                OnPropertyChanged("Units");

            }
        }
        public ObservableCollection<UnitType> UnitTypes
        {
            get
            {
                return _UnitTypes;
            }

            set
            {
                _UnitTypes = value;
                OnPropertyChanged("UnitTypes");
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
        public JamieDBViewModelCommand DeleteIngredientCommand
        {
            get
            {
                return _DeleteIngredientCommand;
            }

            set
            {
                _DeleteIngredientCommand = value;
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
        public JamieDBViewModelCommand NewIngredientCommand
        {
            get
            {
                return _NewIngredientCommand;
            }

            set
            {
                _NewIngredientCommand = value;
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

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region EventHandler
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

                    StatusBarText = "FoodPlanItem Added: "
                    +((SelectedFoodPlanItem == null) ? "null" : SelectedFoodPlanItem.Recipe.Name);

                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (FoodPlanItem FPI in e.OldItems)
                    {
                        _context.FoodPlanItems.DeleteOnSubmit(FPI);
                        StatusBarText = "FoodPlanItem Deleted";
                    }
                }
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

                    StatusBarText = "FoodPlanTemplate Added: " + SelectedFoodPlanTemplate.Name;

                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (FoodPlanTemplate FPT in e.OldItems)
                    {
                        _context.FoodPlanTemplates.DeleteOnSubmit(FPT);
                        StatusBarText = "FoodPlanTemplate Deleted";
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

                    NewIngredientChange.IngredientID = SelectedIngredient.Id;
                    NewIngredientChange.Ingredient = SelectedIngredient;

                    NewIngredientChange.UnitID = SelectedUnit.Id;
                    NewIngredientChange.Unit = SelectedUnit;
                    StatusBarText = "RecipeIngredient Added";

                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (RecipeIngredient RI in e.OldItems)
                    {
                        _context.RecipeIngredients.DeleteOnSubmit(RI);
                        StatusBarText = "RecipeIngredient Deleted";
                    }
                }
            }
       }
        #endregion

        #region Methods
        private Unit DefaultUnit()
        {
            Unit result;

            if (SelectedUnit == null)
            {
                result = Units.FirstOrDefault();
            }
            else result = SelectedUnit;

            return result;
        }
        private Ingredient DefaultIngredient()
        {
            Ingredient result;

            if (SelectedIngredient == null)
            {
                result = Ingredients.FirstOrDefault();
            }
            else result = SelectedIngredient;

            return result;
        }
        private ObservableCollection<Ingredient> GetIngredients()
        {
            var result = _context.Ingredients.OrderBy(i => i.Name);
            var ReturnList = new ObservableCollection<Ingredient>(result);

            SelectedIngredient = result.FirstOrDefault();

            return ReturnList;
        }
        private ObservableCollection<IngredientType> GetIngredientTypes()
        {
            var result = _context.IngredientTypes.OrderBy(i => i.Name);
            var ReturnList = new ObservableCollection<IngredientType>(result);

            return ReturnList;
        }
        private ObservableCollection<Unit> GetUnits()
        {
            var result = _context.Units.OrderBy(u => u.Symbol);
            var ReturnList = new ObservableCollection<Unit>(result);

            SelectedUnit = result.FirstOrDefault();

            return ReturnList;
        }
        private ObservableCollection<UnitType> GetUnitTypes()
        {
            var result = _context.UnitTypes.OrderBy(u => u.Name);
            var ReturnList = new ObservableCollection<UnitType>(result);

            return ReturnList;
        }
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
        public void OnPropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
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

                StatusBarText = "FoodPlanItems refreshed: Selected FoodPlanItem = "
                                + ((SelectedFoodPlanItem == null) ? "null" : SelectedRecipeIngredient.Ingredient.Name);
            }
        }
        private void RefreshFoodPlanTemplates()
        {

            FoodPlanTemplates = new ObservableCollection<FoodPlanTemplate>(_context.FoodPlanTemplates);
            FoodPlanTemplates.CollectionChanged += new NotifyCollectionChangedEventHandler(FoodPlanTemplateChanged);


                StatusBarText = "FoodPlanTemplates refreshed: Selected FoodPlanTemplate = "
                                + ((SelectedFoodPlanTemplate == null) ? "null" : SelectedFoodPlanTemplate.Name);
        }
        private void RefreshIngredients()
        {
            Ingredients = GetIngredients();
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
                StatusBarText = "RecipeIngredients refreshed: Selected RecipeIngredient = "
                                + ((SelectedRecipeIngredient == null)?"null":SelectedRecipeIngredient.Ingredient.Name);
            }
        }
        private void RefreshRecipes()
        {
            var result = _context.Recipes.OrderBy(r => r.Name);

            Recipes = new ObservableCollection<Recipe>(result);

            if (SelectedRecipe == null) SelectedRecipe = result.FirstOrDefault();

        }
        private void RefreshShoppingListItems()
        {
            var result = _context.ShoppingListItems
                                 .OrderBy(sli => sli.Ingredient)
                                 .ThenBy(sli=>sli.FoodPlanDate.Date)
                                 .ThenBy(sli=>sli.FoodPlanDate.TimeOfDay);

            ShoppingListItems = new ObservableCollection<ShoppingListItem>(result);

        }
        private void RefreshUnits()
        {
            Units = GetUnits();
        }
        #endregion
        #region Command Methods

        #region Command Methods: Generic
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
        public bool CanExecuteDeleteFoodPlanItem(object o)
        {
            return (SelectedFoodPlanItem != null);
        }
        public bool CanExecuteDeleteFoodPlanTemplate(object o)
        {
            return (SelectedFoodPlanTemplate != null);
        }
        public bool CanExecuteDeleteIngredient(object o)
        {
            return (SelectedIngredient != null);
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
            return (SelectedUnit != null);
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
                StatusBarText = MessageText;

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
                StatusBarText = MessageText;
            }

        }
        public void ExecuteDeleteIngredient(object o)
        {
            string MessageText;

            if (SelectedIngredient != null)
            {
                var rIndex = Ingredients.IndexOf(SelectedIngredient);
                if (rIndex == Ingredients.Count() - 1) rIndex -= 1;
                MessageText = "Ingredient " + SelectedIngredient.Name + " deleted";


                _context.Ingredients.DeleteOnSubmit(SelectedIngredient);

                // foreach () in Detailtable --> DeleteOnSubmit DetailEntry

                try
                {
                    _context.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Exception Handling");
                    MessageText = "Ingredient " + SelectedIngredient.Name + " NOT deleted";
                    // Make some adjustments.
                    // ...
                    // Try again.
                    //_context.SubmitChanges();
                }

                RefreshIngredients();
                if (rIndex >= 0) SelectedIngredient = Ingredients[rIndex];
                else SelectedIngredient = null;
                StatusBarText = MessageText;
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
                StatusBarText = MessageText;
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
                StatusBarText = MessageText;

            }

        }
        public void ExecuteDeleteUnit(object o)
        {
            string MessageText;

            if (SelectedUnit != null)
            {
                var rIndex = Units.IndexOf(SelectedUnit);
                if (rIndex == Units.Count() - 1) rIndex -= 1;
                MessageText = "Unit " + SelectedUnit.Symbol + " deleted";

                _context.Units.DeleteOnSubmit(SelectedUnit);

                // foreach () in Detailtable --> DeleteOnSubmit DetailEntry

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
                    MessageText = "Unit " + SelectedUnit.Symbol + " NOT deleted";
                }

                RefreshUnits();
                if (rIndex >= 0) SelectedUnit = Units[rIndex];
                else SelectedUnit = null;
                StatusBarText = MessageText;
            }

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
            StatusBarText = "All Saved";
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
                        StatusBarText = "FoodPlanTemplate Loaded: ";
                    }
                }
            }
            else StatusBarText = "Select Food Plan Template first";
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
            StatusBarText = "FoodPlanItem Added: " + SelectedFoodPlanItem.Recipe.Name;
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
                else StatusBarText = "No FoodPlanItems in Range";
            }
            else StatusBarText = "FoodPlanItems is null"; 




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
            StatusBarText = "FoodPlanTemplate Added: ";// + SelectedFoodPlanItem.Recipe.Name;
        }
        public void ExecuteNewIngredient(object o)
        {
            Ingredient NewIngredient = new Ingredient();

            NewIngredient.Name = "<Ingredient>";
            NewIngredient.TargetUnitID = SelectedUnit.Id;
            NewIngredient.IngredientType = IngredientTypes.FirstOrDefault();

            _context.Ingredients.InsertOnSubmit(NewIngredient);

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
            Ingredients = GetIngredients();
            SelectedIngredient = NewIngredient;
            StatusBarText = "Ingredient Added";
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
            StatusBarText = "Recipe Added";
        }
        public void ExecuteNewRecipeIngredient(object o)
        {
            RecipeIngredient NewRecipeIngredient = new RecipeIngredient();

            NewRecipeIngredient.Recipe = SelectedRecipe;
            NewRecipeIngredient.Unit = DefaultUnit();
            NewRecipeIngredient.Ingredient = DefaultIngredient();

            _context.RecipeIngredients.InsertOnSubmit(NewRecipeIngredient);

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
            StatusBarText = "RecipeIngredient Added";
        }
        public void ExecuteNewUnit(object o)
        {
            Unit NewUnit = new Unit();

            NewUnit.Name = "<Unit>: New " + DateTime.Now.ToString();
            NewUnit.Symbol = "new";
            NewUnit.TypeID = 1000004;

            _context.Units.InsertOnSubmit(NewUnit);

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
            Units = GetUnits();
            SelectedUnit = NewUnit;
            StatusBarText = "Unit Added";
        }
        #endregion
    }


}
