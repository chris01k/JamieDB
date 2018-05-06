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
        [FlagsAttribute]
        enum UnitChanges : short
        {
            ucNone = 0,
            ucID = 1,
            ucName = 2,
            ucType = 4,
            ucTypeStd = 8,
            ucTypeFactor = 16,
            ucTypeUniv = 32
        };

        #region Attributes
        private Unit _LastSelectedUnit;
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
            RefreshUnitTranslations();
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
                    UnitChanges SelectedUnitChanges = EvaluateUnitChanges(LastSelectedUnit, SelectedUnit);

                    if (ValidateSelected(_SelectedUnit))
                    {

                        if (SelectedUnitChanges.HasFlag(UnitChanges.ucTypeStd))
                            {
                            if (SelectedUnit.TypeStandard) RecalculateNewStandardUnitFactors(SelectedUnit);
                            else ReverseUnitChanges(LastSelectedUnit, SelectedUnit, SelectedUnitChanges);
                            }



                    }
                    else ReverseUnitChanges(LastSelectedUnit, SelectedUnit, SelectedUnitChanges);

                    _SelectedUnit = value;
                    if (value != null) LastSelectedUnit = value.Clone();
                    else LastSelectedUnit = value;

                    OnPropertyChanged("SelectedUnit");
                    StatusBarMessage = "New Selected Unit = " + SelectedUnit;
                    context.SubmitChanges();
                    StatusBarMessage = "Änderungen gespeichert";
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
        private UnitChanges EvaluateUnitChanges(Unit U1, Unit U2)
        {
            UnitChanges ReturnValue = UnitChanges.ucNone;

            if (U1 != null && U2 != null)
            {
                if (U1.Id != U2.Id) ReturnValue |= UnitChanges.ucID;
                if (U1.Name != U2.Name) ReturnValue |= UnitChanges.ucName;
                if (U1.TypeID != U2.TypeID) ReturnValue |= UnitChanges.ucType;
                if (U1.TypeStandard != U2.TypeStandard) ReturnValue |= UnitChanges.ucTypeStd;
                if (U1.TypeFactor != U2.TypeFactor) ReturnValue |= UnitChanges.ucTypeFactor;
                if (U1.TypeUniversal != U2.TypeUniversal) ReturnValue |= UnitChanges.ucTypeUniv;
            }
            return ReturnValue;
        }
        private void RefreshUnits()
        {
            var result = context.Units.OrderBy(u => u.Symbol);

            Units = new ObservableCollection<Unit>(result);
            Units.CollectionChanged += new NotifyCollectionChangedEventHandler(UnitsChanged);
            SelectedUnit = Units.FirstOrDefault();

        }
        private void RefreshUnitTranslations()
        {
            var result = context.UnitTranslations.ToList();
            if (result.Count()!=0)

            {
                UnitTranslations = new ObservableCollection<UnitTranslation>(result);
                SelectedUnitTranslation = UnitTranslations.FirstOrDefault();
            }
        }
        private void RefreshUnitTypes()
        {
            var result = context.UnitTypes.OrderBy(u => u.Name);
            UnitTypes = new ObservableCollection<UnitType>(result);
        }
        private void ReverseUnitChanges(Unit OldUnit, Unit NewUnit, UnitChanges uc)
        {
            if (uc.HasFlag(UnitChanges.ucID)) NewUnit.Id = OldUnit.Id;
            if (uc.HasFlag(UnitChanges.ucName)) NewUnit.Name = OldUnit.Name;
            if (uc.HasFlag(UnitChanges.ucType)) NewUnit.TypeID = OldUnit.TypeID;
            if (uc.HasFlag(UnitChanges.ucTypeStd)) NewUnit.TypeStandard = OldUnit.TypeStandard;
            if (uc.HasFlag(UnitChanges.ucTypeFactor)) NewUnit.TypeFactor = OldUnit.TypeFactor;
            if (uc.HasFlag(UnitChanges.ucTypeUniv)) NewUnit.TypeUniversal = OldUnit.TypeUniversal;
        }
        public void SomeMethod()
        {

            DateTime RightNow = DateTime.Now;
            string NewText;

            NewText = "Hier geben wir einen Text aus: " + RightNow;

            StatusBarMessage = NewText;

        }
        private void RecalculateNewStandardUnitFactors(Unit NewStandardUnit)
        {

            Unit OldStandard = Units.Where(u => (u.TypeStandard && (u.UnitType.Id == NewStandardUnit.UnitType.Id)) && (u.Id !=NewStandardUnit.Id) ).FirstOrDefault();

            if (OldStandard != null)
            {
                if (NewStandardUnit.TypeFactor!=null && NewStandardUnit.TypeFactor != 0)
                {
                    double? Umrechnung = 1 / NewStandardUnit.TypeFactor;

                    var result = Units.Where(u => (u.UnitType.Id == NewStandardUnit.UnitType.Id) && u.TypeUniversal).ToList();

                    foreach (Unit u in result)
                    {
                        if (u.Id == NewStandardUnit.Id)
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
                }
            }
        }
        private bool ValidateSelected(Unit NewSelectedUnit)
        {
            bool ReturnValue = false;

            if (NewSelectedUnit == null) ReturnValue = true;
            else //NewSelectedUnit != null
            {
                if (NewSelectedUnit.TypeUniversal)
                {
                    if (NewSelectedUnit.TypeFactor > 0) ReturnValue = true;
                    else StatusBarMessage = "Umrechnungsfaktor sollte größer als 0 sein!";
                }
                else // !NewSelectedUnit.TypeUniversal
                {
                    if (NewSelectedUnit.TypeFactor == 0 && !NewSelectedUnit.TypeStandard) ReturnValue = true;
                    else
                    {
                        StatusBarMessage = "Nicht universelle Units müssen einen Faktor 0 haben dürfen nicht Standard sein.";
                    }
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
        public double GetSpecificTranslationFactor(Unit BaseUnit, Unit TargetUnit, ObservableCollection<UnitTranslation> RelevantTranslations)
        {
            double ReturnValue = 0;
            UnitTranslation WorkingTranslation;

            WorkingTranslation = RelevantTranslations.Where(it => it.Factor != 0 && 
                                                                 ((it.Unit == BaseUnit) && (it.Unit1 == TargetUnit) || 
                                                                  (it.Unit1 == BaseUnit) && (it.Unit == TargetUnit))).FirstOrDefault();
            if (WorkingTranslation !=null) ReturnValue = (WorkingTranslation.Unit == BaseUnit ? WorkingTranslation.Factor : 1 / WorkingTranslation.Factor);

            return ReturnValue;
        }
        public double GetTranslationFactor(Ingredient i, Unit BaseUnit, Unit TargetUnit)
        {
            double ReturnValue=0;
            ObservableCollection<UnitTranslation> IngredientRelatedTranslations;
            UnitTranslation[] WorkingTranslation = new UnitTranslation [2];
            
            int CaseSelector=0;

            IngredientRelatedTranslations = IngredientUnitTranslations(i);

            ReturnValue = GetSpecificTranslationFactor(BaseUnit, TargetUnit, IngredientRelatedTranslations); // Ein direkter Versuch....

            if (ReturnValue == 0)
            {
                CaseSelector += (BaseUnit.TypeUniversal ? 0 : 1);
                CaseSelector += (TargetUnit.TypeUniversal ? 0 : 2);
                CaseSelector += (BaseUnit.UnitType == TargetUnit.UnitType ? 0 : 4);

                switch (CaseSelector)
                {
                    case 0: // BaseUnit == universell, TargetUnit == universell, Type1 == Type2
                        ReturnValue = BaseUnit.TypeFactor ?? 0;
                        ReturnValue = (((TargetUnit.TypeFactor ?? 0) == 0) ? 0 : ReturnValue /= (TargetUnit.TypeFactor ?? 1));
                        break;
                    case 1: // BaseUnit != universell, TargetUnit == universell, Type1 == Type2
                        WorkingTranslation[0] = GetTranslationToType(i, BaseUnit, BaseUnit.UnitType, true, IngredientRelatedTranslations);
                        if  (WorkingTranslation[0] != null ) ReturnValue = WorkingTranslation[0].Factor * GetTranslationFactor(i, WorkingTranslation[0].Unit1, TargetUnit);
                        break;
                    case 2: // BaseUnit == universell, TargetUnit != universell, Type1 == Type2
                        WorkingTranslation[0] = GetTranslationToType(i, TargetUnit, TargetUnit.UnitType, true, IngredientRelatedTranslations);
                        if (WorkingTranslation[0] != null) ReturnValue = GetTranslationFactor(i, BaseUnit, WorkingTranslation[0].Unit1) / WorkingTranslation[0].Inverse().Factor;
                        break;
                    case 3: // BaseUnit != universell, TargetUnit != universell, Type1 == Type2
                        WorkingTranslation[0] = GetTranslationToType(i, BaseUnit, TargetUnit.UnitType, true, IngredientRelatedTranslations);
                        WorkingTranslation[1] = GetTranslationToType(i, TargetUnit, TargetUnit.UnitType, true, IngredientRelatedTranslations);
                        if (WorkingTranslation[0]!= null && WorkingTranslation[1] != null)
                        {
                            ReturnValue = WorkingTranslation[0].Factor * 
                                          GetTranslationFactor(i, WorkingTranslation[0].Unit1, WorkingTranslation[1].Unit1) * 
                                          WorkingTranslation[1].Inverse().Factor;
                        }
                        break;
                    case 4: // BaseUnit == universell, TargetUnit == universell, Type1 != Type2
                        // Erster Versuch ...
                        WorkingTranslation[0] = GetTranslationUniversalTypeToType(i, BaseUnit.UnitType, TargetUnit.UnitType, IngredientRelatedTranslations);
                        if (WorkingTranslation[0] != null) ReturnValue = GetTranslationFactor(i, BaseUnit, WorkingTranslation[0].Unit) *
                                                                         WorkingTranslation[0].Factor *
                                                                         GetTranslationFactor(i, WorkingTranslation[0].Unit1, TargetUnit);
                        if (ReturnValue == 0) // Zweiter Versuch durch Einbindung der Nichtuniversellen Translations.
                        {

                        }
                        break;
                    case 5: // BaseUnit != universell, TargetUnit == universell, Type1 != Type2
                            // Getestet Butter EL --> g --> kg OK
                        WorkingTranslation[0] = GetTranslationToType(i, BaseUnit, BaseUnit.UnitType, true, IngredientRelatedTranslations);   // Erster Versuch
                        if (WorkingTranslation[0] == null) WorkingTranslation[0] = GetTranslationToType(i, BaseUnit, TargetUnit.UnitType, true, IngredientRelatedTranslations); // Zweiter Versuch
                        if (WorkingTranslation[0] != null) ReturnValue = WorkingTranslation[0].Factor * GetTranslationFactor(i, WorkingTranslation[0].Unit1, TargetUnit);
                        break;
                    case 6: // BaseUnit == universell, TargetUnit != universell, Type1 != Type2
                        WorkingTranslation[0] = GetTranslationToType(i, TargetUnit, TargetUnit.UnitType, true, IngredientRelatedTranslations);   // Erster Versuch
                        if (WorkingTranslation[0] == null) WorkingTranslation[0] = GetTranslationToType(i, TargetUnit, BaseUnit.UnitType, true, IngredientRelatedTranslations); // Zweiter Versuch
                        if (WorkingTranslation[0] != null) ReturnValue = GetTranslationFactor(i, BaseUnit, WorkingTranslation[0].Unit1) * WorkingTranslation[0].Inverse().Factor;
                        break;
                    case 7: // BaseUnit != universell, TargetUnit != universell, Type1 != Type2
                        WorkingTranslation[0] = GetTranslationToType(i, BaseUnit, BaseUnit.UnitType, true, IngredientRelatedTranslations); // Erster Versuch
                        WorkingTranslation[1] = GetTranslationToType(i, TargetUnit, TargetUnit.UnitType, true, IngredientRelatedTranslations);
                        if (WorkingTranslation[0] == null || WorkingTranslation[1] == null) // Zweiter Versuch
                        {
                            WorkingTranslation[0] = GetTranslationToType(i, BaseUnit, TargetUnit.UnitType, true, IngredientRelatedTranslations);
                            WorkingTranslation[1] = GetTranslationToType(i, TargetUnit, TargetUnit.UnitType, true, IngredientRelatedTranslations);
                        }
                        if (WorkingTranslation[0] != null && WorkingTranslation[1] != null) ReturnValue = WorkingTranslation[0].Factor *
                                                                                                          GetTranslationFactor(i, WorkingTranslation[0].Unit1, WorkingTranslation[1].Unit) *
                                                                                                          WorkingTranslation[1].Inverse().Factor;
                        break;
                    default:
                        ReturnValue = 0;
                        break;
                }

            }
            return ReturnValue;
        }



        /*            if (Unit1 != null && Unit2 != null)
                    {
                        if (Unit1.UnitType == Unit2.UnitType)
                        {
                            if (Unit1.TypeUniversal && Unit2.TypeUniversal)
                            {
                                ReturnValue = Unit1.TypeFactor ?? 0;
                                ReturnValue = (((Unit2.TypeFactor ?? 0) == 0) ? 0 : ReturnValue /= (Unit2.TypeFactor ?? 1));
                            }
                            else // Unit1 oder Unit2 (oder beide) sind nicht Universal - aber gleichen Typs
                            {
                                IngredientRelatedTranslations = IngredientUnitTranslations(i);
                            }
                        }
                        else  
                        {
                            if (Unit1.TypeUniversal && Unit2.TypeUniversal) // Unit1 und Unit2 sind universal aber unterschiedliche Typen
                            {
                                IngredientRelatedTranslations = IngredientUnitTranslations(i);
                            }
                            else // Unit1 oder Unit2 (oder beide) sind nicht universal - und unterschiedliche Typen
                            {
                                IngredientRelatedTranslations = IngredientUnitTranslations(i);
                                WorkingTranslation = IngredientRelatedTranslations.Where(it => it.Factor!=0 && ((it.Unit == Unit1) && (it.Unit1 == Unit2) || (it.Unit1 == Unit1) && (it.Unit == Unit2))).FirstOrDefault();
                                if (WorkingTranslation  == null)
                                {

                                }
                                else
                                {
                                    ReturnValue = (WorkingTranslation.Unit == Unit1 ? WorkingTranslation.Factor : 1/ WorkingTranslation.Factor);
                                }
                            }
                        }
                    }

            */

        public UnitTranslation GetTranslationToType(Ingredient i, Unit BaseUnit, UnitType TargetType, bool isUniversal, ObservableCollection<UnitTranslation> RelevantTranslations)
        {
            UnitTranslation ReturnValue = null;
            UnitTranslation WorkingTranslation;

            WorkingTranslation = RelevantTranslations.Where(it => it.Factor != 0 && it.Ingredient==i &&
                                                                (((it.Unit == BaseUnit) && (it.Unit1.UnitType == TargetType) && (it.Unit1.TypeUniversal == isUniversal) ||
                                                                  (it.Unit1 == BaseUnit) && (it.Unit.UnitType == TargetType) && (it.Unit.TypeUniversal == isUniversal)))).FirstOrDefault();

            if (WorkingTranslation != null) ReturnValue = (WorkingTranslation.Unit == BaseUnit ? WorkingTranslation : WorkingTranslation.Inverse());

            return ReturnValue;
        }

        public UnitTranslation GetTranslationUniversalTypeToType(Ingredient i, UnitType BaseType, UnitType TargetType, ObservableCollection<UnitTranslation> RelevantTranslations)
        {
            UnitTranslation ReturnValue = null;
            UnitTranslation WorkingTranslation;

            WorkingTranslation = RelevantTranslations.Where(it => it.Factor != 0 && it.Ingredient == i &&
                                                                (((it.Unit.UnitType == BaseType) && (it.Unit1.UnitType == TargetType) && (it.Unit1.TypeUniversal) ||
                                                                  (it.Unit1.UnitType == BaseType) && (it.Unit.UnitType == TargetType) && (it.Unit.TypeUniversal)))).FirstOrDefault();

            if (WorkingTranslation != null) ReturnValue = (WorkingTranslation.Unit.UnitType == BaseType ? WorkingTranslation : WorkingTranslation.Inverse());

            return ReturnValue;
        }

        public ObservableCollection<UnitTranslation> IngredientUnitTranslations (Ingredient i)
        {
            return (new ObservableCollection<UnitTranslation>(UnitTranslations.Where(u => u.Ingredient == i)));
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
