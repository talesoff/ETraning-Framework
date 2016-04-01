using ETE.Engine;

namespace ETE.Simulator
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
