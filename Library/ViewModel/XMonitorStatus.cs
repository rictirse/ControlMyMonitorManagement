using CMM.Library.Base;

namespace CMM.Library.ViewModel
{
    public class XMonitorStatus : PropertyBase
    {
        public string VCP_Code
        {
            get => _VCP_Code;
            set { SetProperty(ref _VCP_Code, value); }
        }
        string _VCP_Code;

        public string VCPCodeName
        {
            get => _VCPCodeName;
            set { SetProperty(ref _VCPCodeName, value); }
        }
        string _VCPCodeName;

        public string Read_Write
        {
            get => _Read_Write;
            set { SetProperty(ref _Read_Write, value); }
        }
        string _Read_Write;

        public int? CurrentValue
        {
            get => _CurrentValue;
            set { SetProperty(ref _CurrentValue, value); }
        }
        int? _CurrentValue;

        public int? MaximumValue
        {
            get => _MaximumValue;
            set { SetProperty(ref _MaximumValue, value); }
        }
        int? _MaximumValue;

        public IEnumerable<int> PossibleValues
        {
            get => _PossibleValues;
            set { SetProperty(ref _PossibleValues, value); }
        }
        IEnumerable<int> _PossibleValues;
    }
}
