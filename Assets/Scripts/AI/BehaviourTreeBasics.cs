﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BehaviourTreeBasics : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

/***********************
 * Valid Behaviour States
 * ********************/
public enum Status {
	BH_INVALID,  //Status has not been initialized
	BH_SUCCESS,
	BH_FAILURE,
	BH_RUNNING
};


/***********************
 * Base Class for Actions, Conditions, and Composites
 * ********************/
public class Behaviour {

	//Called every framr
	protected virtual Status update() { return Status.BH_INVALID; }

	//Called only first time before the call to update
	protected virtual void onInitialize() {}
	//Called once the update is finished
	protected virtual void onTerminate(Status s) {}


	public Behaviour() {}

	public Status tick() { 

		if (m_eStatus == Status.BH_INVALID) {
			onInitialize ();
		}

		m_eStatus = update ();

		if (m_eStatus != Status.BH_RUNNING) {
			onTerminate (m_eStatus);
		}

		return m_eStatus;
	}

	private Status m_eStatus;
}

public class Composite : Behaviour {
	//typedef vector(Behaviour*) Behaviours;
	public List<Behaviour> Behaviours; //In example this is a List of Behaviour*

	//Behaviours m_children;
	public List<Behaviour> m_children; // Assume this and line above are equivalent
}

public class Sequence : Composite {
	protected virtual void onInitialize() {
		//get first child
		m_Currentchild = m_children[0];
	}

	protected virtual Status update() {
		//Keep going until a chid behaviour says it's Running
		while (true) {
			Status s = m_Currentchild.tick ();

			//If child child Fails or keeps Running do the same
			if (s != Status.BH_SUCCESS) {
				return s;
			}

			//Hit the end of the array, done
			if (m_Currentchild == m_children[m_children.Count - 1]) {
				return Status.BH_SUCCESS;
			}
		}

		return Status.BH_INVALID;
	}


	//Behaviours::iterator m_Currentchild
	protected Behaviour m_Currentchild;
}



public class Selector : Composite {
	protected virtual void onInitialize() {
		//get first child
		m_Currentchild = (Behaviour)m_children[0];
	}

	protected virtual Status update() {
		//Keep going until a chid behaviour says it's Running
		while (true) {
			Status s = m_Currentchild.tick ();

			//If child child Succeeds or keeps Running do the same
			if (s != Status.BH_FAILURE) {
				return s;
			}

			//Hit the end of the array, done
			if (m_Currentchild == m_children[m_children.Count - 1]) {
				return Status.BH_FAILURE;
			}
		}

		return Status.BH_INVALID;
	}


	//Behaviours::iterator m_Currentchild
	protected Behaviour m_Currentchild;
}

/*public class Node {
	public virtual Task create() { return null; }
	public virtual void destroy(Task task) {}

	//public virtual ~Node() {}
	public Node() {}
}

public class Task {
	public Task(Node node) {}

	//public virtual ~Task()
	public Task() {} 

	public virtual Status update() {}

	public virtual void onInitialize() {}
	public virtual void onTerminate(Status s) {}

	protected Node m_pNode;
}*/