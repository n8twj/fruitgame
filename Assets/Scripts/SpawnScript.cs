using UnityEngine;
using System.Collections;

// This script generates the game pieces and detects matches (from input) 

public class SpawnScript : MonoBehaviour {
	public Transform[] FruitPrefabList;  
	public Vector3 Size;
	public int SpawnCount;
	public float SpawnDelay;
	
	private Transform SpawnPoint; 
	
	// Called at the startup of the app
	void Start() {
		SpawnFruit();
		SpawnPoint = GameObject.Find("SpawnPoint").transform;
		StartCoroutine("SpawnEvent");
	}	
	void SpawnFruit() {
		// 2D array (effectively) - i used to have a specific array defined, which we may have to bring back
		for (int x=0; x<Size.x; x++) {
			for (int y=0; y<Size.y; y++) {
				// this is the phsycial spawn point in game, relative to the SpawnPoint object
				Vector3 spawnVector3 = new Vector3(transform.position.x + x + 2,transform.position.y + y, 0);
				// Actual gamepiece 
				Transform newCell;
				// pick a random piece 
	 			newCell = (Transform)Instantiate(FruitPrefabList[Random.Range (0, FruitPrefabList.Length)], spawnVector3, Quaternion.identity);
				newCell.parent = transform;
			}
		}
	}	
    private IEnumerator SpawnEvent() {
		while (true) { 
			yield return new WaitForSeconds(SpawnDelay);
			// Calculate a full grid amount of fruit, plus one extra row
			int tooManyFruit = (int)Size.x * (int)Size.y + (int)Size.y;
			if (SpawnPoint.childCount >= tooManyFruit) {
				continue;
			}
			int position = Random.Range (0, (int)Size.y);
			for (int i=0; i<SpawnCount; i++) {
				int x_spawn =  Random.Range(0, (int)Size.x);
				Vector3 spawnVector3 = new Vector3(transform.position.x + x_spawn, transform.position.y + position, 0);
				Transform newCell;
	 			newCell = (Transform)Instantiate(FruitPrefabList[Random.Range (0, FruitPrefabList.Length)], spawnVector3, Quaternion.identity);
				newCell.parent = transform;
			}
		}	
	}

	void FixedUpdate() {
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Mouse Button Down");

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
			
			Debug.Log("Hit: " + hit.transform);
			
			if (hit.collider != null) {
				Debug.Log("RayCast2D: " + hit.transform.tag);
				if (hit.transform.tag == "Floor") {
					Debug.Log("Hit the floor");
					return;	
				}
				
				// Grab the attached component
				// hit is the actual gameobject that was clicked in gameplay
				CellScript cScript = hit.transform.gameObject.GetComponent<CellScript>();
				int matched = cScript.DetectMatches(hit.transform.gameObject);
				if (matched >= 1) {
					cScript.DestroyMatches(hit.transform.gameObject);
				} 
			} else {
				Debug.Log("collider null: " + hit.transform);
			}	
		}
	}
}
