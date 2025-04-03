using HighlightsTracker.Models;
using HighlightsTracker.ViewModels;
namespace HighlightsTracker;

public partial class ManagePeoplePage : ContentPage
{
    private readonly PeopleViewModel _vm;

    public ManagePeoplePage(PeopleViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    private async void OnRemovePersonClicked(object sender, EventArgs e)
    {
        if (sender is not Button btn)
            return;

        if (btn.CommandParameter is not Person person)
            return;

        bool delete = await DisplayAlert("Delete" + person.Name + "?", "Are you sure you want to delete " + person.Name + "?", "Yes", "No");
        if (delete)
        {
            _vm.RemovePersonCommand.Execute(person);
        }
    }
}