namespace Hashira.CanvasUI.Option
{
    public class GraphicOptionTap : OptionTab
    {
        public override void Init()
        {
            base.Init();
            foreach (var setter in _optionSetterList)
            {
                switch (setter.setterTarget)
                {
                    case nameof(OptionData.GraphicSaveData.screenEffectValue):
                        if (setter is OptionSetter<float> glitchSetter)
                            glitchSetter.Value = OptionData.GraphicSaveData.screenEffectValue;
                        break;
                    default:
                        return;
                }
            }
        }

        public override void SaveData()
        {
            foreach (var setter in _optionSetterList)
            {
                switch (setter.setterTarget)
                {
                    case nameof(OptionData.GraphicSaveData.screenEffectValue):
                        if (setter is OptionSetter<float> glitchSetter)
                            OptionData.GraphicSaveData.screenEffectValue = glitchSetter.Value;
                        break;
                    default:
                        return;
                }
            }
            OptionData.SaveData();
        }
    }
}
