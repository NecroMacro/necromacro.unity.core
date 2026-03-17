namespace Core.MobileInput
{
    public class ScaleMovedSignal 
    {
        public float ScaleMultiplier;
        public float ScaleMultiplierRange;

        public ScaleMovedSignal(float scale, float range)
        {
            ScaleMultiplier = scale;
            ScaleMultiplierRange = range;
        }
    }
    public class ScaleEndedSignal {}
}
