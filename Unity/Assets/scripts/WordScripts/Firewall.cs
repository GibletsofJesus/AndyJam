using UnityEngine;
using System.Collections;

public class Firewall : Word
{
	protected override void Start ()
	{
		base.Start ();
		word = "FIREWALL";
		behavior = Behavior;
		//throw new System.NotImplementedException ();
	}

	protected override void Behavior ()
	{
		//throw new System.NotImplementedException ();
	}
}
