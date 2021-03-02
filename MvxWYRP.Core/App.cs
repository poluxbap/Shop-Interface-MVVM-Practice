using MvvmCross.ViewModels;
using MvxWYRP.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MvxWYRP.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<GuestBookViewModel>();
        }
    }
}
