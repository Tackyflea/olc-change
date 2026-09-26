using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "UnassignedField.Global")]
public static class GameEvents {

	// when you hit a coin
	public static Action<Vector3> OnCoinCollected; 
}