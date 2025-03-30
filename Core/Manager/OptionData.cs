using Doryu.JBSave;
using UnityEngine;

namespace Hashira
{
    public class SoundSaveData : ISavable<SoundSaveData>
    {
        public float masterSound = 0.5f;
        public float effectSound = 0.5f;
        public float backgroundSound = 0.5f;

        public void OnLoadData(SoundSaveData classData)
        {
            masterSound = classData.masterSound;
            effectSound = classData.effectSound;
            backgroundSound = classData.backgroundSound;
        }

        public void OnSaveData(string savedFileName)
        {

        }

        public void ResetData()
        {
            masterSound = 0.5f;
            effectSound = 0.5f;
            backgroundSound = 0.5f;
        }
    }
    public class GraphicSaveData : ISavable<GraphicSaveData>
    {
        public float screenEffectValue = 1f;

        public void OnLoadData(GraphicSaveData classData)
        {
            screenEffectValue = classData.screenEffectValue;
        }

        public void OnSaveData(string savedFileName)
        {

        }

        public void ResetData()
        {
            screenEffectValue = 1f;
        }
    }

    public static class OptionData
    {
        public static SoundSaveData SoundSaveData { get; private set; }
        public static GraphicSaveData GraphicSaveData { get; private set; }

        public static void Init()
        {
            SoundSaveData = new SoundSaveData();
            GraphicSaveData = new GraphicSaveData();
            LoadData();
        }

        public static void SaveData()
        {
            SoundSaveData.SaveJson("SoundData");
            GraphicSaveData.SaveJson("GraphicData");
        }
        public static void LoadData()
        {
            SoundSaveData.LoadJson("SoundData");
            GraphicSaveData.LoadJson("GraphicData");
        }
    }
}
