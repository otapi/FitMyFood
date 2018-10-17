using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using FitMyFood.Droid.Renderers;
using Android.Support.V7.Widget;

[assembly:ExportRenderer (typeof(Xamarin.Forms.Button), typeof(RoundButtonRenderer))] 
namespace FitMyFood.Droid.Renderers
{
    public class RoundButtonRenderer : ButtonRenderer
    {
        public RoundButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var button = (Xamarin.Forms.Button)e.NewElement;
                button.CornerRadius = 22;
            }
        }

    }
}
