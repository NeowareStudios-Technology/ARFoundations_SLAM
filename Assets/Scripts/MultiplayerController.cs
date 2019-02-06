using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placenote;

public class MultiplayerController : PlacenotePunMultiplayerBehaviour
{
    private List<PlayerController> mPlayerList;
     GameObject mainCastle;

    GameObject playerDragon;

    #region Singleton
    private static MultiplayerController sInstance = null;
    public static MultiplayerController Instance
    {
        get { return sInstance; }
    }

    /// Returns whether the instance has been initialized or not.
    public static bool IsInitialized
    {
        get { return sInstance != null; }
    }

    /// Base awake method that sets the singleton's unique instance.
    protected virtual void Awake()
    {
        if (sInstance != null)
        {
            Debug.LogError("Trying to instantiate a second instance of PlacenoteMultiplayerManager singleton ");
        }
        else
        {
            sInstance = this;
            mPlayerList = new List<PlayerController>();
        }
    }

    protected virtual void OnDestroy()
    {
        if (sInstance == this)
        {
            sInstance = null;
        }
    }
    #endregion Singleton

    #region Override functions
    /// Instantiate objects when game starts.
    protected override void OnGameStart()
    {
        // Only instantiate castle if one doesn't already exist. From resources
        if (mainCastle == null)
            PhotonNetwork.Instantiate("Castle", Vector3.zero, Quaternion.identity, 0);
    }

    /// Reset registered objects when quiting to main menu
    protected override void OnGameQuit()
    {
        mainCastle = null;
        mPlayerList = new List<PlayerController>();
    }
    #endregion Override functions

    #region Registering objects
    /// Registers the player.
    /// Hides/Shows all players based on if local client is playing or not.
    public void RegisterPlayer(PlayerController newPlayer)
    {
        mPlayerList.Add(newPlayer);
        foreach (PlayerController player in mPlayerList)
        {
            player.gameObject.SetActive(PlacenoteMultiplayerManager.Instance.IsPlaying);
        }
    }

    /// Registers the base.
    /// Hides/Shows bases based on if local client is playing or not.
    public void RegisterBase(GameObject newBase)
    {
        mainCastle = newBase;
        mainCastle.gameObject.SetActive(PlacenoteMultiplayerManager.Instance.IsPlaying);
    }

    //Register dragon
    public void RegisterDragon(GameObject newDragon)
    {
        playerDragon = newDragon;
        playerDragon.gameObject.SetActive(PlacenoteMultiplayerManager.Instance.IsPlaying);
    }


    #endregion Registering objects
}