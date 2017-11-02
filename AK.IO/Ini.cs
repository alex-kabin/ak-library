using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AK.Essentials.Extensions;

namespace AK.IO
{
	public static class Ini
	{
		public static IDictionary<string, IDictionary<string, string>> Read(TextReader reader)
		{
			var dict = new Dictionary<string, IDictionary<string, string>>();
			var formatter = new IniFormatter();
			
			string section = string.Empty;

			int lineCounter = 1;
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				var element = formatter.ParseLine(line);
				if (element is IniSection)
					section = element.Value.Trim();
				else if (element is IniKeyValue)
				{
					var keyValueElement = (IniKeyValue)element;
					var key = keyValueElement.Key.Trim();
					var value = keyValueElement.Value.Trim();

					IDictionary<string, string> sectionDict;
					if (!dict.TryGetValue(section, out sectionDict))
					{
						sectionDict = new Dictionary<string, string>();
						dict.Add(section, sectionDict);
					}

					sectionDict.AddOrSet(key, value);
				}

				lineCounter++;
			}

			return dict;
		}

		public static void Write(TextWriter writer, IDictionary<string, IDictionary<string, object>> data)
		{
			var formatter = new IniFormatter();
			foreach (var section in data.Keys)
			{
				var sectionElement = new IniSection(section.Trim());
				var line = formatter.FormatLine(sectionElement);
				writer.WriteLine(line);

				foreach (var keyValue in data[section])
				{
					var keyValueElement = new IniKeyValue(keyValue.Key.Trim(), keyValue.Value.ToString().Trim());
					line = formatter.FormatLine(keyValueElement);
					writer.WriteLine(line);
				}
			}
		}

		public static void Write(TextWriter writer, IEnumerable<IGrouping<string, KeyValuePair<string, object>>> data)
		{
			var formatter = new IniFormatter();
			foreach (var sectionGroup in data)
			{
				var sectionElement = new IniSection(sectionGroup.Key.Trim());
				var line = formatter.FormatLine(sectionElement);
				writer.WriteLine(line);

				foreach (var keyValue in sectionGroup)
				{
					var keyValueElement = new IniKeyValue(keyValue.Key.Trim(), keyValue.Value.ToString().Trim());
					line = formatter.FormatLine(keyValueElement);
					writer.WriteLine(line);
				}
			}
		}
	}

	internal abstract class IniElement
	{
		public static readonly IniEmpty Empty = new IniEmpty();

		public string Value { get; protected set; }

		protected IniElement(string value)
		{
			Value = value;
		}
	}

	internal sealed class IniSection : IniElement
	{
		public IniSection(string section) : base(section) { }
	}

	internal sealed class IniComment : IniElement
	{
		public IniComment(string comment) : base(comment) { }
	}

	internal sealed class IniKeyValue : IniElement
	{
		public string Key { get; private set; }

		public IniKeyValue(string key, string value) : base(value)
		{
			Key = key;
		}
	}

	internal sealed class IniEmpty : IniElement
	{
		internal IniEmpty() : base(string.Empty) { }
	}

	internal class IniFormatter
	{
		private const string CommentPattern = "^;(.*)";
		private const string SectionPattern = "^[(.*)]";
		private const string KeyValuePattern = "^(.+)=(.*)";

		public IniElement ParseLine(string line)
		{
			if (line != null)
			{
				var trimmedLine = line.Trim();
				if (string.IsNullOrEmpty(trimmedLine))
					return IniElement.Empty;

				Match match = Regex.Match(trimmedLine, CommentPattern);
				if (match.Success)
					return new IniComment(match.Groups[0].Value);

				match = Regex.Match(trimmedLine, SectionPattern);
				if (match.Success)
					return new IniSection(match.Groups[0].Value);

				match = Regex.Match(trimmedLine, KeyValuePattern);
				if (match.Success)
					return new IniKeyValue(match.Groups[0].Value, match.Groups[1].Value);
			}
			return null;
		}

		public string FormatLine(IniElement element)
		{
			if (element is IniComment)
				return ";" + element.Value;
			
			if (element is IniSection)
				return "[" + element.Value + "]";
			
			if (element is IniKeyValue)
			{
				var keyValueElement = (IniKeyValue)element;
				return keyValueElement.Key + "=" + keyValueElement.Value;
			}

			return string.Empty;
		}
	}
}