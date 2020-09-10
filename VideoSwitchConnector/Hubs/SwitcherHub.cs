using BmdSwitcher;

using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoSwitchConnectorApi.Hubs
{
    public class SwitcherHub: Hub
    {
        private readonly Switcher switcher;

        public SwitcherHub(Switcher switcher)
        {
            switcher.CurrentProgramInputChanged += Switcher_CurrentProgramInputChanged;
            this.switcher = switcher;
        }

        private void Switcher_CurrentProgramInputChanged(object sender, EventArgs e)
        {
            Clients.All.SendAsync("ProgramInputChanged", this.switcher.Id, this.switcher.CurrentProgramInput);
        }

        public void SetPreviewInput(string switcherId, string input)
        {
        }

        public void SetProgramInput(string switcherId, long input)
        {
            this.switcher.SetProgramInput(input);
        }
    }
}
