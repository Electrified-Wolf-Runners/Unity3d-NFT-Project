// New lines added below
using ABI.Contracts.erc721nft.ContractDefinition;
using Debug = UnityEngine.Debug;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using System.Diagnostics;

public class ERC721SmartContract : MonoBehaviour
{
    //New variables added below
    private string url = "https://rinkeby.infura.io/v3/e0f9e0acea7f4e2a90edba953bbf1277";
    private string account = "0xbd53Ef09C8e5C31004d57E7D297f221C08560FF1";
    private string privateKey = "c566b17567c731f2889ac7fe772d00d4f66e25ca00ae00152cd99bbe4bd0a3c6";
    private string contractAddress = "0x9Dcd1f7868A39A9C51545c88DAf5E7A2108F8820"; //MyHero contract on Rinkeby

    // Use this for initialization
    void Start()
    {
    }

    public void BalanceOfRequest()
    {
        StartCoroutine(ERC721BalanceOf());
    }

    public IEnumerator ERC721BalanceOf()
    {
        //Query request using our acccount and the contracts address (no parameters needed and default values)
        var queryRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(url, account);
        yield return queryRequest.Query(new BalanceOfFunction(){Owner = account}, contractAddress);

        //Getting the dto response already decoded
        var dtoResult = queryRequest.Result;
        Debug.Log(dtoResult.ReturnValue1);

        // if (queryRequest.Exception != null)
        // {
        //     UnityEngine.Debug.Log(queryRequest.Exception.Message);
        // }
        // else
        // {
        //     ResultBlockNumber.text = queryRequest.Result.Value.ToString();
        // }

    }
    void Update()
    {
        
    }
}

