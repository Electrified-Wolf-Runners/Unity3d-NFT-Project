using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using UnityEngine;
using UnityEngine.UI;

public class EtherTransferCoroutinesUnityWebRequest : MonoBehaviour {

    public string Url = "https://ropsten.infura.io/v3/3a5d9e5971094c8c9f773552430aaf5c";
    public string PrivateKey = "0xc566b17567c731f2889ac7fe772d00d4f66e25ca00ae00152cd99bbe4bd0a3c6";
    public string AddressTo = "0xE68F72B760f4AdcDf72Dcab4c018a955abf3E23C";
    public decimal Amount = 0.2m;
    public decimal GasPriceGwei = 2;
    public string TransactionHash = "";
    public decimal BalanceAddressTo = 0m;

    public InputField InputUrl;
    public InputField InputPrivateKey;
    public InputField InputAddressTo;
    public InputField InputAmount;

    public InputField ResultBalanceAddressTo;
    public InputField ResultTxnHash;

    // Use this for initialization
    void Start () {

        InputUrl.text = Url;
        InputPrivateKey.text = PrivateKey;
        InputAddressTo.text = AddressTo;
        InputAmount.text = Amount.ToString();

    }

    public void TransferRequest()
    {
        StartCoroutine(TransferEther());
    }

    public enum FeeStrategy
    {
        Legacy,
        TimePreference,
        MedianFeeHistory
    }

    //Sample of new features / requests
    public IEnumerator TransferEther()
    {
        Url = InputUrl.text;
        PrivateKey = InputPrivateKey.text;
        AddressTo = InputAddressTo.text;
        Amount = System.Decimal.Parse(InputAmount.text);

        //initialising the transaction request sender
        var ethTransfer = new EthTransferUnityRequest(Url, PrivateKey, 3);

        var receivingAddress = AddressTo;

        var feeStrategy = FeeStrategy.MedianFeeHistory;

        if (feeStrategy == FeeStrategy.TimePreference)
        {
            Debug.Log("Time Preference");
            var timePreferenceFeeSuggestion = new TimePreferenceFeeSuggestionUnityRequestStrategy(Url);

            yield return timePreferenceFeeSuggestion.SuggestFees();

            if (timePreferenceFeeSuggestion.Exception != null)
            {
                Debug.Log(timePreferenceFeeSuggestion.Exception.Message);
                yield break;
            }

            //lets get the first one so it is higher priority
            Debug.Log(timePreferenceFeeSuggestion.Result.Length);
            if (timePreferenceFeeSuggestion.Result.Length > 0)
            {
                Debug.Log(timePreferenceFeeSuggestion.Result[0].MaxFeePerGas);
                Debug.Log(timePreferenceFeeSuggestion.Result[0].MaxPriorityFeePerGas);
            }
            var fee = timePreferenceFeeSuggestion.Result[0];

            yield return ethTransfer.TransferEther(receivingAddress, Amount, fee.MaxPriorityFeePerGas.Value, fee.MaxFeePerGas.Value);
            if (ethTransfer.Exception != null)
            {
                Debug.Log("Error transferring Ether using Time Preference Fee Estimation Strategy: " + ethTransfer.Exception.Message);
                yield break;
            }
        }


        if(feeStrategy == FeeStrategy.MedianFeeHistory)
        {
            Debug.Log("MedianFeeHistory mode");
            var medianPriorityFeeStrategy = new MedianPriorityFeeHistorySuggestionUnityRequestStrategy(Url);

            yield return medianPriorityFeeStrategy.SuggestFee();

            if (medianPriorityFeeStrategy.Exception != null)
            {
                Debug.Log(medianPriorityFeeStrategy.Exception.Message);
                yield break;
            }
            
            Debug.Log(medianPriorityFeeStrategy.Result.MaxFeePerGas);
            Debug.Log(medianPriorityFeeStrategy.Result.MaxPriorityFeePerGas);
            
            var fee = medianPriorityFeeStrategy.Result;

            yield return ethTransfer.TransferEther(receivingAddress, Amount, fee.MaxPriorityFeePerGas.Value, fee.MaxFeePerGas.Value);
            if (ethTransfer.Exception != null)
            {
                Debug.Log("Error transferring Ether using Median Fee History Fee Estimation Strategy: " + ethTransfer.Exception.Message);
                yield break;
            }
        }

        if (feeStrategy == FeeStrategy.Legacy)
        {
            Debug.Log("Legacy mode");
            //I am forcing the legacy mode but also I am including the gas price
            ethTransfer.UseLegacyAsDefault = true;

            yield return ethTransfer.TransferEther(receivingAddress, Amount, GasPriceGwei);

            if (ethTransfer.Exception != null)
            {
                Debug.Log("Error transferring Ether using Legacy Gas Price:  " + ethTransfer.Exception.Message);
                yield break;
            }

        }

        TransactionHash = ethTransfer.Result;
        ResultTxnHash.text = TransactionHash;
        Debug.Log("Transfer transaction hash:" + TransactionHash);

        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(Url);
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(TransactionHash, 2);
        
        Debug.Log("Transaction mined");

        var balanceRequest = new EthGetBalanceUnityRequest(Url);
        yield return balanceRequest.SendRequest(receivingAddress, BlockParameter.CreateLatest());

        BalanceAddressTo = UnitConversion.Convert.FromWei(balanceRequest.Result.Value);
        ResultBalanceAddressTo.text = BalanceAddressTo.ToString();

        Debug.Log("Balance of account:" + BalanceAddressTo);
    }



    // Update is called once per frame
    void Update () {
		
	}



   

   
}
