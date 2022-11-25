using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirebaseREST;

public class DownloadFromDatabase : MonoBehaviour
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
	
	public StringEvent OutputKey;
	public StringEvent OutputValue;
	public StringEvent OutputResult;
	
	public void Request(){
		DatabaseReference reference = FirebaseDatabase.Instance.GetReference(DataPath + DataLocation);
		reference.GetValueAsync(10, (res) =>
		{
			if (res.success)
			{
				//Debug.Log("Success fetched data : " + res.data.GetRawJsonValue(),gameObject);
				
				string output = res.data.GetRawJsonValue().Replace('"', ' ').Trim();
				OutputResult.Invoke(output);
				
				
			}
			else
			{
				Debug.Log("Fetch data failed : " + res.message);
			}
		});

	}
	public StringStringDictionaryEvent DictionaryEvent;
	
	public void RequestChildren(){
		DatabaseReference reference = FirebaseDatabase.Instance.GetReference(DataPath + DataLocation);
		
		reference.GetValueAsync(10, (res) =>
		{
			if (res.success)
			{
				//Debug.Log("Success fetched data from " + DataPath+DataLocation+ " : " + res.data.GetRawJsonValue());
				DataSnapshot snapshot = res.data;
				var dictionary = snapshot.Value as Dictionary<string,object>;
				if(dictionary!=null){
					foreach (var child in dictionary)
					{
						string key = child.Key;
						var value = child.Value as Dictionary<string,object>;
						Dictionary<string,string> output = new Dictionary<string, string>();
						//Debug.Log(key + " " + child.Value,gameObject);
						foreach(var val in value){
							//Debug.Log(val.Key + " " + val.Value,gameObject);
							if(val.Value.GetType()==typeof(string)){
								output.Add(val.Key,(string)val.Value);	
							}
						
						}
						DictionaryEvent.Invoke(output);
					}
				}
				
			    
			}
			else
			{
				Debug.Log("Fetch data failed : " + res.message + " " + res.data.GetRawJsonValue(),gameObject);
			}
		});
	}
	public StringStringEvent OutputStringString;
	public void StreamDictionaryData(){
		DatabaseReference reference = FirebaseDatabase.Instance.GetReference(DataPath + DataLocation);
		reference.ValueChanged += (sender, e) =>
		{
			DataSnapshot snapshot = e.Snapshot;
			var dictionary = snapshot.Value as Dictionary<string,object>;
			//Debug.Log(snapshot.Value + " " + gameObject.name, gameObject);
			Dictionary<string,string> output = new Dictionary<string, string>();
			foreach (var child in dictionary)
			{
				string key = child.Key;
				var value = child.Value as Dictionary<string,object>;
				if(value == null){
					//Debug.Log("maybe childvalue is not a dictionary?");
					//therefor output string string event
					//output.Add(key,child.Value.ToString());
					OutputStringString.Invoke(key,child.Value.ToString());
					
				} else {
					//Debug.Log("maybe childvalue IS a dictionary?");
					//therefor output as dictionary
					//but what do with key?
					//perforce we output as a named dictionary
					foreach(var val in value){
						
						if(val.Value.GetType()==typeof(string)){
							output.Add(val.Key,(string)val.Value);	
						} else {
							var newVal = val.Value as Dictionary<string,object>;
							
							foreach(var v in newVal){
								output.Add(v.Key,(string)v.Value);
							}
						}
						
					}
					Debug.Log(gameObject,gameObject);
			
					DictionaryEvent.Invoke(output);
				}
				
			}
		};
		reference.DatabaseError += (sender,e)=>{
			Debug.Log(e.DatabaseError.Message);
			Debug.Log("Streaming connection closed");
		};
	}
	
	
	
}
