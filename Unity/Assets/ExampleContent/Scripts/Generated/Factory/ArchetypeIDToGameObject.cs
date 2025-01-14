
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis
{
	[CreateAssetMenu(fileName = "DefaultArchetypeIDToGameObject", menuName = "Genesis/Factory/ArchetypeIDToGameObject")]
	public sealed partial class ArchetypeIDToGameObject : ScriptableObject
	{
		[Serializable]
		private class Mapping
		{
			#pragma warning disable 0649
			public ExampleContent.ArchetypeID key;

			public UnityEngine.GameObject value;
			#pragma warning restore 0649
		}

		#pragma warning disable 0649
		[SerializeField]
		private List<Mapping> _mappings;
		#pragma warning restore 0649

		private Dictionary<ExampleContent.ArchetypeID, Mapping> MappingLookup
		{
			get
			{
				if(_mappingLookup == null)
				{
					_mappingLookup = new Dictionary<ExampleContent.ArchetypeID, Mapping>();
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

		private Dictionary<ExampleContent.ArchetypeID, Mapping> _mappingLookup;

		private void OnEnable()
		{
			if(_mappings == null)
			{
				_mappings = new List<Mapping>();
			}
		}

		/// <summary>
		/// Returns true if a mapping is found for <see cref="ExampleContent.ArchetypeID"/> <paramref name="key"/> to a
		/// <see cref="UnityEngine.GameObject"/>, otherwise false.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(ExampleContent.ArchetypeID key, out UnityEngine.GameObject value)
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
