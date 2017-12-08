using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RestaurantPro.ViewModels;

namespace RestaurantPro.Commands
{
    internal class MVUserLoginCommand : ICommand
    {
        public MVUserLoginCommand(MVUserViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        private MVUserViewModel _ViewModel;


        #region ICommand Members
        public bool CanExecute(object parameter)
        {
            return _ViewModel.CanLogin;
        }

        public void Execute(object parameter)
        {
            _ViewModel.SaveChanges();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value;  }
        }

        #endregion
    }
}
