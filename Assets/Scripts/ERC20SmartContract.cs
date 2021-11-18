// New lines added below
using ABI.Contracts.fusez.ContractDefinition;
using Debug = UnityEngine.Debug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using System.Diagnostics;

public class ERC20SmartContract : MonoBehaviour
{
    //New variables added below
    private string url = "https://kovan.infura.io/v3/e0f9e0acea7f4e2a90edba953bbf1277";
    private string account = "0xbd53Ef09C8e5C31004d57E7D297f221C08560FF1";
    private string privateKey = "c566b17567c731f2889ac7fe772d00d4f66e25ca00ae00152cd99bbe4bd0a3c6";
    private string contractAddress = "0xb5d0F518805264Ff1D17E8102f9E4121B2c11DE7";

    // Use this for initialization
    void Start()
    {
    }

    public void TransferERC20Request()
    {
        StartCoroutine(TransferERC20());
    }

    public void MintRequest()
    {
        StartCoroutine(MintERC20());
    }


    public IEnumerator TransferERC20()
    {
        var transactionTransferRequest = new TransactionSignedUnityRequest(url, privateKey, 42);
        transactionTransferRequest.UseLegacyAsDefault = true;

        var newAddress = "0xE68F72B760f4AdcDf72Dcab4c018a955abf3E23C";

        var transactionMessage = new TransferFunction
        {
            Recipient = newAddress,
            Amount = 10
        };

        yield return transactionTransferRequest.SignAndSendTransaction(transactionMessage, contractAddress);

        var transactionTransferHash = transactionTransferRequest.Result;

        Debug.Log("Deployment transaction hash:" + transactionTransferHash);
    }

    public IEnumerator MintERC20()
    {
        var transactionTransferRequest = new TransactionSignedUnityRequest(url, privateKey, 42);
        transactionTransferRequest.UseLegacyAsDefault = true;

        var transactionMessage = new MintFunction
        {
        };

        yield return transactionTransferRequest.SignAndSendTransaction(transactionMessage, contractAddress);

        var transactionTransferHash = transactionTransferRequest.Result;

        Debug.Log("Deployment transaction hash:" + transactionTransferHash);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

