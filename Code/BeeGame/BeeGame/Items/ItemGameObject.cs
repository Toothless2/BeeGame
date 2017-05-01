using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Terrain.Chunks;
using BeeGame.Blocks;
using UnityEngine;

namespace BeeGame.Items
{
    /// <summary>
    /// Interface between item and inity gameobjects
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(BoxCollider))]
    public class ItemGameObject : MonoBehaviour
    {
        /// <summary>
        /// Item that this gameobject repersents
        /// </summary>
        public Item item;
        /// <summary>
        /// GameObject to make
        /// </summary>
        public GameObject go;

        /// <summary>
        /// Makes the mesh or instantiates the items gameobject
        /// </summary>
        private void Start()
        {
            if (!item.usesGameObject)
                MakeMesh();

            if (item.usesGameObject)
            {
                Instantiate(item.GetGameObject(), transform, false);
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }

        /// <summary>
        /// Makes the items mesh
        /// </summary>
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
