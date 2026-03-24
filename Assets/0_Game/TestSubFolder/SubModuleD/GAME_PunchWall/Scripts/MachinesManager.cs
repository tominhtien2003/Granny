using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IResetable
{
    public void ResetInit();
}
public class MachinesManager : MonoBehaviour, IResetable
{
    public List<TrainingMachine> m_unOccupiedMachines = new List<TrainingMachine>();
    TrainingMachine[] m_machines;
    public int TraningId;
    public void Awake()
    {
        
        m_machines = GetComponentsInChildren<TrainingMachine>();
        foreach (var m in m_machines)
        {
            m.OnOccupyChanged += SetListState;
        }
        TraningId = m_machines[0].m_bodypartId;
    }
    public void ResetInit()
    {
        m_unOccupiedMachines.Clear();
        foreach (var m in m_machines) m.Occupied = false;
        foreach (var m in m_machines) m_unOccupiedMachines.Add(m);
        
    }
    void SetListState(bool occupied, TrainingMachine machine)
    {
        if (!occupied)
        {
            if (!m_unOccupiedMachines.Contains(machine))
                m_unOccupiedMachines.Add(machine);
            else this.LogError("ALready contains machine");
        }
        else
        {
            if (m_unOccupiedMachines.Contains(machine))
                m_unOccupiedMachines.Remove(machine);
            else this.LogError("Does not contain machine");
        }
    }
}
