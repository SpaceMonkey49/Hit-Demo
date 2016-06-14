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
using Android.Views.Animations;

namespace DesignLibrary_Tutorial.Resources
{
    class MenuAnimation : Animation
    {
        private View Menu;
        public MenuAnimation(View menu, int millis)
        {
            Menu = menu;
            this.Duration = millis;

        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            Menu.Alpha = (float)1.0 - interpolatedTime;
            Menu.TranslationX = -interpolatedTime / 2.0F * Menu.Width;
            Menu.RequestLayout();
        }

        public override bool WillChangeBounds()
        {
            return (true);
        }
    }
}