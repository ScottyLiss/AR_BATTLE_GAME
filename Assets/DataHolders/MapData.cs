using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MapData : DataHolder
{
	public Vector3 petPosition;
	public ResourceMapData[] resourcesData;
	public DateTime resourcesLastUpdated;
	public BreachMapData[] breachesData;
}

