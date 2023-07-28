using System.Collections;
using System.Diagnostics;
using System.Xml;

using Xylia.Extension;
using Xylia.Extension.Class;

namespace Xylia.Xml;

public sealed class XmlInfo : IFile
{
	#region Fields
	public XmlDocument Doc;
	#endregion

	#region Constructor
	public XmlInfo()
	{
		this.Doc = new XmlDocument();

		//保留空白区
		//this.Doc.PreserveWhitespace = true;

		CreateDeclar();
	}

	public XmlInfo(XmlNode RootNode, bool Deep = false)
	{
		this.Doc = new XmlDocument();

		this.CreateDeclar();
		this.CreateRootNode(this.Doc.ImportNode(RootNode, Deep));
	}
	#endregion


	#region Root
	private void CreateRootNode(XmlNode RootNode)
	{
		this.Doc.AppendChild(RootNode);
		this.Doc.DocumentElement.AppendChild(this.Doc.CreateComment(""));
	}

	private void CreateRootNode()
	{
		this.Doc.AppendChild(this.CreateElement("table"));
		this.Doc.DocumentElement.AppendChild(this.Doc.CreateComment(""));
	}
	#endregion

	#region Element
	public XmlElement CreateElement(string ElementName) => this.Doc.CreateElement(ElementName);

	public XmlElement CreateElement(string ElementName, object obj)
	{
		var element = CreateElement(ElementName);
		if (obj is null) return element;

		Type type = obj.GetType();
		if (!type.IsValueType || type.IsEnum || type.IsPrimitive)
		{
			//type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(List<>))
			if (obj is IEnumerable array)
			{
				foreach (var t in array)
				{
					var Child = (XmlElement)element.AppendChild(CreateElement("case"));
					if (t is null) continue;

					//数值类型
					var ValueType = t.GetType();
					if (!ValueType.IsValueType || ValueType.IsEnum || ValueType.IsPrimitive) Child.InnerText = t.ToString();
					else CreateElement(Child, t, ValueType.ContainAttribute<SerializableAttribute>());
				}
			}

			else element.InnerText = obj.ToString();
		}
		else CreateElement(element, obj);

		return element;
	}

	private void CreateElement(XmlElement XmlElement, object obj, bool Mode = false)
	{
		foreach (var Field in obj.GetType().GetFields(ClassExtension.Flags))
		{
			var Value = Field.GetValue(obj);
			if (Value is null) continue;

			if (Value is Array array)
			{
				string result = null;

				if (Mode)
				{
					var ArrayElement = XmlElement.AppendChild(CreateElement(Field.Name));
					foreach (var g in array)
					{
						var e = CreateElement("case");
						e.SetAttribute("value", g.ToString());

						ArrayElement.AppendChild(e);
					}
				}
				else
				{
					foreach (var g in array) result += g.ToString() + ";";
					XmlElement.SetAttribute(Field.Name, result);
				}
			}
			else if (Mode)
			{
				var e = CreateElement(Field.Name);
				XmlElement.AppendChild(e);

				e.SetAttribute("value", Value.ToString());
			}
			else XmlElement.SetAttribute(Field.Name, Value.ToString());
		}
	}
	#endregion

	#region Else
	private void CreateDeclar(string Encoding = "utf-8") => this.Doc.AppendChild(this.Doc.CreateXmlDeclaration("1.0", Encoding, null));

	public XmlCDataSection CreateCData(string ElementName) => this.Doc.CreateCDataSection(ElementName);

	public XmlWhitespace CreateWhitespace(string InnerText) => this.Doc.CreateWhitespace(InnerText);

	public static void CreatePathComment(XmlDocument xmlDoc, string SavePath, bool ShowRelativePath = false, string RelativePrefix = null) =>
		CreatePathComment(xmlDoc, SavePath,
			ShowRelativePath ? string.Empty : null,
			ShowRelativePath ? RelativePrefix : null);

	public static void CreatePathComment(XmlDocument xmlDoc, string SavePath, string RelativePath, string RelativePrefix = null)
	{
		#region 移除路径信息
		var Root = xmlDoc.DocumentElement ?? throw new Exception("请创建根节点");
		var Nodes = Root.ChildNodes;

		//如果首节点是注释类节点
		if (Nodes.Count > 0 && Nodes.Item(0).NodeType == XmlNodeType.Comment)
			Root.RemoveChild(Nodes.Item(0));
		#endregion

		#region 创建顶级注释
		//创建注释内容文本
		string CommentTxt = null;
		if (SavePath is not null || RelativePath is not null)
		{
			SavePath = SavePath?.Replace(@"\", "/");

			CommentTxt = RelativePath;

			//使用存储绝对路径
			if (RelativePrefix is null && RelativePath is null) CommentTxt = SavePath?.Replace(@"sort\", null);
			//自动生成相对路径
			else if (RelativePath == string.Empty) CommentTxt = (string.IsNullOrWhiteSpace(RelativePrefix) ? null : RelativePrefix + "\\") + Path.GetFileName(SavePath);
		}

		//创建注释信息
		XmlComment Comment = xmlDoc.CreateComment(" " + CommentTxt + " ");
		if (Root.ChildNodes == null || Root.ChildNodes.Count == 0) Root.AppendChild(Comment);
		else Root.InsertBefore(Comment, Root.ChildNodes[0]);
		#endregion
	}
	#endregion

	#region AppendChild
	/// <summary>
	/// 向根节点增加子节点
	/// </summary>
	/// <param name="XNode"></param>
	/// <param name="Deep">当克隆其他文档的节点时，进行深度克隆</param>
	/// <returns></returns>
	public XmlNode AppendChild(XmlNode XNode, bool Deep = false)
	{
		//判断是否是自身节点，如果不是先进行克隆
		if (XNode.OwnerDocument != this.Doc) XNode = this.Doc.ImportNode(XNode, Deep);

		//进行字段名排序
		//if(xmlNode.NodeType == XmlNodeType.Element) xmlNode.XmlSort(false);
		if (this.Doc.DocumentElement is null) this.CreateRootNode();
		return this.Doc.DocumentElement.AppendChild(XNode);  //
	}

	public void AppendChild(XmlNodeList XNodeList, bool Deep = false)
	{
		foreach (XmlNode XNode in XNodeList)
		{
			AppendChild(XNode, Deep);
		}
	}

	public void AppendXNode(string NodeName, object NodeValue)
	{
		var Xe = this.CreateElement(NodeName);

		Xe.InnerText = NodeValue.ToString();
		this.AppendChild(Xe);
	}

	public void AppendXNode(object NodeValue)
	{
		// 如果要获取上层函数信息调用 GetFrame(1)， 这样就可以写成通用函数了
		var methodBase = new StackTrace().GetFrame(0).GetMethod();
		//Console.WriteLine("函数名：" + methodBase.Name);

		var parameterInfos = methodBase.GetParameters();
		var Xe = this.CreateElement(parameterInfos.First().Name);

		Xe.InnerText = NodeValue.ToString();
		this.AppendChild(Xe);
	}
	#endregion

	#region Save
	public void Save(string FilePath, bool ShowRelativePath = false, string RelativePrefix = null, int RetryTime = 20) => Save(FilePath, ShowRelativePath ? string.Empty : null, RelativePrefix, RetryTime);

	public void Save(string FilePath, string RelativePath, string RelativePrefix = null, int RetryTime = 20)
	{
		this.SavePathHandle(ref FilePath);

		try
		{
			//创建顶级注释
			CreatePathComment(this.Doc, FilePath, RelativePath, RelativePrefix);

			Doc.Save(FilePath);
		}
		catch (Exception ee)
		{
			throw new Exception($".Xml文件存储失败\n参数:存储路径 => {FilePath}", ee);
		}
	}

	public void Save(string Folder, string RelativePath, bool ShowRelativePath = false, int RetryTime = 20)
	{
		string SavePath = Folder + @"\" + RelativePath;
		this.SavePathHandle(ref SavePath);

		try
		{
			//创建顶级注释
			if (ShowRelativePath) CreatePathComment(Doc, RelativePath);
			else CreatePathComment(Doc, SavePath);

			Doc.Save(SavePath);
		}
		catch (Exception ee)
		{
			throw new Exception($".Xml文件存储失败\n参数:存储路径 => {SavePath}", ee);
		}
	}

	private void SavePathHandle(ref string SavePath)
	{
		#region 创建目录
		SavePath = SavePath.Replace(".*", null).Replace("*", null);

		string DirPath = Path.GetDirectoryName(SavePath);
		if (!Directory.Exists(DirPath)) Directory.CreateDirectory(DirPath);
		#endregion

		#region 路径处理
		string RelativePath = Path.GetFileName(SavePath);

		//追加后缀名
		//x16、x32是剑灵专用标记文档
		if (!RelativePath.MyEndsWith(".xml") &&
			!RelativePath.MyEndsWith(".x16") &&
			!RelativePath.MyEndsWith(".x32")) RelativePath += ".xml";

		//替换非法字符
		if (RelativePath.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
		{
			Path.GetInvalidFileNameChars().ToList().ForEach(c => RelativePath = RelativePath.Replace(c.ToString(), "_"));
		}

		//修改路径
		SavePath = DirPath + @"\" + RelativePath;
		#endregion


		//校验根节点是否存在
		if (this.Doc.DocumentElement is null)
			this.CreateRootNode();
	}

	public byte[] SaveData(string CommentPath = null, bool UseRelative = false)
	{
		try
		{
			#region 路径处理
			if (!string.IsNullOrWhiteSpace(CommentPath))
			{
				if (UseRelative) CommentPath = Path.GetFileName(CommentPath.Replace(".*", null).Replace("*", null));
				if (!CommentPath.Contains('.')) CommentPath = CommentPath += ".xml";
			}
			#endregion


			//创建顶级注释
			CreatePathComment(Doc, CommentPath);

			MemoryStream Stream = new();
			Doc.Save(Stream);

			return Stream.ToBytes();
		}
		catch (Exception ee)
		{
			throw new Exception("[.Xml文件存储失败] " + ee.Message);
		}
	}
	#endregion


	#region Dispose
	private bool disposedValue;

	void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				// TODO: 释放托管状态(托管对象)
			}

			this.Doc = null;

			// TODO: 释放未托管的资源(未托管的对象)并替代终结器
			// TODO: 将大型字段设置为 null
			disposedValue = true;
		}
	}

	// // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
	// ~ExcelInfo()
	// {
	//     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
	//     Dispose(disposing: false);
	// }

	public void Dispose()
	{
		// 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
	#endregion
}