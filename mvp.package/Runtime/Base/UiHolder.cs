using UnityEngine;
using UnityEngine.EventSystems;

namespace NecroMacro.Ui
{
    public class UiHolder : MonoBehaviour
    {
        [SerializeField] private Transform gameplaySpawnParent;
        [SerializeField] private Transform screensSpawnParent;
        [SerializeField] private Transform popupSpawnParent;
        [SerializeField] private Transform topPanelParent;
        [SerializeField] private EventSystem eventSystem;
        
        public Transform GameplaySpawnParent => gameplaySpawnParent;
        public Transform ScreensSpawnParent => screensSpawnParent;
        public Transform PopupSpawnParent  => popupSpawnParent;
        public Transform TopPanelParent => topPanelParent;
        public EventSystem EventSystem => eventSystem;
    }
}