using System;

using ETE.Engine;
using ETE.Geometry;

using ETE.Render.Component;
using ETE.Render.Data;

namespace RenderSample.UserControl
{
    class CreateMesh : UserBehavior
    {
        Mesh mesh;
        Material material;
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;

        Transform transform;

        Vector3[] temVertex = new Vector3[8];
        Vector3[] temNormals = new Vector3[8];
        ushort[] temIndex = new ushort[36];

        public void Start()
        {
            temVertex[0] = new Vector3(-10.0f, 10.0f, 10.0f);
            temVertex[1] = new Vector3(10.0f, -10.0f, 10.0f);
            temVertex[2] = new Vector3(10.0f, 10.0f, 10.0f);
            temVertex[3] = new Vector3(-10.0f, -10.0f, 10.0f);
            temVertex[4] = new Vector3(-10.0f, 10.0f, -10.0f);
            temVertex[5] = new Vector3(10.0f, -10.0f, -10.0f);
            temVertex[6] = new Vector3(10.0f, 10.0f, -10.0f);
            temVertex[7] = new Vector3(-10.0f, -10.0f, -10.0f);

            temNormals[0] = new Vector3(-1.0f, 1.0f, 1.0f);
            temNormals[1] = new Vector3(1.0f, -1.0f, 1.0f);
            temNormals[2] = new Vector3(1.0f, 1.0f, 1.0f);
            temNormals[3] = new Vector3(-1.0f, -1.0f, 1.0f);
            temNormals[4] = new Vector3(-1.0f, 1.0f, -1.0f);
            temNormals[5] = new Vector3(1.0f, -1.0f, -1.0f);
            temNormals[6] = new Vector3(1.0f, 1.0f, -1.0f);
            temNormals[7] = new Vector3(-1.0f, -1.0f, -1.0f);

            temIndex = new ushort[36] {
                                       0, 3, 1, 2, 0, 1, /* front  */
                                       6, 5, 4, 5, 7, 4, /* back   */
                                       4, 7, 3, 0, 4, 3, /* left   */
                                       2, 1, 5, 6, 2, 5, /* right  */
                                       4, 0, 2, 6, 4, 2, /* top    */
                                       3, 7, 1, 7, 5, 1  /* bottom */ 
                                       };

            // mesh를 생성합니다.
            mesh = new ETE.Render.Data.Mesh();
            mesh.vertices = temVertex;
            mesh.normals = temVertex;
            mesh.indices = temIndex;
            //Dirty처리해주지 않으면 메쉬가 생성, 업데이트되지 않습니다. Mesh데이터를 수정하였다면 항상 Dirty해주세요.
            mesh.DirtyMeshData();  


            // Shader를 생성합니다.
            RenderAsset.Load("shader/colormaterial.sh", typeof(Shader));
            RenderShaderMetaInfo[] shaderInfoArray = RenderAsset.GetMetaObjectsOfTypeAll("shader/colormaterial.sh", typeof(Shader)) as RenderShaderMetaInfo[];
            
            // Material을 생성합니다.
            material = new ETE.Render.Data.Material(shaderInfoArray[0].dataObject as Shader);
            material.Ambient = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            material.Diffuse = new Vector4(0.6f, 0.6f, 0.6f, 1.0f);
            material.Specular = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            material.Emission = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

            // MeshFilter를 생성합니다
            meshFilter = SimObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            // MeshRenderer를 생성합니다.
            meshRenderer = SimObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;

            // Transform을 수정합니다.
            transform = GetComponent<Transform>();
            transform.LocalScale = new Vector3(2.0f, 0.3f, 2.0f);
            transform.Position = new Vector3(0.0f, -4.0f, 0.0f);
        }

        [MessageHandler]
        public void Update()
        {

        }
    }
}
