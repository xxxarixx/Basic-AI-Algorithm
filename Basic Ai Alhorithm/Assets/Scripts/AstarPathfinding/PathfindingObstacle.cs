using Astar.Brain;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace Astar.pathfinding
{
    public class PathfindingObstacle : MonoBehaviour
    {
        private Vector2Int[] obstacleEdges;
        private Color obstacleGizmosCol = Color.yellow;
        public Vector3 offsetProjection = new Vector3();
        [ContextMenu(nameof(ProjectObstacleOnGrid))]
        public void ProjectObstacleOnGrid()
        {
            obstacleEdges = new Vector2Int[4] 
            {
                GetObstacleEdge(new Vector2Int(-1,1)), //left top corner
                GetObstacleEdge(new Vector2Int(1,1)), //right top corner
                GetObstacleEdge(new Vector2Int(-1,-1)), //left bottom corner
                GetObstacleEdge(new Vector2Int(1,-1)), //right botttom corner
            };
            Vector2Int leftBottom = obstacleEdges[2];
            Vector2Int rightTop = obstacleEdges[1];
            for (int x = leftBottom.x; x <= rightTop.x; x++)
            {
                for (int z = leftBottom.y; z <= rightTop.y; z++)
                {
                    //these are obstacles
                    AstarBrain.instance.AddObstacle(new Vector2Int(x, z));
                }
            }
        }
        private Vector2Int GetObstacleEdge(Vector2Int edgeDirection)
        {
            edgeDirection = new Vector2Int(Mathf.Clamp(edgeDirection.x, -1, 1), Mathf.Clamp(edgeDirection.y, -1, 1));
            Vector2Int obstacleGridCenter = AstarBrain.instance.WorldToGrid(transform.position);
            Debug.Log(obstacleGridCenter);
            return new Vector2Int(obstacleGridCenter.x + edgeDirection.x * Mathf.RoundToInt( (transform.localScale.x + offsetProjection.x) / 2),
                                obstacleGridCenter.y + edgeDirection.y * Mathf.RoundToInt( (transform.localScale.y + offsetProjection.y) / 2));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(
                Mathf.RoundToInt(transform.localScale.x),
                transform.localScale.y,
                Mathf.RoundToInt(transform.localScale.z)) + offsetProjection);
        }
    }
}

