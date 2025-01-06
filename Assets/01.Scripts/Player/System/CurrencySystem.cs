using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CurrencySystem : MonoBehaviour
{
    public UnityAction<int, bool> OnInGameCurrencyDataToUI;
    public UnityAction<int, bool> OnGlobalCurrencyDataToUI;
    private int inGameCurrency;
    private int globalCurrency;
    private CurrencyData currencyData = new CurrencyData();

    private void InitCurrency()
    {
        inGameCurrency = 0;
        globalCurrency = 0;
    }

    #region /UseCurrency
    public void UseCurrency(bool isInGameCurrency, int useValue)
    {
        if (isInGameCurrency)
        {
            if (inGameCurrency < useValue)
            {
                return;
            }
            else
            {
                inGameCurrency -= useValue;
            }
        }
        else
        {
            if (globalCurrency < useValue)
            {
                return;
            }
            else
            {
                globalCurrency -= useValue;
            }
        }
    }

    public void GetCurrency(int useValue, bool isGlobalCurrency)
    {
        Debug.Log("이벤트?");
        if (isGlobalCurrency)
        {
            globalCurrency += useValue;
            OnGlobalCurrencyDataToUI?.Invoke(globalCurrency, isGlobalCurrency);
        }
        else
        {
            inGameCurrency += useValue;
            OnInGameCurrencyDataToUI?.Invoke(inGameCurrency, isGlobalCurrency);
        }
    }

    public void ResetInGameCurrency()
    {
        inGameCurrency = 0;
    }
    #endregion

    #region /Data
    public CurrencyData SaveCurrencyData()
    {
        currencyData.inGameCurrency = this.inGameCurrency;
        currencyData.globalCurrency = this.globalCurrency;
        return currencyData;
    }

    public void LoadCurrencyData(CurrencyData currencyData)
    {
        this.inGameCurrency = currencyData.inGameCurrency;
        this.globalCurrency = currencyData.globalCurrency;
        OnInGameCurrencyDataToUI?.Invoke(inGameCurrency, false);
        OnGlobalCurrencyDataToUI?.Invoke(globalCurrency, true);
    }
    #endregion
}