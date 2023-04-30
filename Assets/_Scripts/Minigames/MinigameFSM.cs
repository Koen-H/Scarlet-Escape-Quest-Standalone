using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Analytics;

public class MinigameFSM : MonoBehaviour
{
    [SerializeField] private MinigameState _startState;
    [SerializeField] private List<MinigameState> _minigameStates;
    [SerializeField] private GameObject hintGlowPrefab;
   // [SerializeField] private float _hintTimeInSeconds;
    [SerializeField] private float _hintCooldown;
    [SerializeField] private bool _isClueOnCooldown;
    [SerializeField] private float _hintCooldownTimerMultiplier;

    //make an event OnStateChanged
    public event Action<MinigameState> OnStateChanged;



    // [SerializeField] private HintGlowManager 

    private static MinigameFSM _instance;
    public static MinigameFSM Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("MinigameFSM is null");
            }

            return _instance;
        }
    }

    private MinigameState _currentState;

    public MinigameState CurrentState
    {
        get => _currentState;
        private set => _currentState = value;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else Destroy(gameObject);
    }


    private void OnNetworkStarted ()
    {

        _minigameStates = GetComponentsInChildren<MinigameState>().ToList();
        ChangeState(_startState);
    }

    private void Start()
    {
       
        NetworkManager.Singleton.OnServerStarted += OnNetworkStarted;
        if (NetworkManager.Singleton.IsServer) OnNetworkStarted();
    }

    public void NextState()
    {
        if (_currentState == null)
        {
            ChangeState(_startState);
        }
        int index = _minigameStates.IndexOf(_currentState);
        if (index < _minigameStates.Count - 1)
        {
            ChangeState(_minigameStates[index + 1]);
        }
        else
        {
            Debug.Log("No more states");
        }
        Debug.Log("Moved to state:" + _currentState.GetType().ToString());
    }

    private void ChangeState(MinigameState pMinigameState)
    {
        //exit current state if we have one
        if (_currentState != null)
        {
            _currentState.OnStateExit();
            _currentState = null;
        }

        //enter next state if we can find ot
        if (pMinigameState != null)
        {
            _currentState = pMinigameState;
            _currentState.OnStateEnter();
        }
        OnStateChanged.Invoke(_currentState);
    }

    public void GetClue()
    {
        if (!_isClueOnCooldown)
        {
            foreach (GameObject hintObj in _currentState.hintObjects)
            {

                GameObject obj = Instantiate(hintGlowPrefab);
                obj.GetComponent<ObjectTracker>().SetTrackerObj(hintObj.transform);
                obj.GetComponent<NetworkObject>().Spawn();
            }
            StartCoroutine(HintCooldown());
        }
        else
        {
            NerworkProtocolManager.Instance.ClueNotReadyClientRpc();
        }
    }
    
    private IEnumerator HintCooldown()
    {
        _hintCooldown *= _hintCooldownTimerMultiplier;
        NerworkProtocolManager.Instance.SetClueCooldownClientRpc(_hintCooldown);
        _isClueOnCooldown = true;
        yield return new WaitForSeconds(_hintCooldown);
        _isClueOnCooldown = false;
    }
}

