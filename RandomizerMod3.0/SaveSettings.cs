﻿using System.Linq;
using Modding;
using RandomizerMod.Actions;
using SeanprCore;
using RandomizerMod.Randomization;
using static RandomizerMod.LogHelper;

namespace RandomizerMod
{
    public class SaveSettings : BaseSettings
    {
        /*
         * UNLISTED BOOLS
         * rescuedSly is used in room randomizer to control when Sly appears in the shop, separately from when the door is unlocked
         */


        private SerializableStringDictionary _itemPlacements = new SerializableStringDictionary();
        private SerializableIntDictionary _orderedLocations = new SerializableIntDictionary();
        public SerializableStringDictionary _transitionPlacements = new SerializableStringDictionary();
        private SerializableStringDictionary _hintInformation = new SerializableStringDictionary();
        private SerializableIntDictionary _variableCosts = new SerializableIntDictionary();
        private SerializableIntDictionary _shopCosts = new SerializableIntDictionary();
        private SerializableIntDictionary _additiveCounts = new SerializableIntDictionary();

        private SerializableBoolDictionary _obtainedItems = new SerializableBoolDictionary();
        private SerializableBoolDictionary _obtainedLocations = new SerializableBoolDictionary();
        private SerializableBoolDictionary _obtainedTransitions = new SerializableBoolDictionary();

        /// <remarks>item, location</remarks>
        public (string, string)[] ItemPlacements => _itemPlacements.Select(pair => (pair.Key, pair.Value)).ToArray();

        public int MaxOrder => _orderedLocations.Count;

        // index is how many hints, pair is item, location
        public (string, string)[] Hints => _hintInformation.Select(pair => (pair.Key, pair.Value)).ToArray();

        public (string, int)[] VariableCosts => _variableCosts.Select(pair => (pair.Key, pair.Value)).ToArray();
        public (string, int)[] ShopCosts => _shopCosts.Select(pair => (pair.Key, pair.Value)).ToArray();

        public bool RandomizeTransitions => RandomizeAreas || RandomizeRooms;

        public bool FreeLantern => !(DarkRooms || RandomizeKeys);
        public SaveSettings()
        {
            AfterDeserialize += () =>
            {
                /*
                foreach (var pair in VariableCosts)
                {
                    ReqDef def = LogicManager.GetItemDef(pair.Item1);
                    def.cost = pair.Item2;
                    LogicManager.EditItemDef(pair.Item1, def);
                }

                foreach (var pair in ShopCosts)
                {
                    ReqDef def = LogicManager.GetItemDef(pair.Item1);
                    def.shopCost = pair.Item2;
                    LogicManager.EditItemDef(pair.Item1, def);
                }
                */
                RandomizerAction.CreateActions(ItemPlacements, this);
            };
        }

        public int JijiHintCounter
        {
            get => GetInt(0);
            set => SetInt(value);
        }
        public int QuirrerHintCounter
        {
            get => GetInt(0);
            set => SetInt(value);
        }

        public bool AllBosses
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool AllSkills
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool AllCharms
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool CharmNotch
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool Lemm
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool Jiji
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool Quirrel
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool ItemDepthHints
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool EarlyGeo
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool LeverSkips
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool Randomizer
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeAreas
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeRooms
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool ConnectAreas
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool SlyCharm
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeDreamers
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeSkills
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeCharms
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeKeys
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeGeoChests
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeMaskShards
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeVesselFragments
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeCharmNotches
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizePaleOre
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeRancidEggs
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool RandomizeRelics
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool RandomizeMaps
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool RandomizeStags
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool RandomizeGrubs
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool RandomizeWhisperingRoots
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool DuplicateMajorItems
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        internal bool GetRandomizeByPool(string pool)
        {
            switch (pool)
            {
                case "Dreamer":
                    return RandomizeDreamers;
                case "Skill":
                    return RandomizeSkills;
                case "Charm":
                    return RandomizeCharms;
                case "Key":
                    return RandomizeKeys;
                case "Mask":
                    return RandomizeMaskShards;
                case "Vessel":
                    return RandomizeVesselFragments;
                case "Ore":
                    return RandomizePaleOre;
                case "Notch":
                    return RandomizeCharmNotches;
                case "Geo":
                    return RandomizeGeoChests;
                case "Egg":
                    return RandomizeRancidEggs;
                case "Relic":
                    return RandomizeRelics;
                case "Map":
                    return RandomizeMaps;
                case "Stag":
                    return RandomizeStags;
                case "Grub":
                    return RandomizeGrubs;
                case "Root":
                    return RandomizeWhisperingRoots;
                default:
                    return false;
            }
        }


        public bool CreateSpoilerLog
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool Cursed
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool RandomizeStartItems
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool RandomizeStartLocation
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        // The following settings names are referenced in Benchwarp. Please do not change!
        public string StartName
        {
            get => GetString("King's Pass");
            set => SetString(value);
        }

        public string StartSceneName
        {
            get => GetString("Tutorial_01");
            set => SetString(value);
        }

        public string StartRespawnMarkerName
        {
            get => GetString("Randomizer Respawn Marker");
            set => SetString(value);
        }

        public int StartRespawnType
        {
            get => GetInt(0);
            set => SetInt(value);
        }

        public int StartMapZone
        {
            get => GetInt((int)GlobalEnums.MapZone.KINGS_PASS);
            set => SetInt(value);
        }
        // End Benchwarp block.

        public bool ShadeSkips
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool AcidSkips
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool SpikeTunnels
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool MildSkips
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool SpicySkips
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool FireballSkips
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public bool DarkRooms
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public int Seed
        {
            get => GetInt(-1);
            set => SetInt(value);
        }

        public void ResetPlacements()
        {
            _itemPlacements = new SerializableStringDictionary();
            _orderedLocations = new SerializableIntDictionary();
            _transitionPlacements = new SerializableStringDictionary();
            _hintInformation = new SerializableStringDictionary();
            _variableCosts = new SerializableIntDictionary();
            _shopCosts = new SerializableIntDictionary();
            _additiveCounts = new SerializableIntDictionary();

            _obtainedItems = new SerializableBoolDictionary();
            _obtainedLocations = new SerializableBoolDictionary();
            _obtainedTransitions = new SerializableBoolDictionary();
        }

        public void AddItemPlacement(string item, string location)
        {
            _itemPlacements[item] = location;
        }

        public void AddOrderedLocation(string location, int order)
        {
            _orderedLocations[location] = order;
        }

        public int GetLocationOrder(string location)
        {
            return _orderedLocations[location];
        }

        public string GetNthLocation(int n)
        {
            return _orderedLocations.FirstOrDefault(kvp => kvp.Value == n).Key;
        }

        public string[] GetNthLocationItems(int n)
        {
            string location = GetNthLocation(n);
            return ItemPlacements.Where(pair => pair.Item2 == location).Select(pair => pair.Item1).ToArray();
        }
        
        public void AddTransitionPlacement(string entrance, string exit)
        {
            _transitionPlacements[entrance] = exit;
        }

        public void AddNewHint(string item, string location)
        {
            _hintInformation[item] = location;
        }

        public void AddNewCost(string item, int cost)
        {
            _variableCosts[item] = cost;
        }

        public void AddShopCost(string item, int cost)
        {
            _shopCosts[item] = cost;
        }

        public void MarkItemFound(string item)
        {
            _obtainedItems[item] = true;
        }

        public bool CheckItemFound(string item)
        {
            if (!_obtainedItems.TryGetValue(item, out bool found)) return false;
            return found;
        }

        public string[] GetItemsFound()
        {
            return _obtainedItems.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToArray();
        }

        public void MarkLocationFound(string location)
        {
            _obtainedLocations[location] = true;
        }

        public bool CheckLocationFound(string location)
        {
            if (!_obtainedLocations.TryGetValue(location, out bool found)) return false;
            return found;
        }

        public string[] GetLocationsFound()
        {
            return _obtainedLocations.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToArray();
        }

        public void MarkTransitionFound(string transition)
        {
            _obtainedTransitions[transition] = true;
        }

        public bool CheckTransitionFound(string transition)
        {
            if (!_obtainedTransitions.TryGetValue(transition, out bool found)) return false;
            return found;
        }

        public string[] GetTransitionsFound()
        {
            return _obtainedTransitions.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToArray();
        }

        public int GetAdditiveCount(string item)
        {
            string[] additiveSet = LogicManager.AdditiveItemSets.FirstOrDefault(set => set.Contains(item));
            if (additiveSet is null) return 0;
            if (!_additiveCounts.TryGetValue(additiveSet[0], out int count))
            {
                _additiveCounts.Add(additiveSet[0], 0);
                count = 0;
            }
            return count;
        }

        public void IncrementAdditiveCount(string item)
        {
            string[] additiveSet = LogicManager.AdditiveItemSets.FirstOrDefault(set => set.Contains(item));
            if (additiveSet is null) return;
            if (!_additiveCounts.ContainsKey(additiveSet[0]))
            {
                _additiveCounts.Add(additiveSet[0], 0);
            }
            _additiveCounts[additiveSet[0]]++;
        }
    }
}
