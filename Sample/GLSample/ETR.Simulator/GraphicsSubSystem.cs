using ETE.MVC;
using System;
using System.Collections.Generic;

namespace ETE.Simulator
{
    public class GraphicsSubsystem : Subsystem
    {
        public static IntPtr NativeHandle = IntPtr.Zero;
        public static int WindowWidth = 0;
        public static int WindowHeight = 0;

        private int oldWidth = 0;
        private int oldHeight = 0;

        public override void Awake()
        {
            EGLNative.EGLCreate(NativeHandle);
        }

        public override void Start()
        {
            GLNative.GLStart();
        }

        public override void Process()
        {
            if (oldWidth != WindowWidth || oldHeight != WindowHeight)
            {
                oldWidth = WindowWidth;
                oldHeight = WindowHeight;

                GLNative.GLResize(WindowWidth, WindowHeight);
            }

            GLNative.GLClear();

            // 특정 타입의 콤포넌트 목록 조회
            IList<GLDrawComponent> coms = App.Model.FindComponents<GLDrawComponent>();
            foreach (GLDrawComponent com in coms)
            {
                // 데이터 처리 실행
                GLNative.GLDraw(com.matrix);
            }

            GLNative.GLFlush();

            EGLNative.EGLFlush();

            App.SendMessage(typeof(Controller), typeof(RenderProcessor), "EndOfGraphics", null);
        }

        public override void Asleep()
        {
            EGLNative.EGLDestroy();
        }
    }
}
