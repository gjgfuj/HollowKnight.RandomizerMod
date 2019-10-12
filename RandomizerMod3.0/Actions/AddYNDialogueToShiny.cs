﻿using System.Collections;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine.SceneManagement;
using RandomizerMod.Extensions;
using RandomizerMod.FsmStateActions;
using SeanprCore;
using UnityEngine;
using static RandomizerMod.LogHelper;

namespace RandomizerMod.Actions
{
    internal class AddYNDialogueToShiny : RandomizerAction
    {
        public const int TYPE_GEO = 0;
        public const int TYPE_ESSENCE = 1;
        public const int TYPE_SIMPLE = 2;
        public const int TYPE_GRUB = 3;
        public const int TYPE_WRAITHS = 4;
        public const int TYPE_DREAMNAIL = 5;

        private readonly int _cost;
        private readonly string _fsmName;
        private readonly string _itemName;
        private readonly string _objectName;

        private readonly string _sceneName;
        private readonly int _type;

        public AddYNDialogueToShiny(string sceneName, string objectName, string fsmName, string itemName, int cost,
            int type = TYPE_GEO)
        {
            if (cost < 0)
            {
                LogWarn("AddYNDialogueToShiny created with negative cost, setting to 0 instead");
                cost = 0;
            }

            _sceneName = sceneName;
            _objectName = objectName;
            _fsmName = fsmName;
            _itemName = itemName;
            _cost = cost;
            _type = type;
        }

        public override ActionType Type => ActionType.PlayMakerFSM;

        public override void Process(string scene, Object changeObj)
        {
            if (scene != _sceneName || !(changeObj is PlayMakerFSM fsm) || fsm.FsmName != _fsmName ||
                fsm.gameObject.name != _objectName)
            {
                return;
            }

            FsmState noState = new FsmState(fsm.GetState("Idle"))
            {
                Name = "YN No"
            };

            noState.ClearTransitions();
            noState.RemoveActionsOfType<FsmStateAction>();

            noState.AddTransition("FINISHED", "Give Control");

            Tk2dPlayAnimationWithEvents heroUp = new Tk2dPlayAnimationWithEvents
            {
                gameObject = new FsmOwnerDefault
                {
                    OwnerOption = OwnerDefaultOption.SpecifyGameObject,
                    GameObject = Ref.Hero.gameObject
                },
                clipName = "Collect Normal 3",
                animationTriggerEvent = null,
                animationCompleteEvent = FsmEvent.GetFsmEvent("FINISHED")
            };

            noState.AddAction(new RandomizerCallStaticMethod(GetType(), nameof(CloseYNDialogue)));
            noState.AddAction(heroUp);

            FsmState giveControl = new FsmState(fsm.GetState("Idle"))
            {
                Name = "Give Control"
            };

            giveControl.ClearTransitions();
            giveControl.RemoveActionsOfType<FsmStateAction>();

            giveControl.AddTransition("FINISHED", "Idle");

            giveControl.AddAction(new RandomizerExecuteLambda(() => PlayMakerFSM.BroadcastEvent("END INSPECT")));

            fsm.AddState(noState);
            fsm.AddState(giveControl);

            FsmState charm = fsm.GetState("Charm?");
            string yesState = charm.Transitions[0].ToState;
            charm.ClearTransitions();

            charm.AddTransition("HERO DAMAGED", noState.Name);
            charm.AddTransition("NO", noState.Name);
            charm.AddTransition("YES", yesState);

            fsm.GetState(yesState).AddAction(new RandomizerCallStaticMethod(GetType(), nameof(CloseYNDialogue)));

            charm.AddFirstAction(new RandomizerCallStaticMethod(GetType(), nameof(OpenYNDialogue), fsm.gameObject,
                _itemName, _cost, _type));
        }

        private static void OpenYNDialogue(GameObject shiny, string itemName, int cost, int type)
        {
            FSMUtility.LocateFSM(GameObject.Find("DialogueManager"), "Box Open YN").SendEvent("BOX UP YN");
            FSMUtility.LocateFSM(GameObject.Find("Text YN"), "Dialogue Page Control").FsmVariables
                .GetFsmGameObject("Requester").Value = shiny;

            if (type == TYPE_ESSENCE)
            {
                // prevent beginners from being confused by dn-locked dn
                string UIName = LanguageStringManager.GetLanguageString(itemName, "UI");
                if (UIName == "Dream Nail") UIName = "Dream Gate";

                LanguageStringManager.SetString("UI", "RANDOMIZER_YN_DIALOGUE",
                    cost + " Essence: " + UIName);

                if (Ref.PD.dreamOrbs < cost)
                {
                    FSMUtility.LocateFSM(GameObject.Find("Text YN"), "Dialogue Page Control")
                        .StartCoroutine(KillGeoText());
                }

                cost = 0;
            }
            else if (type == TYPE_SIMPLE)
            {
                LanguageStringManager.SetString("UI", "RANDOMIZER_YN_DIALOGUE", cost + " Simple Key: " + LanguageStringManager.GetLanguageString(itemName, "UI"));

                if (PlayerData.instance.simpleKeys < 1 || (PlayerData.instance.simpleKeys < 2 && !PlayerData.instance.openedWaterwaysManhole))
                {
                    FSMUtility.LocateFSM(GameObject.Find("Text YN"), "Dialogue Page Control").StartCoroutine(KillGeoText());
                }

                cost = 0;
            }
            else if (type == TYPE_GRUB)
            {
                LanguageStringManager.SetString("UI", "RANDOMIZER_YN_DIALOGUE", cost + " Grubs: " + LanguageStringManager.GetLanguageString(itemName, "UI"));

                if (PlayerData.instance.grubsCollected < cost)
                {
                    FSMUtility.LocateFSM(GameObject.Find("Text YN"), "Dialogue Page Control").StartCoroutine(KillGeoText());
                }

                cost = 0;
            }
            else if (type == TYPE_WRAITHS)
            {
                LanguageStringManager.SetString("UI", "RANDOMIZER_YN_DIALOGUE", "Have Howling Wraiths: " + LanguageStringManager.GetLanguageString(itemName, "UI"));

                if (PlayerData.instance.screamLevel < 1)
                {
                    FSMUtility.LocateFSM(GameObject.Find("Text YN"), "Dialogue Page Control").StartCoroutine(KillGeoText());
                }

                cost = 0;
            }
            else if (type == TYPE_DREAMNAIL)
            {
                // prevent beginners from being confused by dn-locked dn
                string UIName = LanguageStringManager.GetLanguageString(itemName, "UI");
                if (UIName == "Dream Nail") UIName = "Dream Gate";

                LanguageStringManager.SetString("UI", "RANDOMIZER_YN_DIALOGUE", "Have Dream Nail: " + LanguageStringManager.GetLanguageString(itemName, "UI"));

                if (!PlayerData.instance.hasDreamNail)
                {
                    FSMUtility.LocateFSM(GameObject.Find("Text YN"), "Dialogue Page Control").StartCoroutine(KillGeoText());
                }

                cost = 0;
            }
            else
            {
                LanguageStringManager.SetString("UI", "RANDOMIZER_YN_DIALOGUE",
                    LanguageStringManager.GetLanguageString(itemName, "UI"));
            }

            FSMUtility.LocateFSM(GameObject.Find("Text YN"), "Dialogue Page Control").FsmVariables
                .GetFsmInt("Toll Cost").Value = cost;

            GameObject.Find("Text YN").GetComponent<DialogueBox>().StartConversation("RANDOMIZER_YN_DIALOGUE", "UI");
        }

        private static void CloseYNDialogue()
        {
            FSMUtility.LocateFSM(GameObject.Find("DialogueManager"), "Box Open YN").SendEvent("BOX DOWN YN");
        }

        private static IEnumerator KillGeoText()
        {
            PlayMakerFSM ynFsm = FSMUtility.LocateFSM(GameObject.Find("Text YN"), "Dialogue Page Control");
            while (ynFsm.ActiveStateName != "Ready for Input")
            {
                yield return new WaitForEndOfFrame();
            }

            ynFsm.FsmVariables.GetFsmGameObject("Geo Text").Value.SetActive(false);
            ynFsm.FsmVariables.GetFsmInt("Toll Cost").Value = int.MaxValue;
            PlayMakerFSM.BroadcastEvent("NOT ENOUGH");
        }
    }
}
