using System;
using Xamarin.Forms;

namespace FitMyFood.Views.CustomControls
{
        public interface ISwipeCallBack
    {

        void onLeftSwipe();
        void onRightSwipe();
        void onNothingSwiped();

        void onLeftSwipeProcess(double delta);
        void onRightSwipeProcess(double delta);
    }

    public class SwipeListener : PanGestureRecognizer
    {
        private ISwipeCallBack mISwipeCallback;
        private double translatedX = 0;
        

        public SwipeListener(SwiperStackLayout swiper)
        {
            mISwipeCallback = swiper as ISwipeCallBack;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            swiper.GestureRecognizers.Add(panGesture);
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {

            View Content = (View)sender;

            switch (e.StatusType)
            {

                case GestureStatus.Running:

                    try
                    {
                        translatedX = e.TotalX;
                        if (translatedX < 0)
                        {
                            mISwipeCallback.onLeftSwipeProcess(translatedX);
                        } else if (translatedX > 0)
                        {
                            mISwipeCallback.onRightSwipeProcess(translatedX);
                        }
                    }
                    catch (Exception err)
                    {
                        App.PrintWarning("" + err.Message);
                    }
                    break;

                case GestureStatus.Completed:
                    if (translatedX < (-Content.Width*SwiperStackLayout.Threshold))
                    {
                        mISwipeCallback.onLeftSwipe();
                    }
                    else if (translatedX > (Content.Width * SwiperStackLayout.Threshold))
                    {
                        mISwipeCallback.onRightSwipe();
                    }
                    else
                    {
                        mISwipeCallback.onNothingSwiped();
                    }

                    break;

            }
        }

    }
}