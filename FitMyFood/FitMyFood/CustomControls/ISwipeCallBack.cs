using System;
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

        void onLeftSwipeProcess(View view);

    }
}