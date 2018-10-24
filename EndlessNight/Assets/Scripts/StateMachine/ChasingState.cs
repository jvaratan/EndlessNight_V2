using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : InterfaceAIState
{

    private readonly StatePatternAI m_statePatternAI;

    public ChasingState(StatePatternAI statePatternAI)
    {
        m_statePatternAI = statePatternAI;
    }

    public void UpdateState()
    {

    }

    public void ToEmergeState()
    {
        m_statePatternAI.CurrentState = m_statePatternAI.EmergeState;
    }

    public void ToChasingState()
    {
        Debug.Log("Can't transition to same state");
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
