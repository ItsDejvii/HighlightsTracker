using HighlightsTracker.Models;
using HighlightsTracker.Services;
using HighlightsTracker.ViewModels;


namespace HighlightsTracker
{
    public partial class MainPage : ContentPage
    {
        private readonly PeopleViewModel _vm;

        private bool firstStart = true;

        public MainPage(PeopleViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _vm = vm;
        }

        private void OnStartButtonClicked(object sender, EventArgs e)
        {
            if (firstStart)
            {
                firstStart = false;
                Dispatcher.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
                {
                    if (_vm.IsRunning)
                    {
                        _vm.TimerText = (_vm.TimerTimespan + _vm.Timer.Elapsed).ToString(@"hh\:mm\:ss");
                    }
                    return true;
                });

                //Save timer every 10 seconds
                Dispatcher.StartTimer(TimeSpan.FromMilliseconds(10000), () =>
                {
                    SaveTimer();
                    return true;
                });

            }

            if (!_vm.IsRunning)
            {
                _vm.IsRunning = true;
                _vm.Timer.Start();
            }
        }

        private async void SaveTimer() {
            if (_vm.IsRunning)
            {
                await StorageService.SaveTimerAsync(_vm.TimerTimespan + _vm.Timer.Elapsed);
            }
        }

        private void OnPauseButtonClicked(object sender, EventArgs e)
        {
            if (_vm.IsRunning)
            {
                _vm.IsRunning = false;
                _vm.Timer.Stop();
            }
        }

        private async void OnResetButtonClicked(object sender, EventArgs e)
        {
            bool reset = await DisplayAlert("Reset timer?", "Are you sure you want to reset the timer?", "Yes", "No");
            if (reset)
            {
                _vm.IsRunning = false;
                _vm.Timer.Reset();
                _vm.TimerText = "00:00:00";
                _vm.TimerTimespan = new(0, 0, 0);
                await StorageService.SaveTimerAsync(_vm.TimerTimespan);
            }
        }

        private async void OnEventButtonClicked(object sender, EventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (!Enum.TryParse(btn.Text, true, out EventEnum personEvent))
                return;

            string person = await DisplayActionSheet("Select person - Add" + personEvent, "Cancel", null, _vm.People.Select(p => p.Name).ToArray());

            if (person is null || person == "Cancel")
                return;

            Person? selectedPerson = _vm.People.FirstOrDefault(p => p.Name == person);
            if (selectedPerson != null)
            {
                PersonEvent pe = new(selectedPerson, personEvent);

                _vm.AddEventCommand.Execute(pe);
            }

        }

        private async void OnDeleteEventClicked(object sender, EventArgs e)
        {

            if (e is not TappedEventArgs tappedEvent)
                return;

            if (tappedEvent.Parameter is not Event personEvent)
                return;

            bool delete = await DisplayAlert("Delete " + personEvent.Name + "'s " + personEvent.Title.ToLower() + "?", "Are you sure you want to delete " + personEvent.Name + "'s " + personEvent.Title.ToLower() + "?", "Yes", "No");
            if (delete)
            {
                _vm.RemoveEventCommand.Execute(personEvent);
            }
        }

        private async void OnDeleteEventsClicked(object sender, EventArgs e)
        {
            bool delete = await DisplayAlert("Delete all events?", "Are you sure you want to delete all events?", "Yes", "No");
            if (delete)
            {
                _vm.ClearEventsCommand.Execute(null);
            }
        }
    }
}