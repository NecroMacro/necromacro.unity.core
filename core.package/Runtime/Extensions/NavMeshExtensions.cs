using UnityEngine;
using UnityEngine.AI;

namespace NecroMacro.Core.Extensions
{
	public static class NavMeshExtensions
	{
		/// <summary>
		/// Дистанция до текущего пункта назначения
		/// </summary>
		public static float GetPathRemainingDistance(this NavMeshAgent navMeshAgent)
		{
			if (navMeshAgent.pathPending ||
			    navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
			    navMeshAgent.path.corners.Length == 0)
				return -1f;

			float distance = 0.0f;
			for (int i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
			{
				distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
			}

			return distance;
		}
		
		/// <summary>
		/// Возвращает рандомную позицию в определенном радиусе
		/// </summary>
		public static bool GetRandomPoint(Vector3 center, float maxDistance, ref Vector3 resultPoint) {
			// Get Random Point inside Sphere which position is center, radius is maxDistance
			Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

			NavMeshHit hit; // NavMesh Sampling Info Container

			// from randomPos find a nearest point on NavMesh surface in range of maxDistance
			bool result = NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);

			if (!result) return false;
			
			resultPoint = hit.position;
			return true;
		}
	}
}