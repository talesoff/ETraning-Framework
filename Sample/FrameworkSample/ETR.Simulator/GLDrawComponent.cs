using ETE.Engine;
using ETE.Geometry;
using System.Runtime.Serialization;

namespace ETR.Simulator
{
    [DataContract]
    public class GLDrawComponent : RenderComponent
    {
        private Transform refTransform = null;
        internal Matrix4x4 matrix = Matrix4x4.identity;

        [DataMember]
        private int count = 0;

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {

        }

        [OnSerialized]
        void OnSerialized(StreamingContext context)
        {

        }

        [OnDeserializing]
        void OnDeserializing(StreamingContext context)
        {

        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {

        }

        public GLDrawComponent()
        {

        }

        public void Start()
        {
            Transform t = this.GetComponent<Transform>();
            refTransform = t;
        }

        [MessageHandler]
        public void DrawGL()
        {
            matrix = refTransform.LocalToWorldMatrix;
            count++;
        }

        public void Destroy()
        {

        }
    }
}
