﻿using System.Collections.Generic;
using System.Linq;
using Hourai;
using Hourai.SmashBrew;
using UnityEngine;

namespace Hourai.SmashBrew {

    public class DataManager : Singleton<DataManager> {

        private CharacterData[] availableCharacters;
        private List<string> availableStages;
        public Mediator mediator;
        private Config _config;

        // Use this for initialization
        protected override  void Awake() {
            base.Awake();
            _config = Config.Instance;
            availableCharacters = Resources.LoadAll<CharacterData>("");

            availableStages = new List<string>();

            //TODO: Convert these into a StageData ScriptableObject
            availableStages.Add("Hakurei Shrine");
            availableStages.Add("Marisa's House");
            availableStages.Add("Clocktower");
            availableStages.Add("Misty Lake");
            availableStages.Add("Bamboo Forest");
            availableStages.Add("Hisoutensoku");
            availableStages.Add("Youkai Mountain");
            availableStages.Add("Genbu Ravine");
            availableStages.Add("Makai");
            availableStages.Add("Blazing Hell Reactor");
            availableStages.Add("Netherworld");
            availableStages.Add("Bhava-Agra");
            availableStages.Add("Netherworld");
            availableStages.Add("Old Hakurei Shrine");
            availableStages.Add("New Super Marisa Land");
            availableStages.Add("Twilight City");
            initializeMediator();
        }

        /// <summary>
        /// Check if the battle can start, that is when there is at least
        /// two players or CPUs ready to battle.
        /// </summary>
        /// <returns><c>true</c>, if the battle can start, <c>false</c> otherwise.</returns>
        public bool isReadyToStartGame() {
            int counter = Match.PlayerCount;
            mediator.Publish(new DataCommands.ReadyToFight { isReady = (counter > 1) });
            return (counter > 1);
        }

        public void initializeMediator() {
            mediator = new Mediator();
            mediator.Subscribe<DataCommands.ChangePlayerLevelCommand>(onChangePlayerLevel);
            mediator.Subscribe<DataCommands.ChangePlayerMode>(onChangePlayerMode);
            mediator.Subscribe<DataCommands.UserChangingOptions>(onUserChangingOptions);
        }

        public List<string> getAvailableStages() {
            return availableStages;
        }

        public IEnumerable<CharacterData> getAvailableCharacters() {
            foreach (var data in availableCharacters)
                yield return data;
        }

        public void onChangePlayerLevel(DataCommands.ChangePlayerLevelCommand cmd) {
            if (cmd.playerNum >= 0 && cmd.playerNum < _config.MaxPlayers)
                Match.GetPlayerData(cmd.playerNum).CpuLevel = cmd.newLevel;
            else
                Debug.LogError("Invalid player number : " + cmd.newLevel);
        }

        public void onUserChangingOptions(DataCommands.UserChangingOptions cmd) {
            //Debug.Log ("Ai mano " + cmd.isUserChangingOptions  + "  -  " + Time.time );
            if (cmd.isUserChangingOptions) {
                mediator.Publish(
                                 new DataCommands.ReadyToFight { isReady = false });
            } else
                isReadyToStartGame();
        }

        public void onChangePlayerMode(DataCommands.ChangePlayerMode cmd) {
            if (cmd.playerNum >= 0 && cmd.playerNum < _config.MaxPlayers)
                Match.GetPlayerData(cmd.playerNum).CycleType();
            else
                Debug.LogError("Invalid player number while updating player mode: " + cmd.playerNum);

            isReadyToStartGame();
        }

    }
}
