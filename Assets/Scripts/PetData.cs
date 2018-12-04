using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PetData
{
	public Stats stats;
	public List<Trait> traits;
	public int hunger = 100;
}
