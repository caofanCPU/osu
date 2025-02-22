// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Testing;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Scoring;
using osu.Game.Screens.Play.HUD;

namespace osu.Game.Tests.Visual.Gameplay
{
    public class TestSceneSkinnableScoreCounter : SkinnableTestScene
    {
        private IEnumerable<SkinnableScoreCounter> scoreCounters => CreatedDrawables.OfType<SkinnableScoreCounter>();

        protected override Ruleset CreateRulesetForSkinProvider() => new OsuRuleset();

        [Cached]
        private ScoreProcessor scoreProcessor = new ScoreProcessor();

        [SetUpSteps]
        public void SetUpSteps()
        {
            AddStep("Create score counters", () => SetContents(() => new SkinnableScoreCounter()));
        }

        [Test]
        public void TestScoreCounterIncrementing()
        {
            AddStep(@"Reset all", () => scoreProcessor.TotalScore.Value = 0);

            AddStep(@"Hit! :D", () => scoreProcessor.TotalScore.Value += 300);
        }

        [Test]
        public void TestVeryLargeScore()
        {
            AddStep("set large score", () => scoreCounters.ForEach(counter => scoreProcessor.TotalScore.Value = 1_000_000_000));
        }
    }
}
