using System;
using System.Collections.Generic;

namespace BmdSwitcher
{
    public interface Switcher
    {
        Guid Id { get; }
        string ProductName { get; }

        IList<SwitcherInput> Inputs { get; }

        public SwitcherInput CurrentProgramInput { get; }

        public event EventHandler CurrentProgramInputChanged;

        public void SetProgramInput(long id);
    }
}
