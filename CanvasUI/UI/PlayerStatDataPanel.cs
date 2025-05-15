using UnityEngine;

namespace Hashira.CanvasUI
{
    public class PlayerStatDataPanel : UIBase, IToggleUI
    {
        public string Key { get; set; } = nameof(PlayerStatDataPanel);

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
