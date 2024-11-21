using UnityEngine;

public class Simulator_SLA : SwitchLeverActivator
{
    public bool T, C;
    public override void Do()
    {
        if (!click)
        {
            if(T)
            Simulator.me.IsTeplotv.Switch();
            if (C)
                Simulator.me.IsCompresser.Switch();
            click = true;
        }
    }
}
