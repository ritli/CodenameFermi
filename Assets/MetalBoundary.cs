using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBoundary : MonoBehaviour
{
	public LayerMask objectLayer;
	private List<GameObject> metalObjectList;
	private List<Vector3> objectStartPosList;
	
	void Start ()
	{
		// !!!!!WARNING!!!!! Spaghetti code ahead
		// Collects all metal objects in the box trigger into a list
		Collider2D[] objectArray = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0, objectLayer);
		List<Collider2D> tempList = new List<Collider2D>();
		metalObjectList = new List<GameObject>();
		objectStartPosList = new List<Vector3>();

		tempList.AddRange(objectArray);
		for (int i = 0; i < tempList.Count; i++)
		{
			metalObjectList.Add(tempList[i].gameObject);
		}
		// Also saves their starting position
		for (int f = 0; f < metalObjectList.Count; f++)
		{
			objectStartPosList.Add(metalObjectList[f].transform.position);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (metalObjectList.Contains(collision.gameObject))
		{
			collision.transform.position = objectStartPosList[metalObjectList.IndexOf(collision.gameObject)];
		}
	}
}
