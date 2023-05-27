using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.HttpClientPerformer.Models
{
	public enum Themes : byte
	{
		Light,
		Dark,
		Red,
		Green,
		Blue,
		Purple,
		Orange,
		Lime,
		Emerald,
		Teal,
		Cyan,
		Cobalt,
		Indigo,
		Violet,
		Pink,
		Magenta,
		Crimson,
		Amber,
		Yellow,
		Brown,
		Olive,
		Steel,
		Mauve,
		Taupe,
		Sienna
	}

	public class Theme
	{
		protected static readonly Themes[] BaseThemes = new Themes[] { Themes.Light, Themes.Dark };

		protected static readonly Themes[] ColorSchemes = System.Enum.GetValues(typeof(Themes)).OfType<Themes>().Where(t => !BaseThemes.Contains(t)).ToArray();

		private static Theme[] _All;
		public static Theme[] All
		{
			get
			{
				if(_All==null)
				{
					System.Collections.Generic.List<Theme> all = new List<Theme>();
					foreach(Themes baseTheme in BaseThemes)
					{
						Theme theme = new Theme();
						theme.Value = baseTheme;
						theme.Children = ColorSchemes.Select(colorScheme => new Theme { Parent = theme, Value = colorScheme }).ToArray();
						all.Add(theme);
					}
					_All = all.ToArray();
				}
				return _All;
			}
		}

		private static Theme _Default;
		public static Theme Default
		{
			get
			{
				if (_Default == null)
					_Default = All.First(t => t.Value == Themes.Light).Children.First(t => t.Value == Themes.Blue);
				return _Default;
			}
		}

		[Newtonsoft.Json.JsonProperty]
		public Theme Parent { get; set; }

		[Newtonsoft.Json.JsonProperty]
		public Themes Value { get; set; }

		public Theme[] Children { get; set; }

		private int _ThemeId;
		public int ThemeId
		{
			get
			{
				if (this._ThemeId == 0)
					this._ThemeId = (byte)(this.Parent?.Value ?? 0) * 100 + (byte)this.Value;
				return this._ThemeId;
			}
		}

		private string _Name;
		public string Name
		{
			get
			{
				if (this._Name == null)
					this._Name = this.Parent?.Value + "." + this.Value;
				return this._Name;
			}
		}

		public static Theme FromId(int themeId)
		{
			foreach (Theme baseTheme in All)
				foreach (Theme colorScheme in baseTheme.Children)
					if (colorScheme.ThemeId == themeId)
						return colorScheme;
			return Default;
		}

		public override string ToString()
		{
			return this.Name;
		}

		//public int CompareTo(Theme other)
		//{
		//	return this.CompareValue.CompareTo(other.CompareValue);
		//}

		public override int GetHashCode()
		{
			return this.ThemeId;
		}

		public override bool Equals(object obj)
		{
			return this.ThemeId == ((Theme)obj).ThemeId;
		}
	}
}