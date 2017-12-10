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
        private MVUserViewModel viewModel;

        public MVUserLoginCommand(MVUserViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        #region ICommand Members
        public bool CanExecute(object parameter)
        {
            return string.IsNullOrWhiteSpace(viewModel.MVUser.Error) && !viewModel.IsSecurePasswordNull();
        }

        public void Execute(object parameter)
        {
            viewModel.SaveChanges();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value;  }
        }

        #endregion
    }
}
