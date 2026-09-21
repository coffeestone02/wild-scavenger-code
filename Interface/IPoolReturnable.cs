
using System;
using UnityEngine;

public interface IPoolReturnable
{
    /// <summary> 반환 방법 지정 </summary>
    void SetReturn(Action<GameObject> onReturn);
}
