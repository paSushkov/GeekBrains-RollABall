using System.Collections.Generic;
using LabirinthGame.Common.Interfaces;

namespace LabirinthGame.Common
{
    public class Tracker : ITracker
    {
        #region Private data

        private ITrackable _target;
        private List<PositionChangeProcessor> subscribedProcessors = new List<PositionChangeProcessor>();

        #endregion

        #region Public methods

        public void SubscribeProcess(PositionChangeProcessor processor)
        {
            if (processor != null && _target != null)
            {
                subscribedProcessors.Add(processor);
                _target.OnPositionChange += processor;
            }
        }

        public void UnsubscribeFromTarget()
        {
            if (_target != null)
            {
                foreach (var processor in subscribedProcessors)
                {
                    _target.OnPositionChange -= processor;
                }
            }

            subscribedProcessors.Clear();
        }

        #endregion


        #region ITracker implementation

        public void SetTarget(ITrackable target)
        {
            if (_target != target)
            {
                UnsubscribeFromTarget();
                _target = target;
            }
        }

        public ITrackable GetTarget()
        {
            return _target ?? null;
        }

        #endregion
    }
}