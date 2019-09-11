using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FitMyFood.Views.CustomControls
{
    public class SwiperStackLayout : StackLayout, ISwipeCallBack
    {
        Color leftSwipeColor = Color.Red;
        Color rightSwipeColor = Color.Green;
        public static double Threshold = 0.2;

        Color origBackgroundColor;
        public SwiperStackLayout() : base()
        {
            SwipeListener swipeListener = new SwipeListener(this);
            origBackgroundColor = BackgroundColor;
        }

        public void onLeftSwipe()
        {
            resetControl(); 
        }

    void resetControl()
        {
            this.TranslateTo(0, 0);
            BackgroundColor = origBackgroundColor;
        }
        public void onNothingSwiped()
        {
            resetControl();
        }

        public void onRightSwipe()
        {
            resetControl();
        }

        public void onLeftSwipeProcess(double delta)
        {
            if (delta < -Width * Threshold)
            {
                BackgroundColor = leftSwipeColor;
            }
            this.TranslateTo(delta, 0);
        }

        public void onRightSwipeProcess(double delta)
        {
            if (delta > Width * Threshold)
            {
                BackgroundColor = rightSwipeColor;
            }
            this.TranslateTo(delta, 0);
        }
    }
}
