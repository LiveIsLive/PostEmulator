using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.Models.Converters
{
	public class HttpMethodConverter : Newtonsoft.Json.JsonConverter
	{
		protected static readonly System.Type ObjectType = typeof(System.Net.Http.HttpMethod);

		public override bool CanConvert(Type objectType)
		{
			return objectType == ObjectType;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return new System.Net.Http.HttpMethod(reader.Value.ToString());
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}
	}
}
