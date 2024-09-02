using Astar.pathfinding;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
namespace Astar.Brain
{
    /// <summary>
    /// Is responsible for generating map and setup nodes for usage
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class AstarBrain : MonoBehaviour
    {
        public static AstarBrain instance { get; private set; }
        [Header("Map")]
        [SerializeField] private Transform obstacleFolder;
        [SerializeField] private Vector3Int _inspMapSize = new Vector3Int(100,1,100);
        [SerializeField] private Vector3 _InspCenter;
        public Vector3 center => _InspCenter;
       
        public Vector3Int mapSize => _inspMapSize;
        public ScanData lastScanData;
        public Dictionary<Vector2Int, AstarNode> grid { get; private set; } = new Dictionary<Vector2Int, AstarNode>();
        public int width => _inspMapSize.x;
        public int depth => _inspMapSize.z;
        [SerializeField]private float _InspGridSize = 1f;
        public float gridSize => _InspGridSize==0? 1: _InspGridSize;
        public bool setupped => grid.Count > 0;
        public bool debugModeOn = true;
        public bool scanOnValidate = true;
        /// <summary>
        /// Contains data about astar map node
        /// </summary>
        [System.Serializable]
        public class AstarNode
        {
            private readonly AstarBrain astarBrain;
            public readonly Vector2Int xz;
            public readonly Vector3 worldPosition;
            public int x => xz.x;
            public int z => xz.y;
            public bool walkable = true;
            public AstarNode(Vector2Int xz,AstarBrain astarBrain)
            {
                this.xz = xz;
                this.astarBrain = astarBrain;
                worldPosition = GridToWorld();
            }
            private Vector3 GridToWorld()
            {
                
                float gridX = astarBrain.center.x - (astarBrain.width / 2) + (x * astarBrain.gridSize);
                float gridZ = astarBrain.center.z - (astarBrain.depth / 2) + (z * astarBrain.gridSize);
                return new Vector3(gridX, astarBrain.center.y, gridZ);
            }
        }
        /// <summary>
        /// Stores data that was used to create Astar map scan, if values are different then inspector, hit Scan()
        /// </summary>
        public struct ScanData
        {
            public readonly Vector3 center;
            public readonly Vector3Int mapSize;
            public ScanData(Vector3 center, Vector3Int mapSize)
            {
                this.center = center;
                this.mapSize = mapSize;
            }
        }
        private void Awake()
        {
            instance = this;
            Scan();
        }
        private void OnValidate()
        {
            if (scanOnValidate) Scan();
        }
        /// <summary>
        /// Recreate entire map grid and project all obstacles that are in obstacle folder
        /// </summary>
        [ContextMenu(nameof(Scan))]
        public void Scan()
        {
            instance = this;
            grid.Clear();
            lastScanData = new ScanData(center: center, mapSize: mapSize);
            if (debugModeOn) 
                print("Started setuping Astar");
            DebugHowLongActionTake(action: () =>
            {
                SetupMap();
                AutoAddObstacles();
            }, out float elapesedTimeInMs);
            if (debugModeOn)
                print($"Done setuping Astar in {elapesedTimeInMs} ms");
        }
        /// <summary>
        /// Recreate entire map grid
        /// </summary>
        private void SetupMap()
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    Vector2Int nodeLocation = new Vector2Int(x, z);
                    grid[nodeLocation] = new AstarNode(xz: nodeLocation, astarBrain: this);
                }
            }
        }
        /// <summary>
        /// project all obstacles that are in obstacle folder
        /// </summary>
        private void AutoAddObstacles()
        {
            if (obstacleFolder == null)
                return;
            
            foreach (Transform obstacleTransform in obstacleFolder)
            {
                if (obstacleTransform.TryGetComponent(out PathfindingObstacle obstacle))
                {
                    obstacle.ProjectObstacleOnGrid();
                }
            }
            
        }
        /// <summary>
        /// Add single obstacle
        /// </summary>
        /// <param name="xz">grid position</param>
        public void AddObstacle(Vector2Int xz)
        {
            if (!IsInMapRange(xz)) return;
            grid[xz].walkable = false;
        }
        /// <summary>
        /// Add single obstacle
        /// </summary>
        /// <param name="xz">grid position</param>
        public void AddObstacle(AstarNode node)
        {
            if (!IsInMapRange(node.xz)) return;
            node.walkable = false;
        }
        /// <summary>
        /// World coordinates to astar map coordinates
        /// </summary>
        /// <returns>astar map coordinate CLOSEST to world position</returns>
        public Vector2Int WorldToGrid(Vector3 worldPos)
        {
            int closestGridX = Mathf.RoundToInt((worldPos.x - center.x + (width / 2)) / gridSize);
            int closestGridZ = Mathf.RoundToInt((worldPos.z - center.z + (depth / 2)) / gridSize);
            return new Vector2Int(
                closestGridX,
               closestGridZ);
        }
        public bool IsInMapRange(Vector2Int xz)
        {
            return grid.ContainsKey(xz);
        }
        public List<Vector2Int> _Get4NeighbourNodes(Vector2Int xz)
        {
            List<Vector2Int> nodes = new List<Vector2Int>();
            if (IsInMapRange(xz + new Vector2Int(0,1))) nodes.Add(xz + new Vector2Int(0, 1));
            if (IsInMapRange(xz + new Vector2Int(0,-1))) nodes.Add(xz + new Vector2Int(0, -1));
            if (IsInMapRange(xz + new Vector2Int(1,0))) nodes.Add(xz + new Vector2Int(1, 0));
            if (IsInMapRange(xz + new Vector2Int(-1,0))) nodes.Add(xz + new Vector2Int(-1, 0));
            return nodes;
        }
        public List<Vector2Int> Get8NeighbourNodes(Vector2Int xz)
        {
            List<Vector2Int> nodes = new List<Vector2Int>();
            if (IsInMapRange(xz + new Vector2Int(0, 1))) nodes.Add(xz + new Vector2Int(0, 1));
            if (IsInMapRange(xz + new Vector2Int(1, 1))) nodes.Add(xz + new Vector2Int(1, 1));
            if (IsInMapRange(xz + new Vector2Int(0, -1))) nodes.Add(xz + new Vector2Int(0, -1));
            if (IsInMapRange(xz + new Vector2Int(-1, -1))) nodes.Add(xz + new Vector2Int(-1, -1));
            if (IsInMapRange(xz + new Vector2Int(1, 0))) nodes.Add(xz + new Vector2Int(1, 0));
            if (IsInMapRange(xz + new Vector2Int(1, 1))) nodes.Add(xz + new Vector2Int(1, 1));
            if (IsInMapRange(xz + new Vector2Int(-1, 0))) nodes.Add(xz + new Vector2Int(-1, 0));
            if (IsInMapRange(xz + new Vector2Int(-1, -1))) nodes.Add(xz + new Vector2Int(-1, -1));
            return nodes;
        }
        public bool IsDiagonalNeighbor(Vector2Int xz1, Vector2Int xz2)
        {
            // Calculate the difference in x and y coordinates
            int dx = Math.Abs(xz2.x - xz1.x);
            int dz = Math.Abs(xz2.y - xz1.y);

            // Check if the differences are equal and non-zero (diagonal condition)
            return dx == dz && dx != 0;
        }

        public static void DebugHowLongActionTake(Action action, out float elapsedTimeInMs)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            action?.Invoke();
            sw.Stop();
            elapsedTimeInMs = sw.ElapsedMilliseconds;
        }
    }
}