using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner {

	private readonly GameObject coin;
	private readonly Transform player;
	private List<GameObject> coinsOnStage;
	float minCoinDistance = 15;
	float maxCoinDistance = 25;
	public CoinSpawner(GameObject coinPrefab, Transform playerRef)
	{
		Debug.Log("Spawned");
		coin = coinPrefab;
		player = playerRef;
		coinsOnStage =  new List<GameObject>();
	}
	public void Spawn(int count)
	{ 
		var currentLoc  = player.position;
	
		for (var i = 0; i < count; i++)
		{
			var randomPoint2D = Random.insideUnitCircle.normalized*Random.Range(minCoinDistance,maxCoinDistance);
			var offset = new Vector3(randomPoint2D.x,0, randomPoint2D.y);
			var newLoc = new Vector3(currentLoc.x + offset.x, 50 , currentLoc.z + offset.z);
			// ray it down so its above the ground
			if (Physics.Raycast(newLoc, Vector3.down, out var hit, 250))
			{
				newLoc.y = hit.point.y+1.5f;
				var coinInstance = Object.Instantiate(coin, newLoc, Quaternion.identity);
				coinInstance.SetActive(true);
				coinsOnStage.Add(coinInstance);
			}
			else
			{
				Debug.LogWarning("No bottom object detected within range to snap above.");
			}
		}
		
	}
	public List<GameObject> GetCoins()
	{
		return coinsOnStage;
	}
	 
	// hand assign coin dist
	public void SetMinMaxDistance(Vector2 coinDistanceMinMax)
	{
		minCoinDistance = coinDistanceMinMax.x;
		maxCoinDistance = coinDistanceMinMax.y;
	}
}