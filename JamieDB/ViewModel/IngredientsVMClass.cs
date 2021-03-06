﻿using JamieDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private ObservableCollection<MissingTranslation> _IngredientTranslations;
        
        private Ingredient _SelectedIngredient;
        private MissingTranslation _SelectedIngredientTranslation;
        private UnitTranslation _SelectedUnitTranslation;
        private UnitTranslator _UnitTranslator;
        private ObservableCollection<UnitTranslation> _UnitTranslations;
        #endregion

        #region Attributes: Commands
        private JamieDBViewModelCommand _DeleteIngredientCommand;
        private JamieDBViewModelCommand _NewIngredientCommand;
        #endregion

        #region Constructors
        public IngredientsVMClass()
        {
            _UnitTranslator = new UnitTranslator();
            DeleteIngredientCommand = new JamieDBViewModelCommand(CanExecuteDeleteIngredient, ExecuteDeleteIngredient);
            NewIngredientCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewIngredient);
            RefreshIngredients();
            RefreshIngredientTypes();
            RefreshUnitTranslations();
        }
        #endregion

        #region Events
        //        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        #region Events:EventHandler
        public void UnitTranslationsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    UnitTranslation NewUnitTranslation = (UnitTranslation)e.NewItems[0];

                    NewUnitTranslation.Ingredient = SelectedIngredient;

                    /*                    var NewIngredientChange = (RecipeIngredient)e.NewItems[0];

                                        //Fill Foreign Keys

                                        NewIngredientChange.RecipeID = SelectedRecipe.Id;
                                        NewIngredientChange.Recipe = SelectedRecipe;

                                        NewIngredientChange.IngredientID = SelectedIngredient.Id;
                                        NewIngredientChange.Ingredient = SelectedIngredient;

                                        NewIngredientChange.UnitID = UnitVM.SelectedUnit.Id;
                                        NewIngredientChange.Unit = UnitVM.SelectedUnit;
                                        StatusBarMessage = "RecipeIngredient Added";
                    */
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {

                    foreach (UnitTranslation RI in e.OldItems)
                    {
                        context.UnitTranslations.DeleteOnSubmit(RI);
                        StatusBarMessage = "RecipeIngredient Deleted";
                    }

                }
            }

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
        public ObservableCollection<MissingTranslation> IngredientTranslations
        {
            get
            {
                return _IngredientTranslations;
            }

            set
            {
                if (_IngredientTranslations != value)
                {
                    _IngredientTranslations = value;
                    OnPropertyChanged("IngredientTranslations");
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
                    OnPropertyChanged("IngredientUnitTranslations");
                    RefreshIngredientUnitTranslations();
                }
            }
        }
        public MissingTranslation SelectedIngredientTranslation
        {
            get
            {
                return _SelectedIngredientTranslation;
            }

            set
            {
                if (_SelectedIngredientTranslation != value)
                {
                    _SelectedIngredientTranslation = value;
                    OnPropertyChanged("SelectedIngredientTranslation");
                    StatusBarMessage = "Selected Ingredient Translation " + value;
                }
            }
        }
        public UnitTranslation SelectedUnitTranslation
        {
            get
            {
                return _SelectedUnitTranslation;
            }

            set
            {
                if (_SelectedUnitTranslation != value)
                {
                    _SelectedUnitTranslation = value;
                    OnPropertyChanged("SelectedUnitTranslation");
                    StatusBarMessage = "Selected UnitTranslation" + value;
                }
            }
        }
        public ObservableCollection<UnitTranslation> UnitTranslations
        {
            get
            {
                return _UnitTranslations;
            }

            set
            {
                if (_UnitTranslations != value)
                {
                    _UnitTranslations = value;
                    OnPropertyChanged("UnitTranslations");
                }
            }
        }
        public ObservableCollection<UnitTranslation> IngredientUnitTranslations
        {
            get
            {
                return new ObservableCollection<UnitTranslation>(_UnitTranslations.Where(u=>u.Ingredient == SelectedIngredient));
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
            var result = context.Ingredients.OrderBy(i => i.Name);
            Ingredients = new ObservableCollection<Ingredient>(result);
            SelectedIngredient = result.FirstOrDefault();
        }
        private void RefreshIngredientTypes()
        {
            var result = context.IngredientTypes.OrderBy(i => i.Name);
            IngredientTypes = new ObservableCollection<IngredientType>(result);
        }
        private void RefreshIngredientUnitTranslations()
        {
            var result = context.RecipeIngredients.Where(ri => ri.Ingredient == SelectedIngredient && (ri.UnitID != ri.Ingredient.TargetUnitID));

            IngredientTranslations = new ObservableCollection<MissingTranslation>();

            foreach (var r in result)
            {
                    MissingTranslation x = new MissingTranslation();

                    x.AffectedIngredient = r.Ingredient;
                    x.BaseUnit = r.Unit;
                    x.TargetUnit = r.Ingredient.Unit;
                    x.Factor = _UnitTranslator.GetTranslationFactor(r.Ingredient,r.Unit, r.Ingredient.Unit);
                    x.RelatedRecipe = r.Recipe;
                    IngredientTranslations.Add(x);
            }
        }
        private void RefreshUnitTranslations()
        {
            UnitTranslations = new ObservableCollection<UnitTranslation>(context.UnitTranslations);
            UnitTranslations.CollectionChanged += new NotifyCollectionChangedEventHandler(UnitTranslationsChanged);
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


                context.Ingredients.DeleteOnSubmit(SelectedIngredient);

                // foreach () in Detailtable --> DeleteOnSubmit DetailEntry

                try
                {
                    context.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Exception Handling");
                    MessageText = "Ingredient " + SelectedIngredient.Name + " NOT deleted";
                    // Make some adjustments.
                    // ...
                    // Try again.
                    //context.SubmitChanges();
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

            context.Ingredients.InsertOnSubmit(NewIngredient);

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
            RefreshIngredients();
            SelectedIngredient = NewIngredient;
            StatusBarMessage = "Ingredient Added";
        }
        #endregion

    }
}
