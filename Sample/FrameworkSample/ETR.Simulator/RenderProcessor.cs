using ETE.Engine;

namespace ETR.Simulator
{
    public class RenderProcessor : Processor
    {
        public override void Process()
        {
            EventAggregator.PublishMessage<RenderComponent>("DrawGL", null);
        }

        [MessageHandler]
        public void EndOfGraphics()
        {
        }
    }
}
