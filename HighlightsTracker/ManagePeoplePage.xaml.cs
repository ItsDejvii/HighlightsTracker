using HighlightsTracker.ViewModels;
namespace HighlightsTracker;

public partial class ManagePeoplePage : ContentPage
{
    public ManagePeoplePage(PeopleViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}