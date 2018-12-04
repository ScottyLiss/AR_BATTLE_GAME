using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BreachMapData : DataHolder
{
	private Vector3 breachPosition;
	private int breachDifficulty;
	private DateTime breachStateChanged;
	private BreachState lastBreachState;
}

