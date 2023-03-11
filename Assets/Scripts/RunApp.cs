using Game.Service;
using UnityEngine;

public class RunApp : MonoBehaviour
{
    #region MonoBehaviour
    private void Start()
    {
        Debug.Log($"Game Start！");

        ServiceManager.Instance.Setup();
    }
    #endregion
}
