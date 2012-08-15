using System;
using Gallio.Common.Reflection;
using Gallio.Framework.Data;
using Gallio.Framework.Pattern;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Core
{
	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = true, Inherited = true)]
	public class AUTRowAttribute : RowAttribute
	{
		protected AUT Aut { get; set; }

		public AUTRowAttribute(AUT aut, params object[] _params)
			: base(_params)
		{
			Aut = aut;
		}

		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			if (Aut == Config.AUT)
			{
				base.PopulateDataSource(scope, dataSource, codeElement);
			}
		}
	}
}
