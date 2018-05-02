using JamieDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JamieDB.ViewModel
{
    class IngredientsVMClass:JamieDBViewModelBase
    {

        #region Attributes
        private ObservableCollection<Ingredient> _Ingredients;
        private ObservableCollection<IngredientType> _IngredientTypes;
        private ObservableCollection<Recipe> _RelatedRecipes;
        private ObservableCollection<UnitTranslation> _RelatedUnitTranslations;
        private Ingredient _SelectedIngredient;


        #endregion

        #region Attributes: Commands
        private JamieDBViewModelCommand _DeleteIngredientCommand;
        private JamieDBViewModelCommand _NewIngredientCommand;



        #endregion

        #region Constructors
        public IngredientsVMClass()
        {
            DeleteIngredientCommand = new JamieDBViewModelCommand(CanExecuteDeleteIngredient, ExecuteDeleteIngredient);
            NewIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewIngredient);
            RefreshIngredients();
            RefreshIngredientTypes();

        }
        #endregion

        #region Properties
        public ObservableCollection<Ingredient> Ingredients
        {
            get
            {
                return _Ingredients;
            }

            set
            {
                if (_Ingredients != value)
                {
                    _Ingredients = value;
                    OnPropertyChanged("Ingredients");
                }
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
                if (_IngredientTypes != value)
                {
                    _IngredientTypes = value;
                    OnPropertyChanged("IngredientTypes");
                }
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
                if (_SelectedIngredient != value)
                {
                    _SelectedIngredient = value;
                    OnPropertyChanged("SelectedIngredient");
                }
            }
        }
        #endregion

        #region Properties: Commands
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

        #endregion

        #region Methods
        private void RefreshIngredients()
        {
            var result = _context.Ingredients.OrderBy(i => i.Name);
            Ingredients = new ObservableCollection<Ingredient>(result);

            SelectedIngredient = result.FirstOrDefault();
        }
        private void RefreshIngredientTypes()
        {
            var result = _context.IngredientTypes.OrderBy(i => i.Name);
            IngredientTypes = new ObservableCollection<IngredientType>(result);
        }
        #endregion

        #region Methods:Command Methods
        public bool CanExecuteDeleteIngredient(object o)
        {
            return (SelectedIngredient != null);
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
                StatusBarMessage = MessageText;
            }

        }
        public void ExecuteNewIngredient(object o)
        {
            Ingredient NewIngredient = new Ingredient();

            NewIngredient.Name = "<Ingredient>";
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
            RefreshIngredients();
            SelectedIngredient = NewIngredient;
            StatusBarMessage = "Ingredient Added";
        }
        #endregion

    }
}
