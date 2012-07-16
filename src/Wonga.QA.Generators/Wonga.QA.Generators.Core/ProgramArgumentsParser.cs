using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Generators.Core
{
	public static class ProgramArgumentsParser
	{
		public static void ParseArgumentsParameters(string[] args)
		{
			var size = args.Count();
			if (size <= 0 )
				return;

			if (size == 1)
			{
				if(IsValidOriginArg(args[0]))
					Config.Origin = args[0];

				else
					throw new Exception(String.Format("{0} is not a valid directory path", args[0]));
			}
		}

		private static bool IsValidOriginArg(String arg)
		{
			return !String.IsNullOrEmpty(arg);
		}
	}
}
