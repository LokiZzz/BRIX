using BRIX.Mobile.Resources.Controls;
using BRIX.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.View
{
    public static class AnimationHelper
    {
        public static async void PlayInvalidEntryAnimation(FramedEntry entry)
        {
            if(entry == null)
            {
                return;
            }

            uint length = 60;

            Color? before = entry.EntryColor.Copy();
            Color after = Colors.Red;
            object? colorResource = null;

            Application.Current?.Resources.TryGetValue("BRIXRed", out colorResource);

            if (colorResource != null && colorResource is Color afterColor)
            {
                after = afterColor;
            }

            Animation animation = new(x => entry.EntryColor = GetColorTransitionState(before, after, x));
            Animation revertAnimation = new(x => entry.EntryColor = GetColorTransitionState(after, before, x));

            animation.Commit(entry, "ToColorAnimation", 16, 250, Easing.Linear, (x, y) =>
                revertAnimation.Commit(entry, "RevertColorAnimation", 16, 500, Easing.Linear)
            );

            await entry.TranslateTo(5, 0, length);
            await entry.TranslateTo(-5, 0, length);
            await entry.TranslateTo(5, 0, length);
            await entry.TranslateTo(-5, 0, length);
            await entry.TranslateTo(5, 0, length);
            await entry.TranslateTo(0, 0, length);
        }

        private static Color GetColorTransitionState(Color from, Color to, double transitionCoef)
        {
            return new Color(
                Transition(from.Red, to.Red, transitionCoef),
                Transition(from.Green, to.Green, transitionCoef),
                Transition(from.Blue, to.Blue, transitionCoef)
            );

            float Transition(double from, double to, double coef)
            {
                return (float)(from - (from - to) * coef);
            }
        }
    }
}
