using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;

namespace BeeGame.TerrainGeneration
{
    public class MeshData
    {
        /// <summary>
        /// Mesh Verts
        /// </summary>
        public List<THVector3> verts = new List<THVector3>();
        /// <summary>
        /// Mesh Tris
        /// </summary>
        public List<int> tris = new List<int>();
        /// <summary>
        /// Texture Coords
        /// </summary>
        public List<THVector2> uv = new List<THVector2>();
        /// <summary>
        /// Collider Verts
        /// </summary>
        public List<THVector3> colVerts = new List<THVector3>();
        /// <summary>
        /// Collider Tris
        /// </summary>
        public List<int> colTris = new List<int>();

        public TrianglePositions[,,] trianglePositions = new TrianglePositions[16, 16, 16];

        public bool useRenderForColData = true;

        public MeshData()
        {

        }

        public void AddQuadTriangles(bool addToRender)
        {
            if (addToRender)
            {
                //makes 2 triangles from the last 4 verts given, assumes that tey are in the correct order
                tris.Add(verts.Count - 4);
                tris.Add(verts.Count - 3);
                tris.Add(verts.Count - 2);
                tris.Add(verts.Count - 4);
                tris.Add(verts.Count - 2);
                tris.Add(verts.Count - 1);
            }

            colTris.Add(verts.Count - 4);
            colTris.Add(verts.Count - 3);
            colTris.Add(verts.Count - 2);
            colTris.Add(verts.Count - 4);
            colTris.Add(verts.Count - 2);
            colTris.Add(verts.Count - 1);

        }

        public void AddTriangles(int tri)
        {
            tris.Add(tri);

            colTris.Add(tri - (verts.Count - colVerts.Count));
        }

        public void AddVertex(THVector3 vertex, bool addToRender)
        {
            if(addToRender)
                verts.Add(vertex);

            colVerts.Add(vertex);
        }

        public void AddCompliexMesh(Mesh mesh, int x, int y, int z)
        {
            THVector3[] verts = new THVector3[mesh.vertexCount];

            //moves the verts to the correct place in the mesh
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                verts[i].x = mesh.vertices[i].x + x;
                verts[i].y = mesh.vertices[i].y + y;
                verts[i].z = mesh.vertices[i].z + z;
            }

            //saves the triangle positions so the model can be made into a submesh later
            trianglePositions[x, y, z] = new TrianglePositions() { start = tris.Count, end = tris.Count + mesh.triangles.Length};

            //adds the verts to the list
            this.verts.AddRange(verts);
            //adds the triangles to the list
            tris.AddRange(mesh.triangles);
        }

        public Vector2[] GetCorrectUV(Mesh mesh)
        {
            Vector2[] uvs = mesh.uv;

            return uvs;
        }

        public int[] GetCorrectedTriangles(Mesh mesh, int x, int y, int z)
        {
            int[] pos = new int[trianglePositions[x, y, z].end - trianglePositions[x, y, z].start];

            for (int i = 0, j = trianglePositions[x, y, z].start; j < trianglePositions[x, y, z].end; i++, j++)
            {
                pos[i] = tris[j];
            }

            return pos;
        }

        public struct TrianglePositions
        {
            public int start;
            public int end;
        }
    }
}
