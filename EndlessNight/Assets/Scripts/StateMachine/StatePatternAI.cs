using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Emerge
//Run
//JumpTo
//Attack
//Exhaust

public class StatePatternAI : MonoBehaviour {

    public InterfaceAIState CurrentState { get { return m_currentState; } set { m_currentState = value; } }
    private InterfaceAIState m_currentState;

    public EmergeState EmergeState { get { return m_emergeState; } set { m_emergeState = value; } }
    private EmergeState m_emergeState;
    public ChasingState ChasingState { get { return m_chasingState; } set { m_chasingState = value; } }
    private ChasingState m_chasingState;
    public AttackState AttackState { get { return m_attackState; } set { m_attackState = value; } }
    private AttackState m_attackState;
    public JumpToState JumpToState { get { return m_jumpToState; } set { m_jumpToState = value; } }
    private JumpToState m_jumpToState;
    public ExhaustState ExhaustState { get { return m_exhaustState; } set { m_exhaustState = value; } }
    private ExhaustState m_exhaustState;

    private void Awake()
    {
        m_emergeState = new EmergeState(this);
        m_chasingState = new ChasingState(this);
        m_attackState = new AttackState(this);
        m_jumpToState = new JumpToState(this);
        m_exhaustState = new ExhaustState(this);
    }

    void Start ()
    {
        m_currentState = m_emergeState;
	}
	
	void Update () {
		
	}
}
