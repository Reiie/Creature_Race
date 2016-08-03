using UnityEngine;
using System.Collections;

public class csQulityTest : MonoBehaviour
{
    public void Increase_Quality()
    {
        QualitySettings.IncreaseLevel(true);
    }

    public void Decrease_Quality()
    {
        QualitySettings.DecreaseLevel(true);
    }
    public void Toggle_Shadow()
    {
        QualitySettings.shadowDistance = QualitySettings.shadowDistance == 0 ? 150 : 0;
    }

}
