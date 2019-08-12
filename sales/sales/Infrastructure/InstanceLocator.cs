
using System;
using System.Collections.Generic;
using System.Text;

namespace sales.Infrastructure
{
    using sales.ViewModels;
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
