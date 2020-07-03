using UnityEngine;

public class FloorControl : MonoBehaviour
{
    public int width = 100;
    public int height = 100;
    public int depth = 40;


    public float scale = 20f;
    private Transform frontWall;
    private Transform backWall;
    private Transform leftWall;
    private Transform rightWall;
    private Terrain terrain;

    void Start(){
        terrain = GetComponent<Terrain>();
        frontWall = GameObject.Find("FrontWall").transform;
        backWall = GameObject.Find("BackWall").transform;
        leftWall = GameObject.Find("LeftWall").transform;
        rightWall = GameObject.Find("RightWall").transform;
        SetTerrainData();
    }

    void SetTerrainData(){
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        frontWall.localScale = new Vector3(1,7.5f,width);
        backWall.localScale = new Vector3(1,7.5f,width);
        leftWall.localScale = new Vector3(1,7.5f,height);
        rightWall.localScale = new Vector3(1,7.5f,height);
        frontWall.position = new Vector3(width/2.0f,0,height);
        backWall.position = new Vector3(width/2.0f,0,0);
        leftWall.position = new Vector3(0,0,height/2.0f);
        rightWall.position = new Vector3(width,0,height/2.0f);

    }

    TerrainData GenerateTerrain(TerrainData terrainData){
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width,depth,height);
        return terrainData;    
    }

    
}