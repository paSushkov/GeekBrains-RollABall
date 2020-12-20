using LabirinthGame.Tech.Input;
using UnityEngine;

namespace LabirinthGame.Managers
{
    [CreateAssetMenu(menuName = "Sushkov/Managers/InputManagerListener")]
    public class InputManagerListener : ScriptableObject, IInputListener
    {
        [SerializeField] private string horizontalAxis = "Horizontal";
        [SerializeField] private string verticalAxis = "Vertical";
        [SerializeField] private string cancelAxis = "Cancel";
        [SerializeField] private string fireAxis = "Fire1";
        [SerializeField] private string jumpAxis = "Jump";

        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public float Cancel { get; private set; }
        public float Fire1 { get; private set; }
        public float Jump { get; private set; }

        public void Initialize(IInputTranslator translator)
        {
            translator.SubscribeToAxisInput(horizontalAxis, GetHorizontal);
            translator.SubscribeToAxisInput(verticalAxis, GetVertical);
            translator.SubscribeToAxisInput(cancelAxis, GetCancel);
            translator.SubscribeToAxisInput(fireAxis, GetFire1);
            translator.SubscribeToAxisInput(jumpAxis, GetJump);
        }

        public void Shutdown(IInputTranslator translator)
        {
            translator.UnsubscribeFromAxisInput(horizontalAxis, GetHorizontal);
            translator.UnsubscribeFromAxisInput(verticalAxis, GetVertical);
            translator.UnsubscribeFromAxisInput(cancelAxis, GetCancel);
            translator.UnsubscribeFromAxisInput(fireAxis, GetFire1);
            translator.UnsubscribeFromAxisInput(jumpAxis, GetJump);
            
        }

        private void GetHorizontal(float value)
        {
            Horizontal = value;
        }
        private void GetVertical(float value)
        {
            Vertical = value;

        }
        private void GetCancel(float value)
        {
            Cancel = value;

        }
        private void GetFire1(float value)
        {
            Fire1 = value;

        }
        private void GetJump(float value)
        {
            Jump = value;

        }
    }
}