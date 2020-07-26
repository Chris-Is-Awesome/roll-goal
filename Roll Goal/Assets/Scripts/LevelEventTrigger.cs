using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ASG;

public class LevelEventTrigger : MonoBehaviour
{
	public enum TriggerConditions
	{
		MonoBehaviour,
		CollisionEnter,
		CollisionStay,
		CollisionExit,
		TriggerEnter,
		TriggerStay,
		TriggerExit,
		KeyPress,
		KeyPressDown,
		KeyPressUp,
		TimeInterval,
		OnEnable,
		OnDisable,
		BecameVisible,
		BecameInvisible,
		OnEventStart,
		OnEventEnd,
	}

	List<TriggerConditions> collisionConds = new List<TriggerConditions>()
	{
		TriggerConditions.CollisionEnter,
		TriggerConditions.CollisionStay,
		TriggerConditions.CollisionExit,
	};

	List<TriggerConditions> triggerConds = new List<TriggerConditions>()
	{
		TriggerConditions.TriggerEnter,
		TriggerConditions.TriggerStay,
		TriggerConditions.TriggerExit,
	};

	[SerializeField]
	[TextArea]
	string description;
	[Header("How should event be triggered?")]
	[SerializeField]
	[Tooltip("How should this event be triggered?")]
		TriggerConditions eventTrigger = TriggerConditions.MonoBehaviour;
	[SerializeField]
	[ConditionalField("eventTrigger", false, TriggerConditions.CollisionEnter, TriggerConditions.CollisionStay, TriggerConditions.CollisionExit, TriggerConditions.TriggerEnter, TriggerConditions.TriggerStay, TriggerConditions.TriggerExit)]
	[Tooltip("What incoming collider's tag should trigger event on?")]
	string tagToCompare;

	[Header("What should the event do once triggered?")]
		[SerializeField] UnityEvent OnConnectedAction;

	void OnCollisionEnter2D(Collision2D other)
	{
		if (collisionConds.Contains(eventTrigger))
		{
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (triggerConds.Contains(eventTrigger) && !string.IsNullOrEmpty(tagToCompare))
		{
			if (other.CompareTag(tagToCompare))
				DoConnectedAction();
		}
	}

	public void DoConnectedAction()
	{
		OnConnectedAction?.Invoke();
	}

	public void OutputMessage(string message)
	{
		Debug.Log(message);
	}
}