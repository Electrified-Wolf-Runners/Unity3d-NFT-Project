// New lines added below
using Debug = UnityEngine.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Nethereum.ABI.FunctionEncoding.Attributes;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Contracts;
using Nethereum.Web3;

public class BalanceOfVanilla : MonoBehaviour
{
    private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
    {
        // all certificates are accepted
        return true;
    }

    public string Url = "https://mainnet.infura.io";
    // public string UrlFull = "https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c";


    //New variables added below
    private string url = "https://kovan.infura.io/v3/e0f9e0acea7f4e2a90edba953bbf1277";
    private string account = "0xbd53Ef09C8e5C31004d57E7D297f221C08560FF1";
    private string privateKey = "c566b17567c731f2889ac7fe772d00d4f66e25ca00ae00152cd99bbe4bd0a3c6";
    private string contractAddress = "0xb5d0F518805264Ff1D17E8102f9E4121B2c11DE7";

    // Use this for initialization
    void Start()
    {
    }

    public async void GetBalance()
    {
        var web3 = new Web3(url);
        
        var balance = await web3.Eth.GetBalance.SendRequestAsync(account);
        Debug.Log("Ether Balance is:" + balance.Value);
        // ResultBlockNumber.text = balance.Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
