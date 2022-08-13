using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class BT_Sound_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/Tables/BT_Sound.xls";
	private static readonly string exportPath = "Assets/Resources/Tables/BT_Sound.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			BT_Sound data = (BT_Sound)AssetDatabase.LoadAssetAtPath (exportPath, typeof(BT_Sound));
			if (data == null) {
				data = ScriptableObject.CreateInstance<BT_Sound> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					BT_Sound.Sheet s = new BT_Sound.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						BT_Sound.Param p = new BT_Sound.Param ();
						
					cell = row.GetCell(0); p.Index = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.FilePath = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.FileName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.Volum = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.Loop = (cell == null ? false : cell.BooleanCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
