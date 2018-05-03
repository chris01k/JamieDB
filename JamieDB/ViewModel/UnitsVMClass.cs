using JamieDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JamieDB.ViewModel
{
    class UnitsVMClass : JamieDBViewModelBase
    {

        #region Attributes
        private Unit _LastSelectedUnit;
        private ObservableCollection<MissingTranslation> _MissingTranslations;
        private MissingTranslation _SelectedMissingTranslation;
        private Unit _SelectedUnit;
        private UnitTranslation _SelectedUnitTranslation;
        private ObservableCollection<Unit> _Units;
        private ObservableCollection<UnitTranslation> _UnitTranslations;
        private ObservableCollection<UnitType> _UnitTypes;
        #endregion


        #region Attributes: Commands
        private JamieDBViewModelCommand _DeleteUnitCommand;
        private JamieDBViewModelCommand _DeleteUnitTranslationCommand;
        private JamieDBViewModelCommand _NewUnitCommand;
        private JamieDBViewModelCommand _NewUnitTranslationCommand;
        private JamieDBViewModelCommand _SaveCommand;
        #endregion

        #region Constructors
        public UnitsVMClass()
        {
            /*DeleteUnitCommand = new JamieDBViewModelCommand(CanExecuteDeleteUnit, ExecuteDeleteUnit);
            DeleteUnitTranslationCommand = new JamieDBViewModelCommand(CanExecuteDeleteUnitTranslation, ExecuteDeleteUnitTranslation);

            NewUnitCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewUnit);
            NewUnitTranslationCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewUnitTranslation);

            SaveCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteSaveUnit);

            */
            
            RefreshUnits();
            RefreshUnitTypes();
            RefreshMissingUnitTranslations();
        }
        #endregion

        #region Events
//        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        #region Events:EventHandler
        public void UnitsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add) 
            {
                if (e.NewItems != null)
                {
                    Unit NewUnit = (Unit)e.NewItems[0];

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
 /*                  foreach (RecipeIngredient RI in e.OldItems)
                        {
                        _context.RecipeIngredients.DeleteOnSubmit(RI);
                        StatusBarMessage = "RecipeIngredient Deleted";
                        }
*/
                }
            }

        }
        public void UnitTranslationsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        #endregion


        #region Properties
        public Unit LastSelectedUnit
        {
            get
            {
                return _LastSelectedUnit;
            }

            set
            {
                if (_LastSelectedUnit != value)
                {
                    _LastSelectedUnit = value;
                    OnPropertyChanged("LastSelectedUnit");
                }

            }
        }

        public ObservableCollection<MissingTranslation> MissingTranslations
        {
            get
            {
                return _MissingTranslations;
            }

            set
            {
                if (_MissingTranslations != value)
                {
                    _MissingTranslations = value;
                    OnPropertyChanged("MissingTranslations");
                }
            }
        }
        public MissingTranslation SelectedMissingTranslation
        {
            get
            {
                return _SelectedMissingTranslation;
            }

            set
            {
                if (_SelectedMissingTranslation != value)
                {
                    _SelectedMissingTranslation = value;
                    OnPropertyChanged("SelectedMissingTranslation");
                }

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
                if (_SelectedUnit != value)
                {
                    _SelectedUnit = value;
                    if (ValidateSelectedUnitChanges(LastSelectedUnit,SelectedUnit))
                    {
                        if (value != null) LastSelectedUnit = SelectedUnit.Clone();
                        else LastSelectedUnit = value;
                        OnPropertyChanged("SelectedUnit");
                        StatusBarMessage = "New Selected Unit = " + _SelectedUnit;
                    }
                }
                context.SubmitChanges();
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
                _SelectedUnitTranslation = value;
                OnPropertyChanged("SelectedUnitTranslation");
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
        public ObservableCollection<UnitTranslation> UnitTranslations
        {
            get
            {
                return _UnitTranslations;
            }

            set
            {
                _UnitTranslations = value;
                OnPropertyChanged("UnitTranslations");

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
        public JamieDBViewModelCommand DeleteUnitTranslationCommand
        {
            get
            {
                return _DeleteUnitTranslationCommand;
            }

            set
            {
                _DeleteUnitTranslationCommand = value;
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
        public JamieDBViewModelCommand NewUnitTranslationCommand
        {
            get
            {
                return _NewUnitTranslationCommand;
            }

            set
            {
                _NewUnitTranslationCommand = value;
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

        #region Methods

        public Unit DefaultUnit()
        {
            Unit result;

            if (SelectedUnit == null)
            {
                result = Units.FirstOrDefault();
            }
            else result = SelectedUnit;

            return result;
        }
        private ObservableCollection<UnitTranslation> GetUnitTranslationsInverse()
        {
            var result = context.UnitTranslations.Where(ut => ut.TargetUnitID == SelectedUnit.Id).OrderBy(ut => ut.Unit.Symbol);
            var ReturnList = new ObservableCollection<UnitTranslation>(result);

            return ReturnList;
        }


        private void RefreshMissingUnitTranslations()
        {
            var result = context.RecipeIngredients.Where(ri => ri.UnitID != ri.Ingredient.TargetUnitID);

            ObservableCollection<RecipeIngredient> test = new ObservableCollection<RecipeIngredient>(result);

            MissingTranslations = new ObservableCollection<MissingTranslation>();

            foreach (var r in result)
            {
                if (!(r.Unit.TypeID == r.Ingredient.Unit.TypeID && r.Unit.TypeStandard && r.Unit.TypeStandard))
                {
                    MissingTranslation x = new MissingTranslation();

                    x.AffectedIngredient = r.Ingredient;
                    x.BaseUnit = r.Unit;
                    x.TargetUnit = r.Ingredient.Unit;
                    x.Factor = 0;
                    x.RelatedRecipe = r.Recipe;


                    if (!MissingTranslations.Contains(x)) MissingTranslations.Add(x);

                }

            }


        }

        private void RefreshUnits()
        {
            var result = context.Units.OrderBy(u => u.Symbol);

            Units = new ObservableCollection<Unit>(result);
            Units.CollectionChanged += new NotifyCollectionChangedEventHandler(UnitsChanged);
            SelectedUnit = Units.FirstOrDefault();

        }
        
/*        private void RefreshRelatedUnitTranslations()
        {
            var result = context.UnitTranslations.Where(s => (s.BaseUnitID == SelectedUnit.Id) || (s.TargetUnitID== SelectedUnit.Id));
            if (result.ToList().Count()!=0)

            {
                RelatedUnitTranslations = new ObservableCollection<UnitTranslation>(result);
                RelatedUnitTranslations.CollectionChanged += new NotifyCollectionChangedEventHandler(UnitTranslationsChanged);
                SelectedUnitTranslation = RelatedUnitTranslations.FirstOrDefault();
            }
        }
  */
        private void RefreshUnitTypes()
        {
            var result = context.UnitTypes.OrderBy(u => u.Name);
            UnitTypes = new ObservableCollection<UnitType>(result);
        }

        public void SomeMethod()
        {

            DateTime RightNow = DateTime.Now;
            string NewText;

            NewText = "Hier geben wir einen Text aus: " + RightNow;

            StatusBarMessage = NewText;

        }

        private bool RecalculateFactors(long UTypeID, Unit NewStandardUnit)
        {
            bool ReturnValue = false;




            if (NewStandardUnit.TypeFactor != null)
            {
                double? Umrechnung = 1 / NewStandardUnit.TypeFactor;

                var result = context.Units.Where(u => (u.UnitType.Id == UTypeID) && u.TypeUniversal ).ToList();

                foreach ( Unit u in result)
                {
                    if (u.Id  == NewStandardUnit.Id)
                    {
                        u.TypeStandard = true;
                        u.TypeFactor = 1;
                    }
                    else 
                    {
                        u.TypeStandard = false;
                        u.TypeFactor = u.TypeFactor * Umrechnung;
                    }
                }
                context.SubmitChanges();
                ReturnValue = true;
            }

            return ReturnValue;
            
        }

        private bool ValidateSelectedUnitChanges(Unit OldSelected, Unit NewSelected)
        {
            bool ReturnValue = false;

            if (OldSelected !=null && !OldSelected.TypeStandard)
            {
                if (NewSelected != null && NewSelected.TypeStandard)
                {
                    ReturnValue = RecalculateFactors(OldSelected.TypeID, NewSelected);
                }
            }

            return ReturnValue;



        }
        #endregion
        #region Methods: Command Methods
        public bool CanExecuteDeleteUnit(object o)
        {
            return (SelectedUnit != null);
        }
        public bool CanExecuteDeleteUnitTranslation(object o)
        {
            return (SelectedUnitTranslation != null);
        }
        public bool CanExecuteInitializeBasicUnitTranslations(object o)
        {
            return (context.UnitTypes.Count() > 0);
        }


        public void ExecuteDeleteUnit(object o)
        {
            string MessageText;

            if (SelectedUnit != null)
            {
                var rIndex = Units.IndexOf(SelectedUnit);
                if (rIndex == Units.Count() - 1) rIndex -= 1;
                MessageText = "Unit " + SelectedUnit.Symbol + " deleted";

                context.Units.DeleteOnSubmit(SelectedUnit);

                
                // foreach () in Detailtable --> DeleteOnSubmit DetailEntry

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
                    MessageText = "Unit " + SelectedUnit.Symbol + " NOT deleted";
                }

                //RefreshUnits();
                if (rIndex >= 0) SelectedUnit = Units[rIndex];
                else SelectedUnit = null;
                //StatusBarMessage = MessageText;
            }

        }
/*        public void ExecuteDeleteUnitTranslation(object o)
        {
            string MessageText;

            var rowIndex = RelatedUnitTranslations.IndexOf(SelectedUnitTranslation);
            if (rowIndex == (RelatedUnitTranslations.Count() - 1)) rowIndex -= 1;
            MessageText = "UnitTranslation" + SelectedUnitTranslation.Id + "deleted";

            context.UnitTranslations.DeleteOnSubmit(SelectedUnitTranslation);

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
                MessageText = "UnitTranslation" + SelectedUnitTranslation.Id + "NOT deleted";
            }

            //RefreshUnits();
            if (rowIndex >= 0) SelectedUnitTranslation = RelatedUnitTranslations[rowIndex];
            else SelectedUnitTranslation = null;

            StatusBarMessage = MessageText;

        }
        public void ExecuteInitializeBasicUnitTranslations(object o)
        {
            var result = context.UnitTypes;

            

        }
*/
        public void ExecuteNewUnit(object o)
        {
            Unit NewUnit = new Unit();

            NewUnit.Name = "<Unit>: New " + DateTime.Now.ToString();
            NewUnit.Symbol = "new";
            NewUnit.TypeID = 1000004;

            context.Units.InsertOnSubmit(NewUnit);

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
            //RefreshUnits();
            SelectedUnit = NewUnit;
            StatusBarMessage = "Unit Added";
        }
        public void ExecuteNewUnitTranslation(object o)
        {
            UnitTranslation NewUnitTranslation = new UnitTranslation();

            NewUnitTranslation.Unit = SelectedUnit;
            NewUnitTranslation.Unit1 = SelectedUnit;
            NewUnitTranslation.Factor = 1.0;
            NewUnitTranslation.IsAutomaticCreated = false;
            NewUnitTranslation.IsIngredientDependent = false;
            NewUnitTranslation.IsTypeChange = false;
            NewUnitTranslation.IsOK = true;

            context.UnitTranslations.InsertOnSubmit(NewUnitTranslation);

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
            SelectedUnitTranslation = NewUnitTranslation;
            StatusBarMessage = "UnitTranslation Added";
        }

        public void ExecuteSaveUnit(object o)
        {
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
            //RefreshUnits();
            StatusBarMessage = "All Units Saved";
        }

        #endregion
        #region Command Methods: Generic

        public bool CanAlwaysExecute(object o)
        {
            return true;
        }
        #endregion
    }

    class MissingTranslation : JamieDBViewModelBase, IEquatable<MissingTranslation>
    {
        #region Attributes
        private Ingredient _AffectedIngredient;
        private Unit _BaseUnit;
        private Unit _TargetUnit;
        private double _Factor;
        private Recipe _RelatedRecipe;
        #endregion

        #region Constructors
        public MissingTranslation()
        {
        }
        #endregion

        #region Properties
        
        public Ingredient AffectedIngredient
        {
            get
            {
                return _AffectedIngredient;
            }
            set
            {
                if (_AffectedIngredient != value)
                {
                    _AffectedIngredient = value;
                    OnPropertyChanged("AffectedIngredient");
                    
                }
            }
        }

        public Unit BaseUnit
        {
            get
            {
                return _BaseUnit;
            }
            set
            {
                if (_BaseUnit != value)
                {
                    _BaseUnit = value;
                    OnPropertyChanged("BaseUnit");
                }
            }
        }
        public Unit TargetUnit
        {
            get
            {
                return _TargetUnit;
            }
            set
            {
                if (_TargetUnit != value)
                {
                    _TargetUnit = value;
                    OnPropertyChanged("TargetUnit");
                }
            }
        }
        public double Factor
        {
            get
            {
                return _Factor;
            }
            set
            {
                if (_Factor != value)
                {
                    _Factor = value;
                    OnPropertyChanged("Factor");


                }
            }
        }

        public Recipe RelatedRecipe

        {
            get
            {
                return _RelatedRecipe;
            }
            set
            {
                if (_RelatedRecipe != value)
                {
                    _RelatedRecipe = value;
                    OnPropertyChanged("RelatedRecipe");


                }
            }
        }

        #endregion

        #region Methods

        public bool Equals (MissingTranslation ToBeCompared)
        {
            return (this.BaseUnit.Id == ToBeCompared.BaseUnit.Id && this.TargetUnit.Id == ToBeCompared.TargetUnit.Id && 
                    this.AffectedIngredient.Id == ToBeCompared.AffectedIngredient.Id) ||
                    (this.BaseUnit.Id == ToBeCompared.TargetUnit.Id && this.TargetUnit.Id == ToBeCompared.BaseUnit.Id &&
                    this.AffectedIngredient.Id == ToBeCompared.AffectedIngredient.Id);
        }

        public override string ToString()
        {
            return string.Format("{0}: 1 {1} = {2} {3} " , AffectedIngredient.Name , BaseUnit.Symbol , this.Factor, TargetUnit.Symbol);
        }


        #endregion

    }

    class UnitTranslator:JamieDBViewModelBase
    {

        #region Attributes
        private ObservableCollection<Unit> _Units;
        private ObservableCollection<UnitTranslation> _UnitTranslations;

        #region Attributes: Commands
        #endregion
        #endregion

        #region Constructors
        public UnitTranslator()
        {
            _Units = new ObservableCollection<Unit>(context.Units.ToList());
            _UnitTranslations = new ObservableCollection<UnitTranslation>(context.UnitTranslations.ToList());
        }
        #endregion

        #region Events
        #region Events:EventHandler
        #endregion
        #endregion

        #region Properties
        public ObservableCollection<Unit> Units
        {
            get
            {
                return _Units;
            }
            set
            {
                if (_Units != value)
                {
                    _Units = value;
                    OnPropertyChanged("Units");
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
        #region Properties: Commands
        #endregion
        #endregion

        #region Methods
        public double GetTranslationFactor(Ingredient i, Unit Unit1, Unit Unit2)
        {
            double ReturnValue=0;

///
/// Hier geht es weiter
///            


            return ReturnValue;
        }
        #region Methods: Command Methods
        #region Methods: Generic Command Methods
        #endregion
        #endregion
        #endregion



    }
}


/*
    #region Attributes
    #region Attributes: Commands
    #endregion
    #endregion

    #region Constructors
    #endregion

    #region Events
    #region Events:EventHandler
    #endregion
    #endregion

    #region Properties
    #region Properties: Commands
    #endregion
    #endregion

    #region Methods
    #region Methods: Command Methods
    #region Methods: Generic Command Methods
    #endregion
    #endregion
    #endregion

 */
