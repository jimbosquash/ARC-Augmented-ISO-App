using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// responsible for controling interaction contract
/// </summary>
public abstract class ToolPathManagerBase : MonoBehaviour
{

    public abstract List<GameObject> GetPathPts();

    public abstract float GetPathLength();

    public abstract void AddPointToPath(Transform newPosition);

    public abstract void AddTCPPosition();

}

