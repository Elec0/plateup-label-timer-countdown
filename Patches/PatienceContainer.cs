using KitchenData;

namespace LabelCountdown.Patches
{
    public struct PatienceContainer
    {
        public float patience;
        public PatienceReason reason;
        public string reasonName
        {
            get
            {
                return GetReasonName();
            }
        }
        public float lastUpdateTime;
        public bool isPercent = false;

        public string GetReasonName() => reason switch
        {
            PatienceReason.Service => "Take Order",
            PatienceReason.Queue => "Queue Outside",
            PatienceReason.QueueInRain => "Queue Outside in Rain",
            PatienceReason.QueueInSnow => "Queue Outside in Snow",
            PatienceReason.QueueInDarkness => "Queue Outside in Darkness",
            PatienceReason.WaitForFood => "Waiting for Food",
            PatienceReason.GetFoodDelivered => "Food Delivery in Progress",
            _ => reason.ToString()
        };

        public PatienceContainer(float patience, PatienceReason reason, float lastUpdateTime, bool isPercent)
        {
            this.patience = patience;
            this.reason = reason;
            this.lastUpdateTime = lastUpdateTime;
            this.isPercent = isPercent;
        }
    }

}
