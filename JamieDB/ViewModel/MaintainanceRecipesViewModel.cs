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

        //private RecipeIngredient _SelectedRecipeIngredient;
        #endregion

        #region Context
        private JamieDBLinqDataContext _context;
        #endregion

        #region Attributes: Commands
        private JamieDBViewModelCommand _NewIngredientCommand;
        private JamieDBViewModelCommand _NewRecipeCommand;
        private JamieDBViewModelCommand _NewRecipeIngredientCommand;
        private JamieDBViewModelCommand _SaveCommand;
        #endregion

        #region Constructors
        public MaintainanceRecipesViewModel()
        {
            _context = new JamieDBLinqDataContext();

            NewIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewIngredient);
            NewRecipeCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipe);
            NewRecipeIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipeIngredient);
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
                if ((value !=null) && (value.Recipe == null))
                { value.RecipeID = SelectedRecipe.Id;
                    value.Recipe = SelectedRecipe;
                }

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

        #region Methods
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

            SelectedRecipe = result.FirstOrDefault();


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
        public void OnPropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        private void RefreshRecipeIngredients()
        {
            RecipeIngredients = GetRecipeIngredients(SelectedRecipe.Id);
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

        #region Command Methods: SaveRecipe
        public bool CanExecuteSaveRecipe(object o)
        {
            return true;
        }
        public void ExecuteSaveRecipe(object o)
        {
            try
            {
                _context.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
        }
        #endregion

        #region Command Methods: NewRecipe
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
                Console.WriteLine(e);
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
        }
        public void ExecuteNewRecipe(object o)
        {
            Recipe NewRecipe = new Recipe();

            NewRecipe.Name = "<Recipe>";

            _context.Recipes.InsertOnSubmit(NewRecipe);

            try
            {
                _context.SubmitChanges();
                Recipes = GetRecipes();
                SelectedRecipe = NewRecipe;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
        }
        #endregion

        #region Command Methods: NewRecipeIngredient
        public void ExecuteNewRecipeIngredient(object o)
        {
            RecipeIngredient NewRecipeIngredient = new RecipeIngredient();

            NewRecipeIngredient.RecipeID = SelectedRecipe.Id;
            NewRecipeIngredient.Recipe = SelectedRecipe;

            _context.RecipeIngredients.InsertOnSubmit(NewRecipeIngredient);

//            try
            {
                _context.SubmitChanges();
                RefreshRecipeIngredients();
            }
/*            catch (Exception e)
            {
                Console.WriteLine(e);
                // Make some adjustments.
                // ...
                // Try again.
                //_context.SubmitChanges();
            }
*/
        }
        #endregion

        #endregion
    }


}
