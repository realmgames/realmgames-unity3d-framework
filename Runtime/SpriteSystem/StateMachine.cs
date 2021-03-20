using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames
{
    public interface IState { }

    public interface IStateMachine
    {
        int StateCount { get; }

        int GetStateIndex();
        int GetStateIndex(string name);
        IState GetState();
        IState GetState(int index);
        IState GetState(string name);

        void SetOpacity(float opacity);
        void Set(IState state);
        void Set(IState state, Color color);
        void SetState(int index);
        void SetState(int index, Color color);
        void SetState(string name);
        void SetState(string name, Color color);
    }
}