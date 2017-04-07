using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.Terrain.Chunks
{
    public class MeshData
    {
        public List<Vector3> verts = new List<Vector3>();
        public List<int> tris = new List<int>();
        public List<Vector2> uv = new List<Vector2>();

        public List<Vector3> colVerts = new List<Vector3>();
        public List<int> colTris = new List<int>();

        public bool shareMeshes = true;

        public void AddQuadTriangles(bool addToRenderMesh = true)
        {
            if (addToRenderMesh)
            {
                tris.Add(verts.Count - 4);
                tris.Add(verts.Count - 3);
                tris.Add(verts.Count - 2);
                tris.Add(verts.Count - 4);
                tris.Add(verts.Count - 2);
                tris.Add(verts.Count - 1);
            }

            colTris.Add(colVerts.Count - 4);
            colTris.Add(colVerts.Count - 3);
            colTris.Add(colVerts.Count - 2);
            colTris.Add(colVerts.Count - 4);
            colTris.Add(colVerts.Count - 2);
            colTris.Add(colVerts.Count - 1);
        }

        public void AddVertices(Vector3 pos, bool addToRenderMesh = true)
        {
            if (addToRenderMesh)
                verts.Add(pos);

            colVerts.Add(pos);
        }

        public void AddTriangle(int tri)
        {
            tris.Add(tri);

            colTris.Add(tri - (verts.Count - colVerts.Count));
        }
    }
}
