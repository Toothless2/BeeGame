using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Terrain.Chunks;
using BeeGame.Blocks;
using UnityEngine;

namespace BeeGame.Items
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(BoxCollider))]
    public class ItemGameObject : MonoBehaviour
    {
        public Item item;
        public GameObject go;

        private void Start()
        {
            if (!item.usesGameObject)
                MakeMesh();

            if (item.usesGameObject)
            {
                GetComponent<BoxCollider>().enabled = false;
                Instantiate(item.GetGameObject(), transform, false);
            }
        }

        void MakeMesh()
        {
            MeshData meshData = new MeshData();
            if(item != null)
                meshData = item.ItemMesh(0, 0, 0, meshData);

            Mesh mesh = new Mesh()
            {
                vertices = meshData.verts.ToArray(),
                triangles = meshData.tris.ToArray(),
                uv = meshData.uv.ToArray()
            };

            mesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = mesh;
        }
    }
}
