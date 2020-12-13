using LabirinthGame.Common;
using UnityEngine;


namespace LabirinthGame.Managers
{
    public sealed class InputManager : Singleton<InputManager>
    {
        #region Delegates and events

        public delegate void AxisInputProcessor(float horizontal, float vertical);

        public delegate void MouseMoveProcessor(float X, float Y);
        
        public delegate void ShootProcessor();


        public event AxisInputProcessor OnAxisInputDone;
        public event MouseMoveProcessor OnMouseInputDone;
        public event ShootProcessor OnShootInputDone;

        #endregion


        #region Private data

        private float _horizintalInput;
        private float _verticalInput;
        private bool _emptyAxisWasSent;

        private float _mouseXMoveInput;
        private float _mouseYMoveInput;
        private bool _isEmptyMouseMoveWasSent;

        #endregion


        #region Unity events

        private void Update()
        {
            ReadAxisInput();
            BroadcastAxisInput();

            ReadMouseMoveInput();
            BroadcastMouseMoveInput();

            ReadAndBroadcastShoot();
        }

        #endregion


        #region Methods

        private void ReadAxisInput()
        {
            _horizintalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
        }

        private void BroadcastAxisInput()
        {
            if (Mathf.Abs(_horizintalInput) > 0f || Mathf.Abs(_verticalInput) > 0f)
            {
                OnAxisInputDone?.Invoke(_horizintalInput, _verticalInput);
                _emptyAxisWasSent = false;
            }
            else if (!_emptyAxisWasSent)
            {
                OnAxisInputDone?.Invoke(0f, 0f);
                _emptyAxisWasSent = true;
            }
        }

        private void ReadMouseMoveInput()
        {
            _mouseXMoveInput = Input.GetAxis("Mouse X");
            _mouseYMoveInput = Input.GetAxis("Mouse Y");
        }

        private void BroadcastMouseMoveInput()
        {
            if (Mathf.Abs(_mouseXMoveInput) > 0 || Mathf.Abs(_mouseXMoveInput) > 0)
            {
                OnMouseInputDone?.Invoke(_mouseXMoveInput, _mouseYMoveInput);
                _isEmptyMouseMoveWasSent = false;
            }
            else if (!_isEmptyMouseMoveWasSent)
            {
                OnMouseInputDone?.Invoke(0f, 0f);
                _isEmptyMouseMoveWasSent = true;
            }
        }


        private void ReadAndBroadcastShoot()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnShootInputDone?.Invoke();
            }
        }

        #endregion
    }
}