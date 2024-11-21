using System.Collections.Generic;
using UnityEngine;

public class WindowStatistics : Window
{
    public PanelStatistics panel;
    public List<FieldUIStatistics> suis = new();
    public override void Load()
    {
        panel.Init();
        panel.InitUI();
    }
    void Start()
    {
        Load();

    }


    void Update()
    {
        panel.UpdateSUI();
    }
}
