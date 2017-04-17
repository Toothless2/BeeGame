using System;
using UnityEngine;
using BeeGame.Terrain.Chunks;
using BeeGame.Core.Enums;
using BeeGame.Core;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BeeGame.Items
{
    /// <summary>
    /// Base class for all Items and Blocks in the game
    /// </summary>
    [Serializable]
    public class Item : ICloneable
    {
        #region Data
        /// <summary>
        /// Name of the item
        /// </summary>
        internal string itemName = "Test Item";
        /// <summary>
        /// Is this item placeable. Saves checking if the item is a block type
        /// </summary>
        public bool placeable = false;
        /// <summary>
        /// Does the item use a gameobject
        /// </summary>
        public bool usesGameObject = false;
        /// <summary>
        /// How big are the texture tiles in the texture map (1/tile number x)
        /// </summary>
        private const float tileSize = 0.1f;

        /// <summary>
        /// Number of items in the stack
        /// </summary>
        public int itemStackCount = 1;
        /// <summary>
        /// Max number of items in a stack
        /// </summary>
        public int maxStackCount = 64;
        #endregion

        #region Constructors
        public Item()
        {
            itemName = "TestItem";
        }

        public Item(string name)
        {
            itemName = name;
        }
        #endregion

        #region Item Stuff
        /// <summary>
        /// Returns the <see cref="GameObject"/> for the item of it has one
        /// </summary>
        /// <returns>GameObject for the item</returns>
        public virtual GameObject GetGameObject() { return null; }

        /// <summary>
        /// Returns the id for the item as a string
        /// </summary>
        /// <returns></returns>
        public virtual string GetItemID()
        {
            return $"{GetHashCode()}";
        }

        /// <summary>
        /// Returns the sprite for the item
        /// </summary>
        /// <returns>Sprite for this item</returns>
        public virtual Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("TestSprite");
        }

        public virtual string GetItemName()
        {
            return $"{itemName}";
        }
        #endregion

        #region Item Mesh
        /// <summary>
        /// Texture postion of the items texture
        /// </summary>
        /// <param name="direction">Direction for the texture</param>
        /// <returns>Position of the texture</returns>
        public virtual Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 1, y = 9 };
        }

        /// <summary>
        /// Returns the mesh for the item
        /// </summary>
        /// <param name="x">X pos if the item</param>
        /// <param name="y">Y pos if the item</param>
        /// <param name="z">Z pos if the item</param>
        /// <param name="meshData">data to add the mesh to</param>
        /// <returns>given <see cref="MeshData"/> with the items mesh added</returns>
        public virtual MeshData ItemMesh(int x, int y, int z, MeshData meshData)
        {
            //adds all faces of the item to the mesh as all faces could be seen at any time
            meshData = FaceDataUp(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataDown(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataNorth(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataEast(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataSouth(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataWest(x, y, z, meshData, true, 0.25f);

            return meshData;
        }

        /// <summary>
        /// Sets the UVs for the given <see cref="Direction"/>
        /// </summary>
        /// <param name="direction">Direction to add the texture</param>
        /// <returns>Array of <see cref="Vector2"/> to add to the UVsreturns>
        public virtual Vector2[] FaceUVs(Direction direction)
        {
            //only 4 uvs per face
            Vector2[] UVs = new Vector2[4];
            Tile tilePos = TexturePosition(direction);

            //sets the UVs for each vertex
            UVs[0] = new THVector2(tileSize * tilePos.x + tileSize - 0.01f, tileSize * tilePos.y + 0.01f);
            UVs[1] = new THVector2(tileSize * tilePos.x + tileSize - 0.01f, tileSize * tilePos.y + tileSize - 0.01f);
            UVs[2] = new THVector2(tileSize * tilePos.x + 0.01f, tileSize * tilePos.y + tileSize - 0.01f);
            UVs[3] = new THVector2(tileSize * tilePos.x + 0.01f, tileSize * tilePos.y + 0.01f);

            return UVs;
        }

        /// <summary>
        /// Adds the Upwards face to the given <see cref="MeshData"/>
        /// </summary>
        /// <param name="x">X pos of the item</param>
        /// <param name="y">Y pos of the item</param>
        /// <param name="z">Z pos of the item</param>
        /// <param name="meshData"><see cref="MeshData"/> to add the face to</param>
        /// <param name="addToRenderMesh">Should the mesh be added to the render mesh (default true)</param>
        /// <param name="blockSize">how big is the item</param>
        /// <returns>Given <see cref="MeshData"/> with the face data added</returns>
        protected virtual MeshData FaceDataUp(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            //Adds vertices in a anti-clockwise order
            meshData.AddVertices(new THVector3(x - blockSize, y + blockSize, z + blockSize), addToRenderMesh, Direction.UP);
            meshData.AddVertices(new THVector3(x + blockSize, y + blockSize, z + blockSize), addToRenderMesh, Direction.UP);
            meshData.AddVertices(new THVector3(x + blockSize, y + blockSize, z - blockSize), addToRenderMesh, Direction.UP);
            meshData.AddVertices(new THVector3(x - blockSize, y + blockSize, z - blockSize), addToRenderMesh, Direction.UP);

            //adds teh tirs for the quad
            meshData.AddQuadTriangles(addToRenderMesh);

            //if the data should be added to the render mesh also add the uvs to the mesh
            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.UP));

            return meshData;
        }

        /// <summary>
        /// Adds the Bottom face to the given <see cref="MeshData"/>
        /// </summary>
        /// <param name="x">X pos of the item</param>
        /// <param name="y">Y pos of the item</param>
        /// <param name="z">Z pos of the item</param>
        /// <param name="meshData"><see cref="MeshData"/> to add the face to</param>
        /// <param name="addToRenderMesh">Should the mesh be added to the render mesh (default true)</param>
        /// <param name="blockSize">how big is the item</param>
        /// <returns>Given <see cref="MeshData"/> with the face data added</returns>
        protected virtual MeshData FaceDataDown(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            //Adds vertices in a anti-clockwise order
            meshData.AddVertices(new THVector3(x - blockSize, y - blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x + blockSize, y - blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x + blockSize, y - blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x - blockSize, y - blockSize, z + blockSize), addToRenderMesh);

            //adds teh tirs for the quad
            meshData.AddQuadTriangles(addToRenderMesh);

            //if the data should be added to the render mesh also add the uvs to the mesh
            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.DOWN));

            return meshData;
        }

        /// <summary>
        /// Adds the North face to the given <see cref="MeshData"/>
        /// </summary>
        /// <param name="x">X pos of the item</param>
        /// <param name="y">Y pos of the item</param>
        /// <param name="z">Z pos of the item</param>
        /// <param name="meshData"><see cref="MeshData"/> to add the face to</param>
        /// <param name="addToRenderMesh">Should the mesh be added to the render mesh (default true)</param>
        /// <param name="blockSize">how big is the item</param>
        /// <returns>Given <see cref="MeshData"/> with the face data added</returns>
        protected virtual MeshData FaceDataNorth(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            //Adds vertices in a anti-clockwise order
            meshData.AddVertices(new THVector3(x + blockSize, y - blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x + blockSize, y + blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x - blockSize, y + blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x - blockSize, y - blockSize, z + blockSize), addToRenderMesh);

            //adds teh tirs for the quad
            meshData.AddQuadTriangles(addToRenderMesh);

            //if the data should be added to the render mesh also add the uvs to the mesh
            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.NORTH));

            return meshData;
        }

        /// <summary>
        /// Adds the East face to the given <see cref="MeshData"/>
        /// </summary>
        /// <param name="x">X pos of the item</param>
        /// <param name="y">Y pos of the item</param>
        /// <param name="z">Z pos of the item</param>
        /// <param name="meshData"><see cref="MeshData"/> to add the face to</param>
        /// <param name="addToRenderMesh">Should the mesh be added to the render mesh (default true)</param>
        /// <param name="blockSize">how big is the item</param>
        /// <returns>Given <see cref="MeshData"/> with the face data added</returns>
        protected virtual MeshData FaceDataEast(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            //Adds vertices in a anti-clockwise order
            meshData.AddVertices(new THVector3(x + blockSize, y - blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x + blockSize, y + blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x + blockSize, y + blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x + blockSize, y - blockSize, z + blockSize), addToRenderMesh);

            //adds teh tirs for the quad
            meshData.AddQuadTriangles(addToRenderMesh);

            //if the data should be added to the render mesh also add the uvs to the mesh
            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.EAST));

            return meshData;
        }

        /// <summary>
        /// Adds the South face to the given <see cref="MeshData"/>
        /// </summary>
        /// <param name="x">X pos of the item</param>
        /// <param name="y">Y pos of the item</param>
        /// <param name="z">Z pos of the item</param>
        /// <param name="meshData"><see cref="MeshData"/> to add the face to</param>
        /// <param name="addToRenderMesh">Should the mesh be added to the render mesh (default true)</param>
        /// <param name="blockSize">how big is the item</param>
        /// <returns>Given <see cref="MeshData"/> with the face data added</returns>
        protected virtual MeshData FaceDataSouth(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            //Adds vertices in a anti-clockwise order
            meshData.AddVertices(new THVector3(x - blockSize, y - blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x - blockSize, y + blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x + blockSize, y + blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x + blockSize, y - blockSize, z - blockSize), addToRenderMesh);

            //adds teh tirs for the quad
            meshData.AddQuadTriangles(addToRenderMesh);

            //if the data should be added to the render mesh also add the uvs to the mesh
            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.SOUTH));

            return meshData;
        }

        /// <summary>
        /// Adds the West face to the given <see cref="MeshData"/>
        /// </summary>
        /// <param name="x">X pos of the item</param>
        /// <param name="y">Y pos of the item</param>
        /// <param name="z">Z pos of the item</param>
        /// <param name="meshData"><see cref="MeshData"/> to add the face to</param>
        /// <param name="addToRenderMesh">Should the mesh be added to the render mesh (default true)</param>
        /// <param name="blockSize">how big is the item</param>
        /// <returns>Given <see cref="MeshData"/> with the face data added</returns>
        protected virtual MeshData FaceDataWest(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            //Adds vertices in a anti-clockwise order
            meshData.AddVertices(new THVector3(x - blockSize, y - blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x - blockSize, y + blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x - blockSize, y + blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new THVector3(x - blockSize, y - blockSize, z - blockSize), addToRenderMesh);

            //adds teh tirs for the quad
            meshData.AddQuadTriangles(addToRenderMesh);

            //if the data should be added to the render mesh also add the uvs to the mesh
            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.WEST));

            return meshData;
        }
        #endregion

        #region Interfaces
        /// <summary>
        /// Slow try no to use. Instead use <see cref="Extensions.CloneObject{T}(T)"/>
        /// </summary>
        /// <returns>A deep copy of this</returns>
        public object Clone()
        {
            //Saves this to a file then reads it back so that a copy and not a reference is passed
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);

            return bf.Deserialize(ms);
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the item name an id formatted nicely
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{itemName} \nID: {GetItemID()}";
        }

        /// <summary>
        /// Returns the hashcode for the item
        /// </summary>
        /// <returns>1</returns>
        public override int GetHashCode()
        {
            return 1;
        }

        /// <summary>
        /// Checks if the item is equal to another
        /// </summary>
        /// <param name="obj">object to check against</param>
        /// <returns>true if items are the same</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Item))
                return false;

            return this == (obj as Item);
        }

        /// <summary>
        /// Overides the default == operator as different things need to be checked
        /// </summary>
        /// <param name="a">Item</param>
        /// <param name="b">Item</param>
        /// <returns>true if <paramref name="a"/>  == <paramref name="b"/></returns>
        public static bool operator ==(Item a, Item b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            if(a.GetItemID() == b.GetItemID())
                return true;

            return false;
        }

        /// <summary>
        /// Inverse of <see cref="=="/>
        /// </summary>
        /// <param name="a">Item</param>
        /// <param name="b">Item</param>
        /// <returns>True if <paramref name="a"/> != <paramref name="b"/></returns>
        public static bool operator !=(Item a, Item b)
        {
            return !(a == b);
        }
        #endregion
    }

    /// <summary>
    /// Position of the items texture
    /// </summary>
    [Serializable]
    public struct Tile
    {
        /// <summary>
        /// X pos of the texture
        /// </summary>
        public int x;
        /// <summary>
        /// Y pos of the texture
        /// </summary>
        public int y;
    }
}
