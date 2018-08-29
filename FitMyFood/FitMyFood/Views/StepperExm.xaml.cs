using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitMyFood.Views
{
    public class Result : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int TeamNumber { get; set; }
        private int score { get; set; }
        private int finishOrder { get; set; }
        public int MaxTeam { get; set; }
        public string Players { get; set; }
        public int Score
        {
            set
            {
                if (score != value)
                {
                    score = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                    }
                }
            }
            get
            {
                return score;
            }
        }
        public int FinishOrder
        {
            set
            {
                if (finishOrder != value)
                {
                    finishOrder = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FinishOrder"));
                    }
                }
            }
            get
            {
                return finishOrder;
            }
        }

        public Result()
        { }

        public Result(int TeamNumber, int Score, int FinishOrder, string Players)
        {
            this.TeamNumber = TeamNumber;
            this.Score = Score;
            this.FinishOrder = FinishOrder;
            this.Players = Players;
            this.MaxTeam = 4;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepperExm : ContentPage
    {
        public ObservableCollection<Result> Results { get; set; }
        public StepperExm()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MyMessage.Text = ex.Message;
            }
        }
        protected override async void OnAppearing()
        {
            try
            {
                Results = new ObservableCollection<Result>();
                Result r = new Result(1, 0, 0, "Dave, Bill");
                Results.Add(r);
                r = new Result(2, 0, 0, "Joe, Sam");
                Results.Add(r);
                MyResultsList.ItemsSource = Results;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            foreach (Result r in Results)
            {
                await DisplayAlert("Error", string.Format("Team {0}, Score={1}, Order{2}.", r.TeamNumber, r.Score, r.FinishOrder), "OK");
            }
        }

        private void MyScoreSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Slider s = ((Slider)sender);
            var newStep = Math.Round(e.NewValue / 1);
            s.Value = newStep;
        }

        private void MyResultsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null; //Deselect Item
        }
    }
}