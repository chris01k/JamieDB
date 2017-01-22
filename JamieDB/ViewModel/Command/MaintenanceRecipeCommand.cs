using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JamieDB.ViewModel.Command
{
    class SaveRecipeCommand : ICommand
    {
        private MaintainanceRecipesViewModel _ViewModel;

        public SaveRecipeCommand(MaintainanceRecipesViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _ViewModel.ExecuteSaveRecipe();
        }
    }

    class NewRecipeCommand: ICommand
    {
        private MaintainanceRecipesViewModel _ViewModel;

        public NewRecipeCommand(MaintainanceRecipesViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _ViewModel.ExecuteNewRecipe();
        }

    }

    class NewRecipeIngredientCommand : ICommand
    {
        private MaintainanceRecipesViewModel _ViewModel;

        public NewRecipeIngredientCommand(MaintainanceRecipesViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _ViewModel.ExecuteNewRecipeIngredient();
        }
    }
}
