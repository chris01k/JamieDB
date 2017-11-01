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
    class UnitsVMClass : INotifyPropertyChanged
    {

        #region Attributes
        private Unit _SelectedUnit;
        private UnitTranslation _SelectedUnitTranslation;
        private UnitType _SelectedUnitType;
        private ObservableCollection<Unit> _Units;
        private ObservableCollection<UnitTranslation> _RelatedUnitTranslations;
        private ObservableCollection<UnitType> _UnitTypes;

        private string _StatusBarText;

        #endregion

        #region Attributes: Context
        private JamieDBLinqDataContext _context;
        #endregion

        #region Attributes: Commands
        private JamieDBViewModelCommand _DeleteUnitCommand;
        private JamieDBViewModelCommand _DeleteUnitTranslationCommand;
        private JamieDBViewModelCommand _NewUnitCommand;
        private JamieDBViewModelCommand _NewUnitTranslationCommand;
        private JamieDBViewModelCommand _SaveCommand;
        #endregion

        #region Constructors

        public UnitsVMClass(JamieDBLinqDataContext ctx)
        {
            _context = ctx;

            DeleteUnitCommand = new JamieDBViewModelCommand(CanExecuteDeleteUnit, ExecuteDeleteUnit);
            DeleteUnitTranslationCommand = new JamieDBViewModelCommand(CanExecuteDeleteUnitTranslation, ExecuteDeleteUnitTranslation);

            NewUnitCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewUnit);
            NewUnitTranslationCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteNewUnitTranslation);

            SaveCommand = new JamieDBViewModelCommand(CanAlwaysExecute, ExecuteSaveUnit);


            RefreshUnits();
            RefreshRelatedUnitTranslations();
            RefreshUnitTypes();
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Events:EventHandler
        public void UnitTranslationChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {

                    foreach (UnitTranslation Item in e.NewItems)
                    {
                        //Item.BaseUnitID = SelectedUnit.Id;
                        Item.Unit = SelectedUnit;
                        StatusBarText = Item + "Created";

                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (UnitTranslation Item in e.OldItems)
                    {
                        if (Item.Id != 0)
                        {
                            _context.UnitTranslations.DeleteOnSubmit(Item);
                            _context.SubmitChanges();
                            StatusBarText = Item + "Deleted";
                        }
                    }
                }
            }

        }
        #endregion


        #region Properties
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
                RefreshRelatedUnitTranslations();
                DeleteUnitTranslationCommand.OnCanExecuteChanged();
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
        public UnitType SelectedUnitType
        {
            get
            {
                return _SelectedUnitType;
            }

            set
            {
                _SelectedUnitType = value;
                OnPropertyChanged("SelectedUnitType");
            }
        }
        public string StatusBarText
        {
            get
            {
                return _StatusBarText;
            }

            set
            {
                _StatusBarText = value;
                OnPropertyChanged("StatusBarText");

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
        public ObservableCollection<UnitTranslation> RelatedUnitTranslations
        {
            get
            {
                return _RelatedUnitTranslations;
            }

            set
            {
                _RelatedUnitTranslations = value;
                OnPropertyChanged("RelatedUnitTranslations");

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
            var result = _context.UnitTranslations.Where(ut => ut.TargetUnitID == SelectedUnit.Id).OrderBy(ut => ut.Unit.Symbol);
            var ReturnList = new ObservableCollection<UnitTranslation>(result);

            return ReturnList;
        }
        public void OnPropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        private void RefreshUnits()
        {
            var result = _context.Units.OrderBy(u => u.Symbol);
            Units = new ObservableCollection<Unit>(result);
            SelectedUnit = Units.FirstOrDefault();
        }
        private void RefreshRelatedUnitTranslations()
        {
            var result = _context.UnitTranslations.Where(s => (s.BaseUnitID == SelectedUnit.Id) || (s.TargetUnitID== SelectedUnit.Id));
            RelatedUnitTranslations = new ObservableCollection<UnitTranslation>(result);
            RelatedUnitTranslations.CollectionChanged += new NotifyCollectionChangedEventHandler(UnitTranslationChanged);
            SelectedUnitTranslation = RelatedUnitTranslations.FirstOrDefault();
        }
        private void RefreshUnitTypes()
        {
            var result = _context.UnitTypes.OrderBy(u => u.Name);
            UnitTypes = new ObservableCollection<UnitType>(result);
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
        public void ExecuteDeleteUnit(object o)
        {
            string MessageText;

            if (SelectedUnit != null)
            {
                var rIndex = Units.IndexOf(SelectedUnit);
                if (rIndex == Units.Count() - 1) rIndex -= 1;
                MessageText = "Unit " + SelectedUnit.Symbol + " deleted";

                _context.Units.DeleteOnSubmit(SelectedUnit);

                // foreach () in Detailtable --> DeleteOnSubmit DetailEntry

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
                    MessageText = "Unit " + SelectedUnit.Symbol + " NOT deleted";
                }

                RefreshUnits();
                if (rIndex >= 0) SelectedUnit = Units[rIndex];
                else SelectedUnit = null;
                StatusBarText = MessageText;
            }

        }
        public void ExecuteDeleteUnitTranslation(object o)
        {
            string MessageText;

            var rowIndex = RelatedUnitTranslations.IndexOf(SelectedUnitTranslation);
            if (rowIndex == (RelatedUnitTranslations.Count() - 1)) rowIndex -= 1;
            MessageText = "UnitTranslation" + SelectedUnitTranslation.Id + "deleted";

            _context.UnitTranslations.DeleteOnSubmit(SelectedUnitTranslation);

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
                MessageText = "UnitTranslation" + SelectedUnitTranslation.Id + "NOT deleted";
            }

            RefreshUnits();
            if (rowIndex >= 0) SelectedUnitTranslation = RelatedUnitTranslations[rowIndex];
            else SelectedUnitTranslation = null;

            StatusBarText = MessageText;

        }
        public void ExecuteNewUnit(object o)
        {
            Unit NewUnit = new Unit();

            NewUnit.Name = "<Unit>: New " + DateTime.Now.ToString();
            NewUnit.Symbol = "new";
            NewUnit.TypeID = 1000004;

            _context.Units.InsertOnSubmit(NewUnit);

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
            RefreshUnits();
            SelectedUnit = NewUnit;
            StatusBarText = "Unit Added";
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

            _context.UnitTranslations.InsertOnSubmit(NewUnitTranslation);

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
            RefreshRelatedUnitTranslations();
            SelectedUnitTranslation = NewUnitTranslation;
            StatusBarText = "UnitTranslation Added";
        }

        public void ExecuteSaveUnit(object o)
        {
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
            RefreshUnits();
            StatusBarText = "All Units Saved";
        }

        #endregion
        #region Command Methods: Generic

        public bool CanAlwaysExecute(object o)
        {
            return true;
        }
        #endregion
    }
}
