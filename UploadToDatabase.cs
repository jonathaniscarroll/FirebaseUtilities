using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirebaseREST;
using UnityEngine.Events;

public class UploadToDatabase : MonoBehaviour
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
	
	public StringEvent OutputPushID;

	public void SetData(string data)
    {
	    DatabaseReference reference = FirebaseDatabase.Instance.GetReference(DataPath);
	    reference.Child(DataPath).GetValueAsync(10,(res) =>{
	    	if(res.success){
    			reference.Child(DataPath+DataLocation).SetValueAsync(data, 10, task=>{
	    			if(task.success){
		    			Debug.Log("Set data: " + task.data,gameObject);
	    			}
	    			else{
		    			Debug.Log("Set failed : " + task.message);
	    			}
    			});
	    		
	    	} else {
		    	Debug.Log("Does not exists : " + res.message);
	    	}
	    });
    }
    
	public void PushData(string data){
		DatabaseReference reference = FirebaseDatabase.Instance.GetReference(DataPath + DataLocation);
		reference.Push(data, 10, (res)=>{
		    if(res.success){
			    Debug.Log("Pushed with id: " + res.data);
			    OutputPushID.Invoke(res.data);
			   
		    }
		    else{
			    Debug.Log("Push failed : " + res.message);
		    }
		});
	}
	
	public void PushDictionary(Dictionary<string,string> data){
		DatabaseReference reference = FirebaseDatabase.Instance.GetReference(DataPath + DataLocation);
		reference.Push(data, 10, (res)=>{
			if(res.success){
				//Debug.Log("Pushed with id: " + res.data);
				OutputPushID.Invoke(res.data);
			}
			else{
				Debug.Log("Push failed : " + res.message);
			}
		});
	}
	
	public void SetDictionary(Dictionary<string,string> data){
		DatabaseReference reference = FirebaseDatabase.Instance.GetReference(DataPath + DataLocation);
		reference.SetValueAsync(data, 10, (res)=>{
			if(res.success){
				Debug.Log("dictionary set success: " + res.data);
				//OutputPushID.Invoke(res.data);
			}
			else{
				Debug.Log("Push failed : " + res.message);
			}
		});
	}
    



}
