using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PowerBar : MonoBehaviour
{
    private PlayerLogic _playerLogicComponent;
    private Slider _powerBar;

    // Start is called before the first frame update
    void Start()
    {
        _playerLogicComponent = GetComponentInParent<PlayerLogic>();

        _powerBar = GetComponent<Slider>();
        _powerBar.minValue = _playerLogicComponent.minHitForce;
        _powerBar.maxValue = _playerLogicComponent.maxHitForce;

        BallLogic.Instance.ballHitDelegate += ResetPowerBar;
        _playerLogicComponent.initialPositionSetEvent += ResetBar;
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerLogicComponent.IsChargingHit())
        {
            _powerBar.value = _playerLogicComponent.GetCurrentHitForce();
        }
    }

    private void ResetPowerBar(int hittingPlayerId)
    {
        if(!_playerLogicComponent.IsChargingHit() && hittingPlayerId == _playerLogicComponent.GetId())
        {
            ResetBar();
        }
    }

    private void ResetBar()
    {
        _powerBar.value = _powerBar.minValue;
    }
}
