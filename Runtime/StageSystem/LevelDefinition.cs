using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames
{
    [CreateAssetMenu(fileName = "new level definition", menuName = "level definition")]
    public class LevelDefinition : ScriptableObject
    {
        public StageDefinition[] stages;

        public int StagesCount
        {
            get {
                return stages != null ? stages.Length : 0;
            }
        }

        public int SelectedStageIndex
        {
            get
            {
                return PlayerPrefs.GetInt(name + ".current.stage", 0);
            }
            set
            {
                int v = value;

                if (v < 0)
                    v = 0;

                if (v > stages.Length - 1)
                    v = stages.Length - 1;

                PlayerPrefs.SetInt(name + ".current.stage", v);
            }
        }

        public ScriptableObject SelectedStage {
            get {
                return stages[SelectedStageIndex];
            }
            set
            {
                for (int i = 0; i < stages.Length; i++)
                {
                    if (value == stages[i])
                    {
                        SelectedStageIndex = i;
                        return;
                    }
                }
            }
        }

        public StageDefinition GetStage(int index)
        {
            if (index < 0 || index > stages.Length - 1)
                return null;

            return stages[index];
        }

        public bool IsStageComplete(int index)
        {
            StageDefinition stage = GetStage(index);

            if (stage == null)
                return false;

            return stage.IsComplete;
        }

        public void CompleteStage(int index)
        {
            if (index < 0 || index > stages.Length - 1)
                return;

            if (index > HighestStageIndex)
                HighestStageIndex = index;

            StageDefinition stage = GetStage(index);

            if (stage == null)
                return;

            stage.IsComplete = true;

            if (index > HighestStageIndex)
                HighestStageIndex = index;
        }

        public int NumCompletedStages
        {
            get {

                int count = 0;

                foreach(StageDefinition stage in stages)
                {
                    if (stage.IsComplete)
                        count++;
                }

                return count;
            }
        }

        public int HighestStageIndex
        {
            get
            {
                return PlayerPrefs.GetInt(name + ".highest.level", -1);
            }
            set
            {
                PlayerPrefs.SetInt(name + ".highest.level", value);
            }
        }
    }
}