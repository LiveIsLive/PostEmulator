using ColdShineSoft.PostEmulator.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.ViewModels
{
	public class Code : Screen
	{
		public Code(BaseTemplate template)
		{
			Template = template;
		}

		public Models.Codes.BaseTemplate Template { get; protected set; }


	}
}