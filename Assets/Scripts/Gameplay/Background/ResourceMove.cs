using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMove : MonoBehaviour
{

	private Vector3 positionTarget = Vector3.zero;

    public Vector3 t_pos
    {
	    get { return positionTarget; }

	    set
	    {
		    
		    StopAllCoroutines();
		    positionTarget = value;
		    
		    MoveToLocation();
	    }
    }

    void MoveToLocation()
    {
	    if(t_pos != Vector3.zero)
	    {
		    StartCoroutine(MoveToLocationCoroutine());
	    }
    }

    private IEnumerator MoveToLocationCoroutine()
    {

	    while (Vector3.Distance(this.transform.position, t_pos) > 0.1f)
	    {
		    this.transform.position = Vector3.Lerp(this.transform.position, t_pos, 0.1f);
		    
		    yield return new WaitForEndOfFrame();
	    }
	    
    }
}
