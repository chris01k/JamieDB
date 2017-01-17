using JamieDB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamieDB.ViewModel
{
    class MaintainanceRecipesViewModel:INotifyPropertyChanged
    {
        //Attributes
        private IEnumerable<Recipe> _Recipes;
        private IEnumerable<RecipeIngredient> _RecipeIngredients;
        private Recipe _SelectedRecipe;
        private JamieDBLinqDataContext _context;

        //Constructors
        public MaintainanceRecipesViewModel()
        {
            _context = new JamieDBLinqDataContext();
            Recipes = GetRecipes();
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
                return this._Recipes;
            }
            set
            {
                this._Recipes = value;
                this.OnPropertyChanged("Recipes");
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
             var result = _context.RecipeIngredients.Where(ri => ri.RecipeID == RecipeID).ToList();
             return result;

        }
        private IEnumerable<Recipe> GetRecipes()
        {
            var result = _context.Recipes.ToList();
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

    }
}
