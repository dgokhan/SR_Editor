using System;
using System.IO;
using System.Xml.Serialization;

namespace SR_Editor.Core
{
	public class QBaseEntity
	{
		public QBaseEntity()
		{
		}

		protected virtual new Type GetType()
		{
			return typeof(QBaseEntity);
		}

		private string SerializeEntity()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
			TextWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, this);
			return stringWriter.ToString();
		}

		public override string ToString()
		{
			return this.SerializeEntity();
		}
	}
}