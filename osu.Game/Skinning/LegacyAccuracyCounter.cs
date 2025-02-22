// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Screens.Play;
using osu.Game.Screens.Play.HUD;
using osuTK;

namespace osu.Game.Skinning
{
    public class LegacyAccuracyCounter : GameplayAccuracyCounter, ISkinnableComponent
    {
        private readonly ISkin skin;

        public LegacyAccuracyCounter(ISkin skin)
        {
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;

            Scale = new Vector2(0.6f);
            Margin = new MarginPadding(10);

            this.skin = skin;
        }

        [Resolved(canBeNull: true)]
        private HUDOverlay hud { get; set; }

        protected sealed override OsuSpriteText CreateSpriteText() => new LegacySpriteText(skin, LegacyFont.Score)
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
        };

        protected override void Update()
        {
            base.Update();

            if (hud?.ScoreCounter.Drawable is LegacyScoreCounter score)
            {
                // for now align with the score counter. eventually this will be user customisable.
                Y = Parent.ToLocalSpace(score.ScreenSpaceDrawQuad.BottomRight).Y;
            }
        }
    }
}
