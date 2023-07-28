namespace Xylia.Attribute;
public class IAttribute
{
	public string Key;

	public string Value;


	#region Constructor
	public IAttribute(string key, string value)
	{
		this.Key = key;
		this.Value = value;
	}

	public IAttribute(KeyValuePair<string, string> pair)
	{
		this.Key = pair.Key;
		this.Value = pair.Value;
	}
	#endregion
}
