using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FitMyFood.CustomControls
{
    public class SwipeStackLayout : StackLayout, ISwipeCallBack
    {
        public SwipeStackLayout() : base()
        {
            SwipeListener swipeListener = new SwipeListener(this);
        }
        public void onBottomSwipe(View view)
        {
        }

        public void onLeftSwipe(View view)
        {
        }

        public void onNothingSwiped(View view)
        {
        }

        public void onRightSwipe(View view)
        {
        }

        public void onTopSwipe(View view)
        {
        }

        public void onLeftSwipeProcess(View view, double delta)
        {
            view.BackgroundColor = Color.CadetBlue;
            view.TranslateTo(delta, 0);
        }
    }
}
