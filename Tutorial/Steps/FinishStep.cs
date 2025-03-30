using DG.Tweening;
using Hashira.Core;
using Hashira.MainScreen;
using Hashira.StageSystem;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class FinishStep : TutorialStep
    {
        public override void OnEnter()
        {
            base.OnEnter();
            MainScreenEffect.OnAlpha(0, 0.75f, Ease.InSine);
            MainScreenEffect.OnGlitch(1f, 0.5f, Ease.InCubic);
            MainScreenEffect.OnScaling(0.5f, 1.15f, Ease.Unset).OnComplete(() => SceneLoadingManager.LoadScene(SceneName.TitleScene));
        }
    }
}
