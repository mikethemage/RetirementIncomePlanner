﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetirementIncomePlannerDesktopApp
{
    public class RelayCommand : ICommand
    {
        private readonly Action commandTask;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action workToDo, Func<bool> workCanBeDone)
        {
            commandTask = workToDo;
            canExecute = workCanBeDone;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }

        public void Execute(object? parameter)
        {
            commandTask();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
