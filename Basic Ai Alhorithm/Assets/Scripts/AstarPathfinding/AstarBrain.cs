using Astar.pathfinding;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
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
        public Vector3 mapSize => _inspMapSize;
        public Dictionary<Vector2Int, AstarNode> grid { get; private set; } = new Dictionary<Vector2Int, AstarNode>();
        public int width => _inspMapSize.x;
        public int depth => _inspMapSize.z;
        public const float gridSize = 1f;
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
                return new Vector3(
                    gridSize * (x - astarBrain.width / 2),
                    astarBrain.center.y,
                    gridSize * (z - astarBrain.depth / 2)) + astarBrain.center;
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
            if (debugModeOn) 
                Debug.Log("Started setuping Astar");
            DebugHowLongActionTake(action: () =>
            {
                SetupMap();
                AutoAddObstacles();
            }, out float elapesedTimeInMs);
            if (debugModeOn) 
                Debug.Log($"Done setuping Astar in {elapesedTimeInMs} ms");
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
            worldPos += new Vector3(width / 2,0, depth / 2);
            worldPos -= _InspCenter;
            return new Vector2Int(
                Mathf.RoundToInt(worldPos.x * gridSize),
                Mathf.RoundToInt(worldPos.z * gridSize)
                );
        }
        public bool IsInMapRange(Vector2Int xz)
        {
            return grid.ContainsKey(xz);
        }
        public List<Vector2Int> _GetNeighbour4Nodes(Vector2Int xz)
        {
            List<Vector2Int> nodes = new List<Vector2Int>();
            if (IsInMapRange(xz + new Vector2Int(0,1))) nodes.Add(xz + new Vector2Int(0, 1));
            if (IsInMapRange(xz + new Vector2Int(0,-1))) nodes.Add(xz + new Vector2Int(0, -1));
            if (IsInMapRange(xz + new Vector2Int(1,0))) nodes.Add(xz + new Vector2Int(1, 0));
            if (IsInMapRange(xz + new Vector2Int(-1,0))) nodes.Add(xz + new Vector2Int(-1, 0));
            return nodes;
        }
        public List<Vector2Int> _GetNeighbour8Nodes(Vector2Int xz)
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
    }
}