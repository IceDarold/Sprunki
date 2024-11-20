using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class ModeController : MonoBehaviour
    {
        public static ModeType Mode {  get; private set; }

        public static Action OnModeChanged;

        [SerializeField] private ModeType Start;
        private static ModeController instance;


        private void Awake()
        {
            instance = this;
            Mode = Start;
        }

        [ContextMenu("Set Start Mode")]
        public void SetModeDevelopTool()
        {
            Mode = Start;
            OnModeChanged?.Invoke();
        }

        public static void SetMode(ModeType type)
        {
            Mode = type;
            OnModeChanged?.Invoke();
        }

        
    }
}