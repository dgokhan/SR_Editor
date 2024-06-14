using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace SR_Editor.Core
{
	public interface IMaterializerMethodOptimizer
	{
		Expression OptimizeMethodCall(ReadOnlyCollection<string> fieldNames, ParameterExpression recordParameter, MethodCallExpression methodCall);
	}
}