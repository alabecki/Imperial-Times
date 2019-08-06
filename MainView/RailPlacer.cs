using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;
using assemblyCsharp;

public  class RailPlacer : MonoBehaviour
{
    private WMSK map;

    void Awake()
    {
        map = WMSK.instance;
    }

    public void drawRailroadOnMap(assemblyCsharp.Province prov, Nation player)
    {
        WorldMapStrategyKit.Province mapProvince = map.provinces[prov.getIndex()];
        Vector2 provCenter = mapProvince.center;
        for (int i = 0; i < prov.Neighbours.Count; i++)
        {
            if (PlayerCalculator.getAllProvinces(player).Contains(prov.Neighbours[i]))
            {
                // draw rail segment from prv to neighbourProv
                assemblyCsharp.Province neighbourProvince = State.getProvinces()[prov.Neighbours[i]];
                if (neighbourProvince.railroad && !neighbourProvince.Linked.Contains(prov.getIndex()) && 
                    !prov.Linked.Contains(neighbourProvince.getIndex()))
                {
                    WorldMapStrategyKit.Province mapNeighbourProvince = map.provinces[prov.Neighbours[i]];
                    Vector2 neighbourCenter = mapNeighbourProvince.center;
                    Cell startCell = map.GetCell(provCenter);
                    Cell endCell = map.GetCell(neighbourCenter);
                    List<int> cellIndices = map.FindRoute(startCell, endCell, TERRAIN_CAPABILITY.OnlyGround);
                    if (cellIndices == null) return;
                    int positionsCount = cellIndices.Count;
                    Debug.Log("Number of positions: " + positionsCount);

                    Vector2[] positions = new Vector2[positionsCount];
                    for (int k = 0; k < positionsCount; k++)
                    {
                        positions[k] = map.cells[cellIndices[k]].center;
                    }

                    // Build a railroad along the map coordinates
                    LineMarkerAnimator lma = map.AddLine(positions, Color.white, 0.0f, 0.15f);
                    Texture2D railwayMatt = Resources.Load("Sprites/GUI/railRoad", typeof(Texture2D)) as Texture2D;

                    lma.lineMaterial.mainTexture = railwayMatt;
                    lma.lineMaterial.mainTextureScale = new Vector2(16f, 2f);
                    neighbourProvince.Linked.Add(prov.getIndex());
                    prov.Linked.Add(neighbourProvince.getIndex());

                }
            }

        }
    }


    private void placeRailRoad(int start, int end) {
        
        //Vector2 beginLoc = map.cities[start].unity2DLocation;
       // Vector2 endLoc = map.cities[end].unity2DLocation;
        Cell cell1 = map.GetCell(map.GetCity(start).unity2DLocation);
        Cell cell2 = map.GetCell(map.GetCity(end).unity2DLocation);
        List<int> cellIndices = map.FindRoute(cell1, cell2, TERRAIN_CAPABILITY.OnlyGround);
        if (cellIndices == null) return;
        int positionsCount = cellIndices.Count;
        Debug.Log("Number of positions: " + positionsCount);

        Vector2[] positions = new Vector2[positionsCount];
        for (int k = 0; k < positionsCount; k++)
        {
            positions[k] = map.cells[cellIndices[k]].center;
        }

        // Build a road along the map coordinates
        LineMarkerAnimator lma = map.AddLine(positions, Color.white, 0.0f, 0.15f);
        Texture2D railwayMatt = Resources.Load("Sprites/GUI/railRoad", typeof(Texture2D)) as Texture2D;

        lma.lineMaterial.mainTexture = railwayMatt;
        lma.lineMaterial.mainTextureScale = new Vector2(16f, 2f);
        // Province startProv = map.GetProvince(start);
        //  Province endProv = map.GetProvince(end);
        //  Vector2 startCent = startProv.center;
        //  Vector2 endCent = endProv.center;

        // Cell startCell = map.GetCell(startCent);
        //  Cell endCell = map.GetCell(startCent);
        //Debug.Log(startCell.points);
        //List<int> startCells = map.GetCellsInProvince(start);
        // List<int> endCells =  map.GetCellsInProvince(end);
        // Cell startCell = map.cells[startCells[0]];
        // Cell endCell = map.cells[endCells[0]];
        // List<int> routePoints = map.FindRoute(startProv, endProv, 12);
        // List<int> cellIndices = map.FindRoute(beginLoc, endLoc, TERRAIN_CAPABILITY.OnlyGround);
       // LineMarkerAnimator pathLine = map.AddLine(beginLoc, endLoc, Color.white, 0.5f, 3f);

        /* if (cellIndices == null)
         {
             Debug.Log("No cell indices!!!!");

             return;
         }
         int positionsCount = cellIndices.Count;
         Vector2[] positions = new Vector2[positionsCount];
         for (int k = 0; k < positionsCount; k++)
         {
             positions[k] = map.cells[cellIndices[k]].center;
         }
         Texture2D railwayMatt = Resources.Load("Sprites/GUI/railroad2", typeof(Texture2D)) as Texture2D;
         // Build a road along the map coordinates
         //LineMarkerAnimator pathLine = map.AddLine(startProv.center, endProv.center, Color.white, 0, 0.1f);
         LineMarkerAnimator pathLine = map.AddLine(positions, Color.white, 0, 1.5f); */
       // Texture2D railwayMatt = Resources.Load("Sprites/GUI/railroad2", typeof(Texture2D)) as Texture2D;

      //  pathLine.lineMaterial.mainTexture = railwayMatt;
      //  pathLine.lineMaterial.mainTextureScale = new Vector2(8f, 1f);
    }


    public void drawRailRoadsFromScratch(Nation player)
    {
        HashSet<int> allProvinces = PlayerCalculator.getAllProvinces(player);
        foreach(int provIndex in allProvinces)
        {
            assemblyCsharp.Province prov = State.getProvinces()[provIndex];
            if (prov.railroad)
            {
                drawRailroadOnMap(prov, player);

            }
        }
        
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
