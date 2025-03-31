using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HighlightsTracker.Models;
using HighlightsTracker.ViewModels;
using System.Diagnostics;


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
                        _vm.TimerText = _vm.Timer.Elapsed.ToString(@"hh\:mm\:ss");

                    return true;
                });
                
            }

            if (!_vm.IsRunning)
            {
                _vm.IsRunning = true;
                _vm.Timer.Start();
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

        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            _vm.IsRunning = false;
            _vm.Timer.Reset();
            _vm.TimerText = "00:00:00";
        }
    }
}