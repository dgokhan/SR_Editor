using System;

namespace SR_Editor.Core
{
	public class ParameterValue
	{
		private object @value;

		private bool isStaticValue = true;

		public bool IsStaticValue
		{
			get
			{
				return this.isStaticValue;
			}
			set
			{
				this.isStaticValue = value;
			}
		}

		public object Value
		{
			get
			{
				return this.@value;
			}
			set
			{
				this.@value = value;
			}
		}

		public ParameterValue()
		{
		}
	}
}