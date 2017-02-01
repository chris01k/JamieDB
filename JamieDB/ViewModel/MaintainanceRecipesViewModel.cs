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
        //Attributes
        private IEnumerable<Recipe> _Recipes;
        private IEnumerable<RecipeIngredient> _RecipeIngredients;
        private IEnumerable<Ingredient> _Ingredients;
        private IEnumerable<Unit> _Units;
        private Ingredient _SelectedIngredient;
        private Recipe _SelectedRecipe;
        private RecipeIngredient _SelectedRecipeIngredient;

        //private RecipeIngredient _SelectedRecipeIngredient;
        private JamieDBLinqDataContext _context;

        //Attributes: Commands
        private JamieDBViewModelCommand _SaveRecipeCommand;
        private JamieDBViewModelCommand _NewRecipeCommand;
        private JamieDBViewModelCommand _NewRecipeIngredientCommand;

        //Constructors
        public MaintainanceRecipesViewModel()
        {
            _context = new JamieDBLinqDataContext();

            SaveRecipeCommand = new JamieDBViewModelCommand(CanExecuteSaveRecipe, ExecuteSaveRecipe);
            NewRecipeCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipe);
            NewRecipeIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewRecipeIngredient);

            Recipes = GetRecipes();
            Ingredients = GetIngredients();
            Units = GetUnits();
           
        }

        //Properties
        public IEnumerable<Recipe> Recipes
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
        public IEnumerable<RecipeIngredient> RecipeIngredients
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
        public IEnumerable<Ingredient> Ingredients
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
        public IEnumerable<Unit> Units
        {
            get
            {
                return _Units;
            }

            set
            {
                _Units = value;
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
                _SelectedRecipeIngredient = value;
                OnPropertyChanged("SelectedRecipeIngredient");
            }
        }

        //Properties: Commands
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
        public JamieDBViewModelCommand SaveRecipeCommand
        {
            get
            {
                return _SaveRecipeCommand;
            }

            set
            {
                _SaveRecipeCommand = value;
            }
        }

        //Events
        public event PropertyChangedEventHandler PropertyChanged;

        //Methods
        private IEnumerable<RecipeIngredient> GetRecipeIngredients(long RecipeID)
        {
            var result = _context.RecipeIngredients.Where(ri => ri.RecipeID == RecipeID); //.ToList()
            return result;
        }
        private IEnumerable<Recipe> GetRecipes()
        {
            var result = _context.Recipes;//.ToList();
            SelectedRecipe = result.FirstOrDefault();

            return result;
        }
        private IEnumerable<Unit> GetUnits()
        {
            var result = _context.Units; 
            return result;
        }
        private IEnumerable<Ingredient> GetIngredients()
        {
            var result = _context.Ingredients; //.ToList();
            SelectedIngredient = result.FirstOrDefault();

            return result;
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

        //Command Methods

        //Command Methods: Generic
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

        //Command Methods: SaveRecipe
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

        //Command Methods: NewRecipe
        public void ExecuteNewRecipe(object o)
        {
            Recipe NewRecipe = new Recipe();

            NewRecipe.Name = "<Neu>";

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

        //Command Methods: NewRecipeIngredient
        public void ExecuteNewRecipeIngredient(object o)
        {
            RecipeIngredient NewRecipeIngredient = new RecipeIngredient();

            NewRecipeIngredient.RecipeID = SelectedRecipe.Id;

            _context.RecipeIngredients.InsertOnSubmit(NewRecipeIngredient);

            try
            {
                _context.SubmitChanges();
                RefreshRecipeIngredients();
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

    }


}
