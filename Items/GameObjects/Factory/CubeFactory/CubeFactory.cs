using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CubeFactory : MonoBehaviour , ObjectFactory<AbstractGridObject> {
    
    public GameObject cubePrefab;
    public float CellSize = 0.35f;
    
    [SerializeField] GridStorage gridStorage;
    [SerializeField] Sprite BlueCubeSprite;
    [SerializeField] Sprite RedCubeSprite;
    [SerializeField] Sprite GreenCubeSprite;
    [SerializeField] Sprite YellowCubeSprite;

    public AbstractGridObject CreateCube(string color,Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        switch (color)
        {
            case "b" : return CreateBlueCube(worldPos, gridParent, gridPos);
            case "r" : return CreateRedCube(worldPos, gridParent, gridPos);
            case "g" : return CreateGreenCube(worldPos, gridParent, gridPos);
            case "y"  : return CreateYellowCube(worldPos, gridParent, gridPos);
            case "rand": return (CreateCube(GetRandomColor(), worldPos, gridParent, gridPos));
            default: return null;
        }
    }
    
    
    private AbstractGridObject CreateBlueCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        GameObject cube = Instantiate(cubePrefab, worldPos, Quaternion.identity, gridParent);
        cube.transform.localScale = Vector3.one * (CellSize); 
        CubeObject cubeObj = cube.GetComponent<CubeObject>();
        cubeObj.Initialize(gridPos, "b", BlueCubeSprite);
        cubeObj.GetComponent<SpriteRenderer>().sortingOrder = gridPos.y + 1;
        gridStorage.SetObjectAt(gridPos, cubeObj);
        return cubeObj;
    }
    
    private AbstractGridObject CreateRedCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        GameObject cube = Instantiate(cubePrefab, worldPos, Quaternion.identity, gridParent);
        cube.transform.localScale = Vector3.one * (CellSize); 
        CubeObject cubeObj = cube.GetComponent<CubeObject>();
        cubeObj.Initialize(gridPos, "r", RedCubeSprite);
        cubeObj.GetComponent<SpriteRenderer>().sortingOrder = gridPos.y + 1;
        gridStorage.SetObjectAt(gridPos, cubeObj);
        return cubeObj;
    }
    private AbstractGridObject CreateYellowCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        GameObject cube = Instantiate(cubePrefab, worldPos, Quaternion.identity, gridParent);
        cube.transform.localScale = Vector3.one * (CellSize); 
        CubeObject cubeObj = cube.GetComponent<CubeObject>();
        cubeObj.Initialize(gridPos, "y", YellowCubeSprite); 
        cubeObj.GetComponent<SpriteRenderer>().sortingOrder = gridPos.y + 1;
        gridStorage.SetObjectAt(gridPos, cubeObj);
        return cubeObj;
    }
    private AbstractGridObject CreateGreenCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        GameObject cube = Instantiate(cubePrefab, worldPos, Quaternion.identity, gridParent);
        cube.transform.localScale = Vector3.one * (CellSize); 
        CubeObject cubeObj = cube.GetComponent<CubeObject>();
        cubeObj.Initialize(gridPos, "g", GreenCubeSprite); 
        cubeObj.GetComponent<SpriteRenderer>().sortingOrder = gridPos.y + 1;
        gridStorage.SetObjectAt(gridPos, cubeObj);
        return cubeObj;
    }
    
    
    public string GetRandomColor() 
    {
        string[] options = { "b", "r", "g" };
        int index = UnityEngine.Random.Range(0, options.Length);

        return options[index];
    }

}