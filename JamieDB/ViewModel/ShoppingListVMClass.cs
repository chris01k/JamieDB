using JamieDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamieDB.ViewModel
{
    class ShoppingListsVMClass: JamieDBViewModelBase
    {
        #region Attributes
        //        private ObservableCollection<ShoppingList> _ShoppingLists;
        //        private ShoppingList _SelectedShoppingList;
        private DateTime _TimeFrameStart;
        private DateTime _TimeFrameEnd;
        private ObservableCollection<ShoppingListItem> _TimeFrameShoppingListItems;
        private UnitTranslator _Translator;



        #region Attributes: Commands
        #endregion
        #endregion

        #region Constructors
        public ShoppingListsVMClass()
        {
            _Translator = new UnitTranslator();
            TimeFrameStart = DateTime.Now;
            RefreshShoppingLIstItems();
        }
        #endregion

        #region Events
        #region Events:EventHandler
        #endregion
        #endregion

        #region Properties
        public DateTime TimeFrameStart
        {
            get
            {
                return _TimeFrameStart;
            }

            set
            {
                if (_TimeFrameStart != value)
                {
                    _TimeFrameStart = value;
                    OnPropertyChanged("TimeFrameStart");
                    if (TimeFrameEnd ==null || value >= TimeFrameEnd) TimeFrameEnd = value + new TimeSpan(14, 0, 0, 0);
                    RefreshShoppingLIstItems();
                }
            }
        }
        public DateTime TimeFrameEnd
        {
            get
            {
                return _TimeFrameEnd;
            }

            set
            {
                if (_TimeFrameEnd != value)
                {
                    _TimeFrameEnd = value;
                    OnPropertyChanged("TimeFrameEnd");
                    RefreshShoppingLIstItems();
                }
            }
        }
        public ObservableCollection<ShoppingListItem> TimeFrameShoppingListItems
        {
            get
            {
                return _TimeFrameShoppingListItems;
            }

            set
            {
                if (_TimeFrameShoppingListItems != value)
                {
                    _TimeFrameShoppingListItems = value;
                    OnPropertyChanged("TimeFrameShoppingListItems");
                }
            }
        }

        #region Properties: Commands
        #endregion
        #endregion

        #region Methods
        public void RefreshShoppingLIstItems()
        {
            if (TimeFrameStart != null && TimeFrameEnd != null)
            {
                var ResultList = context.FoodPlanItems.Where(fpi => fpi.DateTime >= TimeFrameStart && fpi.DateTime <= TimeFrameEnd)
                                        .Join(context.Recipes, FoodPlanItem => FoodPlanItem.RecipeID, Recipe => Recipe.Id, (FoodPlanItem, Recipe) => new { FoodPlanItem, Recipe })
                                        .Join(context.RecipeIngredients, FPIRecipes => FPIRecipes.Recipe.Id, RecipeIngredient => RecipeIngredient.RecipeID, (FPIRecipes, RecipeIngredients) =>

                                      new {

                                          FoodPlanItem = FPIRecipes.FoodPlanItem,
                                          Recipe = FPIRecipes.FoodPlanItem.Recipe,
                                          RecipeIngredient = RecipeIngredients,
                                          Ingredient = RecipeIngredients.Ingredient,

                                          FoodPlanPortions = FPIRecipes.FoodPlanItem.PortionCount,
                                          RecipePortionQuantity = FPIRecipes.FoodPlanItem.Recipe.PortionQuantity,
                                          RecipeName = FPIRecipes.FoodPlanItem.Recipe.Name,
                                          RecipeSource = FPIRecipes.FoodPlanItem.Recipe.Source,
                                          RecipePage = FPIRecipes.FoodPlanItem.Recipe.SourcePage,
                                          RecipeIngredientQuantity = RecipeIngredients.Quantity,

                                          RSource = FPIRecipes.Recipe.Source,
                                          RIngredient = RecipeIngredients.Ingredient.Name,
                                          OriginalQuantity = RecipeIngredients.Quantity,
                                          Quantity = (RecipeIngredients.Quantity) * (FPIRecipes.FoodPlanItem.PortionCount) / (FPIRecipes.Recipe.PortionQuantity),
                                          RecipeUnit = RecipeIngredients.Unit,
                                          IngredientUnit = RecipeIngredients.Ingredient.Unit,
                                      }).OrderBy(s => s.FoodPlanItem.DateTime).OrderBy(s => s.RIngredient).ToList();
                ;
                TimeFrameShoppingListItems = new ObservableCollection<ShoppingListItem>();
                foreach(var r in ResultList)
                {
                    ShoppingListItem s = new ShoppingListItem();
                    s.FoodPlanItem = r.FoodPlanItem;
                    s.FoodPlanItemID = r.FoodPlanItem.Id;
                    s.Ingredient = r.Ingredient;
                    s.IngredientID = r.Ingredient.Id;
                    s.RecipeIngredient = r.RecipeIngredient;
                    s.RecipeIngredientID = r.RecipeIngredient.Id;
                    s.Recipe = r.Recipe;
                    s.RecipeID = r.Recipe.Id;
                    s.TargetUnitQuantity = _Translator.GetTranslationFactor(r.Ingredient, r.RecipeIngredient.Unit, r.Ingredient.Unit) * r.RecipeIngredient.Quantity * r.FoodPlanItem.PortionCount / r.Recipe.PortionQuantity;
                    TimeFrameShoppingListItems.Add(s);
                }
            }
        }

        #region Methods: Command Methods
        #region Methods: Generic Command Methods
        #endregion
        #endregion
        #endregion




    }
}
