using JamieDB.Model;
using JamieDB.ViewModel.Command;
using System;
using System.Collections.Generic;
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
        private Recipe _SelectedRecipe;
        //private RecipeIngredient _SelectedRecipeIngredient;
        private JamieDBLinqDataContext _context;

        //Commands
        private SaveRecipeCommand _SaveRecipeCommand;
        private NewRecipeCommand _NewRecipeCommand;
        private NewRecipeIngredientCommand _NewRecipeIngredientCommand;

        //Constructors
        public MaintainanceRecipesViewModel()
        {
            _context = new JamieDBLinqDataContext();

            _SaveRecipeCommand = new SaveRecipeCommand(this);
            _NewRecipeCommand = new NewRecipeCommand(this);
            _NewRecipeIngredientCommand = new NewRecipeIngredientCommand(this);

            Recipes = GetRecipes();
            Ingredients = GetIngredients();
            Units = GetUnits();
            
            if (Recipes.Count() != 0)
            {
                SelectedRecipe = Recipes.First();
            }
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

        public Recipe SelectedRecipe
        {
            get
            {
                return _SelectedRecipe;
            }

            set
            {
                _SelectedRecipe = value;
                this.OnPropertyChanged("SelectedRecipe");
                RefreshRecipeIngredients();
            }
        }

        //Events
        public event PropertyChangedEventHandler PropertyChanged;

        //Methods
        private IEnumerable<RecipeIngredient> GetRecipeIngredients(long RecipeID)
        {
             var result = _context.RecipeIngredients.Where(ri => ri.RecipeID == RecipeID); //ToList()
            return result;

        }
        private IEnumerable<Recipe> GetRecipes()
        {
            var result = _context.Recipes; //.ToList();
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
            return result;
        }


        private void OnPropertyChanged(string PropertyName)
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


        //Commands
        public SaveRecipeCommand SaveRecipeCommand
        {
            get
            {
                return _SaveRecipeCommand;
            }
        }
        public NewRecipeCommand NewRecipeCommand
        {
            get
            {
                return _NewRecipeCommand;
            }
        }
        public NewRecipeIngredientCommand NewRecipeIngredientCommand
        {
            get
            {
                return _NewRecipeIngredientCommand;
            }
        }



        public void ExecuteSaveRecipe()
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
        public void ExecuteNewRecipe()
        {
            Recipe NewRecipe = new Recipe();

            NewRecipe.Name = "<Neu>";

            _context.Recipes.InsertOnSubmit(NewRecipe);

            try
            {
                _context.SubmitChanges();
                SelectedRecipe = NewRecipe;
                Recipes = GetRecipes();
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
        public void ExecuteNewRecipeIngredient()
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
