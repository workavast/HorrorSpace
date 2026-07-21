using UnityEditor.Toolbars;
using UnityEngine;

namespace App.Editor.Toolbar
{
    public class TimescaleSlider
    {
        private const float MIN_TIME_SCALE = 0f;
        private const float MAX_TIME_SCALE = 10f;

        [MainToolbarElement("Custom/Timescale-Slider", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement TimeSlider()
        {
            var content = new MainToolbarContent("Time Scale", "Time Scale");
            var slider = new MainToolbarSlider(content, Time.timeScale, MIN_TIME_SCALE, MAX_TIME_SCALE, OnSliderValueChanged);

            slider.populateContextMenu = (menu) =>
            {
                menu.AppendAction("Reset", _ =>
                {
                    Time.timeScale = 1f;
                    MainToolbar.Refresh("Custom/Timescale-Slider");
                });
            };

            return slider;
        }

        private static void OnSliderValueChanged(float newValue)
        {
            Time.timeScale = newValue;
        }
    }
}