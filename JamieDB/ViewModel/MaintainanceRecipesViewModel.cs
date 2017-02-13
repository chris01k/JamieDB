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
        private ObservableCollection<Recipe> _Recipes;
        private ObservableCollection<RecipeIngredient> _RecipeIngredients;
        private ObservableCollection<Ingredient> _Ingredients;
        private ObservableCollection<IngredientType> _IngredientTypes;
        private ObservableCollection<Unit> _Units;
        private ObservableCollection<UnitType> _UnitTypes;
        private Ingredient _SelectedIngredient;
        private Recipe _SelectedRecipe;
        private RecipeIngredient _SelectedRecipeIngredient;
        private Unit _SelectedUnit;

        private string _StatusBarText;

        //private RecipeIngredient _SelectedRecipeIngredient;
        #endregion

        #region Context
        private JamieDBLinqDataContext _context;
        #endregion

        #region Attributes: Commands
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

            NewIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewIngredient);
            NewRecipeCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipe);
            NewRecipeIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipeIngredient);
            NewUnitCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewUnit);
            SaveCommand = new JamieDBViewModelCommand(CanExecuteSaveRecipe, ExecuteSaveRecipe);

            Recipes = GetRecipes();
            Ingredients = GetIngredients();
            IngredientTypes = GetIngredientTypes();
            Units = GetUnits();
            UnitTypes = GetUnitTypes();
        }
        #endregion

        #region Properties
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
                this.OnPropertyChanged("RecipeIngredients");
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
                GetSelectedRecipeIngredients();
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
        #endregion

        #region Properties: Commands
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
        private ObservableCollection<RecipeIngredient> GetRecipeIngredients(long RecipeID)
        {
            var result = _context.RecipeIngredients.Where(ri => ri.RecipeID == RecipeID); //.ToList()
            var ReturnList = new ObservableCollection<RecipeIngredient>(result);

            return ReturnList;
        }
        private ObservableCollection<Recipe> GetRecipes()
        {
            var result = _context.Recipes.OrderBy(r=>r.Name);
            var ReturnList = new ObservableCollection<Recipe>(result);

            if (SelectedRecipe ==null) SelectedRecipe = result.FirstOrDefault();

            return ReturnList;
        }
        private void GetSelectedRecipeIngredients()
        {
            if (SelectedRecipe != null)
            {
                RecipeIngredients = new ObservableCollection<RecipeIngredient>
                                         (_context.RecipeIngredients.Where(s => (s.RecipeID == SelectedRecipe.Id)));
                if (RecipeIngredients.Count() == 0) SelectedRecipeIngredient = RecipeIngredients.FirstOrDefault();

                if (RecipeIngredients != null) RecipeIngredients.CollectionChanged += 
                                               new System.Collections.Specialized.NotifyCollectionChangedEventHandler(RecipeIngredientChanged);
            }

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
        public void OnPropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        private void RefreshIngredients()
        {
            Ingredients = GetIngredients();
        }
        private void RefreshRecipeIngredients()
        {
            RecipeIngredients = GetRecipeIngredients(SelectedRecipe.Id);
        }
        private void RefreshRecipes()
        {
            Recipes = GetRecipes();
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

        public bool CanExecuteDeleteRecipeIngredient(object o)
        {
            return (SelectedRecipeIngredient != null);
        }
        public bool CanExecuteSaveRecipe(object o)
        {
            return true;
        }
        public void ExecuteDeleteRecipe(object o)
        {
/*

            var rIndex = Recipes.IndexOf(SelectedRecipe);
            if (rIndex == Recipes.Count() - 1) rIndex -= 1;

            foreach (var RI in RecipeIngredients) _context.RecipeIngredients.DeleteOnSubmit(RI);
            _context.Recipes.DeleteOnSubmit(SelectedRecipe);

            ExecuteSaveRecipe(o);

            SelectedRecipe = Recipes[rIndex];

            Recipes = GetRecipes();

            _context.Recipes

    */

        }
        public void ExecuteDeleteRecipeIngredient(object o)
        {
            if (SelectedRecipeIngredient != null)
            {
                var rIndex = RecipeIngredients.IndexOf(SelectedRecipeIngredient);
                if (rIndex == RecipeIngredients.Count() - 1) rIndex -= 1;

                _context.RecipeIngredients.DeleteOnSubmit(SelectedRecipeIngredient);

            try
            {
                _context.SubmitChanges();
                RefreshRecipeIngredients();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }

            }

        }
        public void ExecuteSaveRecipe(object o)
        {
            try
            {
                _context.SubmitChanges();
                RefreshRecipes();
                StatusBarText = "All Saved";


            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
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
                Ingredients = GetIngredients();
                SelectedIngredient = NewIngredient;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
        }
        public void ExecuteNewRecipe(object o)
        {
            Recipe NewRecipe = new Recipe();
            NewRecipe.Name = "<Recipe>: New " + DateTime.Now.ToString();
            _context.Recipes.InsertOnSubmit(NewRecipe);
            
            try
            {
                _context.SubmitChanges();
                Recipes = GetRecipes();
                SelectedRecipe = NewRecipe;
                StatusBarText = "Recipe Added";

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
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
                Units = GetUnits();
                SelectedUnit = NewUnit;
                StatusBarText = "Unit Added";

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Handling");
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
        }
        #endregion
    }


}
