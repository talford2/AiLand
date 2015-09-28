using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetingUtility
{
    private static Dictionary<Team, List<Transform>> targetables;

    public static void AddTargetable(Team team, Transform target)
    {
        if (targetables == null)
            targetables = new Dictionary<Team, List<Transform>>();

        if (!targetables.ContainsKey(team))
            targetables.Add(team, new List<Transform>());
        targetables[team].Add(target);
    }

    public static void RemoveTargetable(Team team, Transform target)
    {
        if (targetables.ContainsKey(team))
            targetables[team].Remove(target);
    }

    public static Transform FindNearest(Team team, Vector3 fromPosition, float maxDistance)
    {
        Transform target = null;
        if (targetables.ContainsKey(team))
        {
            var targetCandidates = targetables[team];
            if (targetCandidates.Any())
            {
                var smallestDistanceSquared = maxDistance * maxDistance;
                foreach (var candidate in targetCandidates)
                {
                    var toTarget = candidate.transform.position - fromPosition;
                    if (toTarget.sqrMagnitude < smallestDistanceSquared)
                    {
                        smallestDistanceSquared = toTarget.sqrMagnitude;
                        target = candidate;
                    }
                }
            }
        }
        return target;
    }
}

public enum Team
{
    Good,
    Bad
}
