using ETE.Geometry;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TestConsole.UnitTest
{
    public class GeometryTest
    {
        [DllImport("GLNative.dll")]
        public static extern void PushMatrix(Matrix4x4 matrix);

        [DllImport("GLNative.dll")]
        public static extern void PushMatrixPtr(ref Matrix4x4 matrix);

        [DllImport("GLNative.dll")]
        public static extern Matrix4x4 PopMatrix();

        [DllImport("GLNative.dll")]
        public static extern void PopMatrixPtr(ref Matrix4x4 matrix);

        public static void QuaternionTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);

            Quaternion qx = Quaternion.Euler(20, 0, 0);
            Quaternion qy = Quaternion.Euler(0, 60, 0);
            Quaternion qz = Quaternion.Euler(0, 0, 45);

            Vector3 vx = qx.eulerAngles;
            Vector3 vy = qy.eulerAngles;
            Vector3 vz = qz.eulerAngles;

            Matrix4x4 mx = qx.rotationMatrix;
            Matrix4x4 my = qy.rotationMatrix;
            Matrix4x4 mz = qz.rotationMatrix;

            Console.WriteLine("Euler X");
            Console.WriteLine(qx);
            Console.WriteLine(vx);
            Console.WriteLine(mx);

            Console.WriteLine("Euler Y");
            Console.WriteLine(qy);
            Console.WriteLine(vy);
            Console.WriteLine(my);

            Console.WriteLine("Euler Z");
            Console.WriteLine(qz);
            Console.WriteLine(vz);
            Console.WriteLine(mz);

            Quaternion q = Quaternion.Euler(20, 60, 45);
            Vector3 v = q.eulerAngles;
            Matrix4x4 m = q.rotationMatrix;
            Console.WriteLine("Euler XYZ");
            Console.WriteLine(q);
            Console.WriteLine(v);
            Console.WriteLine(m);

            //Vector3 p = new Vector3(1, 1, 1);
            //p = m.MultiplyVector(p);
            //Console.WriteLine(p);
        }

        public static void QuaternionAngleAxisTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("Test 1. Z 30");

            float inAngle = 30;
            Vector3 inAxis = new Vector3(0, 1, 1);
            Vector3 inEuler = new Vector3(0, 0, 30);

            Quaternion q1 = Quaternion.AngleAxis(inAngle, inAxis);
            Quaternion q2 = Quaternion.Euler(inEuler);

            Console.WriteLine(String.Format("{0} == {1}", q1, q2));

            Console.WriteLine("Test 2. to angle/axis");
            float angle = 0;
            Vector3 axis = Vector3.zero;
            q1.ToAngleAxis(out angle, out axis);

            Console.WriteLine(String.Format("{0} == {1}", inAngle, angle));
            Console.WriteLine(String.Format("{0} == {1}", inAxis, axis));
        }

        public static void MatrixTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("Matrix");
            Matrix4x4 m = Matrix4x4.Scale(new Vector3(-1, -2, -3));
            m[0, 1] = 1;
            m[0, 2] = 2;
            m[0, 3] = 3;
            m[1, 2] = 4;
            m[1, 3] = 5;
            m[2, 3] = 6;
            Console.WriteLine(m);

            m = m.transpose;
            Console.WriteLine("Transpose");
            Console.WriteLine(m);

            Matrix4x4 trs = Matrix4x4.TRS(new Vector3(10, 100, 200), Quaternion.Euler(0, 0, 45), new Vector3(2, 2, 2));
            Vector3 p = new Vector3(0.5f, 0.5f, 0.0f);
            Vector3 mp = trs.MultiplyPoint3x4(p);
            Console.WriteLine("TRS");
            Console.WriteLine(p);
            Console.WriteLine(trs);
            Console.WriteLine(mp);

            Console.WriteLine("Inverse");
            Matrix4x4 itrs = trs.inverse;
            Vector3 ip = itrs.MultiplyPoint3x4(mp);
            Console.WriteLine(itrs);
            Console.WriteLine(ip);
        }

        public static void MatrixPushPopTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);

            Matrix4x4 mt = Matrix4x4.TRS(new Vector3(-1234567, 100, 1000), Quaternion.identity, new Vector3(-2.2123121233f, 3, 4));
            Console.WriteLine(mt);
            PushMatrix(mt);

            mt = PopMatrix();
            Console.WriteLine(mt);
        }

        public static void MatrixPushPopPtrTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);

            Matrix4x4 mt = Matrix4x4.TRS(new Vector3(10, 100, 1000), Quaternion.identity, new Vector3(2, 3, 4));
            Console.WriteLine(mt);
            PushMatrixPtr(ref mt);

            PopMatrixPtr(ref mt);
            Console.WriteLine(mt);
        }

        public static void VectorOrthoNomalizeTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);

            Vector3 normal = new Vector3(1, 0, 0);
            Vector3 tangent = new Vector3(1, 1, 0);
            Vector3 binormal = new Vector3(1, 0, 1);

            Vector3.OrthoNormalize(ref normal, ref tangent, ref binormal);

            Console.WriteLine(normal);
            Console.WriteLine(tangent);
            Console.WriteLine(binormal);
        }

        public static void QuaternionLookForwardTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
            Vector3 forward = new Vector3(1, 1, 1);
            Vector3 upwards = Vector3.up;

            Quaternion q = Quaternion.LookRotation(forward, upwards);

            Console.WriteLine(q);
        }
    }
}
