using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Astar.Brain
{
    /// <summary>
    /// Pathfinding based  on this article:
    /// https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2
    /// </summary>
    public class AstarPathfinding : MonoBehaviour
    {
        private List<Vector2Int> savedPathForGizmos = new List<Vector2Int>();
        private Coroutine curActivePath;
        [SerializeField]private Transform destinationPoint;
        private class NodeFlag
        {
            public readonly Vector2Int xz;
            /// <summary>
            /// distance to starting node
            /// </summary>
            public float gCost;
            /// <summary>
            ///  +- distance to destination point
            /// </summary>
            public float hCost;
            /// <summary>
            /// total +- cost of path to destination
            /// </summary>
            public float fCost => gCost + hCost;
            public NodeFlag(Vector2Int xz, NodeFlag lastNode,Vector2Int destNode)
            {
                this.xz = xz;
                if(lastNode == null)
                    lastNode = this;
                gCost = Vector2Int.Distance(xz, lastNode.xz);
                hCost = Vector2Int.Distance(xz, destNode);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>closest path points to destination</returns>
        public List<Vector2Int> StartPath(Vector3 startWorldPosition, Vector3 destinationWorldPosition)
        {
            Vector2Int startNode = AstarBrain.instance.WorldToGrid(startWorldPosition);
            Vector2Int destNode = AstarBrain.instance.WorldToGrid(destinationWorldPosition);
            savedPathForGizmos.Clear();

            if (curActivePath != null)
            {
                StopCoroutine(curActivePath);
            }
            curActivePath = StartCoroutine(ProcessPath(startNode, destNode));
            return default;
        }
        private IEnumerator ProcessPath(Vector2Int startNode,Vector2Int destinationNode)
        {
            Dictionary<Vector2Int, NodeFlag> openNodesFlag = new Dictionary<Vector2Int, NodeFlag>();
            Dictionary<Vector2Int, NodeFlag> closedNodesFlag = new Dictionary<Vector2Int, NodeFlag>();
            AstarBrain astar = AstarBrain.instance;
            openNodesFlag.Add(startNode, new NodeFlag(startNode, null, destinationNode));
            while (openNodesFlag.Count > 0)
            {
                NodeFlag lowestFNode = openNodesFlag.Values.OrderBy(x => x.fCost).First();
                if(openNodesFlag.ContainsKey(lowestFNode.xz))
                    openNodesFlag.Remove(lowestFNode.xz);
                if (!closedNodesFlag.ContainsKey(lowestFNode.xz))
                {
                    closedNodesFlag.Add(lowestFNode.xz, lowestFNode);
                }
                if (lowestFNode.xz == destinationNode)
                {
                    break;
                }
                List<Vector2Int> neighbours = astar._GetNeighbour8Nodes(lowestFNode.xz);
                foreach (var nodeLocation in neighbours)
                {
                    //you shall not pass
                    if (!astar.grid[nodeLocation].walkable || closedNodesFlag.ContainsKey(nodeLocation))
                    {
                        continue;
                    }
                        
                    if(openNodesFlag.ContainsKey(nodeLocation))
                    {
                        if (openNodesFlag[nodeLocation].gCost > lowestFNode.gCost)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        openNodesFlag.Add(nodeLocation, new NodeFlag(nodeLocation, lowestFNode, destinationNode));
                    }
                }
                savedPathForGizmos = closedNodesFlag.Keys.ToList();
                yield return null;
            }
            yield return null;
        }
        private void OnDrawGizmos()
        {
            for (int i = 0; i < savedPathForGizmos.Count; i++)
            {
                Vector2Int point = savedPathForGizmos[i];
                Vector2Int nextPoint = (i + 1) > savedPathForGizmos.Count - 1 ? savedPathForGizmos[i] :savedPathForGizmos[i + 1];
                Gizmos.DrawLine(AstarBrain.instance.grid[point].worldPosition, AstarBrain.instance.grid[nextPoint].worldPosition);
            }
        }
        [ContextMenu(nameof(StartIT))]
        public void StartIT()
        {
            Debug.Log("Starting finding path");
            StartPath(transform.position, destinationPoint.transform.position);
        }
    }
    
}
