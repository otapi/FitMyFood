using FitMyFood.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using static CoreText.CTFontFeatureAllTypographicFeatures;

[assembly: ExportRenderer(typeof(Entry), typeof(SelectAllOnFocusTextRenderer))]
namespace FitMyFood.iOS.Renderers
{
    public class SelectAllOnFocusTextRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsFocused")
            {
                Control.SelectAll(null);
            }
        }

    }
}