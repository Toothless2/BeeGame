using System.Collections.Generic;
using UnityEngine;
using BeeGame.Core.Enums;
using BeeGame.Core;

namespace BeeGame.Terrain.Chunks
{
    /// <summary>
    /// The data for a <see cref="Chunks"/>'s <see cref="Mesh"/>
    /// </summary>
    public class MeshData
    {
        /// <summary>
        /// Verticies for the <see cref="Chunk"/> render <see cref="Mesh"/>
        /// </summary>
        public List<Vector3> verts = new List<Vector3>();
        /// <summary>
        /// Triangles for the <see cref="Chunk"/> render <see cref="Mesh"/>
        /// </summary>
        public List<int> tris = new List<int>();
        /// <summary>
        /// UV mapping for the <see cref="Chunk"/> render <see cref="Mesh"/>
        /// </summary>
        public List<Vector2> uv = new List<Vector2>();

        /// <summary>
        /// Vertices for the <see cref="Chunk"/> collider <see cref="Mesh"/>
        /// </summary>
        public List<Vector3> colVerts = new List<Vector3>();
        /// <summary>
        /// Triangles for the <see cref="Chunk"/> collider <see cref="Mesh"/>
        /// </summary>
        public List<int> colTris = new List<int>();

        /// <summary>
        /// Should thic chunk share is collider and render <see cref="Mesh"/>es
        /// </summary>
        public bool shareMeshes = true;

        public bool done = false;

        /// <summary>
        /// Adds 2 triangles to the triangle list
        /// </summary>
        /// <param name="addToRenderMesh">Should the triangles be added to the render <see cref="Mesh"/></param>
        public void AddQuadTriangles(bool addToRenderMesh = true)
        {
            //*adds the triangles in an anticlockwise order

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

        /// <summary>
        /// Adds vertices to the render and collision <see cref="Mesh"/>es
        /// </summary>
        /// <param name="pos">Position of the vertice</param>
        /// <param name="addToRenderMesh">Should the vertice be added to the render <see cref="Mesh"/></param>
        /// <param name="direction">What face is this vertice on</param>
        public void AddVertices(THVector3 pos, bool addToRenderMesh = true, Direction direction = Direction.DOWN)
        {
            if (addToRenderMesh)
                verts.Add(pos);

            //*if the vertice is on the top face make its positon slightly smaller
            if(direction == Direction.UP)
                colVerts.Add(pos - new THVector3(0.01f, 0, 0.01f));
        }

        /// <summary>
        /// Adds a triangle to both the render and collidson meshes
        /// </summary>
        /// <param name="tri">triangle</param>
        /// <remarks>
        /// not used anymore remove?
        /// </remarks>
        public void AddTriangle(int tri)
        {
            tris.Add(tri);

            colTris.Add(tri - (verts.Count - colVerts.Count));
        }
    }
}
