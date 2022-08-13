using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BT_Sound : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public int Index;
		public string FilePath;
		public string FileName;
		public float Volum;
		public bool Loop;
	}
}

