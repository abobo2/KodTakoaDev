using System;
using TMPro;
using UniRx;
using UnityEngine;

public class InteractableTimer : MonoBehaviour
{

	public TextMeshProUGUI text;

	private InteractableController target;
	
	private IDisposable disposable;
	private bool setup;
	private bool isTrackingCD;

	public void Start()
	{
		text.text = "";
	}

	public void Begin(InteractableController ctrl)
	{
		Debug.Log("setting target");
		target = ctrl;
		isTrackingCD = false;
		disposable = target.ObserveEveryValueChanged(t => t.InteractionCompleteness)
			.Do((prop) => { text.text = prop.ToString("0.00"); })
			.Subscribe();
		if (!setup)
		{
			setup = true;
			target.OnInteractionComplete.AddListener(OnComplete);
			target.OnInteractionCooldownReset.AddListener(OnResetCd);
		}
	}

	public void OnEnd()
	{
		if (!isTrackingCD)
		{
			disposable.Dispose();
			text.text = "";
		}
	}

	public void OnResetCd()
	{
		if (isTrackingCD)
		{
			disposable.Dispose();
			text.text = "";
		}
	}

	public void OnComplete()
	{
		if (target.IsRepeatable)
		{
			isTrackingCD = true;
			disposable.Dispose();
			disposable = target.ObserveEveryValueChanged(t => t.CooldownNormalized)
				.Do((prop) => { text.text = prop.ToString("0.00"); })
				.Subscribe();
		}
		else
		{
			disposable.Dispose();
			text.text = "";
		}
	}
}