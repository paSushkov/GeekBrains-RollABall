namespace LabirinthGame.Common
{
    public struct MinMaxCurrent
    {
        #region Fields

        public readonly float minValue;
        public readonly float maxValue;
        public readonly float currentValue;

        #endregion


        #region ClassLifeCycles

        public MinMaxCurrent(float minValue, float maxValue, float currentValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.currentValue = currentValue;

        }

        #endregion
    }
}