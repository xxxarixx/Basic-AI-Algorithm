using System;
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
        private List<Vector3> savedPath = new List<Vector3>();
        private Coroutine curActivePath;
        private class NodeFlag
        {
            /// <summary>
            /// mainly for backtracing path
            /// </summary>
            public readonly NodeFlag lastNode;
            public readonly Vector2Int xz;
            /// <summary>
            /// distance to starting node
            /// </summary>
            public readonly float gCost;
            /// <summary>
            ///  +- distance to destination point
            /// </summary>
            public readonly float hCost;
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
                this.lastNode = lastNode; 
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>closest path points to destination</returns>
        public void StartPath(Vector3 startWorldPosition, Vector3 destinationWorldPosition, Action<List<Vector3>> OnSuccessGeneratingPath, Action OnFailed)
        {
            Vector2Int startNode = AstarBrain.instance.WorldToGrid(startWorldPosition);
            Vector2Int destNode = AstarBrain.instance.WorldToGrid(destinationWorldPosition);
            savedPath.Clear();
            if (!AstarBrain.instance.grid[destNode].walkable)
            {
                ///Find close by node if destination node is impossible
                List<Vector2Int> potencialNewDestination = new List<Vector2Int>();
                foreach (var neighbour in AstarBrain.instance._Get8NeighbourNodes(destNode))
                {
                    if (!AstarBrain.instance.grid[neighbour].walkable)
                        continue;
                    potencialNewDestination.Add(neighbour);
                }
                if(potencialNewDestination.Count == 0)
                {
                    OnFailed?.Invoke();
                    return;
                }
                destNode = potencialNewDestination.OrderBy(nodeLocation => Vector2.Distance(nodeLocation, destNode)).First();

            }
                
            if (curActivePath != null)
            {
                StopCoroutine(curActivePath);
            }
            curActivePath = StartCoroutine(ProcessPath(startNode, destNode, OnSuccessGeneratingPath));
        }
        private IEnumerator ProcessPath(Vector2Int startNode,Vector2Int destinationNode, Action<List<Vector3>> OnSuccessGeneratingPath)
        {
            Dictionary<Vector2Int, NodeFlag> openNodesFlag = new Dictionary<Vector2Int, NodeFlag>();
            Dictionary<Vector2Int, NodeFlag> closedNodesFlag = new Dictionary<Vector2Int, NodeFlag>();
            AstarBrain astar = AstarBrain.instance;
            var startNodeFlag = new NodeFlag(startNode, null, destinationNode);
            openNodesFlag.Add(startNode, startNodeFlag);
            ///find path
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
                List<Vector2Int> neighbours = astar._Get8NeighbourNodes(lowestFNode.xz);
                foreach (var nodeLocation in neighbours)
                {
                    //you shall not pass
                    if (!astar.grid[nodeLocation].walkable || closedNodesFlag.ContainsKey(nodeLocation))
                    {
                        continue;
                    }
                        
                    if(openNodesFlag.ContainsKey(nodeLocation))
                    {
                        if (lowestFNode.gCost > openNodesFlag[nodeLocation].gCost)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        openNodesFlag.Add(nodeLocation, new NodeFlag(nodeLocation, lowestFNode, destinationNode));
                    }
                }
            }
            ///backtrace to get most optimal path
            List<Vector3> BackTraceClosedNodesFlag()
            {
                List<Vector3> backtraced = new List<Vector3>();
                NodeFlag backtraceTarget = closedNodesFlag[destinationNode];
                backtraced.Add(AstarBrain.instance.grid[backtraceTarget.xz].worldPosition);
                while (backtraceTarget.lastNode != backtraceTarget)
                {
                    backtraceTarget = backtraceTarget.lastNode;
                    backtraced.Add(AstarBrain.instance.grid[backtraceTarget.xz].worldPosition);
                }
                backtraced.Reverse();
                return backtraced;
            }

            savedPath = BackTraceClosedNodesFlag();
            OnSuccessGeneratingPath?.Invoke(savedPath);
            yield return null;
        }
        private void OnDrawGizmos()
        {
            for (int i = 0; i < savedPath.Count; i++)
            {
                Vector3 point = savedPath[i];
                Vector3 nextPoint = (i + 1) > savedPath.Count - 1 ? savedPath[i] :savedPath[i + 1];
                Gizmos.DrawLine(point, nextPoint);
            }
        }
    }
    
}
