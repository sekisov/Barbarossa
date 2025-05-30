namespace Barbarossa.Services
{
    public class ScaleAction : TriggerAction<VisualElement>
    {
        public double Scale { get; set; } = 1.0;
        public uint Duration { get; set; } = 100;

        protected override async void Invoke(VisualElement sender)
        {
            await sender.ScaleTo(Scale, Duration);
        }
    }
}
