using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Ghenterprise.Data
{
    public class ToastService
    {
        public void ShowToast(string title, string description)
        {
            var content = new ToastContent()
            {
                Launch = "ToastContentActivationParams",

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title
                            },

                            new AdaptiveText()
                            {
                                 Text = description
                            }
                        }
                    }
                }
            };

            var toast = new ToastNotification(content.GetXml());

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

    }
}
