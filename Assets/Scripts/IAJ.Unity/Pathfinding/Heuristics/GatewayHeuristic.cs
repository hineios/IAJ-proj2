using RAIN.Navigation.Graph;
using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.HPStructures;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class GatewayHeuristic : IHeuristic
    {
        private ClusterGraph ClusterGraph { get; set; }

        public GatewayHeuristic(ClusterGraph clusterGraph)
        {
            this.ClusterGraph = clusterGraph;
        }

        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            //for now just returns the euclidean distance
            Cluster StartCluster = ClusterGraph.Quantize(node);
            Cluster GoalCluster = ClusterGraph.Quantize(goalNode);

            if (StartCluster.Equals(GoalCluster))
                return EuclideanDistance(node.LocalPosition, goalNode.LocalPosition);
            else
            {
                float min = float.MaxValue;
                foreach(Gateway g1 in StartCluster.gateways)
                {
                    foreach(Gateway g2 in GoalCluster.gateways)
                    {
                        float h = 
                            EuclideanDistance(node.LocalPosition, g1.center) + 
                            ClusterGraph.gatewayDistanceTable[g1.id].entries[g2.id].shortestDistance + 
                            EuclideanDistance(g2.center, goalNode.LocalPosition);
                        if (h < min)
                            min = h;
                    }
                }
                return min;
            }
        }

        public float EuclideanDistance(Vector3 startPosition, Vector3 endPosition)
        {
            return (endPosition - startPosition).magnitude;
        }
    }
}
