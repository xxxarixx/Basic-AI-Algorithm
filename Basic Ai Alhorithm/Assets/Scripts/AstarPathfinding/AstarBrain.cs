using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace Astar.Brain
{
    [DefaultExecutionOrder(-1)]
    public class AstarBrain : MonoBehaviour
    {
        public static AstarBrain instance { get; private set; }
        [Header("Map")]
        [SerializeField]private Vector3Int _mapSize = new Vector3Int(100,1,100);
        [SerializeField] private Vector3 _center;
        public Vector3 center => _center;
        public Vector3 mapSize => _mapSize;
        public Dictionary<Vector2Int, AstarNode> grid { get; private set; } = new Dictionary<Vector2Int, AstarNode>();
        public int width => _mapSize.x;
        public int depth => _mapSize.z;
        public const float gridSize = 1f;
        public bool setupped => grid.Count > 0;
        public bool debugModeOn = true;
        public bool scanOnValidate = true;
        private void Awake()
        {
            instance = this;
            Scan();
        }
        private void OnValidate()
        {
            if (scanOnValidate) Scan();
        }
        [ContextMenu(nameof(Scan))]
        public void Scan()
        {
            if(debugModeOn) 
                Debug.Log("Started setuping Astar");
            DebugHowLongActionTake(action: () =>
            {
                for (int x = 0; x < mapSize.x; x++)
                {
                    for (int z = 0; z < mapSize.z; z++)
                    {
                        Vector2Int nodeLocation = new Vector2Int(x, z);
                        grid[nodeLocation] = new AstarNode(xz:nodeLocation,astarBrain:this);
                    }
                }
            }, out float elapesedTimeInMs);
            if (debugModeOn) 
                Debug.Log($"Done setuping Astar in {elapesedTimeInMs} ms");
        }
        public void AddObstacle(Vector2Int xz)
        {
            grid[xz].walkable = false;
        }
        public void AddObstacle(AstarNode node)
        {
            node.walkable = false;
        }
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
    }
}