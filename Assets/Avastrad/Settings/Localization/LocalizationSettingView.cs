using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Avastrad.Settings.Localization
{
    public class LocalizationSettingView : MonoBehaviour, ISettingView
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private LocalizationSettingViewModel viewModel;

        public IReadOnlyList<Locale> Options => viewModel.Options;
        private int SelectedOptionIndex => viewModel.SelectedOptionIndex;

        public void Initialize()
        {
            dropdown.ClearOptions();
            foreach (var option in Options)
                dropdown.options.Add(new TMP_Dropdown.OptionData(option.LocaleName));
            UpdateView();
        }

        public void OnEnabledManual()
        {
            viewModel.OnChanged += UpdateView;
            dropdown.onValueChanged.AddListener(SetValue);
            UpdateView();
        }

        public void OnDisabledManual()
        {
            viewModel.OnChanged -= UpdateView;
            dropdown.onValueChanged.RemoveListener(SetValue);
        }

        private void UpdateView()
        {
            dropdown.SetValueWithoutNotify(SelectedOptionIndex);
            dropdown.RefreshShownValue();
        }

        private void SetValue(int optionIndex)
        {
            viewModel.OnChanged -= UpdateView;
            viewModel.Set(optionIndex, true);
            viewModel.OnChanged += UpdateView;
        }
    }
}