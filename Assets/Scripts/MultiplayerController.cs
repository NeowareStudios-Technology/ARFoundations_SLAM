using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placenote;

public class MultiplayerController : PlacenotePunMultiplayerBehaviour
{
    private List<PlayerController> mPlayerList;
     GameObject mPlayer1Base;
     GameObject mPlayer2Base;

     GameObject dragon;

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
        // Only instantiate player 1 base if one doesn't already exist.
        if (mPlayer1Base == null)
            PhotonNetwork.Instantiate("PlayerBase1", Vector3.zero, Quaternion.identity, 0);
        // If a p1base already does exist then set it to active.
        else
            mPlayer1Base.gameObject.SetActive(true);
        
        // Only instantiate player 2 base if one doesn't already exist.
        if (mPlayer2Base == null)
            PhotonNetwork.Instantiate("PlayerBase2", Vector3.zero, Quaternion.identity, 0);
        // If a p2base already does exist then set it to active.
        else
            mPlayer2Base.gameObject.SetActive(true);


        // Create player
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
    }

    /// Reset registered objects when quiting to main menu
    protected override void OnGameQuit()
    {
        mPlayer1Base = null;
        mPlayer2Base = null;
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
        mPlayer1Base = newBase;
        mPlayer1Base.gameObject.SetActive(PlacenoteMultiplayerManager.Instance.IsPlaying);
    }

    //Register dragon
    public void RegisterDragon(GameObject newDragon)
    {
        dragon = newDragon;
        dragon.gameObject.SetActive(PlacenoteMultiplayerManager.Instance.IsPlaying);
    }


    #endregion Registering objects
}