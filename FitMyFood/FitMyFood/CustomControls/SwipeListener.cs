﻿using System;
using Xamarin.Forms;

namespace FitMyFood.CustomControls
{
        public interface ISwipeCallBack
    {

        void onLeftSwipe(View view);
        void onRightSwipe(View view);
        void onTopSwipe(View view);
        void onBottomSwipe(View view);
        void onNothingSwiped(View view);

        void onLeftSwipeProcess(View view, double delta);
    }

    public class SwipeListener : PanGestureRecognizer
    {
        private ISwipeCallBack mISwipeCallback;
        private double translatedX = 0, translatedY = 0;

        public SwipeListener(SwipeStackLayout swiper)
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
                        translatedY = e.TotalY;
                        if (translatedX < 0 && Math.Abs(translatedX) > Math.Abs(translatedY))
                        {
                            mISwipeCallback.onLeftSwipeProcess(Content, translatedX);
                        }
                    }
                    catch (Exception err)
                    {
                        System.Diagnostics.Debug.WriteLine("" + err.Message);
                    }
                    break;

                case GestureStatus.Completed:

                    System.Diagnostics.Debug.WriteLine("translatedX : " + translatedX);
                    System.Diagnostics.Debug.WriteLine("translatedY : " + translatedY);

                    if (translatedX < 0 && Math.Abs(translatedX) > Math.Abs(translatedY))
                    {
                        mISwipeCallback.onLeftSwipe(Content);
                    }
                    else if (translatedX > 0 && translatedX > Math.Abs(translatedY))
                    {
                        mISwipeCallback.onRightSwipe(Content);
                    }
                    else if (translatedY < 0 && Math.Abs(translatedY) > Math.Abs(translatedX))
                    {
                        mISwipeCallback.onTopSwipe(Content);
                    }
                    else if (translatedY > 0 && translatedY > Math.Abs(translatedX))
                    {
                        mISwipeCallback.onBottomSwipe(Content);
                    }
                    else
                    {
                        mISwipeCallback.onNothingSwiped(Content);
                    }

                    break;

            }
        }

    }
}