﻿using System;

using ETE.Engine;
using ETE.Geometry;

using ETE.Render.Component;

namespace RenderSample.UserControl
{
    class MeshControl : UserBehavior
    {
        Transform transform = null;

        public void Start()
        {
            // 해당 시뮬레이션 오브젝트에서 트랜스폼 컴포넌트를 얻어옵니다.
            transform = this.GetComponent<Transform>();
        }

        [MessageHandler]
        public void Update()
        {
            // 트랜스폼을 회전시킵니다.
            float rotation = Time.DeltaSecnods * 180.0f;
            transform.Rotate(new Vector3(0, rotation, 0));
        }
    }
}
