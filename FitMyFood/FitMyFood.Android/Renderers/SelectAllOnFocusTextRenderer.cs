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
using System.ComponentModel;

[assembly:ExportRenderer (typeof(Xamarin.Forms.Entry), typeof(SelectAllOnFocusTextRenderer))] 
namespace FitMyFood.Droid.Renderers
{
    public class SelectAllOnFocusTextRenderer : EntryRenderer
    {
        public SelectAllOnFocusTextRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;
                nativeEditText.SetSelectAllOnFocus(true);
            }
        }
        /*
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);
            try
            {
                if (args.PropertyName == "IsFocused")
                {
                    var nativeEditText = (global::Android.Widget.EditText)Control;
                    nativeEditText.SetSelectAllOnFocus(true);
                }

            }
            catch
            {

            }
        }
        */
    }
}
