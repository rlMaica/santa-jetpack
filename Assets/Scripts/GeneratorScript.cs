using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour {

	public GameObject[] salas;
	public List<GameObject> salasAtuais;
	private float screenWidthInPoints;

	public GameObject[] availableObjects;    
	public List<GameObject> objects;

	public float objectsMinDistance = 5.0f;    
	public float objectsMaxDistance = 10.0f;

	public float objectsMinY = -1.4f;
	public float objectsMaxY = 1.4f;

	public float objectsMinRotation = -45.0f;
	public float objectsMaxRotation = 45.0f; 

	// Use this for initialization
	void Start () {
		float height = 2.0f * Camera.main.orthographicSize;
		screenWidthInPoints = height * Camera.main.aspect;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {
		GenerateRoomIfRequired ();
		GenerateObjectsIfRequired ();
	}

	void AddSala (float farhtestRoomEndX) {
		int randomRoomIndex = Random.Range(0, salas.Length);
		GameObject room = (GameObject)Instantiate(salas[randomRoomIndex]);
		float roomWidth = room.transform.Find("PISO").localScale.x;
		float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;
		room.transform.position = new Vector3(roomCenter, 0, 0);
		salasAtuais.Add(room);         
	} 

	void GenerateRoomIfRequired () {
		List<GameObject> salasRemover = new List<GameObject>();
		bool addSalas = true;
		float playerX = transform.position.x;
		float removeSalaX = playerX - screenWidthInPoints;        
		float addSalaX = playerX + screenWidthInPoints;
		float farthestRoomEndX = 0;

		foreach(var sala in salasAtuais){
			float salaWidth = sala.transform.Find("PISO").localScale.x;
			float salaStartX = sala.transform.position.x - (salaWidth * 0.5f);    
			float salaEndX = salaStartX + salaWidth;                            

			if (salaStartX > addSalaX) {
				addSalas = false;
			}
			
			if (salaEndX < removeSalaX) {
				salasRemover.Add (sala);
			}
			
			farthestRoomEndX = Mathf.Max(farthestRoomEndX, salaEndX);
		}

		foreach(var sala in salasRemover){
			salasAtuais.Remove(sala);
			Destroy(sala);            
		}

		if (addSalas) {
			AddSala (farthestRoomEndX);
		}
	}

	void AddObject(float lastObjectX){
		int randomIndex = Random.Range(0, availableObjects.Length);
		GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);
		float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
		float randomY = Random.Range(objectsMinY, objectsMaxY);
		obj.transform.position = new Vector3(objectPositionX,randomY,0); 
		float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
		obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
		objects.Add(obj);            
	}

	void GenerateObjectsIfRequired(){
		float playerX = transform.position.x;        
		float removeObjectsX = playerX - screenWidthInPoints;
		float addObjectX = playerX + screenWidthInPoints;
		float farthestObjectX = 0;

		List<GameObject> objectsToRemove = new List<GameObject>();

		foreach (var obj in objects){
			float objX = obj.transform.position.x;
			farthestObjectX = Mathf.Max(farthestObjectX, objX);

			if (objX < removeObjectsX)            
				objectsToRemove.Add(obj);
		}

		foreach (var obj in objectsToRemove)
		{
			objects.Remove(obj);
			Destroy(obj);
		}

		if (farthestObjectX < addObjectX)
			AddObject(farthestObjectX);
	}
}
