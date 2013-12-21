using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject main;                     // Main menu group
	public GameObject gameBoard;				// The gameboard group

	enum GameState { MainMenu, Playing, Paused };
	GameState gameState = GameState.MainMenu;
	
	Ray ray;
    RaycastHit2D hit;                           // The raycast to detect the target item
    Vector3 inputPos;
	
	// Update is called once per frame
	void Update () {
        ray = Camera.main.ScreenPointToRay(GetInputPosition());
		hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
	
		if (HasInput() && hit.collider != null) {
			switch (gameState) {
		    	case (GameState.MainMenu):
		    		if (hit.transform.name == "Play") {
                		StartCoroutine(PlayPressed(hit.transform));
					} 
				break;
				case (GameState.Paused):
					if (hit.transform.name == "backButton") {
						StartCoroutine(BackPressed(hit.transform));
					}
					break;
				case (GameState.Playing):
					//
					Debug.Log("Game on!!");
					break;
			}
		}
	}
	
    //Called when the play button is pressed
    IEnumerator PlayPressed(Transform button)
    {
        StartCoroutine(Animate(button, 0.1f, 0.2f));
        yield return new WaitForSeconds(0.3f);
		gameState = GameState.Playing;
        main.SetActive(false);
		gameBoard.SetActive(true);
    }

	//Called when the back button is pressed
	IEnumerator BackPressed(Transform button)
	{
		StartCoroutine(Animate(button, 0.1f, 0.2f));
		yield return new WaitForSeconds(0.3f);
		gameBoard.SetActive(false);
		main.SetActive(true);
	}
	
    //Returns true if there is an active input
    bool HasInput()
    {
		return Input.GetMouseButtonDown(0);
    }
    //Returns true, if the input was released
    bool InputReleased()
    {
        return Input.GetMouseButtonUp(0);
    }
    //Returns the input position
    Vector3 GetInputPosition()
    {
        return Input.mousePosition;
    }

    //Animates a button
    IEnumerator Animate(Transform button, float scaleFactor, float time)
    {
        Vector3 originalScale = button.localScale;

        float rate = 1.0f / time;
        float t = 0.0f;

        float d = 0;

        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            button.localScale = originalScale + (originalScale * (scaleFactor * Mathf.Sin(d * Mathf.Deg2Rad)));

            d = 180 * t;
            yield return new WaitForEndOfFrame();
        }

        button.localScale = originalScale;
    }

}
