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
        private void OnValidate()
        {
            astarBrain = GetComponent<AstarBrain>();
        }

        private void OnDrawGizmos()
        {
            if (!astarBrain.setupped)
                astarBrain.Scan();
            VisualizeBounds();
            VisualizeGrid();
        }
        [ContextMenu(nameof(AddObstacle))]
        public void AddObstacle()
        {
            astarBrain.AddObstacle(sampleObstacleLocation);
        }
        private void VisualizeBounds()
        {
            Gizmos.DrawWireCube(astarBrain.center, astarBrain.mapSize);
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
