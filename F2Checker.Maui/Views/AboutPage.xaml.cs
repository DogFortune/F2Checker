using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F2Checker.ViewModels;

namespace F2Checker.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage(AboutPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}