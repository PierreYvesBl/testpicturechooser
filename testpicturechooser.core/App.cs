using System;
using MvvmCross;
using MvvmCross.ViewModels;
using testpicturechooser.core.ViewModels;

namespace testpicturechooser.core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterAppStart<MainViewModel>();
        }
    }
}
