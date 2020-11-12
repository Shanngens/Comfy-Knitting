﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class LevelGen : MonoBehaviour
{
  public Camera mainCam; //Main camera
  public GameObject textObject;

  public TextMeshProUGUI letterText;
  public Texture2D map; //The image material for the level
  private ColorToPrefab[] colorMappings; //Tile data array

  public GameObject prefab; //Prefab for tile generation
  public GameObject playerprefab; //Prefab for player generation

  private GameObject player; //Player object after instantiation
  private string[] currentPassword; //String array of current tile's password
  private int lettersLeft; //Number of letters left
  private int tilesLeft; //Number of tiles left

  public int passwordLength;

    //Called at start
    void Start()
    {
      cameraSetup(); //Sets camera dimensions according to map dimensions
        Debug.Log("Camera setup");
      GenerateLevel(); //Generates array of tile information and instantiates tiles


      //Initializes player state
      tilesLeft = colorMappings.Length-1;
      currentPassword = colorMappings[tilesLeft].password;
      lettersLeft = passwordLength-1;
      letterText.text = currentPassword[lettersLeft];
    }

    //Sets camera dimensions according to map dimensions. Subject to change.
    void cameraSetup(){
      mainCam.enabled = true;
      mainCam.orthographic = true;
      mainCam.orthographicSize = (map.width/2.0f)*1.5f;
      mainCam.transform.position = new Vector3(map.width/2.0f-0.5f, map.height/2.0f-.5f, -10);
      letterText = textObject.GetComponent<TextMeshProUGUI>();
      letterText.transform.position = new Vector3(map.width/2.0f, map.height*0.7f);
    }

    //Called every frame
    void Update()
    {
      if (lettersLeft>=0){
        keyActions(currentPassword[lettersLeft]);
      }
      else{
        if(tilesLeft>0){
          colorMappings[tilesLeft].gameobject.GetComponent<SpriteRenderer>().color = colorMappings[tilesLeft].color;
          tilesLeft--;
          currentPassword = colorMappings[tilesLeft].password;
          lettersLeft = passwordLength-1;
          letterText.text = currentPassword[lettersLeft];
          player.transform.position = colorMappings[tilesLeft].pos;
        }
        else{
          colorMappings[tilesLeft].gameobject.GetComponent<SpriteRenderer>().color = colorMappings[tilesLeft].color;
          Destroy(player);
          letterText.text = "You did it!";
        }
      }
    }

    //Uses player input to progress passwords.
    void keyActions(string k){
      if (Input.GetKeyDown(k)){
        lettersLeft--;
        if(lettersLeft>=0) letterText.text = currentPassword[lettersLeft];
      }
      else if (Input.anyKeyDown){
        lettersLeft = passwordLength-1;
        letterText.text = currentPassword[lettersLeft];
      }
    }

    //Instantiates player, initializes and fills array.
    void GenerateLevel(){
      colorMappings = new ColorToPrefab[map.width*map.height];
      for(int i = 0; i<map.width; i++){
        for(int j = 0; j<map.height; j++){
          GenerateTile(i, j);
          if(i == map.width-1 && j==map.height-1){
            Vector2 pos = new Vector2(i, j);
            player = Instantiate(playerprefab, pos, Quaternion.identity, transform);
          }
        }
      }
    }

    //Generates tile data array and fills initial data.
    void GenerateTile(int x, int y){
        Debug.Log("unity pls");
        Debug.Log(map.GetPixel(x, y)); //
        Color pixelColor = map.GetPixel(x, y);
        Debug.Log("Help");

      if (pixelColor.a == 0) { 
         Debug.Log("??????");
      }
        // return; //ignores transparent pixel

        ColorToPrefab colorMapping = new ColorToPrefab();
      colorMappings[x+y*map.width] = colorMapping;
      colorMapping.password = CreateRandomString(passwordLength);
      colorMapping.pos = new Vector2(x, y);
      colorMapping.color = pixelColor;
      colorMapping.gameobject = Instantiate(prefab, colorMapping.pos, Quaternion.identity, transform);
      //colorMapping.gameobject.GetComponent<SpriteRenderer>().color = pixelColor;
      
    } 

    //Generates random array of characters 
    private string[] CreateRandomString(int stringLength) {
        int _stringLength = stringLength - 1;
        string[] randomString = new string[stringLength];
        string[] characters = new string[] {"a", "s", "d", "q", "w", "e"};
        for (int i = 0; i <= _stringLength; i++) {
            randomString[i] = (characters[Random.Range(0, characters.Length)]);
        }
        return randomString;
    }

}
