namespace CatchErrors;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
public class ExceptionPanelExt : SingletonLite<ExceptionPanelExt> {
    private ExceptionPanel m_exceptionPanel;
    public UICheckBox m_chbSuppressThis;
    public ExceptionPanelExt() {
        m_exceptionPanel = UIView.library.Get<ExceptionPanel>(typeof(ExceptionPanel).Name);
        m_chbSuppressThis = UIUtil.CreateCheckBox(m_exceptionPanel.component);
        m_chbSuppressThis.name = "HealkitMod_SuppressThis";
        m_chbSuppressThis.label.text = "Suppress";
        //m_chbSuppressThis.tooltip = "[Default tooltip]";
        m_chbSuppressThis.isChecked = false;
        m_chbSuppressThis.width = 250f;
        m_chbSuppressThis.relativePosition = new Vector3(14f, 223f);
    }
}
