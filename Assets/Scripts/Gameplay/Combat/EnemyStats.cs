using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{

	private int _health;
	private int _maxHealth;
	private int _armour;
	private int _damage;
	private float _attackSpeed;
	private int _staggerResist;

	public int Health
	{
		get { return _health; }
		set { _health = value; }
	}

	public int Armour
	{
		get { return _armour; }
		set { _armour = value; }
	}

	public int Damage
	{
		get { return _damage; }
		set { _damage = value; }
	}

	public float AttackSpeed
	{
		get { return _attackSpeed; }
		set { _attackSpeed = value; }
	}

	public int StaggerResist
	{
		get { return _staggerResist; }
		set { _staggerResist = value; }
	}

	public int MaxHealth
	{
		get { return _maxHealth; }
		set { _maxHealth = value; }
	}
}
