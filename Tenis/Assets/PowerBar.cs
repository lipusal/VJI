using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PowerBar : MonoBehaviour
{
    // Reference to our player
    public GameObject player;

    private PlayerLogic _playerLogicComponent;
    private Slider _powerBar;

    // Start is called before the first frame update
    void Start()
    {
        _playerLogicComponent = player.GetComponent<PlayerLogic>();

        _powerBar = GetComponent<Slider>();
        _powerBar.minValue = _playerLogicComponent.minHitForce;
        _powerBar.maxValue = _playerLogicComponent.maxHitForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerLogicComponent.IsChargingHit() && !_playerLogicComponent.IsServing())
        {
            _powerBar.value = _powerBar.minValue;
        }
        else
        {
            _powerBar.value = _playerLogicComponent.GetCurrentHitForce();
        }
    }
}
