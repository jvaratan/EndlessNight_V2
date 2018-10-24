using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergeState : InterfaceAIState {

    private readonly StatePatternAI m_statePatternAI;

    public EmergeState(StatePatternAI statePatternAI)
    {
        m_statePatternAI = statePatternAI;
    }

    public void UpdateState()
    {

    }

    public void ToEmergeState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToChasingState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.ChasingState;
    }

    public void ToAttackState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.AttackState;
    }

    public void ToJumpToState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.JumpToState;
    }

    public void ToExhaustState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.CurrentState;
    }
}
