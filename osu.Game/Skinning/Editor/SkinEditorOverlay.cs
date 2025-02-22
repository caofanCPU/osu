// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Input.Bindings;

namespace osu.Game.Skinning.Editor
{
    /// <summary>
    /// A container which handles loading a skin editor on user request for a specified target.
    /// This also handles the scaling / positioning adjustment of the target.
    /// </summary>
    public class SkinEditorOverlay : CompositeDrawable, IKeyBindingHandler<GlobalAction>
    {
        private readonly ScalingContainer target;
        private SkinEditor skinEditor;

        private const float visible_target_scale = 0.8f;

        [Resolved]
        private OsuColour colours { get; set; }

        public SkinEditorOverlay(ScalingContainer target)
        {
            this.target = target;
            RelativeSizeAxes = Axes.Both;
        }

        public bool OnPressed(GlobalAction action)
        {
            switch (action)
            {
                case GlobalAction.Back:
                    if (skinEditor?.State.Value == Visibility.Visible)
                    {
                        skinEditor.ToggleVisibility();
                        return true;
                    }

                    break;

                case GlobalAction.ToggleSkinEditor:
                    if (skinEditor == null)
                    {
                        LoadComponentAsync(skinEditor = new SkinEditor(target), AddInternal);
                        skinEditor.State.BindValueChanged(editorVisibilityChanged);
                    }
                    else
                        skinEditor.ToggleVisibility();

                    return true;
            }

            return false;
        }

        private void editorVisibilityChanged(ValueChangedEvent<Visibility> visibility)
        {
            if (visibility.NewValue == Visibility.Visible)
            {
                target.ScaleTo(visible_target_scale, SkinEditor.TRANSITION_DURATION, Easing.OutQuint);

                target.Masking = true;
                target.BorderThickness = 5;
                target.BorderColour = colours.Yellow;
                target.AllowScaling = false;
            }
            else
            {
                target.BorderThickness = 0;
                target.AllowScaling = true;

                target.ScaleTo(1, SkinEditor.TRANSITION_DURATION, Easing.OutQuint).OnComplete(_ => target.Masking = false);
            }
        }

        public void OnReleased(GlobalAction action)
        {
        }

        /// <summary>
        /// Exit any existing skin editor due to the game state changing.
        /// </summary>
        public void Reset()
        {
            skinEditor?.Hide();
            skinEditor?.Expire();
            skinEditor = null;
        }
    }
}
