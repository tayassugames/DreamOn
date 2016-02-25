using UnityEngine;
using System.Collections;
 
public class AnimatedLevelTiles : MonoBehaviour
{
    public int columns = 2;
    public int rows = 2;
    public float framesPerSecond = 10f;
	public bool PlayOnce = false;
 
    //the current frame to display
    private int index = 0;
	private Vector3 storedPos;
	private Vector2 screenSize;
	
 
    void Start() {
        storedPos = transform.localPosition;
		resetPos();
		Vector2 size = new Vector2(1f / columns, 1f / rows);
        GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", size);
		screenSize = new Vector2(480f,320f);
    }
 
    public void PlaySwipeHorizontal(float xScale, float yScale, Vector2 screenPos) {
		if(!PlayOnce && screenPos != Vector2.zero) {
			setPos();
			PlayOnce = true;
			StartCoroutine(updateTiling());
			
			if(screenPos.y > (screenSize.y * 0.5f)) {
				
				screenPos = new Vector2(screenPos.x, (screenPos.y - screenSize.y * 0.5f) * 4f);
				Debug.Log("Right");
			}
			else {
		
				screenPos = new Vector2(screenPos.x, (screenSize.y * 0.5f - screenPos.y) * -4f);
				Debug.Log("Left");
			}
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)*xScale,
											   Mathf.Abs(transform.localScale.y),
											   transform.localScale.z);
			
			transform.localPosition = new Vector3((storedPos.x), 
				 								  (screenPos.y ), 
				                                  storedPos.z);
		}
	}
	
	public void PlaySwipeVertical(float xScale, float yScale, Vector2 screenPos) {
		if(!PlayOnce && screenPos != Vector2.zero) {
			setPos();
			PlayOnce = true;
			StartCoroutine(updateTiling());
			
			if(screenPos.x > (screenSize.x * 0.5f)) {
				
				screenPos = new Vector2( (screenPos.x - screenSize.x*0.5f)*4f ,screenPos.y);
				Debug.Log("Right");
			}
			else {
		
				screenPos = new Vector2( (screenSize.x*0.5f - screenPos.x)*-4f ,screenPos.y);
				Debug.Log("Left");
			}
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),
											   Mathf.Abs(transform.localScale.y)*yScale,
											   transform.localScale.z);
			
			transform.localPosition = new Vector3((screenPos.x), 
				 								  (storedPos.y - screenPos.y), 
				                                  storedPos.z);
			
		}
		else {
			index = 0;
			setPos();
			PlayOnce = true;
			
			if(screenPos.x > (screenSize.x * 0.5f)) {
				screenPos = new Vector2( (screenPos.x - screenSize.x*0.5f)*4f ,screenPos.y);
				Debug.Log("Right");
			}
			else {
				screenPos = new Vector2( (screenSize.x*0.5f - screenPos.x)*-4f ,screenPos.y);
			
			}
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),
											   Mathf.Abs(transform.localScale.y)*yScale,
											   transform.localScale.z);
			
			transform.localPosition = new Vector3((screenPos.x), 
				 								  (storedPos.y - screenPos.y), 
				                                  storedPos.z);
		}
	}
	
	private void setPos() {
		transform.localPosition = storedPos;
	}
	
	private void resetPos() {
		transform.localPosition = new Vector3(0,10000,0);
	}
	
	private IEnumerator updateTiling()
    {
        index = 0;
		while (PlayOnce)
        {
            //move to the next index
            index++;
            if (index >= rows * columns) {
				PlayOnce = false;
			}
 
            //split into x and y indexes
            Vector2 offset = new Vector2((float)index / columns - (index / columns), //x index
										  (index / columns) );          //y index
										  
 
            GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
 
            yield return new WaitForSeconds(1f / framesPerSecond);
        }
		
		resetPos();
    }
}