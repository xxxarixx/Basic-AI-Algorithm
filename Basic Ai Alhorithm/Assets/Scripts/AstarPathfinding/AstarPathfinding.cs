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
            ///NOTE for future me:
            ///Weird stair behaviour with pathing left top and right bottom probably something is messed up with costs
            ///time spend on trying to fix it 2h+

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
                gCost = Vector2Int.Distance(xz,lastNode.xz);
                hCost = Vector2Int.Distance(xz, destNode);
                this.lastNode = lastNode; 
            }
        }
        
        /// <returns>closest path points to destination</returns>
        public void StartLinearPath(Vector3 startWorldPosition, Vector3 destinationWorldPosition, Action<List<Vector3>> OnSuccessGeneratingPath, Action OnFailed)
        {
            Vector2Int startNode = AstarBrain.instance.WorldToGrid(startWorldPosition);
            Vector2Int destNode = AstarBrain.instance.WorldToGrid(destinationWorldPosition);
            savedPath.Clear();
            if (!AstarBrain.instance.grid[destNode].walkable)
            {
                ///Find close by node if destination node is impossible
                List<Vector2Int> potencialNewDestination = new List<Vector2Int>();
                foreach (var neighbour in AstarBrain.instance.Get8NeighbourNodes(destNode))
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
        /// <returns>closest path points to destination but more smooth and with more points</returns>
        public void StartBezierPath(Vector3 startWorldPosition, Vector3 destinationWorldPosition, Action<List<Vector3>> OnSuccessGeneratingPath, Action OnFailed, int pointsPerCurve = 4)
        {
            Vector2Int startNode = AstarBrain.instance.WorldToGrid(startWorldPosition);
            Vector2Int destNode = AstarBrain.instance.WorldToGrid(destinationWorldPosition);
            savedPath.Clear();
            if (!AstarBrain.instance.grid[destNode].walkable)
            {
                ///Find close by node if destination node is impossible
                List<Vector2Int> potencialNewDestination = new List<Vector2Int>();
                foreach (var neighbour in AstarBrain.instance.Get8NeighbourNodes(destNode))
                {
                    if (!AstarBrain.instance.grid[neighbour].walkable)
                        continue;
                    potencialNewDestination.Add(neighbour);
                }
                if (potencialNewDestination.Count == 0)
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
            curActivePath = StartCoroutine(ProcessPath(startNode, destNode, 
            OnSuccessGeneratingPath:(List<Vector3> pointsWorldPosition) =>
            {
                savedPath = new List<Vector3>();
                savedPath = LinearPathToBezierPath(pointsWorldPosition, pointsPerCurve: pointsPerCurve);
                OnSuccessGeneratingPath?.Invoke(savedPath);
            }));
        }
        /// <summary>
        /// More smooth path based on this article: https://javascript.info/bezier-curve
        /// </summary>
        /// <param name="pointsWorldPosition"></param>
        /// <param name="pointsPerCurve"> numbers of additional points in between </param>
        /// <returns></returns>
        private List<Vector3> LinearPathToBezierPath(List<Vector3> pointsWorldPosition, int pointsPerCurve)
        {
            List<Vector3> curvedPath = new List<Vector3>();
            List<Vector3> worldPositionsFlag = new List<Vector3>();
            worldPositionsFlag.AddRange(pointsWorldPosition);

            //t -> time stamp 0-1
            //P = (1−t)^2*P1 + 2(1−t)t*P2 + t^2*P3
            for (int i = 0; i <= worldPositionsFlag.Count - 3; i += 1)
            {
                Vector3 p1 = worldPositionsFlag[i];
                Vector3 p2 = worldPositionsFlag[i + 1];
                Vector3 p3 = worldPositionsFlag[i + 2];
                Vector3 lastPointOnCurve = Vector3.zero;
                float tRange = 0.5f;
                if (i + 1 > worldPositionsFlag.Count - 3)
                    tRange = 1f;
                for (int j = 1; j <= pointsPerCurve; j++)
                {
                    float t = (float)j * tRange / pointsPerCurve;
                    Vector3 p = Mathf.Pow(1 - t, 2) * p1 + 2 * (1 - t) * t * p2 + Mathf.Pow(t, 2) * p3;
                    lastPointOnCurve = p;
                    curvedPath.Add(p);
                }
                worldPositionsFlag[i + 1] = lastPointOnCurve;
            }

            return curvedPath;
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
                NodeFlag curLowestFNode = openNodesFlag.Values.OrderBy(x => x.fCost).First();
                if(openNodesFlag.ContainsKey(curLowestFNode.xz))
                    openNodesFlag.Remove(curLowestFNode.xz);
                if (!closedNodesFlag.ContainsKey(curLowestFNode.xz))
                {
                    closedNodesFlag.Add(curLowestFNode.xz, curLowestFNode);
                }
                if (curLowestFNode.xz == destinationNode)
                {
                    break;
                }
                List<Vector2Int> neighbours = astar.Get8NeighbourNodes(curLowestFNode.xz);
                foreach (var neighbour in neighbours)
                {
                    //you shall not pass
                    if (!astar.grid[neighbour].walkable || closedNodesFlag.ContainsKey(neighbour))
                    {
                        continue;
                    }
                    if(!openNodesFlag.ContainsKey(neighbour))
                    {
                        openNodesFlag.Add(neighbour, new NodeFlag(neighbour, curLowestFNode, destinationNode));
                        
                    }
                }
            }
            if (openNodesFlag.Count <= 0 && !closedNodesFlag.ContainsKey(destinationNode))
            {
                Debug.LogError("Something went wrong with generating path, didnt reached destination!!!!");
                yield return null;
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
                Gizmos.DrawSphere(point, 0.05f);
            }
        }
    }
    
}
