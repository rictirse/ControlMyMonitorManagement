using CMM.Library.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Library.ViewModel
{
    public class XMonitor : PropertyBase
    {
        /// <summary>
        /// 裝置路徑
        /// </summary>
        public string MonitorDeviceName
        {
            get => _MonitorDeviceName;
            set { SetProperty(ref _MonitorDeviceName, value); }
        }
        string _MonitorDeviceName;

        /// <summary>
        /// 裝置名稱
        /// </summary>
        public string MonitorName
        {
            get => _MonitorName;
            set { SetProperty(ref _MonitorName, value); }
        }
        string _MonitorName;

        /// <summary>
        /// 裝置序號
        /// </summary>
        public string SerialNumber
        {
            get => _SerialNumber;
            set { SetProperty(ref _SerialNumber, value); }
        }
        string _SerialNumber;

        /// <summary>
        /// 訊號裝置
        /// </summary>
        public string AdapterName
        {
            get => _AdapterName;
            set { SetProperty(ref _AdapterName, value); }
        }
        string _AdapterName;

        /// <summary>
        /// 裝置識別碼
        /// </summary>
        public string MonitorID
        {
            get => _MonitorID;
            set { SetProperty(ref _MonitorID, value); }
        }
        string _MonitorID;

        /// <summary>
        /// 狀態
        /// </summary>
        public ObservableRangeCollection<XMonitorStatus> Status
        {
            get => _Status;
            set { SetProperty(ref _Status, value); }
        }
        ObservableRangeCollection<XMonitorStatus> _Status = new();
    }
}
