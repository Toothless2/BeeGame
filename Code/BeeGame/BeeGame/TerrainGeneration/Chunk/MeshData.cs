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

        /// <summary>
        /// Should the same mesh be used for the collider and the renderer
        /// </summary>
        public bool useRenderForColData = true;

        public MeshData(){}

        /// <summary>
        /// Makes triangles out of the previous 4 given verts
        /// </summary>
        /// <param name="addToRender">Should the triangles also be added to the render list</param>
        public void AddQuadTriangles(bool addToRender)
        {
            if (addToRender)
            {
                //makes 2 triangles from the last 4 verts given, assumes that they are in the correct order
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

        /// <summary>
        /// Adds a triangles to the triangle list
        /// </summary>
        /// <param name="tri">triangle index </param>
        public void AddTriangles(int tri)
        {
            tris.Add(tri);

            colTris.Add(tri - (verts.Count - colVerts.Count));
        }

        /// <summary>
        /// Adds a vertex to the vertex list
        /// </summary>
        /// <param name="vertex">vertex position within the mesh</param>
        /// <param name="addToRender">should the vertex also be added to the render mesh?</param>
        public void AddVertex(THVector3 vertex, bool addToRender)
        {
            if(addToRender)
                verts.Add(vertex);

            colVerts.Add(vertex);
        }
    }
}
