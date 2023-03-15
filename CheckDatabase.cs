using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirebaseREST;

public class CheckDatabase : MonoBehaviour
{
	public string DataPath{
		get{
			return _dataPath;
		}
		set{
			_dataPath = value;
		}
	}
	[SerializeField]
	private string _dataPath;
	
	public string DataLocation{
		get{
			return _dataLocation;
		}
		set{
			_dataLocation = value;
		}
	}
	[SerializeField]
	private string _dataLocation;
	
	public BoolEvent DataExistsEvent;
	
	
	public void CheckIfExists(string data){
		bool output = false;
		DatabaseReference reference = FirebaseDatabase.Instance.GetReference(DataPath+DataLocation);
		reference.GetValueAsync(10,(res) =>{
			if(res.success){
				DataSnapshot snapshot = res.data;
				Debug.Log(snapshot.GetRawJsonValue(),gameObject);
				output = snapshot.HasChild(data);
			} else {
				Debug.Log("failed " + res.message);
			}
			Debug.Log("data exists " + DataPath + DataLocation + data + " " + output,gameObject);
			DataExistsEvent.Invoke(output);
		});
		
	}
}
