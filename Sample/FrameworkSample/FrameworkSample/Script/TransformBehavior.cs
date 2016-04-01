using ETE.Engine;
using ETE.Geometry;
using System.Runtime.Serialization;

namespace FrameworkSample.Script
{
    [DataContract]
    public class TransformBehavior : UserBehavior
    {
        bool rotationToggle = true;
        Transform transform = null;

        private void Awake()
        {
        }

        private void OnEnable()
        {
        }

        private void Start()
        {
            // 해당 시뮬레이션 오브젝트에서 트랜스폼 컴포넌트를 얻어옵니다.
            transform = this.GetComponent<Transform>();
        }

        [MessageHandler]
        public void Update()
        {
            if (true == rotationToggle)
            {
                // 트랜스폼을 회전시킵니다.
                float rotation = Time.DeltaSecnods * 180.0f;
                transform.Rotate(new Vector3(0, rotation, 0));
            }
        }

        private void OnDisable()
        {
        }

        private void Destroy()
        {
        }

        private void Asleep()
        {
        }
    }
}
