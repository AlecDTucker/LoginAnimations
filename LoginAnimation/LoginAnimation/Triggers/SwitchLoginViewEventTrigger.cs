using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoginAnimation.Triggers
{
    public class SwitchLoginViewEventTrigger : TriggerAction<Button>
    {
        public enum eDirection
        {
            Left,
            Right
        }

        protected override async void Invoke(Button sender)
        {
            View hideMe = null;
            View showMe = null;

            if (Target != null)
            {
                // Find showMe based on property set in xaml
                showMe = ((View)sender.Parent.Parent).FindByName<View>(Target);

                if (showMe != null)
                {
                    // Find hideMe based on property set in xaml
                    if (Source != null)
                    {
                        hideMe = ((View)sender.Parent.Parent).FindByName<View>(Source);

                        if (hideMe != null)
                        {
                            await PerformAnimation(hideMe, showMe);
                        }
                    }
                }
            }
        }

        public string Source { get; set; }
        public string Target { get; set; }
        public eDirection Direction { get; set; }

        private async Task PerformAnimation(View pHideMe, View pShowMe)
        {
            int hideStart = 0;
            int hideStop = (Direction == eDirection.Left ? -90 : 90);
            int showStart = (Direction == eDirection.Left ? 90 : 270);
            int showStop = (Direction == eDirection.Left ? 0 : 360);

            // Prep - put the stacklayouts (views) to their animation start positions

            await pHideMe.RotateYTo(hideStart, 0);
            await pShowMe.RotateYTo(showStart, 0);
            await pShowMe.ScaleTo(0.2, 0);
            pShowMe.IsVisible = true;    // This is rotated at 90 or 270 degrees, so it can't be seen yet

            // Animate

            // Kick off the fade, and then start rotating immediately
            pHideMe.FadeTo(0.5, 100, Easing.SinOut);
            pHideMe.ScaleTo(0.2, 100, Easing.Linear);
            await pHideMe.RotateYTo(hideStop, 150, Easing.Linear);

            // Kick off the fade, and then start rotating immediately
            pShowMe.FadeTo(1, 100, Easing.SinIn);
            pShowMe.ScaleTo(1, 150, Easing.Linear);
            await pShowMe.RotateYTo(showStop, 150, Easing.Linear);

            // Tidy up
            pHideMe.IsVisible = false;   // This can now be hidden as it's already out of sight
        }
    }
}
