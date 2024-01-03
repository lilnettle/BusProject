﻿using ClientBus.Inafrac.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBus.Inafrac.Commands
{
    internal class RelayCommand : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;


        public RelayCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentException(nameof(Execute));
            _CanExecute = CanExecute;

        }
        public override bool CanExecute(object parameter) => _CanExecute == null || _CanExecute(parameter);

        public override void Execute(object parameter) => _Execute(parameter);

    }
}
