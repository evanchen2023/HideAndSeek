using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class MultiplayerChat : NetworkBehaviour
{
    public Text _messages; 
    public InputField input; 
    public InputField usernameInput; 
    public string username = "Default"; // Sets the username to have a default value.

    public void SetUsername()
    {
        username = usernameInput.text;
    }

    public void CallMessagesRPC()
    {
        string message = input.text; 
        RPC_SendMessage(username, message);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string username, string message, RpcInfo rpcinfo = default)
    {
        _messages.text += $"{username}: {message}\n";
    }
}
