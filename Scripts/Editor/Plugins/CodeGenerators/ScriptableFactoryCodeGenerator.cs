﻿/*

MIT License

Copyright (c) Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System.Linq;

namespace JCMG.Genesis.Editor.Plugins
{
	internal sealed class ScriptableFactoryCodeGenerator : ICodeGenerator
	{
		/// <summary>
		/// The name of the plugin.
		/// </summary>
		public string Name => NAME;

		/// <summary>
		/// The priority value this plugin should be given to execute with regards to other plugins,
		/// ordered by ASC value.
		/// </summary>
		public int Priority => 0;

		/// <summary>
		/// Returns true if this plugin should be executed in Dry Run Mode, otherwise false.
		/// </summary>
		public bool RunInDryMode => true;

		private const string NAME = "Scriptable Factory Lookup";
		private static readonly string GENERATOR_NAME;

		static ScriptableFactoryCodeGenerator()
		{
			GENERATOR_NAME = typeof(ScriptableFactoryCodeGenerator).Name;
		}

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var codeGenData = data
				.OfType<FactoryKeyEnumData>()
				.ToArray();

			var codeGenFiles = codeGenData.Select(CreateCodeGenFile).ToArray();

			return codeGenFiles;
		}
		public CodeGenFile CreateCodeGenFile(FactoryKeyEnumData data)
		{
			return new CodeGenFile(
				data.GetFilename(),
				data.ReplaceTemplateTokens(TEMPLATE),
				GENERATOR_NAME);
		}

		private const string TEMPLATE
= @"
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis
{
	[CreateAssetMenu(fileName = ""Default${TypeName}"", menuName = ""Genesis/Factory/${TypeName}"")]
	public sealed partial class ${TypeName} : ScriptableObject
	{
		[Serializable]
		private class Mapping
		{
			#pragma warning disable 0649
			public ${KeyFullType} key;

			public ${ValueFullType} value;
			#pragma warning restore 0649
		}

		#pragma warning disable 0649
		[SerializeField]
		private List<Mapping> _mappings;
		#pragma warning restore 0649

		private Dictionary<${KeyFullType}, Mapping> MappingLookup
		{
			get
			{
				if(_mappingLookup == null)
				{
					_mappingLookup = new Dictionary<${KeyFullType}, Mapping>();
					for (var i = 0; i < _mappings.Count; i++)
					{
						if(_mappingLookup.ContainsKey(_mappings[i].key))
						{
							continue;
						}

						_mappingLookup.Add(_mappings[i].key, _mappings[i]);
					}
				}

				return _mappingLookup;
			}
		}

		private Dictionary<${KeyFullType}, Mapping> _mappingLookup;

		private void OnEnable()
		{
			if(_mappings == null)
			{
				_mappings = new List<Mapping>();

				var values = (${KeyFullType}[])Enum.GetValues(typeof(${KeyFullType}));
				for (var i = 0; i < values.Length; i++)
				{
					_mappings.Add(new Mapping
					{
						key = values[i]
					});
				}
			}
		}

		/// <summary>
		/// Returns true if a mapping is found for <see cref=""${KeyFullType}""/> <paramref name=""key""/> to a
		/// <see cref=""${ValueFullType}""/>, otherwise false.
		/// </summary>
		/// <param name=""key""></param>
		/// <param name=""value""></param>
		/// <returns></returns>
		public bool TryGetValue(${KeyFullType} key, out ${ValueFullType} value)
		{
			value = null;

			Mapping mapping;
			if (!MappingLookup.TryGetValue(key, out mapping))
			{
				return false;
			}

			value = mapping.value;

			return true;
		}
	}
}
";
	}
}
