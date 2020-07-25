using UnityEngine;
using UnityEngine.Events;

public class LevelEventTrigger : MonoBehaviour
{
    public UnityEvent OnConnectedAction;
    public void DoConnectedAction()
	{
		OnConnectedAction?.Invoke();
	}
}