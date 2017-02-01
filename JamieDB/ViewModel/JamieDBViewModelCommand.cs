﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JamieDB.ViewModel
{
    class JamieDBViewModelCommand : ICommand
    {
        public JamieDBViewModelCommand(JamieDBViewModelCommandCanExecute CanExecuteCommand, JamieDBViewModelCommandExecute ExecuteCommand)
        {
            CanExecuteMethod += CanExecuteCommand;
            ExecuteMethod += ExecuteCommand;

        }

        public delegate bool JamieDBViewModelCommandCanExecute(object parameter);
        public delegate void JamieDBViewModelCommandExecute(object parameter);

        public event EventHandler CanExecuteChanged;

        JamieDBViewModelCommandCanExecute CanExecuteMethod;
        JamieDBViewModelCommandExecute ExecuteMethod;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteMethod != null) { return CanExecuteMethod(parameter); }
            else return true;
        }

        public void Execute(object parameter)
        {
            ExecuteMethod?.Invoke(parameter);
            // Vereinfachte Syntax für
            //if (ExecuteMethod != null) ExecuteMethod(parameter);

        }
    }
}

/*
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

 */