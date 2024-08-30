using Astar.Brain;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;
namespace Astar.pathfinding
{
    public class PathfindingObstacle : MonoBehaviour
    {
        private Vector2Int[] obstacleEdges;
        private Color obstacleGizmosCol = Color.yellow;
        public Vector3 offsetScaleProjection = new Vector3();
        [SerializeField] private Vector3 offsetProjection;
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
            var maxX = GetObstaclePathfindingSize().x % 2 == 1 ? rightTop.x : rightTop.x + 1;
            var maxZ = GetObstaclePathfindingSize().x % 2 == 1 ? rightTop.y : rightTop.y + 1;
            for (int x = leftBottom.x; x < maxX; x++)
            {
                for (int z = leftBottom.y; z < maxZ; z++)
                {
                    //these are obstacles
                    AstarBrain.instance.AddObstacle(new Vector2Int(x, z));
                }
            }
        }
        private Vector3Int GetObstaclePathfindingSize()
        {
            return new Vector3Int(Mathf.RoundToInt(transform.localScale.x + offsetScaleProjection.x) / 2, 0, Mathf.RoundToInt((transform.localScale.z + offsetScaleProjection.z) / 2));
        }
        private Vector2Int GetObstacleEdge(Vector2Int edgeDirection)
        {
            edgeDirection = new Vector2Int(Mathf.Clamp(edgeDirection.x, -1, 1), Mathf.Clamp(edgeDirection.y, -1, 1));
            Vector2Int obstacleGridCenter = AstarBrain.instance.WorldToGrid(transform.position + offsetProjection);
            return new Vector2Int(obstacleGridCenter.x + edgeDirection.x * Mathf.RoundToInt( (transform.localScale.x + offsetScaleProjection.x) / 2),
                                obstacleGridCenter.y + edgeDirection.y * Mathf.RoundToInt( (transform.localScale.z + offsetScaleProjection.z) / 2));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position + offsetProjection, new Vector3(
                Mathf.RoundToInt(transform.localScale.x + offsetScaleProjection.x),
                transform.localScale.y + offsetScaleProjection.y,
                Mathf.RoundToInt(transform.localScale.z + offsetScaleProjection.z)));
        }
    }
}

