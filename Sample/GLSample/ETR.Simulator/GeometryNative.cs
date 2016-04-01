using ETE.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ETR.Simulator
{

    public class GeometryNative
    {
        [DllImport("GLNative.dll")]
        public static extern void GeometryGetVectors([In,Out] Vector3[] vectors, int size);


        public static void Run()
        {
            Vector3[] vectors = new Vector3[12];
            for(int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector3();
                vectors[i].x = i;
                vectors[i].y = i;
                vectors[i].z = i;
            }

            GeometryGetVectors(vectors, vectors.Length);
        }
    }    
}
