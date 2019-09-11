using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitMyFood.Views.CustomControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FoodDiagram : ContentView
	{
		public FoodDiagram()
		{
			InitializeComponent();
		}

        #region Caption (Bindable string)
        public static readonly BindableProperty CaptionProperty = BindableProperty.Create(
                                                                  "Caption", //Public name to use
                                                                      typeof(string), //this type
                                                                      typeof(FoodDiagram), //parent type (this control)
                                                                      string.Empty); //default value
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }
        #endregion Caption (Bindable string)
        #region Data
        public static readonly BindableProperty DataProperty = BindableProperty.Create(
                                                                  "Data", //Public name to use
                                                                      typeof(Models.FoodDiagramModel), //this type
                                                                      typeof(FoodDiagram), //parent type (this control)
                                                                      new Models.FoodDiagramModel() //default value
                                                                     ); 
        public Models.FoodDiagramModel Data
        {
            get { return (Models.FoodDiagramModel)GetValue(DataProperty); }
            set {
                SetValue(DataProperty, value);
                AbsoluteLayout.SetLayoutFlags(ActualBar, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.XProportional);
                AbsoluteLayout.SetLayoutBounds(ActualBar, new Rectangle(0f, 1f, 0.3, 5));
            }
        }
        #endregion Data
    }


}