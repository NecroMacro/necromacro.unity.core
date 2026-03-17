using Unity.Cinemachine;
using UnityEngine;

namespace NecroMacro.Ui
{
    public class CameraHolder : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera renderTextureCamera;
        [SerializeField] private CinemachineCamera lobbyCamera;
        [SerializeField] private CinemachineCamera levelCamera;
        [SerializeField] private CinemachineBrain cinemachineBrain;
        
        public Camera Camera => mainCamera;
        public Camera RenderTextureCamera => renderTextureCamera;
        public CinemachineCamera LobbyCamera => lobbyCamera;
        public CinemachineCamera LevelCamera => levelCamera;
        public CinemachineBrain Brain => cinemachineBrain;
    }
}