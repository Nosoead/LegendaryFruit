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
    private int persistentGlobalCurrency;
    private CurrencyData currencyData = new CurrencyData();
    private PersistentData persistentData = new PersistentData();

    private void InitCurrency()
    {
        inGameCurrency = 0;
        globalCurrency = 0;
    }

    #region /UseCurrency
    public void UseCurrency(int useValue, bool isGlobalCurrency)
    {
        if (isGlobalCurrency)
        {
            if (persistentGlobalCurrency < useValue)
            {
                return;
            }
            else
            {
                persistentGlobalCurrency -= useValue;
                OnGlobalCurrencyDataToUI?.Invoke(persistentGlobalCurrency, isGlobalCurrency);
            }
        }
        else
        {
            if (inGameCurrency < useValue)
            {
                return;
            }
            else
            {
                inGameCurrency -= useValue;
                OnInGameCurrencyDataToUI?.Invoke(inGameCurrency, isGlobalCurrency);
            }
        }
    }

    public void GetCurrency(int useValue, bool isGlobalCurrency)
    {
        if (isGlobalCurrency)
        {
            globalCurrency += useValue;
            persistentGlobalCurrency += useValue;
            OnGlobalCurrencyDataToUI?.Invoke(persistentGlobalCurrency, isGlobalCurrency);
        }
        else
        {
            inGameCurrency += useValue;
            OnInGameCurrencyDataToUI?.Invoke(inGameCurrency, isGlobalCurrency);
        }
    }

    public int GetCurrencyData(bool isGlobalCurrency)
    {
        if (isGlobalCurrency)
        {
            return persistentGlobalCurrency;
        }
        else
        {
            return inGameCurrency;
        }
    }

    public void ResetInGameCurrency()
    {
        inGameCurrency = 0;
    }
    #endregion

    #region /Data
    public CurrencyData SaveInGameCurrencyData()
    {
        currencyData.inGameCurrency = this.inGameCurrency;
        currencyData.globalCurrency = this.globalCurrency;
        return currencyData;
    }

    public void LoadInGameCurrencyData(CurrencyData currencyData)
    {
        this.inGameCurrency = currencyData.inGameCurrency;
        this.globalCurrency = currencyData.globalCurrency;
        OnInGameCurrencyDataToUI?.Invoke(inGameCurrency, false);
    }

    public PersistentData SaveGlobalCurrencyData()
    {
        persistentData.globalCurrency = this.persistentGlobalCurrency;
        return persistentData;
    }

    public void LoadGlobalCurrencyData(PersistentData persistentData)
    {
        this.persistentGlobalCurrency = persistentData.globalCurrency;
        OnGlobalCurrencyDataToUI?.Invoke(globalCurrency, true);
    }
    #endregion
}