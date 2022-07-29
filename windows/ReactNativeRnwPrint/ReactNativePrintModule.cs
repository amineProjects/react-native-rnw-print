using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using System.Threading.Tasks;
using System;

namespace ReactNativeRnwPrint
{
    [ReactModule("RnwPrint")]
    internal sealed class ReactNativePrintModule
    {
        private ReactContext _reactContext;

        [ReactInitializer]
        public void Initialize(ReactContext reactContext)
        {
            _reactContext = reactContext;
        }

        [ReactMethod("launchFullTrustProcess")]
        public async Task LaunchFullTrustProcessAsync()
        {
            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
        }


        [ReactMethod("print")]
        public async Task<string> GetRegistryKey(string filePath)
        {
            var ns = ReactPropertyBagHelper.GetNamespace("RegistryChannel");
            var name = ReactPropertyBagHelper.GetName(ns, "AppServiceConnection");

            var content = _reactContext.Handle.Properties.Get(name);

            var _connection = content as AppServiceConnection;

            ValueSet valueSet = new ValueSet
            {
                { "filePath", filePath }
            };

            var result = await _connection.SendMessageAsync(valueSet);

            string message = result.Message["PrintingStatus"].ToString();
            return message;
        }
    }
}
