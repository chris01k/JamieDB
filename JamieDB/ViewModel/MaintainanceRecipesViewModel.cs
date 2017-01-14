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

        //Constructors
        public MaintainanceRecipesViewModel()
        {
            _Recipes = GetRecipes();
            if (_Recipes.Count() != 0)
            {
                SelectedRecipe = _Recipes.First();
                _RecipeIngredients = GetRecipeIngredients(_SelectedRecipe.Id);
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
            }
        }

        //Methods
        private IEnumerable<RecipeIngredient> GetRecipeIngredients(long RecipeID)
        {
            using (var context = new JamieDBLinqDataContext())
            {
                var result = context.RecipeIngredients.Where(ri => ri.RecipeID == RecipeID);
                return result;
            }

        }
        private IEnumerable<Recipe> GetRecipes()
        {
            using (var context = new JamieDBLinqDataContext())
            {
                var result = context.Recipes.ToList();
                return result;
            }

        }
        private void OnPropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        





    }
}
