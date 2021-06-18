﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController
{
    private const int _gridRows = 25, _gridColumns = 25;
    private const int _tileSize = 1;
    private GameObject _gridParent;
    private GameManager _gameManager;
    private UniteController _unitController;

    private GameObject _tilePrefab, _singleTile;
    private GameObject[,] _tilesPositions;

    private Material _defaultTile, _highlightTile, _defaultAvailableTiles, _highlightAvailableTiles;
    private Transform _selectionTile, _availableSelectionTile;
    private List<GameObject> _highlight;

    public List<GameObject> tiles;

    public GridController(GameObject parent)
    {
        _gridParent = parent.transform.Find("Group_Grid").gameObject;
        _defaultTile = Resources.Load<Material>("Materials/DefaultTileMat");
        _highlightTile = Resources.Load<Material>("Materials/HighlightTileMat");
    }

    public void Start()
    {
        _gameManager = Main.Instance.gameManager;
        _unitController = Main.Instance.gameManager.uniteController;
        _tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
        _defaultAvailableTiles = Resources.Load<Material>("Materials/DefaultAvailableTilesMat");
        _highlightAvailableTiles = Resources.Load<Material>("Materials/HighliteAvailableTilesMat");
        tiles = new List<GameObject>();
        _tilesPositions = new GameObject[_gridRows, _gridColumns];
        _highlight = new List<GameObject>();
        GenerateTiles();
    }

    public void Update()
    {
        if (_selectionTile != null)
        {
            Renderer tileRenderer = _selectionTile.GetComponent<Renderer>();
            tileRenderer.material = _defaultTile;
            _selectionTile = null;
        }
        Ray ray = _gameManager.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Tile")))
        {
            GameObject tileSelection = hit.transform.gameObject;
            Renderer tileRenderer = tileSelection.GetComponent<Renderer>();
            if (tileRenderer != null)
            {
                tileRenderer.material = _highlightTile;
            }
            _selectionTile = tileSelection.transform;
        }

        if (_availableSelectionTile != null)
        {
            Renderer availableTileRenderer = _availableSelectionTile.GetComponent<Renderer>();
            availableTileRenderer.material = _defaultAvailableTiles;
            _availableSelectionTile = null;
        }
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("AvailableTile")))
        {
            GameObject availableTileSelection = hit.transform.gameObject;
            Renderer availableTileRenderer = availableTileSelection.GetComponent<Renderer>();
            if (availableTileRenderer != null)
            {
                _availableSelectionTile = availableTileSelection.transform;
                availableTileRenderer.material = _highlightAvailableTiles;
                if (Input.GetMouseButtonDown(0))
                {
                    if (_unitController.selectedPlayerUnit.transform.position != _availableSelectionTile.position)
                    {

                    }

                    // if (_unitController.selectedPlayerUnit.transform.position != availableTileSelection.transform.position)
                    // {
                    //     for (int j = 0; j <= _unitController.playerUnits.Count - 1; j++)
                    //     {
                    //         if (_unitController.playerUnits[j].unitObject == _unitController.selectedPlayerUnit)
                    //         {
                    //             _unitController.selectedPlayerUnit.transform.position = new Vector3(availableTileSelection.transform.position.x, 0, availableTileSelection.transform.position.z);
                    //             _unitController.playerUnits[j].isUnitCanMove = false;
                    //         }
                    //     }
                    // }
                    // else if (_unitController.selectedPlayerUnit.transform.position != availableTileSelection.transform.position)
                    // {
                    //     if (_unitController.isEnemySelected)
                    //     {
                    //         if (_unitController.selectedEnemyUnit.transform.position == availableTileSelection.transform.position)
                    //         {
                    //             for (int j = 0; j <= _unitController.playerUnits.Count - 1; j++)
                    //             {
                    //                 if (_unitController.playerUnits[j].unitObject == _unitController.selectedPlayerUnit)
                    //                 {
                    //                     _unitController.TakeDamage(_unitController.selectedPlayerUnit, _unitController.selectedEnemyUnit);
                    //                     _unitController.playerUnits[j].isUnitCanMove = false;
                    //                 }
                    //             }
                    //         }
                    //     }
                    // }
                }
            }
        }
    }

    private void GenerateTiles()
    {
        for (int x = 0; x < _gridRows; x++)
        {
            for (int y = 0; y < _gridColumns; y++)
            {
                _singleTile = GameObject.Instantiate(_tilePrefab, new Vector3(x * _tileSize, 0, y * _tileSize), Quaternion.identity, _gridParent.transform);
                _singleTile.layer = LayerMask.NameToLayer("Tile");
                _singleTile.name = $"Tile X:{x} Y:{y}";
                tiles.Add(_singleTile);
                _tilesPositions[x, y] = _singleTile;
            }
        }
    }

    public void HighlightAvailableTiles(Vector3 selectedUnitPos, int moveDistance)
    {
        int xPos = (int)selectedUnitPos.x;
        int zPos = (int)selectedUnitPos.z;

        for (int i = 0; i <= moveDistance; i++)
        {
            for (int j = 0; j <= moveDistance; j++)
            {
                if (xPos - i < 0) xPos = 0;
                if (xPos + i > 24) xPos = 24;
                if (zPos - j < 0) zPos = 0;
                if (zPos + j > 24) zPos = 24;

                if (xPos < 24 && zPos < 24)
                {
                    _highlight.Add(_tilesPositions[xPos + i, zPos + j]);
                }

                if (xPos > 0 && zPos < 24)
                {
                    _highlight.Add(_tilesPositions[xPos - i, zPos + j]);
                }

                if (xPos < 24 && zPos > 0)
                {
                    _highlight.Add(_tilesPositions[xPos + i, zPos - j]); // BAG
                }

                if (xPos > 0 && zPos > 0)
                {
                    _highlight.Add(_tilesPositions[xPos - i, zPos - j]); // BAG
                }
            }
        }

        foreach (var item in _highlight)
        {
            item.GetComponent<MeshRenderer>().material = _defaultAvailableTiles;
            item.layer = LayerMask.NameToLayer("AvailableTile");
        }
    }

    public void HideAvailableTile()
    {
        foreach (var item in _highlight)
        {
            item.GetComponent<MeshRenderer>().material = _defaultTile;
            item.layer = LayerMask.NameToLayer("Tile");
        }
        _highlight.Clear();
    }
}