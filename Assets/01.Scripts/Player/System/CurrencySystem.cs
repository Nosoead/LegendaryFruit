using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    private int inGameCurrency;
    private int globalCurrency;

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

    public void GetCurrency(bool isInGameCurrency, int useValue)
    {
        if (isInGameCurrency)
        {
            inGameCurrency += useValue;
        }
        else
        {
            globalCurrency += useValue;
        }
    }

    public void ResetInGameCurrency()
    {
        inGameCurrency = 0;
    }
    #endregion

    #region /Data
    public void SaveCurrencyData()
    {

    }

    public void LoadCurrencyData()
    {

    }
    #endregion
}