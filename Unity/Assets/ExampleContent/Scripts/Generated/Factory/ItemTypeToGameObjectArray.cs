
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis
{
	[CreateAssetMenu(fileName = "DefaultItemTypeToGameObjectArray", menuName = "Genesis/Factory/ItemTypeToGameObjectArray")]
	public sealed partial class ItemTypeToGameObjectArray : ScriptableObject
	{
		[Serializable]
		private class Mapping
		{
			#pragma warning disable 0649
			public ExampleContent.ItemType key;

			public UnityEngine.GameObject[] value;
			#pragma warning restore 0649
		}

		#pragma warning disable 0649
		[SerializeField]
		private List<Mapping> _mappings;
		#pragma warning restore 0649

		private Dictionary<ExampleContent.ItemType, Mapping> MappingLookup
		{
			get
			{
				if(_mappingLookup == null)
				{
					_mappingLookup = new Dictionary<ExampleContent.ItemType, Mapping>();
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

		private Dictionary<ExampleContent.ItemType, Mapping> _mappingLookup;

		private void OnEnable()
		{
			if(_mappings == null)
			{
				_mappings = new List<Mapping>();

				var values = (ExampleContent.ItemType[])Enum.GetValues(typeof(ExampleContent.ItemType));
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
		/// Returns true if a mapping is found for <see cref="ExampleContent.ItemType"/> <paramref name="key"/> to a
		/// <see cref="UnityEngine.GameObject[]"/>, otherwise false.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(ExampleContent.ItemType key, out UnityEngine.GameObject[] value)
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
