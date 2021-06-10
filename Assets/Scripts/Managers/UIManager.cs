﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    private GameManager _gameManager;
    private Canvas _canvas;
    private Button _marketBtn, _startGameBtn, _buyPawnBtn, _buyKnightBtn, _buyBishopBtn, _buyRookBtn, _buyQueenBtn;
    private Text _goldText;
    private GameObject _groupMarket;

    public UIManager()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _groupMarket = _canvas.transform.Find("Group_Market").gameObject;

        _goldText = _groupMarket.transform.Find("Text_GoldAmount").GetComponent<Text>();

        _marketBtn = _canvas.transform.Find("Btn_Market").GetComponent<Button>();
        _startGameBtn = _canvas.transform.Find("Btn_StartGame").GetComponent<Button>();
        _buyPawnBtn = _groupMarket.transform.Find("Group_Pawn/Btn_BuyPawn").GetComponent<Button>();
        _buyKnightBtn = _groupMarket.transform.Find("Group_Knight/Btn_BuyKnight").GetComponent<Button>();
        _buyBishopBtn = _groupMarket.transform.Find("Group_Bishop/Btn_BuyBishop").GetComponent<Button>();
        _buyRookBtn = _groupMarket.transform.Find("Group_Rook/Btn_BuyRook").GetComponent<Button>();
        _buyQueenBtn = _groupMarket.transform.Find("Group_Queen/Btn_BuyQueen").GetComponent<Button>();




        _marketBtn.onClick.AddListener(OpenMarket);
        _startGameBtn.onClick.AddListener(StartGame);
        _buyPawnBtn.onClick.AddListener(BuyPawn);
        _buyKnightBtn.onClick.AddListener(BuyKnight);
        _buyBishopBtn.onClick.AddListener(BuyBishop);
        _buyRookBtn.onClick.AddListener(BuyRook);
        _buyQueenBtn.onClick.AddListener(BuyQueen);
    }

    public void Start()
    {

        _gameManager = Main.Instance.gameManager;

        _groupMarket.SetActive(false);
        _startGameBtn.gameObject.SetActive(false);

        _goldText.text = "Gold: " + _gameManager.gold.ToString();
    }

    public void Update()
    {
        
    }
    
    private void OpenMarket()
    {
        Debug.Log("Market is open");
        _groupMarket.SetActive(true);
        _marketBtn.gameObject.SetActive(false);
        _startGameBtn.gameObject.SetActive(true);

    }

    private void StartGame()
    {
        Debug.Log("Game is start now");
        _groupMarket.SetActive(false);
        _startGameBtn.gameObject.SetActive(false);
    }

    private void BuyPawn()
    {
        Debug.Log("You are bought a Pawn now");
        _gameManager.BuyAUnit(Enums.UniteType.Pawn);
    }

    private void BuyKnight()
    {
        Debug.Log("You are bought a Knight now");
        _gameManager.BuyAUnit(Enums.UniteType.Knight);
    }

    private void BuyBishop()
    {
        Debug.Log("You are bought a Bishop now");
        _gameManager.BuyAUnit(Enums.UniteType.Bishop);
    }

    private void BuyRook()
    {
        Debug.Log("You are bought a Rook now");
        _gameManager.BuyAUnit(Enums.UniteType.Rook);
    }

    private void BuyQueen()
    {
        Debug.Log("You are bought a Queen now");
        _gameManager.BuyAUnit(Enums.UniteType.Queen);
    }
}