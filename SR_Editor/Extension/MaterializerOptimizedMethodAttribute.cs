using System;
using System.Globalization;
using System.Reflection;

namespace SR_Editor.Core
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public sealed class MaterializerOptimizedMethodAttribute : Attribute
	{
		private readonly IMaterializerMethodOptimizer optimizer;

		internal IMaterializerMethodOptimizer Optimizer
		{
			get
			{
				return this.optimizer;
			}
		}

		public Type OptimizerType
		{
			get
			{
				return this.optimizer.GetType();
			}
		}

		public MaterializerOptimizedMethodAttribute(Type optimizerType)
		{
			NETUtility.CheckArgumentNotNull<Type>(optimizerType, "optimizerType");
			ConstructorInfo constructor = optimizerType.GetConstructor(Type.EmptyTypes);
			if ((!typeof(IMaterializerMethodOptimizer).IsAssignableFrom(optimizerType) ? true : null == constructor))
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				string invalidOptimizerType = ""/*Messages.InvalidOptimizerType*/;
				object[] objArray = new object[] { typeof(IMaterializerMethodOptimizer) };
				throw new ArgumentException(string.Format(invariantCulture, invalidOptimizerType, objArray), "optimizerType");
			}
			this.optimizer = (IMaterializerMethodOptimizer)constructor.Invoke(null);
		}
	}
}