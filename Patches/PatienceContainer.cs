using KitchenData;

namespace LabelCountdown.Patches
{
    public struct PatienceContainer
    {
        public float patience;
        public PatienceReason reason;
        public float lastUpdateTime;
        public bool isPercent = false;

        public PatienceContainer(float patience, PatienceReason reason, float lastUpdateTime, bool isPercent)
        {
            this.patience = patience;
            this.reason = reason;
            this.lastUpdateTime = lastUpdateTime;
            this.isPercent = isPercent;
        }
    }

}
