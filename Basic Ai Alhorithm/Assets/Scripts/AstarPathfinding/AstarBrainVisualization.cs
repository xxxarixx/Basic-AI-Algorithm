using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Astar.Brain
{
    [ExecuteInEditMode,RequireComponent(typeof(AstarBrain))]
    public class AstarBrainVisualization : MonoBehaviour
    {
        private AstarBrain astarBrain;
        [SerializeField] private float visNodeScale = 0.5f;
        [SerializeField]private Color walkableGridCol = Color.white;
        [SerializeField]private Color blokedGridCol = Color.white;
        [SerializeField] private Vector2Int sampleObstacleLocation = new Vector2Int();
        [SerializeField] private GameObject Go_worldToGridVis;
        private void OnValidate()
        {
            astarBrain = GetComponent<AstarBrain>();
        }

        private void OnDrawGizmosSelected()
        {
            if (!astarBrain.setupped)
                astarBrain.Scan();
            VisualizeBounds();
            VisualizeGrid();
            WorldToGridVis();
        }
        private void WorldToGridVis()
        {
            if (Go_worldToGridVis == null)
                return;
            Gizmos.color = Color.red;
            var node = astarBrain.WorldToGrid(Go_worldToGridVis.transform.position);
            if (!astarBrain.grid.ContainsKey(node))
                return;
            Gizmos.DrawWireCube(astarBrain.grid[node].worldPosition, visNodeScale * Vector3.one);
        }
        [ContextMenu(nameof(AddObstacle))]
        public void AddObstacle()
        {
            astarBrain.AddObstacle(sampleObstacleLocation);
        }
        private void VisualizeBounds()
        {
            var offset = (astarBrain.center - astarBrain.lastScanData.center);
            var node = new Vector2Int(Mathf.RoundToInt(astarBrain.lastScanData.mapSize.x / 2), Mathf.RoundToInt(astarBrain.lastScanData.mapSize.z / 2));
            var center = astarBrain.grid[node].worldPosition;
            Gizmos.DrawWireCube(center + offset, (Vector3)astarBrain.mapSize * astarBrain.gridSize);
        }
        private void VisualizeGrid()
        {
            foreach (var astarNode in astarBrain.grid.Values)
            {
                Gizmos.color = astarNode.walkable?walkableGridCol : blokedGridCol;
                Gizmos.DrawWireSphere(astarNode.worldPosition, visNodeScale);
            }
        }
    }
}
