using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ChangeSetup : MonoBehaviour {
    public InputActionReference moveAction;
    public Rigidbody carBody;
    public Transform carVisual;
    
    public float accel=22;
    public float torque = 100f;
    public float rollResistance = 2f;

    private int _coinsCollected = 0;
    // how many coins you need to win
    public int winCoinsCollectedAmount = 50;

    private CoinSpawner _spawner;
    private Vector2 _inputVector;
    [Header("Coin details")]
    public GameObject coinPrefab;
    public Vector2 coinDistanceMinMax;
    
    [Header("UI")]
    public UIDocument uiDocument ;
    private Label _coinValueText;

    public UIDocument youWinDocument ;
    private Label _youWinValueText;

    private void Start()
    {
	    if (!moveAction)
	    {
		    Debug.LogWarning("No move action assigned!");
		    return;
	    }
	    if (!carBody || !carVisual)
	    {
		    Debug.LogWarning("No car assigned!");
		    return;
	    }
	    
	    moveAction.action.Enable();
	    
	    // ui
	    youWinDocument.gameObject.SetActive(true);
	    youWinDocument.rootVisualElement.style.display = DisplayStyle.None;
	    _coinValueText = uiDocument.rootVisualElement.Q<Label>("CoinsValue");
	    _youWinValueText = youWinDocument.rootVisualElement.Q<Label>("YouWinScore");
	    if (_coinValueText == null)
		    Debug.LogWarning("Coin value text not assigned!");
	    if (_youWinValueText == null)
		    Debug.LogWarning("Coin value on win screen text not assigned!");
	    
		//setup spawner
	    _spawner = new CoinSpawner(coinPrefab, carVisual);
	    _spawner.SetMinMaxDistance(coinDistanceMinMax);

	    GameEvents.OnCoinCollected += CoinCollected;
	    //test spawn
	    SpawnCoins();
    }
    private void SpawnCoins()
    {
	    _spawner.Spawn(55);
	    
    }
    private async void CoinCollected(Vector3 obj)
    {
	    try
	    {
		    // Debug.Log("Game heard about coin collected");
		    _coinsCollected++;
		    _coinsCollected= Mathf.Clamp(_coinsCollected, 0, _coinsCollected);
		    _coinValueText.text = _coinsCollected.ToString();
		    if (_coinsCollected < winCoinsCollectedAmount)
		    {
			    return;
		    }
	    
		    Debug.Log("Level win");
		    _youWinValueText.text = $"Score: {winCoinsCollectedAmount}";
		    youWinDocument.rootVisualElement.style.display = DisplayStyle.Flex;
		    await Task.Delay(3000);
		    youWinDocument.rootVisualElement.style.display = DisplayStyle.None;
		    
		    //reset
		    var coins = _spawner.GetCoins();
		    _coinsCollected = 0;
		    _coinValueText.text = _coinsCollected.ToString();
		    foreach (var coin in coins)
		    {
			    Destroy(coin.gameObject);
		    }
		    SpawnCoins();
	    }
	    catch (Exception e)
	    {
		    
		    Debug.LogWarning("couldnt update score");
		    Debug.LogWarning(e);
	    }
    }
    
    private void FixedUpdate()
    {
	    _inputVector = moveAction.action.ReadValue<Vector2>();

	    if (Mathf.Abs(_inputVector.y) > 0.01f)
		    //forward
		    carBody.AddForce(carVisual.forward * (_inputVector.y * accel), ForceMode.Acceleration);

	    if (Mathf.Abs(_inputVector.x) > 0.01f)
		    // turny
		    carBody.AddTorque(Vector3.up * (_inputVector.x * torque));
	
	    var lateralVelocity = Vector3.Dot(carBody.linearVelocity, carVisual.right) * carVisual.right;
	    carBody.linearVelocity -= lateralVelocity;
		//resistance
	    var forwardVelocity = Vector3.Dot(carBody.linearVelocity, carVisual.forward) * carVisual.forward;
	    carBody.AddForce(-forwardVelocity * rollResistance, ForceMode.Acceleration);
    }
    private void OnDestroy()
    {
	    
	    GameEvents.OnCoinCollected -= CoinCollected;
	    moveAction.action.Disable();
    }
}
