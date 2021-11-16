// New lines added below
using ABI.Contracts.erc721nft.ContractDefinition;
using Debug = UnityEngine.Debug;
using System.Collections;
using System.Collections.Generic;
using System.Numerics; //BigInteger works after importing this
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
    private string contractAddress = "0x8B7995c357592Ee93FD88bA16e146E619FcCFCD0"; //MyHero contract on Rinkeby
    public BigInteger[] tokenids;
    bool characters_loaded = false;

    // Use this for initialization
    void Start()
    {
        tokenids = new BigInteger[10];

    }

    public void BalanceOfRequest()
    {
        StartCoroutine(ERC721BalanceOf());
        characters_loaded = true;
    }
    
    public void Character0Request()
    {   
        if(characters_loaded)
        {
            Debug.Log("Token id: " + tokenids[0]);
            StartCoroutine(CallGetCharacterStats(tokenids[0]));
        }
        Debug.Log("No character available");
    }

    public void Character1Request()
    {   
        if(characters_loaded)
        {
            Debug.Log("Token id: " + tokenids[1]);
            StartCoroutine(CallGetCharacterStats(tokenids[1]));
        }
        Debug.Log("No character available");
    }

    public IEnumerator ERC721BalanceOf()
    {
        //Query request using our acccount and the contracts address (no parameters needed and default values)
        var queryRequest = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(url, account);
        yield return queryRequest.Query(new BalanceOfFunction(){Owner = account}, contractAddress);

        //Getting the dto response already decoded
        var dtoResult = queryRequest.Result;
        var tokensOwned = dtoResult.ReturnValue1;

        Debug.Log("Getting token owner token balances...");
        
        Debug.Log(tokensOwned);

        Debug.Log("Getting token owner Indices...");

        for(int i = 0; i < tokensOwned; i++)
        {
            StartCoroutine(GetTokenOfOwnerByIndex(i));
        }

        Debug.Log("Getting token owner Indices from Array...");
        
        for(int i = 0; i < tokensOwned; i++)
        {
            Debug.Log("i: " + tokenids[i]);
        }

        // Debug.Log("Getting character stats...");
        // for(int i = 0; i < tokensOwned; i++)
        // {
        //     StartCoroutine(GetTokenOfOwnerByIndex(i));
        // }


        // Error handling
        // if (queryRequest.Exception != null)
        // {
        //     UnityEngine.Debug.Log(queryRequest.Exception.Message);
        // }
        // else
        // {
        //     ResultBlockNumber.text = queryRequest.Result.Value.ToString();
        // }

    }

    public IEnumerator GetTokenOfOwnerByIndex(int i)
    {
        var queryRequest = new QueryUnityRequest<TokenOfOwnerByIndexFunction, TokenOfOwnerByIndexOutputDTO>(url, account);
        yield return queryRequest.Query(new TokenOfOwnerByIndexFunction(){Owner = account, Index = i}, contractAddress);

        var dtoResult = queryRequest.Result;
        
        Debug.Log("Inside: " + dtoResult.ReturnValue1);

        tokenids[i] = dtoResult.ReturnValue1;
    }

    //getCharacterStats
    public IEnumerator CallGetCharacterStats(BigInteger tokenid)
    {
        Debug.Log("Getting Character with token id: " + tokenid);
        var queryRequest = new QueryUnityRequest<GetCharacterStatsFunction, GetCharacterStatsOutputDTO>(url, account);
        yield return queryRequest.Query(new GetCharacterStatsFunction(){TokenId = tokenid}, contractAddress);

        var dtoResult = queryRequest.Result;

        var speed = dtoResult.ReturnValue1; 
        var strength = dtoResult.ReturnValue2; 
        var agility = dtoResult.ReturnValue3; 
        var charisma = dtoResult.ReturnValue4; 
        var power = dtoResult.ReturnValue5;
        var intelligence = dtoResult.ReturnValue6; 
        var name = dtoResult.ReturnValue7;

        Debug.Log("Speed: " + speed + "  Strength: " + strength + "  Agility: " + agility + "  Charisma: " + charisma + "  Power: " + power + "  Intelligence: " + intelligence);
    }
    void Update()
    {
        
    }
}

