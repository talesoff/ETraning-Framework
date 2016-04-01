using ETE.Engine;
using ETE.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.UnitTest
{
    public static class TransformTest
    {
        public static void MatrixTest()
        {
            Scene scene = new Scene();
            Transform transform = scene.AddObject().GetComponent<Transform>();

            //transform.LocalPosition = new Vector3(100, 10, 0);
            //transform.LocalEulerAngles = new Vector3(0, 0, 45);
            //transform.LocalScale = new Vector3(2, 2, 1);

            //Matrix4x4 m = transform.LocalToWorldMatrix;
            //Matrix4x4 im = transform.WorldToLocalMatrix;

            //Matrix4x4 r = m * im;

            //Console.WriteLine(r);

            Matrix4x4 m0 = Matrix4x4.TRS(new Vector3(0, 2, 0), Quaternion.identity, Vector3.one);
            Matrix4x4 m1 = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1,3,2));

            Matrix4x4 r = m0 * m1;
        }

        public static void RotationTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);

            Scene scene = new Scene();
            SimulationObject so0 = scene.AddObject();
            so0.Name = "0";
            Transform transform0 = so0.GetComponent<Transform>();

            SimulationObject so1 = scene.AddObject();
            so1.Name = "1";
            Transform transform1 = so1.GetComponent<Transform>();

            transform1.SetParent(transform0);

            transform1.LocalRotation = Quaternion.Euler(45, 30, 15);
            transform0.LocalRotation = Quaternion.Euler(15, 30, 45);

            Console.WriteLine(so0.Name + " " + transform0.LocalRotation + " " + transform0.LocalEulerAngles);
            Console.WriteLine(so0.Name + " " + transform0.Rotation + " " + transform0.EulerAngles);

            Console.WriteLine(so1.Name + " " + transform1.LocalRotation + " " + transform1.LocalEulerAngles);
            Console.WriteLine(so1.Name + " " + transform1.Rotation + " " + transform1.EulerAngles);
        }

        public static void ScaleTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);

            Scene scene = new Scene();
            SimulationObject so0 = scene.AddObject();
            so0.Name = "0";
            Transform transform0 = so0.GetComponent<Transform>();

            SimulationObject so1 = scene.AddObject();
            so1.Name = "1";
            Transform transform1 = so1.GetComponent<Transform>();

            transform1.SetParent(transform0);

            transform1.LocalScale = new Vector3(4, 2, 1);
            Console.WriteLine(so1.Name + " " + transform1.LocalScale + " " + transform1.LossyScale);

            transform0.LocalScale = new Vector3(1, 2, 4);

            Console.WriteLine(so0.Name + " " + transform0.LocalScale + " " + transform0.LossyScale);
            Console.WriteLine(so1.Name + " " + transform1.LocalScale + " " + transform1.LossyScale);

            ETE.Engine.Object.DestroyImmediate(so0);
        }

        public static void HierarchyTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);

            Scene scene = new Scene();
            SimulationObject so0 = scene.AddObject();
            so0.Name = "0";
            Transform transform0 = so0.GetComponent<Transform>();

            SimulationObject so1 = scene.AddObject();
            so1.Name = "1";
            Transform transform1 = so1.GetComponent<Transform>();

            transform1.SetParent(transform0);

            transform1.LocalScale = new Vector3(1, 3, 2);
            transform0.LocalPosition = new Vector3(0, 2, 0);

            Console.WriteLine(so0.Name + "\n" + transform0.LocalToWorldMatrix);
            Console.WriteLine(so1.Name + "\n" + transform1.LocalToWorldMatrix);

            ETE.Engine.Object.DestroyImmediate(so0);
        }

        public static void HierarchyPositionTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);


            Scene scene = new Scene();
            SimulationObject so0 = scene.AddObject();
            so0.Name = "0";
            Transform transform0 = so0.GetComponent<Transform>();

            SimulationObject so1 = scene.AddObject();
            so1.Name = "1";
            Transform transform1 = so1.GetComponent<Transform>();

            transform0.LocalPosition = new Vector3(0, 10, 0);
            transform1.LocalPosition = new Vector3(0, 5, 0);

            transform1.SetParent(transform0);

            Console.WriteLine(so0.Name + "\n" + transform0.LocalPosition + " " + transform0.Position);
            Console.WriteLine(so1.Name + "\n" + transform1.LocalPosition + " " + transform1.Position);

            transform1.Position = new Vector3(0, 20, 0);

            Console.WriteLine(so0.Name + "\n" + transform0.LocalPosition + " " + transform0.Position);
            Console.WriteLine(so1.Name + "\n" + transform1.LocalPosition + " " + transform1.Position);
        }


        public static void HierarchyRotationTest()
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);


            Scene scene = new Scene();
            SimulationObject so0 = scene.AddObject();
            so0.Name = "0";
            Transform transform0 = so0.GetComponent<Transform>();

            SimulationObject so1 = scene.AddObject();
            so1.Name = "1";
            Transform transform1 = so1.GetComponent<Transform>();

            transform0.LocalRotation = Quaternion.Euler(15, 30, 45);
            transform1.LocalRotation = Quaternion.Euler(45, 30, 15);

            transform1.SetParent(transform0);

            Console.WriteLine(so0.Name + "\n" + transform0.LocalRotation.eulerAngles + " " + transform0.Rotation.eulerAngles);
            Console.WriteLine(so1.Name + "\n" + transform1.LocalRotation.eulerAngles + " " + transform1.Rotation.eulerAngles);

            transform1.Rotation = Quaternion.Euler(0, 0, 0);

            Console.WriteLine(so0.Name + "\n" + transform0.LocalRotation.eulerAngles + " " + transform0.Rotation.eulerAngles);
            Console.WriteLine(so1.Name + "\n" + transform1.LocalRotation.eulerAngles + " " + transform1.Rotation.eulerAngles);
        }
    }
}
